using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using CloudMagick_Client_Gui.JSONstuff;
using CloudMagick_Client_Gui.ServerSelection;
using CloudMagick_Client_Gui.WebSocketClients;

namespace CloudMagick_Client_Gui.GUI
{
    public partial class Form1 : Form
    {
        public static WorkerWsClient WorkerWsClient;
        private Thread _serverSelectionThread;
        public static List<ClientWorker> ActiveWorkers = new List<ClientWorker>();
        private static readonly List<Button> CommandButtons = new List<Button>();
        public static MasterWsClient MasterWs;
        public RedoUndo RedoUndo;
        public readonly ServerSelector ServerSelector;
        public bool Servermaychange = true;
        public List<Command> FunctionList=new List<Command>();


        public Form1(string ipport)
        {
            MasterWs = new MasterWsClient(ipport,this);
            MasterWs.Start();
            InitializeComponent();
            RedoUndo = new RedoUndo(pictureBox1);
            InitDisableCommandBtns();
            CommandButtons.Add(selimage);
            CommandButtons.Add(clearimage);
            ServerSelector = new ServerSelector(this);
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

        public static void RegisterCommandButton(CommandButton btn)
        {
            CommandButtons.Add(btn);
        }

        private void InitDisableCommandBtns()
        {
            foreach (var btn in CommandButtons)
            {
                if (btn.GetType() == typeof(CommandButton))
                {
                    btn.Enabled = false;
                }
            }
        }
        public void DisableCommandBtns()
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

        public void DisableBtns(bool servermaychange)
        {
            MethodInvoker mi = delegate () {
                foreach (var btn in CommandButtons)
                {
                    btn.Enabled = false;
                }
            };
            if (!servermaychange)
            {
                Servermaychange = false;
            }
            this.Invoke(mi);
        }


        public void EnableBtns()
        {
            MethodInvoker mi = delegate()
            {
                if (WorkerWsClient.WebSocket.IsAlive & RedoUndo.GetCurrentImage()!=null)
                {
                    CommandButtons.Where(
                        cmd =>
                            cmd.GetType() != typeof(CommandButton) ||
                            (cmd.GetType() == typeof(CommandButton) &&
                             FunctionList.Contains(((CommandButton) cmd)._command))).ToList().ForEach(btn=>btn.Enabled=true);
                    //foreach (var btn in CommandButtons)
                    //{
                    //    btn.Enabled = true;
                    //}
                }
            };
            Servermaychange = true;
            this.Invoke(mi);

        }




        private void selimage_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //pictureBox1.Load(openFileDialog1.FileName);
                Image mgkImage=Image.FromFile(openFileDialog1.FileName);
                RedoUndo.AddImage(mgkImage);
                WorkerWsClient.send(new UserCommand{cmd=Command.None,Image = Utility.ImageToBase64(mgkImage)});
            }
            //this.Cursor = Cursors.WaitCursor;
        }

        private void clearimage_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            RedoUndo.AddImage(null);
            DisableCommandBtns();
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
            if (checkBox1.Checked)
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            else
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RedoUndo.Undo();
            if (pictureBox1.Image == null)
            {
                DisableCommandBtns();
            }
            else
            {
                EnableBtns();
            }
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RedoUndo.Redo();
            if (pictureBox1.Image == null)
            {
                DisableCommandBtns();
            }
            else
            {
                EnableBtns();
            }
        }
    }
}
