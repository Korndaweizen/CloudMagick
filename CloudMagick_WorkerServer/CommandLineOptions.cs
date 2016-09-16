using CommandLine;
using CommandLine.Text;

namespace CloudMagick_WorkerServer
{
    class CommandLineOptions

    {
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
