using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using CloudMagick_Client_Gui.JSONstuff;
using GraphicsMagick;
using WebSocketSharp;

namespace CloudMagick_Client_Gui.WebSocketClients
{
    public class WorkerWSClient
    {
        private static ClientUser _user = new ClientUser();
        public WebSocket WebSocket;
        public string IPport;
        private Form1 _form1;

        public WorkerWSClient(string ipport, Form1 form1)
        {
            _form1 = form1;
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
                WebSocket.Send("REGISTER:"+json);
                _form1.EnableBtns();
            };

            WebSocket.OnMessage += (sender, e) =>
            {
                if (e.IsText)
                {
                    // Do something with e.Data.
                    Console.WriteLine("Server sends: ");
                    if (e.Data.StartsWith("RESULT"))
                    {
                        Console.WriteLine("RESULT");
                        var json = e.Data.Split(new[] {':'}, 2).Last();
                        UserCommand usrcmd = Newtonsoft.Json.JsonConvert.DeserializeObject<UserCommand>(json);
                        if (usrcmd.cmd!=Command.None)
                        {
                            MagickImage image = MagickImage.FromBase64(usrcmd.Image);
                            RedoUndo.AddImage(image);
                        }
                        _form1.EnableBtns();
                    }
                    if (e.Data.StartsWith("RESEND"))
                    {
                        Console.WriteLine("RESEND");
                        var json = e.Data.Split(new[] { ':' }, 2).Last();
                        UserCommand usrcmd = Newtonsoft.Json.JsonConvert.DeserializeObject<UserCommand>(json);

                        using (MagickImage image = new MagickImage(RedoUndo.GetCurrentImage()))
                        {
                            usrcmd.Image = image.ToBase64();
                        }
                        

                        send(usrcmd);
                    }
                    return;
                }
                if (e.IsBinary)
                {
                    MagickImage image = new MagickImage(e.RawData);
                    RedoUndo.AddImage(image);
                    _form1.EnableBtns();
                    // Do something with e.RawData.
                    Console.WriteLine("Server sends: BINARY");
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

            WebSocket.OnClose += (sender, eventArgs) =>
            {
                _form1.DisableBtns();
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
            string tmp = "IMAGE";
            
            if (cmd.Image == null)
            {
                
                if (!RedoUndo.IsPointerAtNewest())
                {
                    cmd.Image = RedoUndo.GetCurrentImage().ToBase64();
                }
                else
                {
                    tmp = "NULL";
                }
            }
            Console.WriteLine("Sending COMMAND: " + tmp +", " + cmd.cmd.ToString() );
            WebSocket.Send("COMMAND:"+Newtonsoft.Json.JsonConvert.SerializeObject(cmd));
            _form1.DisableBtns();
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