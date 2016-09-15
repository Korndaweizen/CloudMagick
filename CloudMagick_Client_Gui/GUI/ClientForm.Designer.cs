using System.ComponentModel;
using System.Windows.Forms;
using CloudMagick_Client_Gui.JSONstuff;

namespace CloudMagick_Client_Gui.GUI
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
            this.commandButton1 = new CloudMagick_Client_Gui.GUI.CommandButton(Command.ReduceBrightness, this);
            this.commandButton2 = new CloudMagick_Client_Gui.GUI.CommandButton(Command.IncreaseBrightness, this);
            this.commandButton3 = new CloudMagick_Client_Gui.GUI.CommandButton(Command.Blur, this);
            this.commandButton4 = new CloudMagick_Client_Gui.GUI.CommandButton(Command.Sharpen, this);
            this.commandButton5 = new CloudMagick_Client_Gui.GUI.CommandButton(Command.Border, this);
            this.commandButton6 = new CloudMagick_Client_Gui.GUI.CommandButton(Command.Emboss, this);
            this.commandButton7 = new CloudMagick_Client_Gui.GUI.CommandButton(Command.Oilpaint, this);
            this.commandButton8 = new CloudMagick_Client_Gui.GUI.CommandButton(Command.Sepia, this);
            this.commandButton9 = new CloudMagick_Client_Gui.GUI.CommandButton(Command.Solarize, this);
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
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
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
            this.pictureBox1.Size = new System.Drawing.Size(740, 391);
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
            this.flowLayoutPanel1.Size = new System.Drawing.Size(637, 31);
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
            this.flowLayoutPanel2.Controls.Add(this.commandButton1);
            this.flowLayoutPanel2.Controls.Add(this.commandButton2);
            this.flowLayoutPanel2.Controls.Add(this.commandButton3);
            this.flowLayoutPanel2.Controls.Add(this.commandButton4);
            this.flowLayoutPanel2.Controls.Add(this.commandButton5);
            this.flowLayoutPanel2.Controls.Add(this.commandButton6);
            this.flowLayoutPanel2.Controls.Add(this.commandButton7);
            this.flowLayoutPanel2.Controls.Add(this.commandButton8);
            this.flowLayoutPanel2.Controls.Add(this.commandButton9);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(749, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.tableLayoutPanel1.SetRowSpan(this.flowLayoutPanel2, 2);
            this.flowLayoutPanel2.Size = new System.Drawing.Size(125, 428);
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
            this.panel1.Size = new System.Drawing.Size(740, 149);
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
            this.textBox1.Size = new System.Drawing.Size(736, 145);
            this.textBox1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioLatency);
            this.groupBox2.Controls.Add(this.radioBandwidth);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(749, 437);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(125, 149);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Selection Mode";
            // 
            // radioLatency
            // 
            this.radioLatency.AutoSize = true;
            this.radioLatency.Location = new System.Drawing.Point(6, 19);
            this.radioLatency.Name = "radioLatency";
            this.radioLatency.Size = new System.Drawing.Size(63, 17);
            this.radioLatency.TabIndex = 0;
            this.radioLatency.TabStop = true;
            this.radioLatency.Text = "Latency";
            this.radioLatency.UseVisualStyleBackColor = true;
            this.radioLatency.CheckedChanged += new System.EventHandler(this.radioLatency_CheckedChanged);
            this.radioLatency.Checked = true;
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
            // commandButton1
            // 
            this.commandButton1.Location = new System.Drawing.Point(3, 3);
            this.commandButton1.Name = "commandButton1";
            this.commandButton1.Size = new System.Drawing.Size(113, 23);
            this.commandButton1.TabIndex = 0;
            this.commandButton1.UseVisualStyleBackColor = true;
            // 
            // commandButton2
            // 
            this.commandButton2.Location = new System.Drawing.Point(3, 32);
            this.commandButton2.Name = "commandButton2";
            this.commandButton2.Size = new System.Drawing.Size(113, 23);
            this.commandButton2.TabIndex = 1;
            this.commandButton2.UseVisualStyleBackColor = true;
            // 
            // commandButton3
            // 
            this.commandButton3.Location = new System.Drawing.Point(3, 61);
            this.commandButton3.Name = "commandButton3";
            this.commandButton3.Size = new System.Drawing.Size(113, 23);
            this.commandButton3.TabIndex = 2;
            this.commandButton3.UseVisualStyleBackColor = true;
            // 
            // commandButton4
            // 
            this.commandButton4.Location = new System.Drawing.Point(3, 90);
            this.commandButton4.Name = "commandButton4";
            this.commandButton4.Size = new System.Drawing.Size(113, 23);
            this.commandButton4.TabIndex = 3;
            
            this.commandButton4.UseVisualStyleBackColor = true;
            // 
            // commandButton5
            // 
            this.commandButton5.Location = new System.Drawing.Point(3, 119);
            this.commandButton5.Name = "commandButton5";
            this.commandButton5.Size = new System.Drawing.Size(113, 23);
            this.commandButton5.TabIndex = 4;
            
            this.commandButton5.UseVisualStyleBackColor = true;
            // 
            // commandButton6
            // 
            this.commandButton6.Location = new System.Drawing.Point(3, 148);
            this.commandButton6.Name = "commandButton6";
            this.commandButton6.Size = new System.Drawing.Size(113, 23);
            this.commandButton6.TabIndex = 5;
          
            this.commandButton6.UseVisualStyleBackColor = true;
            // 
            // commandButton7
            // 
            this.commandButton7.Location = new System.Drawing.Point(3, 177);
            this.commandButton7.Name = "commandButton7";
            this.commandButton7.Size = new System.Drawing.Size(113, 23);
            this.commandButton7.TabIndex = 6;
          
            this.commandButton7.UseVisualStyleBackColor = true;
            // 
            // commandButton8
            // 
            this.commandButton8.Location = new System.Drawing.Point(3, 206);
            this.commandButton8.Name = "commandButton8";
            this.commandButton8.Size = new System.Drawing.Size(113, 23);
            this.commandButton8.TabIndex = 7;
     
            this.commandButton8.UseVisualStyleBackColor = true;
            // 
            // commandButton9
            // 
            this.commandButton9.Location = new System.Drawing.Point(3, 235);
            this.commandButton9.Name = "commandButton9";
            this.commandButton9.Size = new System.Drawing.Size(113, 23);
            this.commandButton9.TabIndex = 8;
            this.commandButton9.UseVisualStyleBackColor = true;
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
    }
}

