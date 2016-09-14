using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudMagick_Client_Gui.JSONstuff;
using CloudMagick_Client_Gui.ServerSelection;
using CloudMagick_Client_Gui.WebSocketClients;

namespace CloudMagick_Client_Gui.GUI
{
    public interface IUserClient
    {
        void DisableCommands();
        void DisableSending(bool servermaychange);
        void EnableSending();
        void InitDisableCommands();
        MasterWsClient MasterWs { get; set; }
        List<ClientWorker> ActiveWorkers { get; set; }
        WorkerWsClient WorkerWsClient { get; set; }
        List<Command> FunctionList { get; set; }
        ServerSelector ServerSelector { get; set; }
        bool ServerMayChange { get; set; }

    }
}
