using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CloudMagick_Client_Gui.GUI;
using CloudMagick_Client_Gui.JSONstuff;

namespace CloudMagick_Client_Gui
{
   public class CommandButton : Button
    {
        public readonly Command _command;
        public CommandButton(Command cmd)
        {
            _command = cmd;
            this.Text = cmd.ToString();
            Form1.RegisterCommandButton(this);
        }
        public CommandButton()
        {
            Form1.RegisterCommandButton(this);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            UserCommand cmd = new UserCommand {cmd = _command};
            Form1.WorkerWsClient.send(cmd);
        }
    }
}
