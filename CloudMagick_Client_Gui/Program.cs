using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Anotar.Log4Net;
using CloudMagick_Client_Gui.GUI;
using CloudMagick_Client_Gui.WebSocketClients;

namespace CloudMagick_Client_Gui
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string ip = "127.0.0.1";
            string port = "1150";
            if (args.Length > 0)
            {
                ip = args[0];
                if (args.Length > 1)
                {
                    port = args[1];
                }
            }
            var ipport = ip+":"+port;


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(ipport));
        }
    }
}
