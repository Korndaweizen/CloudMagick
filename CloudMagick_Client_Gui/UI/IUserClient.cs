using System.Collections.Generic;
using CloudMagick_Client_UI.ServerSelection;
using CloudMagick_Client_UI.WebSocketClients;
using CloudMagick_WorkerServer.JSONstuff;

namespace CloudMagick_Client_UI.UI
{
    public interface IUserClient
    {
        void DisableCommands();
        void DisableSending(bool servermaychange);
        void EnableSending();
        void InitDisableCommands();
        MasterWsClient MasterWs { get; set; }
        List<ClientWorker> ActiveWorkers { get; set; }
        IWorkerWebSocketClient WorkerWsClient { get; set; }
        List<Command> FunctionList { get; set; }
        ServerSelector ServerSelector { get; set; }
        bool ServerMayChange { get; set; }
        string Mode { get; set; }

    }
}
