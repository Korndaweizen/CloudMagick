using System;
using System.Windows.Forms;
using Anotar.Log4Net;
using CloudMagick_Client_UI.UI;
using System.IO;

namespace CloudMagick_Client_UI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            var options = new CommandLineOptions();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                // Values are available here
                var ipport = options.Ip;

                var config = ReadConfig(options.Configpath);
                ipport = (config.MasterIPPort != "") ? config.MasterIPPort : ipport;

                if (options.Console || !options.Console)
                {
                    LogTo.Debug("Console Mode");
                    var clientConsole = new ClientConsole(ipport, config);
                }
                else
                {
                    LogTo.Debug("GUI Mode");
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new ClientForm(ipport));
                }
            }
        }

        private static JSONConfig ReadConfig(string configpath)
        {
            JSONConfig conf;
            if (File.Exists(configpath))
            {
                string json = File.ReadAllText(configpath);
                conf = Newtonsoft.Json.JsonConvert.DeserializeObject<JSONConfig>(json);
            }
            else
            {
                throw new Exception(configpath + " not found!");
            }
            return conf;
        }
    }
}
