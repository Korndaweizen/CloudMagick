using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using Anotar.Log4Net;
using CloudMagick_WorkerServer.JSONstuff;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace CloudMagick_WorkerServer.WebSocketBehaviors
{
    class BehaviorUser : WebSocketBehavior
    {
        //private ConcurrentDictionary<string,MagickImage> imgDictionary = new ConcurrentDictionary<string, MagickImage>();
        private ClientUser GetCurrentUser()
        {
            ClientUser tmpuser;
            Program.ActiveUsers.TryGetValue(ID, out tmpuser);
            return tmpuser;
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            var stopwatch = Stopwatch.StartNew();
            var date = DateTime.Now.Millisecond;
            if (e.IsText)
            {
                LogTo.Info( @"Client {0} sent: ",ID);
                if (e.Data.StartsWith("COMMAND"))
                {
                    var json = e.Data.Split(new[] { ':' }, 2).Last();
                    var command = Newtonsoft.Json.JsonConvert.DeserializeObject<UserCommand>(json);
                    ClientUser tmpuser = GetCurrentUser();
                    string imagepath = "images/" + tmpuser.Secret + ".png";
                    int conversionTime;
                    int sendingTime;
                    int execTime;
                    byte[] bytes;
                    if (command.Image == null && !File.Exists(imagepath))
                    {
                        LogTo.Info("Command No Image");
                        //if (imgDictionary.ContainsKey(tmpuser.Secret))
                        Send("RESEND:" + Newtonsoft.Json.JsonConvert.SerializeObject(command));
                        return;
                    }
                    if (command.Image != null)
                    {
                        LogTo.Info(@"Command {0} with Image", command.Cmd);
                        Image image = Utility.ImageFromBase64(command.Image);
                        var bmp = new Bitmap(image);
                        bmp.Save(imagepath, ImageFormat.Png);
                    }
                    int time1 = unchecked((int) stopwatch.ElapsedMilliseconds);
                    if (command.Cmd == Command.None)
                    {
                        execTime = time1;
                        bytes = File.ReadAllBytes(imagepath);
                        conversionTime = 0;
                        sendingTime = 0;
                    }
                    else
                    {
                        execTime = command.Execute(imagepath) + time1;
                        var time2 = stopwatch.ElapsedMilliseconds;

                        bytes = File.ReadAllBytes(imagepath);
                        var time3 = stopwatch.ElapsedMilliseconds;

                        Send(bytes);

                        var time4 = stopwatch.ElapsedMilliseconds;

                        conversionTime = unchecked((int) (time3 - time2));
                        sendingTime = unchecked((int) (time4 - time3));
                    }

                    LogTo.Info(@"[IMGSIZE] {0}B [COMMAND] {1} [CONVERSION] {2}ms [EXECUTION] {3}ms [SENDING] {4}ms [COMPLETE] {5}", bytes.Length, command.Cmd, conversionTime, execTime, sendingTime, stopwatch.ElapsedMilliseconds);
                    Send("TIME:" + new Result { ImgSize = bytes.Length,RequestContainedImg = (command.Image != null), Cmd = command.Cmd, ConversionTime = conversionTime, ExecutionTime = execTime, SendingTime = sendingTime });
                    stopwatch.Stop();

                }
                else if(e.Data.StartsWith("REGISTER"))
                {
                    var json = e.Data.Split(new[] { ':' }, 2).Last();
                    var user = Newtonsoft.Json.JsonConvert.DeserializeObject<ClientUser>(json);
                    user.ID = ID;
                    Program.ActiveUsers.AddOrUpdate(ID, user, (key, oldvalue) => user);
                    Send("Registered Successfully!");
                    LogTo.Info("Registration of user: " + json);
                }
            }
            
        }

        protected override void OnOpen()
        {
            LogTo.Info("A new user connected with ID "+ID+". Active users: "+Sessions.ActiveIDs.ToArray());
        }

        /// <summary>
        /// Handles user disconnects.
        /// On User disconnect, the user gets removed from the userdictionary and all associated files are deleted
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClose(CloseEventArgs e)
        {
            ClientUser tmpuser = new ClientUser();
            Program.ActiveUsers.TryGetValue(ID, out tmpuser);
            var msg = "A Client disconnected, ID: " + ID;
            string imagepath = "images/" + tmpuser.Secret + ".png";
            if (File.Exists(imagepath))
            {
                File.Delete(imagepath);
            }
            LogTo.Info(msg);
        }
    }
}
