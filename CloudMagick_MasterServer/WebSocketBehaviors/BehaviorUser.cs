using System.Linq;
using Anotar.Log4Net;
using WebSocketSharp;
using WebSocketSharp.Server;
using CloudMagick_WorkerServer.JSONstuff;

namespace CloudMagick_MasterServer.WebSocketBehaviors
{
    class BehaviorUser : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            LogTo.Info(e.Data);
            foreach (var variable in Program.ActiveWorkers.Values)
            {
                LogTo.Info("Server: "+variable);
            }
            Send(Newtonsoft.Json.JsonConvert.SerializeObject(Program.ActiveWorkers.Values.ToList()));
        }

        protected override void OnOpen()
        {
            LogTo.Info("A new user connected with ID " + ID + ". Active users: " + Sessions.ActiveIDs.ToArray());
        }
        protected override void OnClose(CloseEventArgs e)
        {
            LogTo.Info("A user disconnected with ID " + ID + ". Active users: " + Sessions.ActiveIDs.ToArray());
        }
    }
}
