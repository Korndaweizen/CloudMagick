using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using Anotar.Log4Net;
using CloudMagick_Client_UI.UI;
using CloudMagick_Client_UI.WebSocketClients;
using CloudMagick_WorkerServer.JSONstuff;

namespace CloudMagick_Client_UI.ServerSelection
{
    public class ServerSelector
    {
        private readonly IUserClient _userClient;
        private int _interval = 30000;

        public ServerSelector(IUserClient userClient)
        {
            _userClient = userClient;
            if (userClient.Config.TimeBetweenServerProbes > 0)
                _interval = userClient.Config.TimeBetweenServerProbes*1000;
        }

        public void DoThisAllTheTime()
        {
            while (true)
            {
                if (_userClient.ActiveWorkers.Count > 0)
                {
                    var mode = _userClient.Mode;
                    LogTo.Debug("[SELECT] Selecting best Server with mode {0}:", mode);
                    if (mode.Equals("Latency"))
                    {
                        SelectBestServerPing();
                    }
                    else if (mode.Equals("Bandwidth"))
                    {
                        SelectBestServerBandwidth();
                    }
                    else if (mode.Equals("Random"))
                    {
                        SelectBestServerRandom();
                    }
                    Thread.Sleep(_interval);
                }
                else
                {
                    LogTo.Debug("[SELECT] Waiting for active worker server");
                    Thread.Sleep(100);
                }

            }
        }

        public void SelectBestServerRandom()
        {
            Random rand = new Random();
            LogTo.Debug("Active workers count: "+ _userClient.ActiveWorkers.Count);
            var nextWorker = _userClient.ActiveWorkers.ElementAt(rand.Next(0,_userClient.ActiveWorkers.Count-1));
            if (_userClient.WorkerWsClient == null)
            {
                LogTo.Info("[SELECT] [INITIAL] " + nextWorker.IpAddress + " " + "Random" + " " + "rand" + " [ALLVALUES] " + "0");
                _userClient.WorkerWsClient = CreateWorkerClient(nextWorker.IpAddress, _userClient);
                _userClient.FunctionList = nextWorker.Functionality;
                _userClient.WorkerWsClient.Start();
            }
            else if (!_userClient.WorkerWsClient.IPport.Equals(nextWorker.IpAddress) ||
                     !_userClient.WorkerWsClient.WebSocket.IsAlive)
            {
                if (!_userClient.ServerMayChange)
                {
                    LogTo.Debug("[SELECT] Server not allowed to change!");
                }
                while (!_userClient.ServerMayChange & _userClient.WorkerWsClient.WebSocket.IsAlive)
                {
                    Thread.Sleep(100);
                }
                _userClient.FunctionList = nextWorker.Functionality;
                _userClient.WorkerWsClient.Close();
                _userClient.WorkerWsClient = CreateWorkerClient(nextWorker.IpAddress, _userClient);
                LogTo.Debug(_userClient.FunctionList.ToArray().Length.ToString);
                _userClient.WorkerWsClient.Start();
                LogTo.Info("[SELECT] [UPDATE] " + nextWorker.IpAddress + " " + "Random" + " " + "rand" + " [ALLVALUES] " + "0");
            }
            else
            {
                LogTo.Info("[SELECT] [KEEP] " + nextWorker.IpAddress + " " + "Random" + " " + "rand" + " [ALLVALUES] " + "0");
            }
        }

        public void SelectBestServerBandwidth()
        {
            Dictionary<long, ClientWorker> workerDictionary = new Dictionary<long, ClientWorker>();
            foreach (var worker in _userClient.ActiveWorkers)
            {
                int sleeptime = 0;
                int sleepval = 100;

                var ws = new WorkerWSClientBandwidthTest(worker.IpAddress);
                ws.Start();
                while (ws.Dltime==0  && sleeptime<60000)
                {
                    if (!ws.WebSocket.IsAlive)
                    {
                        ws.Start();
                    }
                    sleeptime += sleepval;
                    Thread.Sleep(sleepval);
                    LogTo.Debug("[SELECT] Waiting for testfile");
                }

                if (ws.Bandwidth > 0)
                {
                    LogTo.Debug("[SELECT] [BANDWIDTH] " + ws.Bandwidth + " KB/s");
                    workerDictionary.Add(ws.Bandwidth,worker);
                }
                else
                {
                    LogTo.Debug("[SELECT] [BANDWIDTH] " + "leqz" + " KB/s");
                }
            }
            EvalAndChangeServer(workerDictionary);
        }

