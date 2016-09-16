using System.Collections.Generic;

namespace CloudMagick_WorkerServer.JSONstuff
{
    public class ClientWorker
    {
        public string ID { get; set; }
        public string IpAddress { get; set; }
        public List<Command> Functionality { get; set; }
        public string Secret { get; set; }
        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}
