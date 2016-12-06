using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Anotar.Log4Net;
using CloudMagick_Client_UI.ServerSelection;
using CloudMagick_Client_UI.WebSocketClients;
using CloudMagick_WorkerServer.JSONstuff;
using WebSocketSharp;

namespace CloudMagick_Client_UI.UI
{
    public class ClientConsole : IUserClient
    {
        public MasterWsClient MasterWs { get; set; }
        public List<ClientWorker> ActiveWorkers { get; set; } = new List<ClientWorker>();
        public IWorkerWebSocketClient WorkerWsClient { get; set; }
        public List<Command> FunctionList { get; set; } = new List<Command>();
        public ServerSelector ServerSelector { get; set; }
        public bool ServerMayChange { get; set; } = true;
        public string Mode { get; set; } = "Latency";
        public RedoUndo RedoUndo;
        private Thread _serverSelectionThread;
        private bool _allowedToSendCommand;
        public JSONConfig Config { get; set; }

        public ClientConsole(string ipport, JSONConfig config)
        {
            Config = config;
            Mode = Config.Mode;
            Init(ipport);
            if (Config.Stresstest)
            {
                Stresstest();
            }
            else
            {
                DoTesting();
            }
            Thread.Sleep(20000);
            
            End();
        }

        private void Init(string ipport)
        {
            MasterWs = new MasterWsClient(ipport, this);
            MasterWs.Start();

            RedoUndo = new RedoUndo();
            InitDisableCommands();
            ServerSelector = new ServerSelector(this);

            _serverSelectionThread = new Thread(ServerSelector.DoThisAllTheTime);
            _serverSelectionThread.Start();
        }

        private void End()
        {
            _serverSelectionThread.Abort();
            LogTo.Debug("[SELECT] Aborted");

            MasterWs.Close();
            WorkerWsClient.Close();
        }
        private void DoTesting()
        {
            foreach (var path in Config.PathsList)
            {
                if (Directory.Exists(path))
                {
                    var files = Directory.GetFiles(path);
                    foreach (var file in files)
                    {
                        ProcessFile(file);
                    }
                }
                else
                {
                    LogTo.Debug("Directory "+ path +" does not exist");
                }
                
            }
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        private void Stresstest()
        {
            var path = Config.PathsList.First();
            if (Directory.Exists(path))
            {
                var file = Directory.GetFiles(path).First();

                Image img = Image.FromFile(file);
                UserCommand userCmd = new UserCommand();
                var funclist = Config.FunctionList.Select(x => (Command)Enum.Parse(typeof(Command), x)).ToList();
                var function = funclist.First();

                Stopwatch watch = new Stopwatch();
                watch.Start();

                while (watch.ElapsedMilliseconds/1000 < Config.Stresstime)
                {
                    RedoUndo.AddImage(img);

                    userCmd.Image = Utility.ImageToBase64(img);
                    userCmd.Cmd = Command.None;
                    while (!_allowedToSendCommand)
                    {
                        Thread.Sleep(100);
                    }
                    WorkerWsClient.Send(userCmd);

                    if (ActiveWorkers.Count > 0 && Mode == "Random")
                    {
                        ServerSelector.SelectBestServerRandom();
                    }

                    userCmd.Image = null;
                    userCmd.Cmd = function;
                    long sleeper = 0;
                    while (!_allowedToSendCommand || sleeper < Config.TimeBetweenRequests * 1000)
                    {
                        Thread.Sleep(100);
                        sleeper += 100;
                    }
                    WorkerWsClient.Send(userCmd);

                    //Thread.Sleep(Config.TimeBetweenRequests*1000);

                }
                watch.Stop();
                LogTo.Debug("Stresstest lastet " + watch.ElapsedMilliseconds/1000 +"s");
            }
            else
            {
                LogTo.Debug("Directory " + path + " does not exist");
            }
            return;
        }

        private void ProcessFile(string file)
        {
            Image img = Image.FromFile(file);
            UserCommand userCmd = new UserCommand();
            var funclist = Config.FunctionList.Select(x => (Command)Enum.Parse(typeof(Command), x)).ToList();
            foreach (var function in funclist)
            {
                for (int i = 0; i < Config.Iterations; i++)
                {
                    RedoUndo.AddImage(img);

                    userCmd.Image = Utility.ImageToBase64(img);
                    userCmd.Cmd = Command.None;
                    while (!_allowedToSendCommand)
                    {
                        Thread.Sleep(100);
                    }
                    WorkerWsClient.Send(userCmd);

                    userCmd.Image = null;
                    userCmd.Cmd = function;
                    while (!_allowedToSendCommand)
                    {
                        Thread.Sleep(100);
                    }
                    WorkerWsClient.Send(userCmd);
                }
            }
            
        }


        public void DisableCommands()
        {
            //throw new NotImplementedException();
            _allowedToSendCommand = false;
            LogTo.Debug("[CLIENT] MaySendCommands? " + _allowedToSendCommand);
        }

        public void DisableSending(bool servermaychange)
        {
            if (!servermaychange)
            {
                ServerMayChange = false;
            }
            _allowedToSendCommand = false;
            LogTo.Debug("[CLIENT] MaySendCommands? " + _allowedToSendCommand);
        }

        public void EnableSending()
        {
            ServerMayChange = true;
            _allowedToSendCommand = true;
            LogTo.Debug("[CLIENT] MaySendCommands? " + _allowedToSendCommand);
        }

        public void InitDisableCommands()
        {
            //throw new NotImplementedException();
            _allowedToSendCommand = false;
            LogTo.Debug("[CLIENT] MaySendCommands? " + _allowedToSendCommand);
        }
    }
}
