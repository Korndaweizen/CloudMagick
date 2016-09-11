using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.NetworkInformation;
using CloudMagick_Client_Gui.JSONstuff;
using CloudMagick_Client_Gui.WebSocketClients;
using GraphicsMagick;

namespace CloudMagick_Client_Gui
{
    public partial class Form1 : Form
    {
        public static WorkerWSClient _workerWsClient;
        private Thread _serverSelectionThread;
        public static List<ClientWorker> ActiveWorkers = new List<ClientWorker>();
        private static List<Button> _commandButtons = new List<Button>();
        public static MasterWsClient MasterWs;
        public RedoUndo RedoUndo;


        public Form1(string ipport)
        {
            MasterWs = new MasterWsClient(ipport,this);
            MasterWs.Start();
            InitializeComponent();
            RedoUndo = new RedoUndo(pictureBox1);
            InitDisableCommandBtns();
            _commandButtons.Add(selimage);
            _commandButtons.Add(clearimage);
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
            _serverSelectionThread = new Thread(DoThisAllTheTime);
            _serverSelectionThread.Start();
        }

        public static void RegisterCommandButton(CommandButton btn)
        {
            _commandButtons.Add(btn);
        }

        private void InitDisableCommandBtns()
        {
            foreach (var btn in _commandButtons)
            {
                if (btn.GetType() == typeof(CommandButton))
                {
                    btn.Enabled = false;
                }
            }
        }
        public void DisableCommandBtns()
        {
            MethodInvoker mi = delegate () {
                foreach (var btn in _commandButtons)
                {
                    if (btn.GetType() == typeof(CommandButton))
                    {
                        btn.Enabled = false;
                    }
                }
            };
            this.Invoke(mi);
        }

        public void DisableBtns()
        {
            MethodInvoker mi = delegate () {
                foreach (var btn in _commandButtons)
                {
                    btn.Enabled = false;
                }
            };
            this.Invoke(mi);
        }

        public void EnableBtns()
        {
            MethodInvoker mi = delegate()
            {
                if (_workerWsClient.WebSocket.IsAlive & RedoUndo.GetCurrentImage()!=null)
                {
                    foreach (var btn in _commandButtons)
                    {
                        btn.Enabled = true;
                    }
                }
                
            };
            this.Invoke(mi);

        }

        private void DoThisAllTheTime()
        {
            while (true)
            {
                SelectBestServerPing();
                Thread.Sleep(30000);
            }
        }

        public void SelectBestServerPing()
        {
            Dictionary<long, string> workerDictionary = new Dictionary<long, string>();
            foreach (var worker in ActiveWorkers)
            {
                Ping sender = new Ping();
                PingReply result = sender.Send(worker.IpAddress.Split(':').First());

                if (result.Status == IPStatus.Success)
                {
                    Console.WriteLine("Success, Time Succeeded: " + result.RoundtripTime + "ms");
                    workerDictionary.Add(result.RoundtripTime, worker.IpAddress);
                }

                else
                    Console.WriteLine("Error pinging " + worker.IpAddress);
            }
            if (workerDictionary.Count > 0)
            {
                workerDictionary.OrderBy(pair => pair.Key);
                var workerPair = workerDictionary.First();
                if (_workerWsClient == null)
                {
                    _workerWsClient = new WorkerWSClient(workerPair.Value, this);
                    _workerWsClient.start();
                }
                else
                {
                    if (!_workerWsClient.IPport.Equals(workerPair.Value) || !_workerWsClient.WebSocket.IsAlive)
                    {
                        _workerWsClient.Close();
                        _workerWsClient = new WorkerWSClient(workerPair.Value, this);
                        _workerWsClient.start();
                    }
                }

            }
        }


        private void selimage_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //pictureBox1.Load(openFileDialog1.FileName);
                MagickImage mgkImage=new MagickImage(openFileDialog1.FileName);
                RedoUndo.AddImage(mgkImage);
                _workerWsClient.send(new UserCommand{cmd=Command.None,Image = mgkImage.ToBase64()});
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
