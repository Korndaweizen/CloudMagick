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
        private readonly IUserClient _clientForm;
        private Stopwatch _stopper;
        private int _sendingTime;

        public WorkerWsClient(string ipport, IUserClient clientForm)
        {
            _clientForm = clientForm;
            IPport = ipport;
            WebSocket = new WebSocket("ws://" + ipport + "/User");
        }

        public void Start()
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
                _clientForm.EnableSending();
            };

            WebSocket.OnMessage += (sender, e) =>
            {
                if (e.IsText)
                {
                    if (e.Data.StartsWith("RESEND"))
                    {
                        LogTo.Info("[WORKER] Image must be resent");
                        var json = e.Data.Split(new[] { ':' }, 2).Last();
                        UserCommand usrcmd = Newtonsoft.Json.JsonConvert.DeserializeObject<UserCommand>(json);

                        usrcmd.Image = Utility.ImageToBase64(RedoUndo.GetCurrentImage());
                        Send(usrcmd);
                    }
                    if (e.Data.StartsWith("TIME"))
                    {
                        var json = e.Data.Split(new[] { ':' }, 2).Last();
                        var result = Newtonsoft.Json.JsonConvert.DeserializeObject<Result>(json);
                        var combineConversionSending = result.ConversionTime + result.SendingTime;
                        var completeTime = unchecked((int)_stopper.ElapsedMilliseconds);
                        _stopper.Reset();
                        _clientForm.EnableSending();
                        LogTo.Info("[WORKER] [IMGSIZE] "+result.ImgSize+" [COMMAND] "+result.Cmd+" [COMPLETE] "+completeTime+"ms [ULTIME] "+_sendingTime+"ms [EXECTIME] " + result.ExecutionTime +"ms [DLTIME] " + combineConversionSending + "ms");
                    }
                    return;
                }
                if (e.IsBinary)
                {
                    Image image = Utility.ImageFromByte(e.RawData);
                    RedoUndo.AddImage(image);
                    // Do something with e.RawData.
                    //var time = unchecked ((int) _stopper.ElapsedMilliseconds);
                    _stopper.Stop();
                    //LogTo.Info("[WORKER] Processing complete");
                    //LogTo.Info("[CLIENT] Processing took "+time+ "ms");
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
                _clientForm.DisableSending(servermaychange:true);
            };

            WebSocket.Connect();


        }


        public void Close()
        {
            WebSocket.Close();
        }

        //public void Send(string msg)
        //{
        //    WebSocket.Send(msg);
        //}

        public void Send(UserCommand userCommand)
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
                //_stopper.Start();
            }
            _stopper.Start();
            LogTo.Info("[CLIENT] Sending COMMAND: " + tmp +", " + userCommand.cmd );
            WebSocket.Send("COMMAND:"+Newtonsoft.Json.JsonConvert.SerializeObject(userCommand));
            _sendingTime = unchecked ((int) _stopper.ElapsedMilliseconds);
            _clientForm.DisableSending(servermaychange: false);
        }
    }
}