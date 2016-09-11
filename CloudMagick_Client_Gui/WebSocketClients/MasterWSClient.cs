using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CloudMagick_Client_Gui.JSONstuff;
using WebSocketSharp;

namespace CloudMagick_Client_Gui.WebSocketClients
{
    public class MasterWsClient
    {
        private static ClientUser _user = new ClientUser();
        public WebSocket WebSocket;
        private Form1 _form1;


        public MasterWsClient(string ipport, Form1 form1)
        {
            _form1 = form1;
            WebSocket = new WebSocket("ws://"+ipport+"/User");
        }

        public void Start()
        {
            var localip = GetLocalIpAddress();
            _user.IpAddress = localip;
            _user.Secret = RandomString(15);
            Console.WriteLine("IP Address {0}: {1} ", 1, localip);

            WebSocket.EmitOnPing = true;

            WebSocket.OnOpen += (sender, eventArgs) =>
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(_user);
                WebSocket.Send(json);
            };

            WebSocket.OnMessage += (sender, e) =>
            {
                if (e.IsText)
                {
                    // Do something with e.Data.
                    //Console.WriteLine("Server sends: " + e.Data);
                    Form1.ActiveWorkers = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ClientWorker>>(e.Data);
                    Console.WriteLine("\nAvailable Workers:");
                    foreach (var activeWorker in Form1.ActiveWorkers)
                    {
                        Console.WriteLine(activeWorker.ToString());
                    }
                    Console.WriteLine();

                    _form1.SelectBestServerPing();

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
                    var ret = WebSocket.Ping();
                    Console.WriteLine("Ping Received" + e.Data + " " + ret);
                    return;
                }
            };

            WebSocket.Connect();

        }

        public void Close()
        {
            WebSocket.Close();
        }

        public void send(string msg)
        {
            WebSocket.Send(msg);
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