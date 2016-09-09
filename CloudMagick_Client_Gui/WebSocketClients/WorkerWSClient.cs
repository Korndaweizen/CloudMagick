using System;
using System.Linq;
using System.Net;
using CloudMagick_Client_Gui.JSONstuff;
using WebSocketSharp;

namespace CloudMagick_Client_Gui.WebSocketClients
{
    public class WorkerWSClient
    {
        private static ClientUser _user = new ClientUser();
        public WebSocket WebSocket;
        public string IPport;

        public WorkerWSClient(string ipport)
        {
            IPport = ipport;
            WebSocket = new WebSocket("ws://" + ipport + "/User");
        }

        public void start()
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
                    Console.WriteLine("Server sends: " + e.Data);
                    if (e.Data.StartsWith("RESULT"))
                    {
                        var json = e.Data.Split(new[] { ':' }, 2).Last();
                        UserCommand usrcmd = Newtonsoft.Json.JsonConvert.DeserializeObject<UserCommand>(json);
                        RedoUndo.AddImage(usrcmd.Image);
                    }
                    if (e.Data.StartsWith("RESEND"))
                    {
                        var json = e.Data.Split(new[] { ':' }, 2).Last();
                    }
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
            WebSocket.CloseAsync();
        }

        public void send(string msg)
        {
            WebSocket.Send(msg);
        }

        public void send(UserCommand cmd)
        {
            WebSocket.Send("COMMAND:"+Newtonsoft.Json.JsonConvert.SerializeObject(cmd));
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