using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;

namespace CloudMagick_WorkerServer
{
    public class Utility
    {
        public static string ImageToBase64(Image img)
        {
            if (img != null)
            {
                ImageConverter ic = new ImageConverter();
                byte[] buffer = (byte[])ic.ConvertTo(img, typeof(byte[]));
                return Convert.ToBase64String(
                    buffer,
                    Base64FormattingOptions.InsertLineBreaks);
            }
            else
                return null;
        }
        public static Image ImageFromBase64(string base46)
        {
            byte[] bytes = Convert.FromBase64String(base46);

            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }
            return image;
        }
        public static Image ImageFromByte(byte[] bytes)
        {
            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }
            return image;
        }
        public static string GetLocalIpAddress()
        {
            String strHostName = string.Empty;
            // Getting MasterIpport address of local machine...
            // First get the host name of local machine.
            strHostName = Dns.GetHostName();
            //Console.WriteLine("Local Machine's Host Name: " + strHostName);
            // Then using host name, get the OwnIP address list..
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;

            var localip = addr.Where(o => o.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToArray().First().ToString();
            return localip;
        }


        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
