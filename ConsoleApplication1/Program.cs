using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Anotar.Log4Net;
using log4net;
using log4net.Core;
using log4net.Repository.Hierarchy;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            LogTo.Debug("test");
            LogTo.Info("test info");
            var win = "/C convert test.jpg -border 20 test2.png";
            var linux = "-solarize 50% test.jpg";
            if (Environment.OSVersion.ToString().Contains("Windows"))
            {
                Console.WriteLine("Windoof");
                CommandlineExec(win,"cmd.exe");
            }
            else
            {
                Console.WriteLine(Environment.OSVersion.ToString());
                CommandlineExec(linux,"mogrify");
            }
            Console.ReadLine();
        }
        public static void CommandlineExec(string cmd,string name)
        {
            LogTo.Debug("TheMessage");
            Process proc = new Process();
            proc.StartInfo.FileName = name;
            proc.StartInfo.Arguments = cmd;
            //proc.StartInfo.UseShellExecute = true;
            //proc.StartInfo.RedirectStandardError = true;
            //proc.StartInfo.RedirectStandardInput = true;
            //proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            proc.WaitForExit();
            //var output = proc.StandardOutput.ReadToEnd();
            //Console.WriteLine("stdout: {0}", output);
        }
    }
}
