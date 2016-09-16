using System.Linq;
using Anotar.Log4Net;
using CloudMagick_WorkerServer.JSONstuff;
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
                LogTo.Info("Received: "+e.Data);
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
                LogTo.Info(tmp);
                Send(tmp);
            }
            else
            {
                string tmp = "Server Received Ping!";
                LogTo.Info(tmp);
                Send(tmp);
            }

            
        }

        protected override void OnOpen()
        {
            LogTo.Info("A new worker connected with ID " + ID + ". Active users: " + Sessions.ActiveIDs.ToArray());
        }

        //Send(msg);
       

        protected override void OnClose(CloseEventArgs e)
        {
            ClientWorker ignore;
            Program.ActiveWorkers.TryRemove(ID, out ignore);
            var msg = "A Client disconnected, ID: " + ID;
            LogTo.Info(msg);
        }

        protected override void OnError(ErrorEventArgs e)
        {
            var msg = "An Error Ocurred";
            LogTo.Info(msg + e.Message);
        }
    }
}
