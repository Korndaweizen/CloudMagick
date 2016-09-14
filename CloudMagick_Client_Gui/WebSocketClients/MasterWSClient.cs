using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Anotar.Log4Net;
using CloudMagick_Client_Gui.GUI;
using CloudMagick_Client_Gui.JSONstuff;
using WebSocketSharp;

namespace CloudMagick_Client_Gui.WebSocketClients
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
            LogTo.Info("[MASTER] Local client IP Address {0}: {1} ", 1, localip);

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
                    // Do something with e.Data.
                    //Console.WriteLine("Server sends: " + e.Data);
                    _clientForm.ActiveWorkers = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ClientWorker>>(e.Data);
                    LogTo.Info("[MASTER] Available workers:");
                    //Console.WriteLine("\nAvailable Workers:");
                    foreach (var activeWorker in _clientForm.ActiveWorkers)
                    {
                        LogTo.Info("[MASTER] " + activeWorker.ToString());
                        //Console.WriteLine(activeWorker.ToString());
                    }
                    Console.WriteLine();

                    _clientForm.ServerSelector.SelectBestServerPing();

                    return;
                }

                if (e.IsBinary)
                {
                    // Do something with e.RawData.
                    LogTo.Warn("[MASTER] Sends unhandled binary");
                    //Console.WriteLine("Server sends: UNREADABLESHIT");
                    return;
                }

                if (e.IsPing)
                {
                    // Do something to notify that a ping has been received.
                    var ret = WebSocket.Ping();
                    LogTo.Debug("[MASTER] Ping Received" + e.Data + " " + ret);
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

        public void send(string msg)
        {
            WebSocket.Send(msg);
        }

        
    }
}