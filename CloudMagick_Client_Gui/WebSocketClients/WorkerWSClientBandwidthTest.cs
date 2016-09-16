using System;
using Anotar.Log4Net;
using WebSocketSharp;

namespace CloudMagick_Client_UI.WebSocketClients
{
    public class WorkerWSClientBandwidthTest
    {
        public WebSocket WebSocket;
        private int _filesize;
        private int _dltime;
        public int Bandwidth => _filesize*1000/_dltime/1000;

        public WorkerWSClientBandwidthTest(string ipport)
        {
            WebSocket = new WebSocket("ws://" + ipport + "/BandwidthTest");
        }

        public void Start()
        {


            WebSocket.OnMessage += (sender, e) =>
            {
                if (e.IsText)
                {
                    var text = e.Data.Split(new[] { ':' }, 2);
                    _filesize = Int32.Parse(text[0]);
                    _dltime = Int32.Parse(text[1]);
                    //LogTo.Info("[BANDWIDTHTEST] "+_filesize+"B "+_dltime+"ms "+Bandwidth + "B/s");
                    Close();
                }
                if (e.IsBinary)
                {
                    //LogTo.Info("[BANDWIDTHTEST] Received 1MB File");
                }

                if (e.IsPing)
                {

                }
            };


            WebSocket.Connect();

        }

        public void Close()
        {
            WebSocket.Close();
        }

        public void Send(string msg)
        {
            LogTo.Debug("[BANDWIDTHTEST] Sending, this should not happen");
        }
    }
}