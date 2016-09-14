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
            string ip = "127.0.0.1";
            string port="1150";
            if (args.Length > 0)
            {
                ip = args[0];
                if (args.Length > 1)
                {
                    port = args[1];
                }
            }
            var wssv = new WebSocketServer(1151);
            wssv.AddWebSocketService<BehaviorUser>("/User");
            wssv.Start();

            WSClient client = new WSClient(ip,port);
            client.start();

            Console.ReadLine();
            wssv.Stop();
            client.send("Bye");
            client.stop();
        }
    }
}