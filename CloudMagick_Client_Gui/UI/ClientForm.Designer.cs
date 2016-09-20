using System.ComponentModel;
using System.Windows.Forms;
using CloudMagick_WorkerServer.JSONstuff;

namespace CloudMagick_Client_UI.UI
{
    partial class ClientForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
            _serverSelectionThread.Abort();
            MasterWs.Close();
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.selimage = new System.Windows.Forms.Button();
            this.clearimage = new System.Windows.Forms.Button();
            this.changebgcol = new System.Windows.Forms.Button();
            this.close = new System.Windows.Forms.Button();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioLatency = new System.Windows.Forms.RadioButton();
            this.radioBandwidth = new System.Windows.Forms.RadioButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.radioRandom = new System.Windows.Forms.RadioButton();
            this.commandButton10 = new CloudMagick_Client_UI.UI.CommandButton(Command.IncreaseBrightness, this);
            this.commandButton11 = new CloudMagick_Client_UI.UI.CommandButton(Command.ReduceBrightness, this);
            this.commandButton12 = new CloudMagick_Client_UI.UI.CommandButton(Command.Blur, this);
            this.commandButton13 = new CloudMagick_Client_UI.UI.CommandButton(Command.Sharpen, this);
            this.commandButton14 = new CloudMagick_Client_UI.UI.CommandButton(Command.Border, this);
            this.commandButton15 = new CloudMagick_Client_UI.UI.CommandButton(Command.Emboss, this);
            this.commandButton16 = new CloudMagick_Client_UI.UI.CommandButton(Command.Oilpaint, this);
            this.commandButton17 = new CloudMagick_Client_UI.UI.CommandButton(Command.Sepia, this);
            this.commandButton18 = new CloudMagick_Client_UI.UI.CommandButton(Command.Solarize, this);
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.82114F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 86.17886F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 131F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.checkBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 2, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 91.48418F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.515815F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 154F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(877, 589);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanel1.SetColumnSpan(this.pictureBox1, 2);
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(739, 391);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(21, 407);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(60, 17);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "Stretch";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add(this.selimage);
            this.flowLayoutPanel1.Controls.Add(this.clearimage);
            this.flowLayoutPanel1.Controls.Add(this.changebgcol);
            this.flowLayoutPanel1.Controls.Add(this.close);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(106, 400);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(636, 31);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // selimage
            // 
            this.selimage.AutoSize = true;
            this.selimage.Location = new System.Drawing.Point(3, 3);
            this.selimage.Name = "selimage";
            this.selimage.Size = new System.Drawing.Size(80, 23);
            this.selimage.TabIndex = 0;
            this.selimage.Text = "Show Picture";
            this.selimage.UseVisualStyleBackColor = true;
            this.selimage.Click += new System.EventHandler(this.selimage_Click);
            // 
            // clearimage
            // 
            this.clearimage.AutoSize = true;
            this.clearimage.Location = new System.Drawing.Point(89, 3);
            this.clearimage.Name = "clearimage";
            this.clearimage.Size = new System.Drawing.Size(77, 23);
            this.clearimage.TabIndex = 1;
            this.clearimage.Text = "Clear Picture";
            this.clearimage.UseVisualStyleBackColor = true;
            this.clearimage.Click += new System.EventHandler(this.clearimage_Click);
            // 
            // changebgcol
            // 
            this.changebgcol.AutoSize = true;
            this.changebgcol.Location = new System.Drawing.Point(172, 3);
            this.changebgcol.Name = "changebgcol";
            this.changebgcol.Size = new System.Drawing.Size(75, 23);
            this.changebgcol.TabIndex = 2;
            this.changebgcol.Text = "BG Color";
            this.changebgcol.UseVisualStyleBackColor = true;
            this.changebgcol.Click += new System.EventHandler(this.changebgcol_Click);
            // 
            // close
            // 
            this.close.AutoSize = true;
            this.close.Location = new System.Drawing.Point(253, 3);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 3;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            this.close.Click += new System.EventHandler(this.close_Click);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.commandButton10);
            this.flowLayoutPanel2.Controls.Add(this.commandButton11);
            this.flowLayoutPanel2.Controls.Add(this.commandButton12);
            this.flowLayoutPanel2.Controls.Add(this.commandButton13);
            this.flowLayoutPanel2.Controls.Add(this.commandButton14);
            this.flowLayoutPanel2.Controls.Add(this.commandButton15);
            this.flowLayoutPanel2.Controls.Add(this.commandButton16);
            this.flowLayoutPanel2.Controls.Add(this.commandButton17);
            this.flowLayoutPanel2.Controls.Add(this.commandButton18);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(748, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.tableLayoutPanel1.SetRowSpan(this.flowLayoutPanel2, 2);
            this.flowLayoutPanel2.Size = new System.Drawing.Size(126, 428);
            this.flowLayoutPanel2.TabIndex = 3;
            this.flowLayoutPanel2.WrapContents = false;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanel1.SetColumnSpan(this.panel1, 2);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 437);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(739, 149);
            this.panel1.TabIndex = 4;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(735, 145);
            this.textBox1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioRandom);
            this.groupBox2.Controls.Add(this.radioLatency);
            this.groupBox2.Controls.Add(this.radioBandwidth);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(748, 437);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(126, 149);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Selection Mode";
            // 
            // radioLatency
            // 
            this.radioLatency.AutoSize = true;
            this.radioLatency.Checked = true;
            this.radioLatency.Location = new System.Drawing.Point(6, 19);
            this.radioLatency.Name = "radioLatency";
            this.radioLatency.Size = new System.Drawing.Size(63, 17);
            this.radioLatency.TabIndex = 0;
            this.radioLatency.TabStop = true;
            this.radioLatency.Text = "Latency";
            this.radioLatency.UseVisualStyleBackColor = true;
            this.radioLatency.CheckedChanged += new System.EventHandler(this.radioLatency_CheckedChanged);
            // 
            // radioBandwidth
            // 
            this.radioBandwidth.AutoSize = true;
            this.radioBandwidth.Location = new System.Drawing.Point(6, 42);
            this.radioBandwidth.Name = "radioBandwidth";
            this.radioBandwidth.Size = new System.Drawing.Size(75, 17);
            this.radioBandwidth.TabIndex = 1;
            this.radioBandwidth.TabStop = true;
            this.radioBandwidth.Text = "Bandwidth";
            this.radioBandwidth.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Image Files (*.jpg, *.png, *.bmp)|*.jpg;*.png;*.bmp|All files (*.*)|*.*";
            this.openFileDialog1.Title = "Select an image file";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(877, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.redoToolStripMenuItem.Text = "Redo";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
            // 
            // radioRandom
            // 
            this.radioRandom.AutoSize = true;
            this.radioRandom.Location = new System.Drawing.Point(6, 65);
            this.radioRandom.Name = "radioRandom";
            this.radioRandom.Size = new System.Drawing.Size(65, 17);
            this.radioRandom.TabIndex = 2;
            this.radioRandom.TabStop = true;
            this.radioRandom.Text = "Random";
            this.radioRandom.UseVisualStyleBackColor = true;
            // 
            // commandButton10
            // 
            this.commandButton10.Location = new System.Drawing.Point(3, 3);
            this.commandButton10.Name = "commandButton10";
            this.commandButton10.Size = new System.Drawing.Size(110, 23);
            this.commandButton10.TabIndex = 0;
            this.commandButton10.UseVisualStyleBackColor = true;
            // 
            // commandButton11
            // 
            this.commandButton11.Location = new System.Drawing.Point(3, 32);
            this.commandButton11.Name = "commandButton11";
            this.commandButton11.Size = new System.Drawing.Size(110, 23);
            this.commandButton11.TabIndex = 1;
            this.commandButton11.UseVisualStyleBackColor = true;
            // 
            // commandButton12
            // 
            this.commandButton12.Location = new System.Drawing.Point(3, 61);
            this.commandButton12.Name = "commandButton12";
            this.commandButton12.Size = new System.Drawing.Size(110, 23);
            this.commandButton12.TabIndex = 2;
            this.commandButton12.UseVisualStyleBackColor = true;
            // 
            // commandButton13
            // 
            this.commandButton13.Location = new System.Drawing.Point(3, 90);
            this.commandButton13.Name = "commandButton13";
            this.commandButton13.Size = new System.Drawing.Size(110, 23);
            this.commandButton13.TabIndex = 3;
            this.commandButton13.UseVisualStyleBackColor = true;
            // 
            // commandButton14
            // 
            this.commandButton14.Location = new System.Drawing.Point(3, 119);
            this.commandButton14.Name = "commandButton14";
            this.commandButton14.Size = new System.Drawing.Size(110, 23);
            this.commandButton14.TabIndex = 4;
            this.commandButton14.UseVisualStyleBackColor = true;
            // 
            // commandButton15
            // 
            this.commandButton15.Location = new System.Drawing.Point(3, 148);
            this.commandButton15.Name = "commandButton15";
            this.commandButton15.Size = new System.Drawing.Size(110, 23);
            this.commandButton15.TabIndex = 5;
            this.commandButton15.UseVisualStyleBackColor = true;
            // 
            // commandButton16
            // 
            this.commandButton16.Location = new System.Drawing.Point(3, 177);
            this.commandButton16.Name = "commandButton16";
            this.commandButton16.Size = new System.Drawing.Size(110, 23);
            this.commandButton16.TabIndex = 6;
            this.commandButton16.UseVisualStyleBackColor = true;
            // 
            // commandButton17
            // 
            this.commandButton17.Location = new System.Drawing.Point(3, 206);
            this.commandButton17.Name = "commandButton17";
            this.commandButton17.Size = new System.Drawing.Size(110, 23);
            this.commandButton17.TabIndex = 7;
            this.commandButton17.UseVisualStyleBackColor = true;
            // 
            // commandButton18
            // 
            this.commandButton18.Location = new System.Drawing.Point(3, 235);
            this.commandButton18.Name = "commandButton18";
            this.commandButton18.Size = new System.Drawing.Size(110, 23);
            this.commandButton18.TabIndex = 8;
            this.commandButton18.UseVisualStyleBackColor = true;
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(877, 613);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ClientForm";
            this.Text = "CloudMagick";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private PictureBox pictureBox1;
        private CheckBox checkBox1;
        private FlowLayoutPanel flowLayoutPanel1;
        public Button selimage;
        private Button clearimage;
        private Button changebgcol;
        private Button close;
        private OpenFileDialog openFileDialog1;
        private ColorDialog colorDialog1;
        private FlowLayoutPanel flowLayoutPanel2;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem undoToolStripMenuItem;
        private ToolStripMenuItem redoToolStripMenuItem;
        private Panel panel1;
        private TextBox textBox1;
        private RadioButton radioBandwidth;
        private RadioButton radioLatency;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button6;
        private Button button7;
        private Button button8;
        private Button button9;
        private GroupBox groupBox2;
        private CommandButton commandButton1;
        private CommandButton commandButton2;
        private CommandButton commandButton3;
        private CommandButton commandButton4;
        private CommandButton commandButton5;
        private CommandButton commandButton6;
        private CommandButton commandButton7;
        private CommandButton commandButton8;
        private CommandButton commandButton9;
        private RadioButton radioRandom;
        private CommandButton commandButton10;
        private CommandButton commandButton11;
        private CommandButton commandButton12;
        private CommandButton commandButton13;
        private CommandButton commandButton14;
        private CommandButton commandButton15;
        private CommandButton commandButton16;
        private CommandButton commandButton17;
        private CommandButton commandButton18;
    }
}

