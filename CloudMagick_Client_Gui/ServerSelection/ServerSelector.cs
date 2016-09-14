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
        private Form1 _form1;

        public ServerSelector(Form1 form1)
        {
            _form1 = form1;
        }
        public void DoThisAllTheTime()
        {
            while (true)
            {
                var mode = "LATENCY";
                LogTo.Info("[SELECT] Selecting best Server with mode {0}:", mode);
                SelectBestServerPing();
                Thread.Sleep(10000);
            }
        }

        public void SelectBestServerPing()
        {
            Dictionary<long, ClientWorker> workerDictionary = new Dictionary<long, ClientWorker>();
            foreach (var worker in Form1.ActiveWorkers)
            {
                Ping sender = new Ping();
                PingReply result = sender.Send(worker.IpAddress.Split(':').First());

                if (result.Status == IPStatus.Success)
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
                workerDictionary.OrderBy(pair => pair.Key);
                var workerPair = workerDictionary.First();
                if (Form1.WorkerWsClient == null)
                {
                    LogTo.Info("[SELECT] Initiated server to IP " + workerPair.Value + ", rtt " + workerPair.Key + "ms");
                    Form1.WorkerWsClient = new WorkerWsClient(workerPair.Value.IpAddress, _form1);
                    _form1.FunctionList = workerPair.Value.Functionality;
                    Form1.WorkerWsClient.start();
                }
                else
                {
                    if (!Form1.WorkerWsClient.IPport.Equals(workerPair.Value) || !Form1.WorkerWsClient.WebSocket.IsAlive)
                    {
                        while (!_form1.Servermaychange)
                        {
                            LogTo.Warn("[SELECT] Server not allowed to change!");
                            Thread.Sleep(100);
                        }
                        _form1.FunctionList = workerPair.Value.Functionality;
                        Form1.WorkerWsClient.Close();
                        Form1.WorkerWsClient = new WorkerWsClient(workerPair.Value.IpAddress, _form1);
                        LogTo.Info(_form1.FunctionList.ToArray().Length.ToString);
                        Form1.WorkerWsClient.start();
                        LogTo.Info("[SELECT] Updated server to IP " + workerPair.Value + ", rtt " + workerPair.Key + "ms");
                    }
                }

            }
        }

    }
}
