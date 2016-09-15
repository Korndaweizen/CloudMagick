using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Anotar.Log4Net;
using CloudMagick_Client_Gui.GUI;
using CloudMagick_Client_Gui.JSONstuff;
using CloudMagick_Client_Gui.WebSocketClients;
using WebSocketSharp;

namespace CloudMagick_Client_Gui.ServerSelection
{
    public class ServerSelector
    {
        private readonly IUserClient _userClient;
        private const int Interval = 10000;

        public ServerSelector(IUserClient userClient)
        {
            _userClient = userClient;
        }
        public void DoThisAllTheTime()
        {
            while (true)
            {
                var mode= _userClient.Mode;
                LogTo.Debug("[SELECT] Selecting best Server with mode {0}:", mode);
                if (mode.Equals("Latency"))
                {
                    SelectBestServerPing();
                }else if (mode.Equals("Bandwidth"))
                {
                    SelectBestServerBandwidth();
                }
                Thread.Sleep(Interval);
            }
        }

        public void SelectBestServerBandwidth()
        {
            Dictionary<long, ClientWorker> workerDictionary = new Dictionary<long, ClientWorker>();
            foreach (var worker in _userClient.ActiveWorkers)
            {
                int sleeptime = 0;
                int sleepval = 100;

                var ws = new WorkerWSBandwidthTest(worker.IpAddress);
                ws.Start();
                while (ws.WebSocket.IsAlive && sleeptime<60000)
                {
                    sleeptime += sleepval;
                    Thread.Sleep(sleepval);
                    LogTo.Debug("[SELECT] Waiting for testfile");
                }
                if (ws.WebSocket.IsAlive)
                {
                    ws.Close();
                }
                LogTo.Debug("[SELECT] [BANDWIDTH] " + ws.Bandwidth+ " KB/s");
                if (ws.Bandwidth > 0)
                {
                    workerDictionary.Add(ws.Bandwidth,worker);
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
                    //LogTo.Info("[SELECT] IP "+worker.IpAddress+" " + result.RoundtripTime + "ms");
                    //Console.WriteLine("Success, Time Succeeded: " + result.RoundtripTime + "ms");
                    workerDictionary.Add(result.RoundtripTime, worker);
                }

                else
                    LogTo.Debug("[SELECT] Pinging IP " + worker.IpAddress + " failed");
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
                    _userClient.WorkerWsClient = new WorkerWsClient(workerPair.Value.IpAddress, _userClient);
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
                    while (!_userClient.ServerMayChange)
                    {
                        Thread.Sleep(100);
                    }
                    _userClient.FunctionList = workerPair.Value.Functionality;
                    _userClient.WorkerWsClient.Close();
                    _userClient.WorkerWsClient = new WorkerWsClient(workerPair.Value.IpAddress, _userClient);
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
    }
}
