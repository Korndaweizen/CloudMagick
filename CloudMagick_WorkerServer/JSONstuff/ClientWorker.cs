using System.Collections.Generic;
using System.Net;
using CloudMagick_WorkerServer.JSONstuff;

namespace CloudMagick_WorkerServer
{
    public class ClientWorker
    {
        public string ID { get; set; }
        public string IpAddress { get; set; }
        public List<Command> Functionality { get; set; }
        public string Secret { get; set; }
    }
}
