using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CloudMagick_MasterServer.Clients;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace CloudMagick_MasterServer.WebSocketBehaviors
{
    public class BehaviorWorker : WebSocketBehavior
    {
        
        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.IsText)
            {
                Console.WriteLine("Received: "+e.Data);
                if (e.Data.StartsWith("REGISTER:"))
                {
                    var json = e.Data.Split(new[] {':'}, 2).Last();
                    var worker = Newtonsoft.Json.JsonConvert.DeserializeObject<ClientWorker>(json);
                    worker.ID = ID;
                    Program.ActiveWorkers.AddOrUpdate(ID, worker, (key, oldvalue) => worker);
                    Send("Registered Successfully!");
                }
            }
            else if (e.IsBinary)
            {
                string tmp = "Server Received Binary?!";
                Console.WriteLine(tmp);
                Send(tmp);
            }
            else
            {
                string tmp = "Server Received Ping!";
                Console.WriteLine(tmp);
                Send(tmp);
            }

            
        }

        protected override void OnOpen()
        {
            var msg = "A Client did Connect";
            Console.WriteLine(msg + ID);
            Console.WriteLine("Available Clients:");
            foreach (var activeID in Sessions.ActiveIDs)
            {
                Console.WriteLine(activeID);
            }
            //Send(msg);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            ClientWorker ignore;
            Program.ActiveWorkers.TryRemove(ID, out ignore);
            var msg = "A Client disconnected, ID: " + ID;
            Console.WriteLine(msg);
        }

        protected override void OnError(ErrorEventArgs e)
        {
            var msg = "An Error Ocurred";
            Console.WriteLine(msg + e.Message);
        }
    }
}
