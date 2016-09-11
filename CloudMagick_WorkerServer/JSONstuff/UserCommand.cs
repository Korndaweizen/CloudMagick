using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using GraphicsMagick;

namespace CloudMagick_WorkerServer.JSONstuff
{
    class UserCommand
    {
        public string Image { get; set; }
        public Command cmd { get; set; }

        public void Execute(MagickImage mgkImage)
        {
            var stopwatch = Stopwatch.StartNew();
            switch (cmd)
            {
                case Command.Blur:
                    mgkImage.Blur();
                    break;
                case Command.Sharpen:
                    mgkImage.Sharpen();
                    break;
                case Command.Charcoal:
                    mgkImage.Charcoal();
                    break;
                case Command.Sketch:
                    mgkImage.Solarize();
                    break;
                case Command.Oilpaint:
                    mgkImage.OilPaint();
                    break;
                case Command.Negate:
                    mgkImage.Negate();
                    break;
                case Command.Sepia:
                    mgkImage.Border(5);
                    break;
            }
            var time = stopwatch.ElapsedMilliseconds;
            Console.WriteLine(@"Execution of command {0} took {1}ms", cmd, time);
            //Image = mgkImage.ToBase64();
            //Console.WriteLine(@"Conversion to base64 took {0}ms", stopwatch.ElapsedMilliseconds-time);
            stopwatch.Stop();
        }

    }

    public enum Command
    {
        Blur,
        Sharpen,
        Charcoal,
        Sketch,
        Oilpaint,
        Negate,
        Sepia,
        None
    }




}
