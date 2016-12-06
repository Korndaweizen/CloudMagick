using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using Anotar.Log4Net;
using CloudMagick_WorkerServer.JSONstuff;
using CloudMagick_WorkerServer.WebSocketBehaviors;
using WebSocketSharp.Server;

namespace CloudMagick_WorkerServer
{
    public class Program
    {
        public static ConcurrentDictionary<string, ClientUser> ActiveUsers = new ConcurrentDictionary<string, ClientUser>();

        public static void Main(string[] args)
        {
            var options = new CommandLineOptions();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                var config =
                    Newtonsoft.Json.JsonConvert.DeserializeObject<WorkerConfig>(File.ReadAllText(options.Configpath));

                var wssv = new WebSocketServer(config.OwnPort);
                wssv.AddWebSocketService<BehaviorUser>("/User");
                wssv.AddWebSocketService<BehaviorBandwidthTest>("/BandwidthTest");
                wssv.Start();

                WSClient client = new WSClient(config);
                client.Start();

                while (true)
                {
                    if (!client.ws.IsAlive)
                    {
                        client.Start();
                    }

                    Thread.Sleep(1000);
                }
                wssv.Stop();
                client.Send("Bye");
                client.Stop();
            }
            LogTo.Debug("Error");
        }
    }
}