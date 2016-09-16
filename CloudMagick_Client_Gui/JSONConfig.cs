using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudMagick_WorkerServer.JSONstuff;

namespace CloudMagick_Client_UI
{
    public class JSONConfig
    {
        public string MasterIPPort { get; set; }
        public List<string> PathsList { get; set; }
        public int Iterations { get; set; }
        public List<string> FunctionList { get; set; }
        public string Mode { get; set; }
    }
}
