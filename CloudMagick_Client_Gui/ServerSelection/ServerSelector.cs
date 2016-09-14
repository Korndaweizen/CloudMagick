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

namespace CloudMagick_Client_Gui.ServerSelection
{
    public class ServerSelector
    {
        private readonly IUserClient _userClient;
        private const int Interval = 30000;

        public ServerSelector(IUserClient userClient)
        {
            _userClient = userClient;
        }
        public void DoThisAllTheTime()
        {
            while (true)
            {
                var mode = "LATENCY";
                LogTo.Info("[SELECT] Selecting best Server with mode {0}:", mode);
                SelectBestServerPing();
                Thread.Sleep(Interval);
            }
        }

        public void SelectBestServerBandwidth()
        {
            
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
                    LogTo.Info("[SELECT] IP "+worker.IpAddress+", rtt " + result.RoundtripTime + "ms");
                    //Console.WriteLine("Success, Time Succeeded: " + result.RoundtripTime + "ms");
                    workerDictionary.Add(result.RoundtripTime, worker);
                }

                else
                    LogTo.Error("[SELECT] IP " + worker.IpAddress + " failed");
            }
            if (workerDictionary.Count > 0)
            {
                var test=workerDictionary.OrderBy(pair => pair.Key);
                var workerPair = test.First();
                if (_userClient.WorkerWsClient == null)
                {
                    LogTo.Info("[SELECT] Initiated server to IP " + workerPair.Value + ", rtt " + workerPair.Key + "ms");
                    _userClient.WorkerWsClient = new WorkerWsClient(workerPair.Value.IpAddress, _userClient);
                    _userClient.FunctionList = workerPair.Value.Functionality;
                    _userClient.WorkerWsClient.Start();
                }
                else
                {
                    if (!_userClient.WorkerWsClient.IPport.Equals(workerPair.Value.IpAddress) || !_userClient.WorkerWsClient.WebSocket.IsAlive)
                    {
                        if (!_userClient.ServerMayChange)
                        {
                            LogTo.Warn("[SELECT] Server not allowed to change!");
                        }
                        while (!_userClient.ServerMayChange)
                        {
                            Thread.Sleep(100);
                        }
                        _userClient.FunctionList = workerPair.Value.Functionality;
                        _userClient.WorkerWsClient.Close();
                        _userClient.WorkerWsClient = new WorkerWsClient(workerPair.Value.IpAddress, _userClient);
                        LogTo.Info(_userClient.FunctionList.ToArray().Length.ToString);
                        _userClient.WorkerWsClient.Start();
                        LogTo.Info("[SELECT] Updated server to IP " + workerPair.Value + ", rtt " + workerPair.Key + "ms");
                    }
                }

            }
        }

    }
}
