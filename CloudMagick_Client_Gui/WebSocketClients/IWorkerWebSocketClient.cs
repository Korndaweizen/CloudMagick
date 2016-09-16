using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudMagick_WorkerServer.JSONstuff;
using WebSocketSharp;

namespace CloudMagick_Client_UI.WebSocketClients
{
    public interface IWorkerWebSocketClient
    {
        void Start();
        void Close();
        void Send(UserCommand userCommand);
        WebSocket WebSocket { get; set; }
        string IPport { get; set; }
    }
}
