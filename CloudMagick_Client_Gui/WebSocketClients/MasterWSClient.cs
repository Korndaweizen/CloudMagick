﻿using System.Collections.Generic;
using System.Linq;
using Anotar.Log4Net;
using CloudMagick_Client_UI.UI;
using CloudMagick_WorkerServer.JSONstuff;
using WebSocketSharp;

namespace CloudMagick_Client_UI.WebSocketClients
{
    public class MasterWsClient
    {
        private static ClientUser _user = new ClientUser();
        public WebSocket WebSocket;
        private IUserClient _clientForm;


        public MasterWsClient(string ipport, IUserClient clientForm)
        {
            _clientForm = clientForm;
            WebSocket = new WebSocket("ws://"+ipport+"/User");
        }

        public void Start()
        {
            var localip = Utility.GetLocalIpAddress();
            _user.IpAddress = localip;
            _user.Secret = Utility.RandomString(15);
            LogTo.Debug("[MASTER] Local client OwnIP Address {0}: {1} ", 1, localip);

            WebSocket.EmitOnPing = true;

            WebSocket.OnOpen += (sender, eventArgs) =>
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(_user);
                WebSocket.Send(json);
            };

            WebSocket.OnMessage += (sender, e) =>
            {
                if (e.IsText)
                {
                    _clientForm.ActiveWorkers = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ClientWorker>>(e.Data);
                    LogTo.Debug("[MASTER] Available workers:");
                    LogTo.Warn("[MASTER] [WORKERS] " +
                               string.Join(",", _clientForm.ActiveWorkers.Select(worker => "OwnIP:" + worker.IpAddress+" F:"+worker.Functionality.Count)));
                    //_clientForm.ServerSelector.InitTestServers();

                    return;
                }

                if (e.IsBinary)
                {
                    // Do something with e.RawData.
                    LogTo.Debug("[MASTER] Sends unhandled binary");
                    return;
                }

                if (e.IsPing)
                {
                    // Do something to notify that a ping has been received.
                    //var ret = WebSocket.Ping();
                    //LogTo.Debug("[MASTER] Ping Received" + e.Data + " " + ret);
                    //Console.WriteLine("Ping Received" + e.Data + " " + ret);
                    return;
                }
            };

            WebSocket.Connect();

        }

        public void Close()
        {
            WebSocket.Close();
        }


        public void Send(string msg)
        {
            WebSocket.Send(msg);
        }

        
    }
}