using System.Collections.Generic;

namespace CloudMagick_WorkerServer.JSONstuff
{
    public class WorkerConfig
    {
        public string OwnIP { get; set; }
        public string MasterIPPort { get; set; }
        public int OwnPort { get; set; }
        public List<string> FunctionList { get; set; }
    }
}
