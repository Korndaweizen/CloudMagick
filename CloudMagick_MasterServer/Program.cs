using System;
using System.Collections.Concurrent;
using CloudMagick_MasterServer.Clients;
using CloudMagick_MasterServer.WebSocketBehaviors;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace CloudMagick_MasterServer
{
    public class Program
    {

        public static ConcurrentDictionary<string,ClientWorker> ActiveWorkers = new ConcurrentDictionary<string,ClientWorker>();

        public static void Main(string[] args)
        {
            var wssv = new WebSocketServer(1150);

            wssv.AddWebSocketService<BehaviorUser>("/User");
            wssv.AddWebSocketService<BehaviorWorker>("/Worker");
            wssv.Start();

            Console.ReadLine();
            wssv.Stop();
        }
    }
}