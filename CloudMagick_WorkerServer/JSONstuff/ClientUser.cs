﻿namespace CloudMagick_WorkerServer.JSONstuff
{
    public class ClientUser
    {
        public string ID { get; set; }
        public string IpAddress { get; set; }
        public string Secret { get; set; } = "";

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}
