using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using CloudMagick_Client_UI.ServerSelection;
using CloudMagick_Client_UI.WebSocketClients;
using CloudMagick_WorkerServer.JSONstuff;

namespace CloudMagick_Client_UI.UI
{
    public partial class ClientForm : Form,IUserClient
    {
        private Thread _serverSelectionThread;
        private static readonly List<Button> CommandButtons = new List<Button>();
        public RedoUndo RedoUndo;
        public ServerSelector ServerSelector { get; set; }
        public bool ServerMayChange { get; set; }= true;
        public string Mode { get; set; } = "Latency";
        public MasterWsClient MasterWs { get; set; }

        public List<ClientWorker> ActiveWorkers { get; set; } = new List<ClientWorker>();

        public IWorkerWebSocketClient WorkerWsClient { get; set; }
        public List<Command> FunctionList { get; set; } = new List<Command>();


        public ClientForm(string ipport)
        {
            ServerSelector = new ServerSelector(this);

            MasterWs = new MasterWsClient(ipport,this);
            MasterWs.Start();
            InitializeComponent();
            RedoUndo = new RedoUndo(pictureBox1);
            InitDisableCommands();
            CommandButtons.Add(selimage);
            CommandButtons.Add(clearimage);
            
        }

        //
        /// <summary>
        /// Starting the background thread, which is responsible for selectin an appropriate content server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            //create and start a new thread in the load event.
            //passing it a method to be run on the new thread.
            _serverSelectionThread = new Thread(ServerSelector.DoThisAllTheTime);
            _serverSelectionThread.Start();
        }

        public void RegisterCommandButton(CommandButton btn)
        {
            CommandButtons.Add(btn);
        }

        public void InitDisableCommands()
        {
            foreach (var btn in CommandButtons)
            {
                if (btn.GetType() == typeof(CommandButton))
                {
                    btn.Enabled = false;
                }
            }
        }



        public void DisableCommands()
        {
            MethodInvoker mi = delegate()
            {
                foreach (var btn in CommandButtons)
                {
                    if (btn.GetType() == typeof(CommandButton))
                    {
                        btn.Enabled = false;
                    }
                }
            };
            this.Invoke(mi);
        }

        public void DisableSending(bool servermaychange)
        {
            MethodInvoker mi = delegate () {
                foreach (var btn in CommandButtons)
                {
                    btn.Enabled = false;
                }
            };
            if (!servermaychange)
            {
                ServerMayChange = false;
            }
            this.Invoke(mi);
        }


        public void EnableSending()
        {
            MethodInvoker mi = delegate()
            {
                if (WorkerWsClient.WebSocket.IsAlive & RedoUndo.GetCurrentImage()!=null)
                {
                    CommandButtons.Where(
                        cmd =>
                            cmd.GetType() != typeof(CommandButton) ||
                            (cmd.GetType() == typeof(CommandButton) &&
                             FunctionList.Contains(((CommandButton) cmd).Command))).ToList().ForEach(btn=>btn.Enabled=true);
                    //foreach (var btn in CommandButtons)
                    //{
                    //    btn.Enabled = true;
                    //}
                }
            };
            ServerMayChange = true;
            this.Invoke(mi);

        }




        private void selimage_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //pictureBox1.Load(openFileDialog1.FileName);
                Image mgkImage=Image.FromFile(openFileDialog1.FileName);
                RedoUndo.AddImage(mgkImage);
                WorkerWsClient.Send(new UserCommand{Cmd=Command.None,Image = Utility.ImageToBase64(mgkImage)});
            }
            //this.Cursor = Cursors.WaitCursor;
        }

        private void clearimage_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            RedoUndo.AddImage(null);
            DisableCommands();
            //this.Cursor = Cursors.WaitCursor;

        }

        private void changebgcol_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.BackColor = colorDialog1.Color;
            }
        }

        private void close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = checkBox1.Checked ? PictureBoxSizeMode.StretchImage : PictureBoxSizeMode.Zoom;
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RedoUndo.Undo();
            if (pictureBox1.Image == null)
            {
                DisableCommands();
            }
            else
            {
                EnableSending();
            }
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RedoUndo.Redo();
            if (pictureBox1.Image == null)
            {
                DisableCommands();
            }
            else
            {
                EnableSending();
            }
        }

        private void radioLatency_CheckedChanged(object sender, EventArgs e)
        { 
            foreach (Control control in this.groupBox2.Controls)
            {
                if (control is RadioButton)
                {
                    RadioButton radio = control as RadioButton;
                    if (radio.Checked)
                    {
                        Mode = radio.Text;
                    }
                }
            }
            this.Text = "CloudMagick - " + Mode;
        }
    }
}
