using System;
using System.Collections.Concurrent;
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
            var wssv = new WebSocketServer(1151);
            wssv.AddWebSocketService<BehaviorUser>("/User");
            wssv.Start();

            WSClient client = new WSClient();
            client.start();

            Console.ReadKey(true);
            wssv.Stop();
            client.send("Bye");
            client.stop();
        }
    }
}