using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ImageMagick;

namespace CloudMagick_WorkerServer.JSONstuff
{
    class UserCommand
    {
        public Image Image { get; set; }
        public Command cmd { get; set; }

        public void Execute()
        {
            Bitmap bmp = new Bitmap(Image);
            // Read from file.
            using (MagickImage mgkImage = new MagickImage(bmp))
            {
                mgkImage.Blur();
                Image = mgkImage.ToBitmap();
            }
            return;
        }

    }

    public enum Command
    {
        Blur,
        Sharpen
    }




}
