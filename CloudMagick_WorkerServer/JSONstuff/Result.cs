using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudMagick_WorkerServer.JSONstuff
{
    class Result
    {
        public int ExecutionTime { get; set; }
        public int ConversionTime { get; set; }
        public int SendingTime { get; set; }
        public Command Cmd { get; set; }
        public int ImgSize { get; set; }

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}
