using System;

using System.IO;
using System.Linq;
using System.Net;
using Anotar.Log4Net;
using CloudMagick_WorkerServer.JSONstuff;
using WebSocketSharp;

namespace CloudMagick_WorkerServer
{
    class WSClient
    {
        private ClientWorker _worker = new ClientWorker();
        private WebSocket ws;
        private WorkerConfig _config;

        public WSClient(WorkerConfig config)
        {
            _config = config;
            string connectTo = "ws://" + config.MasterIPPort + "/Worker";
            ws = new WebSocket(connectTo);
        }

        public void Start()
        {
            var localip = (_config.OwnIP=="") ? Utility.GetLocalIpAddress() : _config.OwnIP;
            _worker.IpAddress = localip+":"+_config.OwnPort;
            _worker.Secret = Utility.RandomString(15);
            var funclist = _config.FunctionList;
            _worker.Functionality = funclist.Select(x => (Command)Enum.Parse(typeof(Command), x))
          .ToList();
            LogTo.Info("OwnIP Address {0}: {1} ", 1, localip);

            ws.EmitOnPing = true;

            ws.OnOpen += (sender, eventArgs) =>
            {
                var json = "REGISTER:" + Newtonsoft.Json.JsonConvert.SerializeObject(_worker);
                LogTo.Info("Sending " + json + "to server");
                ws.Send(json);
            };

            ws.OnMessage += (sender, e) =>
            {
                if (e.IsText)
                {
                    // Do something with e.Data.
                    LogTo.Info("Server sends: " + e.Data);
                    return;
                }

                if (e.IsBinary)
                {
                    // Do something with e.RawData.
                    LogTo.Info("Server sends: unreadable binary");
                    return;
                }

                if (e.IsPing)
                {
                    // Do something to notify that a ping has been received.
                    var ret = ws.Ping();
                    LogTo.Info("Ping Received" + e.Data + " " + ret);
                    return;
                }
            };

            ws.Connect();
        }

        public void Send(string msg)
        {
            ws.Send(msg);
        }
        public void Stop()
        {
            ws.CloseAsync();
        }
    }
}