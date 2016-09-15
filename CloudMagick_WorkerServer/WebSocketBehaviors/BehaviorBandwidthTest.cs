using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using Anotar.Log4Net;
using CloudMagick_WorkerServer.JSONstuff;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace CloudMagick_WorkerServer.WebSocketBehaviors
{
    class BehaviorBandwidthTest : WebSocketBehavior
    {
        private static byte[] _fileBytes = File.ReadAllBytes("1mb.jpg");
        protected override void OnMessage(MessageEventArgs e)
        {
            
        }

        protected override void OnOpen()
        {
            var stopper = new Stopwatch();
            stopper.Start();
            Send(_fileBytes);
            var time = stopper.ElapsedMilliseconds;
            stopper.Reset();
            Send(_fileBytes.Length + ":" + time);
        }

        /// <summary>
        /// Handles user disconnects.
        /// On User disconnect, the user gets removed from the userdictionary and all associated files are deleted
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClose(CloseEventArgs e)
        {
            
        }
    }
}
