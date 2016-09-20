using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class JSONConfig
    {
        public List<string> PathsList { get; set; }
        public bool Stresstest { get; set; }
        public int Iterations { get; set; }
        public List<string> FunctionList { get; set; }
    }

    public enum Command

    {
        Sepia,
        None,
        Blabla
    }
}