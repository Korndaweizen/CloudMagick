using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using CloudMagick_WorkerServer.JSONstuff;
using GraphicsMagick;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace CloudMagick_WorkerServer.WebSocketBehaviors
{
    class BehaviorUser : WebSocketBehavior
    {
        private ConcurrentDictionary<string,MagickImage> imgDictionary = new ConcurrentDictionary<string, MagickImage>();
        private ClientUser getCurrentUser()
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
                Console.WriteLine( @"Client {0} sent: ",ID);
                if (e.Data.StartsWith("COMMAND"))
                {
                    var json = e.Data.Split(new[] { ':' }, 2).Last();
                    var command = Newtonsoft.Json.JsonConvert.DeserializeObject<UserCommand>(json);
                    ClientUser tmpuser = getCurrentUser();
                    string imagepath = "images/" + tmpuser.Secret + ".png";
                    if (command.Image == null)
                    {
                        Console.WriteLine("Command No Image");
                        if (imgDictionary.ContainsKey(tmpuser.Secret))
                        {
                            MagickImage image;
                            imgDictionary.TryGetValue(tmpuser.Secret, out image);
                            command.Execute(image);
                            var time2 = stopwatch.ElapsedMilliseconds;
                            //var date2 = DateTime.Now.Millisecond;
                            //Console.WriteLine(@"Command: {0} took {1}ms to execute", command.cmd, time2);
                            var bytes = image.ToByteArray();
                            var time3 = stopwatch.ElapsedMilliseconds;
                            Send(bytes);
                            //Send("RESULT:" + Newtonsoft.Json.JsonConvert.SerializeObject(command));
                            var time4 = stopwatch.ElapsedMilliseconds;
                            //var date3 = DateTime.Now.Millisecond;
                            Console.WriteLine(@"Conversion to bytes took {0}ms", time3 - time2);
                            Console.WriteLine(@"Sending took {0}ms", time4 - time3);
                            Console.WriteLine(@"Process: {0} took {1}ms overall", command.cmd, stopwatch.ElapsedMilliseconds);
                            stopwatch.Stop();
                            imgDictionary.AddOrUpdate(tmpuser.Secret, image, (key, oldvalue) => image);
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
                        Console.WriteLine(@"Command {0} with Image",command.cmd);
                        using (MagickImage image = MagickImage.FromBase64(command.Image))
                        {
                            image.Write(imagepath);
                        }
                        MagickImage img = new MagickImage(imagepath);
                        if (command.cmd==Command.None)
                        {
                            command.Image = null;
                            Send("RESULT:" + Newtonsoft.Json.JsonConvert.SerializeObject(command));
                        }
                        else
                        {
                            command.Execute(img);
                            var time2 = stopwatch.ElapsedMilliseconds;
                            //var date2 = DateTime.Now.Millisecond;
                            //Console.WriteLine(@"Command: {0} took {1}ms to execute", command.cmd, time2);
                            var bytes = img.ToByteArray();
                            var time3 = stopwatch.ElapsedMilliseconds;
                            Send(bytes);
                            //Send("RESULT:" + Newtonsoft.Json.JsonConvert.SerializeObject(command));
                            var time4 = stopwatch.ElapsedMilliseconds;
                            //var date3 = DateTime.Now.Millisecond;
                            Console.WriteLine(@"Conversion to bytes took {0}ms", time3 - time2);
                            Console.WriteLine(@"Sending took {0}ms", time4 - time3);
                            Console.WriteLine(@"Process: {0} took {1}ms overall", command.cmd, stopwatch.ElapsedMilliseconds);
                        }
                        imgDictionary.AddOrUpdate(tmpuser.Secret, img, (key, oldvalue) => img);
                        File.Delete(imagepath);
                        stopwatch.Stop();

                    }


                }
                else if(e.Data.StartsWith("REGISTER"))
                {
                    Console.WriteLine("Registration");
                    var json = e.Data.Split(new[] { ':' }, 2).Last();
                    var user = Newtonsoft.Json.JsonConvert.DeserializeObject<ClientUser>(json);
                    user.ID = ID;
                    Program.ActiveUsers.AddOrUpdate(ID, user, (key, oldvalue) => user);
                    Send("Registered Successfully!");
                }
            }
            
        }

        protected override void OnOpen()
        {
            var msg = "A Userclient did Connect";
            Console.WriteLine(msg);
            Console.WriteLine("Available Clients:");
            foreach (var activeID in Sessions.ActiveIDs)
            {
                Console.WriteLine(activeID);
            }
            //Send(msg);
        }

        /// <summary>
        /// Handles user disconnects.
        /// On User disconnect, the user gets removed from the userdictionary and all associated files are deleted
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClose(CloseEventArgs e)
        {
            ClientUser ignore;
            Program.ActiveUsers.TryRemove(ID, out ignore);
            var msg = "A Client disconnected, ID: " + ID;
            ClientUser tmpuser=new ClientUser();
            Program.ActiveUsers.TryGetValue(ID, out tmpuser);
            string imagepath = "images/" + tmpuser.Secret + ".png";
            if (File.Exists(imagepath))
            {
                File.Delete(imagepath);
                Console.WriteLine("File Deleted");
            }
            Console.WriteLine(msg);
        }
    }
}
