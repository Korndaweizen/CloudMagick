using System;
using System.Collections.Generic;
using System.Windows.Forms;
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
            var ipport = "127.0.0.1:1150";


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(ipport));
        }
    }
}
