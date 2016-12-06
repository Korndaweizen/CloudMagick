using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
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
            Dictionary<ClientWorker,long> workerDictionary = new Dictionary<ClientWorker,long>();
            Parallel.ForEach(_userClient.ActiveWorkers, worker =>
            {
                int sleeptime = 0;
                int sleepval = 100;

                var ws = new WorkerWSClientBandwidthTest(worker.IpAddress);
                ws.Start();
                while (ws.Dltime == 0 && sleeptime < 60000)
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
                    workerDictionary.Add(worker,ws.Bandwidth);
                }
                else
                {
                    LogTo.Debug("[SELECT] [BANDWIDTH] " + "leqz" + " KB/s");
                }
            });
            EvalAndChangeServer(workerDictionary);
        }

        public void SelectBestServerPing()
        {
            Dictionary<ClientWorker,long> workerDictionary = new Dictionary<ClientWorker,long>();
            foreach (var worker in _userClient.ActiveWorkers)
            {
                Ping sender = new Ping();
                PingReply result = sender.Send(worker.IpAddress.Split(':').First());

                if (result?.Status == IPStatus.Success)
                {
                    //LogTo.Info("[SELECT] OwnIP "+worker.IpAddress+" " + result.RoundtripTime + "ms");
                    //Console.WriteLine("Success, Time Succeeded: " + result.RoundtripTime + "ms");
                    workerDictionary.Add(worker,result.RoundtripTime);
                }

                else
                    LogTo.Debug("[SELECT] Pinging OwnIP " + worker.IpAddress + " failed");
            }
            EvalAndChangeServer(workerDictionary);
        }

        public void EvalAndChangeServer(Dictionary<ClientWorker,long> workerDictionary)
        {
            if (workerDictionary.Count > 0)
            {
                var test = workerDictionary.OrderBy(pair => pair.Value);
                var workerPair = test.First();
                if (_userClient.Mode=="Bandwidth")
                {
                    workerPair = test.Last();
                }
                var unit = (_userClient.Mode == "Latency") ? "ms" : "KB/s";
                if (_userClient.WorkerWsClient == null)
                {
                    LogTo.Info("[SELECT] [INITIAL] " + workerPair.Key.IpAddress + " " + workerPair.Value + " "+unit + " [ALLVALUES] " + string.Join(",", workerDictionary.Values.ToArray()));
                    _userClient.WorkerWsClient = CreateWorkerClient(workerPair.Key.IpAddress, _userClient);
                    _userClient.FunctionList = workerPair.Key.Functionality;
                    _userClient.WorkerWsClient.Start();
                }
                else if (!_userClient.WorkerWsClient.IPport.Equals(workerPair.Key.IpAddress) ||
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
                    _userClient.FunctionList = workerPair.Key.Functionality;
                    _userClient.WorkerWsClient.Close();
                    _userClient.WorkerWsClient = CreateWorkerClient(workerPair.Key.IpAddress, _userClient);
                    LogTo.Debug(_userClient.FunctionList.ToArray().Length.ToString);
                    _userClient.WorkerWsClient.Start();
                    LogTo.Info("[SELECT] [UPDATE] " + workerPair.Key.IpAddress + " " + workerPair.Value + " " + unit +
                               " [ALLVALUES] " + string.Join(",", workerDictionary.Values.ToArray()));
                }
                else
                {
                    LogTo.Info("[SELECT] [KEEP] " + workerPair.Key.IpAddress + " " + workerPair.Value + " " + unit +
                               " [ALLVALUES] " + string.Join(",", workerDictionary.Values.ToArray()));
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
