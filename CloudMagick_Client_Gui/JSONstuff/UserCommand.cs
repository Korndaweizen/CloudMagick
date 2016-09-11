using System.Drawing;

namespace CloudMagick_Client_Gui.JSONstuff
{
    public class UserCommand
    {
        public string Image { get; set; }
        public Command cmd { get; set; }
        public override string ToString()
        {
            return "COMMAND:" + Newtonsoft.Json.JsonConvert.SerializeObject(this);
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
