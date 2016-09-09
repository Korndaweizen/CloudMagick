using System.Collections.Generic;
using System.Net;

namespace CloudMagick_WorkerServer
{
    public class ClientWorker
    {
        public string ID { get; set; }
        public string IpAddress { get; set; }
        public List<string> Functionality { get; set; }
        public string Secret { get; set; }
    }
}
