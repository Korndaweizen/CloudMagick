using System.Drawing;

namespace CloudMagick_Client_Gui.JSONstuff
{
    public class UserCommand
    {
        public string Image { get; set; }
        public Command Cmd { get; set; }
        public override string ToString()
        {
            return "COMMAND:" + Newtonsoft.Json.JsonConvert.SerializeObject(this);
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
