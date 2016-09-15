using System;
using System.Windows.Forms;
using CloudMagick_Client_Gui.JSONstuff;

namespace CloudMagick_Client_Gui.GUI
{
    public class CommandButton : Button
    {
        public readonly Command Command;
        private readonly ClientForm _form;

        public CommandButton(Command cmd, ClientForm form)
        {
            _form = form;
            Command = cmd;
            Text = cmd.ToString();
            _form.RegisterCommandButton(this);
        }

        public CommandButton()
        {
            //_form.RegisterCommandButton(this);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            var cmd = new UserCommand {cmd = Command};
            _form.WorkerWsClient.Send(cmd);
        }
    }
}
