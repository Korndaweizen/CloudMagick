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
            ClientUser tmpuser = new ClientUser();
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
                    if (command.Image == null)
                    {
                        LogTo.Info("Command No Image");
                        //if (imgDictionary.ContainsKey(tmpuser.Secret))
                        if (File.Exists(imagepath))
                        {
                            var time1 = unchecked((int) stopwatch.ElapsedMilliseconds);
                            var execTime=command.Execute(imagepath)+time1;
                            var time2 = stopwatch.ElapsedMilliseconds;
                            //var date2 = DateTime.Now.Millisecond;
                            //Console.WriteLine(@"Command: {0} took {1}ms to execute", command.cmd, time2);
                            var bytes = File.ReadAllBytes(imagepath);
                            var time3 = stopwatch.ElapsedMilliseconds;
                            Send(bytes);
                            //Send("RESULT:" + Newtonsoft.Json.JsonConvert.SerializeObject(command));
                            var time4 = stopwatch.ElapsedMilliseconds;
                            //var date3 = DateTime.Now.Millisecond;
                            var conversionTime = unchecked((int)(time3 - time2));
                            var sendingTime = unchecked((int)(time4 - time3));
                            LogTo.Info(@"Conversion to bytes took {0}ms", time3 - time2);
                            LogTo.Info(@"Sending took {0}ms", time4 - time3);
                            LogTo.Info(@"Process: {0} took {1}ms overall", command.cmd,
                                stopwatch.ElapsedMilliseconds);
                            stopwatch.Stop();
                            Send("TIME:"+new Result {ConversionTime = conversionTime,ExecutionTime = execTime,SendingTime = sendingTime});
                            //imgDictionary.AddOrUpdate(tmpuser.Secret, image, (key, oldvalue) => image);
                            //image.Write(imagepath);
                        }
                        /*if (File.Exists(imagepath))
                        {
                            using (MagickImage image = new MagickImage(imagepath))
                            {
                                command.Execute(image);
                                Send("RESULT:" + Newtonsoft.Json.JsonConvert.SerializeObject(command));
                                image.Write(imagepath);
                            }
                        }*/
                        else
                        {
                            Send("RESEND:" + Newtonsoft.Json.JsonConvert.SerializeObject(command));
                        }
                    }
                    else
                    {
                        LogTo.Info(@"Command {0} with Image",command.cmd);
                        Image image = Utility.ImageFromBase64(command.Image);
                        var bmp = new Bitmap(image);
                        bmp.Save(imagepath,ImageFormat.Png);
                        //MagickImage img = new MagickImage(imagepath);
                        if (command.cmd==Command.None)
                        {
                            command.Image = null;
                            Send("RESULT:" + Newtonsoft.Json.JsonConvert.SerializeObject(command));
                        }
                        else
                        {
                            var time1 = unchecked((int)stopwatch.ElapsedMilliseconds);
                            var execTime = command.Execute(imagepath) + time1;
                            var time2 = stopwatch.ElapsedMilliseconds;
                            //var date2 = DateTime.Now.Millisecond;
                            //Console.WriteLine(@"Command: {0} took {1}ms to execute", command.cmd, time2);
                            var bytes = File.ReadAllBytes(imagepath);
                            var time3 = stopwatch.ElapsedMilliseconds;
                            Send(bytes);
                            //Send("RESULT:" + Newtonsoft.Json.JsonConvert.SerializeObject(command));
                            var time4 = stopwatch.ElapsedMilliseconds;
                            //var date3 = DateTime.Now.Millisecond;
                            var conversionTime = unchecked((int)(time3 - time2));
                            var sendingTime = unchecked((int)(time4 - time3));
                            LogTo.Info(@"Conversion to bytes took {0}ms", time3 - time2);
                            LogTo.Info(@"Sending took {0}ms", time4 - time3);
                            LogTo.Info(@"Process: {0} took {1}ms overall", command.cmd,
                                stopwatch.ElapsedMilliseconds);
                            stopwatch.Stop();
                            Send("TIME:" + new Result { ConversionTime = conversionTime, ExecutionTime = execTime, SendingTime = sendingTime });
                        }
                        //imgDictionary.AddOrUpdate(tmpuser.Secret, img, (key, oldvalue) => img);
                        //img.Write(imagepath);
                        stopwatch.Stop();

                    }


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
