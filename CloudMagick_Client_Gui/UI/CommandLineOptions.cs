using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using Newtonsoft.Json;

namespace CloudMagick_Client_UI.UI
{
    class CommandLineOptions

    {
        [Option('c', "console",
          HelpText = "Starts in console mode.")]
        public bool Console { get; set; }

        [Option('a', "ip", DefaultValue = "127.0.0.1:1150",
          HelpText = "OwnIP:OwnPort of the master server, default = 127.0.0.1:1150")]
        public string Ip { get; set; }

        [Option('f', "config", DefaultValue = "conf.json",
          HelpText = "Path to the config file, default = conf.json")]
        public string Configpath { get; set; }

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
