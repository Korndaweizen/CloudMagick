using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using WebSocketSharp.Server;

namespace CloudMagick_MasterServer.Clients
{
    public class ClientWorker
    {
        public string ID { get; set; }
        public string IpAddress { get; set; }
        public List<string> Functionality { get; set; }
        public string Secret { get; set; }

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}
