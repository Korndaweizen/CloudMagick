using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using Anotar.Log4Net;
using CloudMagick_Client_Gui.GUI;
using CloudMagick_Client_Gui.JSONstuff;
using WebSocketSharp;

namespace CloudMagick_Client_Gui.WebSocketClients
{
    public class WorkerWsClient
    {
        private static ClientUser _user = new ClientUser();
        public WebSocket WebSocket;
        public string IPport;
        private readonly Form1 _form1;
        private Stopwatch _stopper;

        public WorkerWsClient(string ipport, Form1 form1)
        {
            _form1 = form1;
            IPport = ipport;
            WebSocket = new WebSocket("ws://" + ipport + "/User");
        }

        public void start()
        {
            _stopper = new Stopwatch();
            var localip = Utility.GetLocalIpAddress();
            _user.IpAddress = localip;
            _user.Secret = Utility.RandomString(15);
            //Console.WriteLine("IP Address {0}: {1} ", 1, localip);


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
                    //LogTo.Info("[WORKER] Workerserver sends: ");
                    if (e.Data.StartsWith("RESULT"))
                    {
                        //LogTo.Info("RESULT");
                        var json = e.Data.Split(new[] {':'}, 2).Last();
                        UserCommand usrcmd = Newtonsoft.Json.JsonConvert.DeserializeObject<UserCommand>(json);
                        if (usrcmd.cmd!=Command.None)
                        {
                            Image image = Utility.ImageFromBase64(usrcmd.Image);
                            RedoUndo.AddImage(image);
                            LogTo.Info("[WORKER] Image received");
                        }
                        _form1.EnableBtns();
                    }
                    if (e.Data.StartsWith("RESEND"))
                    {
                        LogTo.Info("[WORKER] Image must be resent");
                        var json = e.Data.Split(new[] { ':' }, 2).Last();
                        UserCommand usrcmd = Newtonsoft.Json.JsonConvert.DeserializeObject<UserCommand>(json);

                        usrcmd.Image = Utility.ImageToBase64(RedoUndo.GetCurrentImage());
                        send(usrcmd);
                    }
                    if (e.Data.StartsWith("TIME"))
                    {
                        var json = e.Data.Split(new[] { ':' }, 2).Last();
                        var result = Newtonsoft.Json.JsonConvert.DeserializeObject<Result>(json);
                        LogTo.Info("[WORKER] Executiontime " + result.ExecutionTime +"ms");
                        LogTo.Info("[WORKER] Conversiontime " + result.ConversionTime + "ms");
                        LogTo.Info("[WORKER] Sendingtime " + result.SendingTime + "ms");
                    }
                    return;
                }
                if (e.IsBinary)
                {
                    Image image = Utility.ImageFromByte(e.RawData);
                    RedoUndo.AddImage(image);
                    _form1.EnableBtns();
                    // Do something with e.RawData.
                    var time = unchecked ((int) _stopper.ElapsedMilliseconds);
                    _stopper.Reset();
                    LogTo.Info("[WORKER] Processing complete");
                    LogTo.Info("[CLIENT] Processing took "+time+ "ms");
                    return;
                }

                if (e.IsPing)
                {
                    // Do something to notify that a ping has been received.
                    var ret = WebSocket.Ping();
                    LogTo.Debug("[CLIENT] Ping received from worker " + e.Data + " " + ret);
                    return;
                }
            };

            WebSocket.OnClose += (sender, eventArgs) =>
            {
                _form1.DisableBtns(servermaychange:true);
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

        public void send(UserCommand userCommand)
        {
            string tmp = "IMAGE";
            
            if (userCommand.Image == null)
            {
                
                if (!RedoUndo.IsPointerAtNewest())
                {
                    userCommand.Image = Utility.ImageToBase64(RedoUndo.GetCurrentImage());
                }
                else
                {
                    tmp = "NULL";
                }
            }
            if (!userCommand.cmd.Equals(Command.None))
            {
                _stopper.Start();
            }
            LogTo.Info("[CLIENT] Sending COMMAND: " + tmp +", " + userCommand.cmd );
            WebSocket.Send("COMMAND:"+Newtonsoft.Json.JsonConvert.SerializeObject(userCommand));
            _form1.DisableBtns(servermaychange: false);
        }
    }
}