using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Anotar.Log4Net;
using CloudMagick_Client_UI.UI;
using CloudMagick_WorkerServer.JSONstuff;
using WebSocketSharp;

namespace CloudMagick_Client_UI.WebSocketClients
{
    public class WorkerWsClientGUI : IWorkerWebSocketClient
    {
        private static ClientUser _user = new ClientUser();
        public WebSocket WebSocket { get; set; }
        public string IPport { get; set; }
        private readonly IUserClient _clientForm;
        private Stopwatch _stopper;
        private int _sendingTime;
        private string _tmp;
        private int _sentImgSize;

        public WorkerWsClientGUI(string ipport, IUserClient clientForm)
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
            //Console.WriteLine("OwnIP Address {0}: {1} ", 1, localip);


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
                        LogTo.Debug("[WORKER] Image must be resent");
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
                        LogTo.Error("[WORKER] [IMGSIZERECEIVED] " + result.ImgSize + "B [IMGSIZESENT] " + _sentImgSize + "B [IMGINCLUDED] " + result.RequestContainedImg + " [COMMAND] " + result.Cmd + " [COMPLETE] " + completeTime + "ms [ULTIME] " + _sendingTime + "ms [EXECTIME] " + result.ExecutionTime + "ms [DLTIME] " + combineConversionSending + "ms");
                    }
                    return;
                }
                if (e.IsBinary)
                {
                    Image image = Utility.ImageFromByte(e.RawData);
                    RedoUndo.AddImage(image);
                    
                    _stopper.Stop();
                    return;
                }

                if (e.IsPing)
                {
                    // Do something to notify that a ping has been received.
                    //var ret = WebSocket.Ping();
                    //LogTo.Debug("[CLIENT] Ping received from worker " + e.Data + " " + ret);
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
            _clientForm.DisableSending(servermaychange: false);
            _tmp = "Image";

            if (userCommand.Image == null)
            {

                if (!RedoUndo.IsPointerAtNewest())
                {
                    var imgByte = Utility.ImageToBytes(RedoUndo.GetCurrentImage());
                    userCommand.Image = Utility.BytesToBas64(imgByte);
                    _sentImgSize = imgByte.Length;
                }
                else
                {
                    _tmp = "NoImage";
                    _sentImgSize = 0;
                }
            }
            else
            {
                _sentImgSize = Utility.ImageToBytes(RedoUndo.GetCurrentImage()).Length;
            }
            _stopper.Start();
            LogTo.Debug("[CLIENT] Sending COMMAND: " + _tmp + ", " + userCommand.Cmd);
            WebSocket.Send("COMMAND:" + Newtonsoft.Json.JsonConvert.SerializeObject(userCommand));
            _sendingTime = unchecked((int)_stopper.ElapsedMilliseconds);
        }
    }
}