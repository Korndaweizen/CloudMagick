using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anotar.Log4Net;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace CloudMagick_MasterServer.WebSocketBehaviors
{
    class BehaviorUser : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            LogTo.Info(e.Data);
            foreach (var VARIABLE in Program.ActiveWorkers.Values)
            {
                LogTo.Info("Server: "+VARIABLE);
            }
            Send(Newtonsoft.Json.JsonConvert.SerializeObject(Program.ActiveWorkers.Values.ToList()));
        }

        protected override void OnOpen()
        {
            LogTo.Info("A new worker connected with ID " + ID + ". Active users: " + Sessions.ActiveIDs.ToArray());
        }
    }
}
