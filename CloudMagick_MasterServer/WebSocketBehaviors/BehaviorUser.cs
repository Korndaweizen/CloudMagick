using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace CloudMagick_MasterServer.WebSocketBehaviors
{
    class BehaviorUser : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            Console.WriteLine(e.Data);
            foreach (var VARIABLE in Program.ActiveWorkers.Values)
            {
                Console.WriteLine("Server: "+VARIABLE);
            }
            Send(Newtonsoft.Json.JsonConvert.SerializeObject(Program.ActiveWorkers.Values.ToList()));
        }

        protected override void OnOpen()
        {
            var msg = "A Client did Connect";
            Console.WriteLine(msg);
            Console.WriteLine("Available Clients:");
            foreach (var activeID in Sessions.ActiveIDs)
            {
                Console.WriteLine(activeID);
            }
            //Send(msg);
        }
    }
}
