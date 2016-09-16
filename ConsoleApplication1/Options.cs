using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace ConsoleApplication1
{
    class Options
    {
        [Option('c', "console", 
          HelpText = "Starts in console mode.")]
        public bool Console { get; set; }

        [Option('a', "ip", Required = false,
          HelpText = "IP:Port of the master server")]
        public string Ip { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
