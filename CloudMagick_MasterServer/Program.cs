using System;
using System.Collections.Concurrent;
using System.IO;
using CloudMagick_MasterServer.WebSocketBehaviors;
using CloudMagick_WorkerServer.JSONstuff;
using WebSocketSharp.Server;

namespace CloudMagick_MasterServer
{
    public class Program
    {

        public static ConcurrentDictionary<string,ClientWorker> ActiveWorkers = new ConcurrentDictionary<string,ClientWorker>();

        public static void Main(string[] args)
        {
            var config =
                    Newtonsoft.Json.JsonConvert.DeserializeObject<MasterConfig>(File.ReadAllText("conf.json"));
            var wssv = new WebSocketServer(config.OwnPort);

            wssv.AddWebSocketService<BehaviorUser>("/User");
            wssv.AddWebSocketService<BehaviorWorker>("/Worker");
            wssv.Start();

            Console.ReadLine();
            wssv.Stop();
        }
    }
}