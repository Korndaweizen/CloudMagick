using System;
using System.Drawing;
using System.IO;
using System.Linq;
using CloudMagick_WorkerServer.JSONstuff;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace CloudMagick_WorkerServer.WebSocketBehaviors
{
    class BehaviorUser : WebSocketBehavior
    {
        private ClientUser getCurrentUser()
        {
            ClientUser tmpuser = new ClientUser();
            Program.ActiveUsers.TryGetValue(ID, out tmpuser);
            return tmpuser;
        }
        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.IsText)
            {
                Console.WriteLine( @"Client {0} sent: {1}",ID,e.Data);
                if (e.Data.StartsWith("COMMAND"))
                {
                    var json = e.Data.Split(new[] { ':' }, 2).Last();
                    var command = Newtonsoft.Json.JsonConvert.DeserializeObject<UserCommand>(json);

                    if (command.Image.Equals(null))
                    {
                        ClientUser tmpuser = getCurrentUser();
                        string imagepath = "images/" + tmpuser.Secret + ".png";
                        if (File.Exists(imagepath))
                        {
                            command.Image = Image.FromFile(imagepath);
                            command.Execute();
                            command.Image.Save("images/" + tmpuser.Secret + ".png");
                            Send("RESULT:" + Newtonsoft.Json.JsonConvert.SerializeObject(command));
                        }
                        else
                        {
                            Send("RESEND:" + Newtonsoft.Json.JsonConvert.SerializeObject(command));
                        }
                    }
                    else
                    {
                        ClientUser tmpuser = getCurrentUser();
                        command.Execute();
                        command.Image.Save("images/" + tmpuser.Secret + ".png");
                        Send("RESULT:" + Newtonsoft.Json.JsonConvert.SerializeObject(command));
                    }

                    
                }
                else if(e.Data.StartsWith("REGISTER"))
                {
                    var json = e.Data.Split(new[] { ':' }, 2).Last();
                    var user = Newtonsoft.Json.JsonConvert.DeserializeObject<ClientUser>(json);
                    user.ID = ID;
                    Program.ActiveUsers.AddOrUpdate(ID, user, (key, oldvalue) => user);
                    Send("Registered Successfully!");
                }
                Send(e.Data);
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