        public void SelectBestServerPing()
        {
            Dictionary<long, ClientWorker> workerDictionary = new Dictionary<long, ClientWorker>();
            foreach (var worker in _userClient.ActiveWorkers)
            {
                Ping sender = new Ping();
                PingReply result = sender.Send(worker.IpAddress.Split(':').First());

                if (result?.Status == IPStatus.Success)
                {
                    //LogTo.Info("[SELECT] OwnIP "+worker.IpAddress+" " + result.RoundtripTime + "ms");
                    //Console.WriteLine("Success, Time Succeeded: " + result.RoundtripTime + "ms");
                    workerDictionary.Add(result.RoundtripTime, worker);
                }

                else
                    LogTo.Debug("[SELECT] Pinging OwnIP " + worker.IpAddress + " failed");
            }
            EvalAndChangeServer(workerDictionary);
        }

        public void EvalAndChangeServer(Dictionary<long, ClientWorker> workerDictionary)
        {
            if (workerDictionary.Count > 0)
            {
                var test = workerDictionary.OrderBy(pair => pair.Key);
                var workerPair = test.First();
                var unit = (_userClient.Mode == "Latency") ? "ms" : "KB/s";
                if (_userClient.WorkerWsClient == null)
                {
                    LogTo.Info("[SELECT] [INITIAL] " + workerPair.Value.IpAddress + " " + workerPair.Key + " "+unit + " [ALLVALUES] " + string.Join(",", workerDictionary.Keys.ToArray()));
                    _userClient.WorkerWsClient = CreateWorkerClient(workerPair.Value.IpAddress, _userClient);
                    _userClient.FunctionList = workerPair.Value.Functionality;
                    _userClient.WorkerWsClient.Start();
                }
                else if (!_userClient.WorkerWsClient.IPport.Equals(workerPair.Value.IpAddress) ||
                         !_userClient.WorkerWsClient.WebSocket.IsAlive)
                {
                    if (!_userClient.ServerMayChange)
                    {
                        LogTo.Debug("[SELECT] Server not allowed to change!");
                    }
                    while (!_userClient.ServerMayChange & _userClient.WorkerWsClient.WebSocket.IsAlive)
                    {
                        Thread.Sleep(100);
                    }
                    _userClient.FunctionList = workerPair.Value.Functionality;
                    _userClient.WorkerWsClient.Close();
                    _userClient.WorkerWsClient = CreateWorkerClient(workerPair.Value.IpAddress, _userClient);
                    LogTo.Debug(_userClient.FunctionList.ToArray().Length.ToString);
                    _userClient.WorkerWsClient.Start();
                    LogTo.Info("[SELECT] [UPDATE] " + workerPair.Value.IpAddress + " " + workerPair.Key + " " + unit +
                               " [ALLVALUES] " + string.Join(",", workerDictionary.Keys.ToArray()));
                }
                else
                {
                    LogTo.Info("[SELECT] [KEEP] " + workerPair.Value.IpAddress + " " + workerPair.Key + " " + unit +
                               " [ALLVALUES] " + string.Join(",", workerDictionary.Keys.ToArray()));
                }

            }
        }

        public IWorkerWebSocketClient CreateWorkerClient(string ipAddress, IUserClient userClient)
        {
            IWorkerWebSocketClient worker = null;
            if (userClient.GetType()==typeof(ClientConsole))
            {
                worker=new WorkerWsClientConsole(ipAddress,userClient);
            }
            else if(userClient.GetType() == typeof(ClientForm))
            {
                worker=new WorkerWsClientGUI(ipAddress, userClient);
            }
            return worker;
        }

        public void InitTestServers()
        {
            if (_userClient.ActiveWorkers.Count > 0)
            {
                var mode = _userClient.Mode;
                LogTo.Debug("[SELECT] Selecting best Server with mode {0}:", mode);
                if (mode.Equals("Latency"))
                {
                    SelectBestServerPing();
                }
                else if (mode.Equals("Bandwidth"))
                {
                    SelectBestServerBandwidth();
                }
                else if (mode.Equals("Random"))
                {
                    SelectBestServerRandom();
                }
                Thread.Sleep(_interval);
            }
            else
            {
                LogTo.Debug("[SELECT] Waiting for active worker server");
                Thread.Sleep(100);
            }
        }
    }
}
