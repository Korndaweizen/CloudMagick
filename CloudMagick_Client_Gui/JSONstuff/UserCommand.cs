using System.Drawing;

namespace CloudMagick_Client_Gui.JSONstuff
{
    public class UserCommand
    {
        public Image Image { get; set; }
        public Command cmd { get; set; }
        public override string ToString()
        {
            return "COMMAND:" + Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }

    public enum Command
    {
        Blur,
        Sharpen
    }

    

}
