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
    public class WorkerWSBandwidthTest
    {
        public WebSocket WebSocket;
        private int _filesize;
        private int _dltime;
        public int Bandwidth => _filesize*1000/_dltime/1000;

        public WorkerWSBandwidthTest(string ipport)
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
    }
}