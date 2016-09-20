using System;
using System.Diagnostics;
using Anotar.Log4Net;

namespace CloudMagick_WorkerServer.JSONstuff
{
    public class UserCommand
    {
        public string Image { get; set; }
        public Command Cmd { get; set; }

        public override string ToString()
        {
            return "COMMAND:" + Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public int Execute(string path)
        {
            //CommandlineExec("");
            string shellCommand="";
            var stopwatch = Stopwatch.StartNew();
            switch (Cmd)
            {
                case Command.ReduceBrightness:
                    shellCommand = "-modulate 90";
                    break;
                case Command.IncreaseBrightness:
                    shellCommand = "-modulate 110";
                    break;
                case Command.Blur:
                    shellCommand = "-blur 0x4";
                    break;
                case Command.Sharpen:
                    shellCommand = "-sharpen 0x4";
                    break;
                //case Command.Charcoal:
                //    shellCommand = "-charcoal 3";
                //   break;
                case Command.Emboss:
                    shellCommand = "-emboss 0x.5";
                    break;
                case Command.Oilpaint:
                    shellCommand = "-paint 3";
                    break;
                case Command.Border:
                    shellCommand = "-border 10";
                    break;
                //case Command.Negate:
                //    shellCommand = "-negate";
                //    break;
                case Command.Sepia:
                    shellCommand = "-sepia-tone 60%";
                    break;
                case Command.Solarize:
                    shellCommand = "-solarize 50%";
                    break;
            }
            CommandlineExec(shellCommand, path);
            var time = unchecked((int)stopwatch.ElapsedMilliseconds);
            LogTo.Info(@"Execution of command {0} took {1}ms", Cmd, time);
            //Image = mgkImage.ToBase64();
            //Console.WriteLine(@"Conversion to base64 took {0}ms", stopwatch.ElapsedMilliseconds-time);
            stopwatch.Stop();
            return time;
        }

        public static void CommandlineExec(string cmd, string path)
        {
            Process proc = new Process();
            if (Environment.OSVersion.ToString().Contains("Windows"))
            {
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.Arguments = "/C mogrify " + cmd + " " + path;
            }
            else
            {
                proc.StartInfo.FileName = "mogrify";
                proc.StartInfo.Arguments = cmd + " " + path;
            }
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            proc.WaitForExit();
            //var output = proc.StandardOutput.ReadToEnd();
            //Console.WriteLine("stdout: {0}", output);
        }

    }

    public enum Command
    {
        ReduceBrightness,
        IncreaseBrightness,
        Blur,
        Sharpen,
        Emboss,
        Oilpaint,
        Border,
        Sepia,
        Solarize,
        None
    }




}
