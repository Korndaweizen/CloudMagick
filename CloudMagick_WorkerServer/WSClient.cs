using System;

using System.IO;
using System.Linq;
using System.Net;

using WebSocketSharp;

namespace CloudMagick_WorkerServer
{
    class WSClient
    {
        private ClientWorker _worker = new ClientWorker();

        private WebSocket ws= new WebSocket("ws://127.0.0.1:1150/Worker");
        public void start()
        {
            var localip = GetLocalIpAddress();
            _worker.IpAddress = localip+":1151";
            _worker.Secret = RandomString(15);
            _worker.Functionality = File.ReadAllLines("functionality.txt").ToList();
            Console.WriteLine("IP Address {0}: {1} ", 1, localip);

            ws.EmitOnPing = true;

            ws.OnOpen += (sender, eventArgs) =>
            {
                var json = "REGISTER:" + Newtonsoft.Json.JsonConvert.SerializeObject(_worker);
                Console.WriteLine("Sending " + json + "to server");
                ws.Send(json);
            };

            ws.OnMessage += (sender, e) =>
            {
                if (e.IsText)
                {
                    // Do something with e.Data.
                    Console.WriteLine("Server sends: " + e.Data);
                    return;
                }

                if (e.IsBinary)
                {
                    // Do something with e.RawData.
                    Console.WriteLine("Server sends: UNREADABLESHIT");
                    return;
                }

                if (e.IsPing)
                {
                    // Do something to notify that a ping has been received.
                    var ret = ws.Ping();
                    Console.WriteLine("Ping Received" + e.Data + " " + ret);
                    return;
                }
            };

            ws.Connect();
        }

        public void send(string msg)
        {
            ws.Send(msg);
        }
        public void stop()
        {
            ws.CloseAsync();
        }
        public static string GetLocalIpAddress()
        {
            String strHostName = string.Empty;
            // Getting Ip address of local machine...
            // First get the host name of local machine.
            strHostName = Dns.GetHostName();
            Console.WriteLine("Local Machine's Host Name: " + strHostName);
            // Then using host name, get the IP address list..
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;

            var localip = addr.Where(o => o.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToArray().First().ToString();
            return localip;
        }


        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}