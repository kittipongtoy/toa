namespace TOAMediaPlayer
{
    partial class PlayerControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayerControl));
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRandom = new System.Windows.Forms.Button();
            this.btnLoop = new System.Windows.Forms.Button();
            this.btnItemListRemove = new System.Windows.Forms.Button();
            this.btnFolderBrowser = new System.Windows.Forms.Button();
            this.forwardButton = new FontAwesome.Sharp.IconButton();
            this.rewardButton = new FontAwesome.Sharp.IconButton();
            this.stopButton = new FontAwesome.Sharp.IconButton();
            this.playButton = new FontAwesome.Sharp.IconButton();
            this.lblStateChangeMsg = new System.Windows.Forms.Label();
            this.gbPlay = new System.Windows.Forms.GroupBox();
            this.labelSongName = new System.Windows.Forms.Label();
            this.btnFileBrowser = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.increaseVolumeButton = new FontAwesome.Sharp.IconButton();
            this.decreaseVolumeButton = new FontAwesome.Sharp.IconButton();
            this.labelCurrentVolumeLevel = new System.Windows.Forms.Label();
            this.labelVolume = new System.Windows.Forms.Label();
            this.labelPlayTime = new System.Windows.Forms.Label();
            this.labelDurationTime = new System.Windows.Forms.Label();
            this.trackBarSongPlay = new System.Windows.Forms.TrackBar();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.panel1.SuspendLayout();
            this.gbPlay.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSongPlay)).BeginInit();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.panel1.Controls.Add(this.btnRandom);
            this.panel1.Controls.Add(this.btnLoop);
            this.panel1.Controls.Add(this.btnItemListRemove);
            this.panel1.Controls.Add(this.btnFolderBrowser);
            this.panel1.Controls.Add(this.forwardButton);
            this.panel1.Controls.Add(this.rewardButton);
            this.panel1.Controls.Add(this.stopButton);
            this.panel1.Controls.Add(this.playButton);
            this.panel1.Controls.Add(this.lblStateChangeMsg);
            this.panel1.Controls.Add(this.gbPlay);
            this.panel1.Controls.Add(this.btnFileBrowser);
            this.panel1.Controls.Add(this.listBox1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.labelPlayTime);
            this.panel1.Controls.Add(this.labelDurationTime);
            this.panel1.Controls.Add(this.trackBarSongPlay);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(688, 90);
            this.panel1.TabIndex = 1;
            // 
            // btnRandom
            // 
            this.btnRandom.Location = new System.Drawing.Point(448, 62);
            this.btnRandom.Name = "btnRandom";
            this.btnRandom.Size = new System.Drawing.Size(15, 23);
            this.btnRandom.TabIndex = 139;
            this.btnRandom.Tag = "Random";
            this.btnRandom.Text = "R";
            this.btnRandom.UseVisualStyleBackColor = true;
            this.btnRandom.Click += new System.EventHandler(this.btnRandom_Click);
            // 
            // btnLoop
            // 
            this.btnLoop.Location = new System.Drawing.Point(426, 62);
            this.btnLoop.Name = "btnLoop";
            this.btnLoop.Size = new System.Drawing.Size(16, 23);
            this.btnLoop.TabIndex = 138;
            this.btnLoop.Tag = "Loop";
            this.btnLoop.Text = "L";
            this.btnLoop.UseVisualStyleBackColor = true;
            this.btnLoop.Click += new System.EventHandler(this.btnLoop_Click);
            // 
            // btnItemListRemove
            // 
            this.btnItemListRemove.Location = new System.Drawing.Point(404, 62);
            this.btnItemListRemove.Name = "btnItemListRemove";
            this.btnItemListRemove.Size = new System.Drawing.Size(15, 23);
            this.btnItemListRemove.TabIndex = 137;
            this.btnItemListRemove.Tag = "Remove";
            this.btnItemListRemove.Text = "-";
            this.btnItemListRemove.UseVisualStyleBackColor = true;
            this.btnItemListRemove.Click += new System.EventHandler(this.btnItemListRemove_Click);
            // 
            // btnFolderBrowser
            // 
            this.btnFolderBrowser.Location = new System.Drawing.Point(325, 62);
            this.btnFolderBrowser.Name = "btnFolderBrowser";
            this.btnFolderBrowser.Size = new System.Drawing.Size(30, 23);
            this.btnFolderBrowser.TabIndex = 136;
            this.btnFolderBrowser.Tag = "Browse Folder";
            this.btnFolderBrowser.Text = "Dir";
            this.btnFolderBrowser.UseVisualStyleBackColor = true;
            this.btnFolderBrowser.Click += new System.EventHandler(this.btnFolderBrowser_Click);
            // 
            // forwardButton
            // 
            this.forwardButton.IconChar = FontAwesome.Sharp.IconChar.FastForward;
            this.forwardButton.IconColor = System.Drawing.Color.Black;
            this.forwardButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.forwardButton.IconSize = 24;
            this.forwardButton.Location = new System.Drawing.Point(433, 6);
            this.forwardButton.Name = "forwardButton";
            this.forwardButton.Size = new System.Drawing.Size(30, 30);
            this.forwardButton.TabIndex = 135;
            this.forwardButton.UseVisualStyleBackColor = true;
            this.forwardButton.Click += new System.EventHandler(this.forwardButton_Click);
            // 
            // rewardButton
            // 
            this.rewardButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.rewardButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rewardButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.rewardButton.IconChar = FontAwesome.Sharp.IconChar.FastBackward;
            this.rewardButton.IconColor = System.Drawing.Color.Black;
            this.rewardButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.rewardButton.IconSize = 24;
            this.rewardButton.Location = new System.Drawing.Point(325, 6);
            this.rewardButton.Name = "rewardButton";
            this.rewardButton.Size = new System.Drawing.Size(30, 30);
            this.rewardButton.TabIndex = 134;
            this.rewardButton.UseVisualStyleBackColor = false;
            this.rewardButton.Click += new System.EventHandler(this.rewardButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.AutoSize = true;
            this.stopButton.IconChar = FontAwesome.Sharp.IconChar.Stop;
            this.stopButton.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.stopButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.stopButton.IconSize = 24;
            this.stopButton.Location = new System.Drawing.Point(397, 6);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(30, 30);
            this.stopButton.TabIndex = 133;
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // playButton
            // 
            this.playButton.AutoSize = true;
            this.playButton.IconChar = FontAwesome.Sharp.IconChar.Play;
            this.playButton.IconColor = System.Drawing.Color.Black;
            this.playButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.playButton.IconSize = 24;
            this.playButton.Location = new System.Drawing.Point(361, 6);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(30, 30);
            this.playButton.TabIndex = 132;
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // lblStateChangeMsg
            // 
            this.lblStateChangeMsg.AutoSize = true;
            this.lblStateChangeMsg.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblStateChangeMsg.Location = new System.Drawing.Point(600, 72);
            this.lblStateChangeMsg.Name = "lblStateChangeMsg";
            this.lblStateChangeMsg.Size = new System.Drawing.Size(47, 13);
            this.lblStateChangeMsg.TabIndex = 131;
            this.lblStateChangeMsg.Text = "Ready...";
            // 
            // gbPlay
            // 
            this.gbPlay.Controls.Add(this.labelSongName);
            this.gbPlay.Location = new System.Drawing.Point(3, 3);
            this.gbPlay.Name = "gbPlay";
            this.gbPlay.Size = new System.Drawing.Size(316, 32);
            this.gbPlay.TabIndex = 126;
            this.gbPlay.TabStop = false;
            // 
            // labelSongName
            // 
            this.labelSongName.AutoSize = true;
            this.labelSongName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.labelSongName.Location = new System.Drawing.Point(6, 12);
            this.labelSongName.Name = "labelSongName";
            this.labelSongName.Size = new System.Drawing.Size(63, 13);
            this.labelSongName.TabIndex = 124;
            this.labelSongName.Text = "Song Name";
            // 
            // btnFileBrowser
            // 
            this.btnFileBrowser.Location = new System.Drawing.Point(361, 62);
            this.btnFileBrowser.Name = "btnFileBrowser";
            this.btnFileBrowser.Size = new System.Drawing.Size(37, 23);
            this.btnFileBrowser.TabIndex = 125;
            this.btnFileBrowser.Tag = "Add More File";
            this.btnFileBrowser.Text = "File+";
            this.btnFileBrowser.UseVisualStyleBackColor = true;
            this.btnFileBrowser.Click += new System.EventHandler(this.btnFileBrowser_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(469, 48);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(125, 30);
            this.listBox1.TabIndex = 123;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this.listBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDoubleClick);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.DimGray;
            this.panel2.Controls.Add(this.increaseVolumeButton);
            this.panel2.Controls.Add(this.decreaseVolumeButton);
            this.panel2.Controls.Add(this.labelCurrentVolumeLevel);
            this.panel2.Controls.Add(this.labelVolume);
            this.panel2.Location = new System.Drawing.Point(469, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(218, 46);
            this.panel2.TabIndex = 115;
            // 
            // increaseVolumeButton
            // 
            this.increaseVolumeButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("increaseVolumeButton.BackgroundImage")));
            this.increaseVolumeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.increaseVolumeButton.IconChar = FontAwesome.Sharp.IconChar.None;
            this.increaseVolumeButton.IconColor = System.Drawing.Color.Black;
            this.increaseVolumeButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.increaseVolumeButton.Location = new System.Drawing.Point(120, 9);
            this.increaseVolumeButton.Margin = new System.Windows.Forms.Padding(2);
            this.increaseVolumeButton.Name = "increaseVolumeButton";
            this.increaseVolumeButton.Size = new System.Drawing.Size(30, 29);
            this.increaseVolumeButton.TabIndex = 68;
            this.increaseVolumeButton.UseVisualStyleBackColor = true;
            this.increaseVolumeButton.Click += new System.EventHandler(this.increaseVolumeButton_Click);
            this.increaseVolumeButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.increaseVolumeButton_MouseDown);
            // 
            // decreaseVolumeButton
            // 
            this.decreaseVolumeButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("decreaseVolumeButton.BackgroundImage")));
            this.decreaseVolumeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.decreaseVolumeButton.IconChar = FontAwesome.Sharp.IconChar.None;
            this.decreaseVolumeButton.IconColor = System.Drawing.Color.Black;
            this.decreaseVolumeButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.decreaseVolumeButton.Location = new System.Drawing.Point(86, 8);
            this.decreaseVolumeButton.Margin = new System.Windows.Forms.Padding(2);
            this.decreaseVolumeButton.Name = "decreaseVolumeButton";
            this.decreaseVolumeButton.Size = new System.Drawing.Size(30, 29);
            this.decreaseVolumeButton.TabIndex = 67;
            this.decreaseVolumeButton.UseVisualStyleBackColor = true;
            this.decreaseVolumeButton.Click += new System.EventHandler(this.decreaseVolumeButton_Click);
            this.decreaseVolumeButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.decreaseVolumeButton_MouseDown);
            // 
            // labelCurrentVolumeLevel
            // 
            this.labelCurrentVolumeLevel.AutoSize = true;
            this.labelCurrentVolumeLevel.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCurrentVolumeLevel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.labelCurrentVolumeLevel.Location = new System.Drawing.Point(164, 16);
            this.labelCurrentVolumeLevel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelCurrentVolumeLevel.Name = "labelCurrentVolumeLevel";
            this.labelCurrentVolumeLevel.Size = new System.Drawing.Size(48, 19);
            this.labelCurrentVolumeLevel.TabIndex = 66;
            this.labelCurrentVolumeLevel.Text = "50%";
            // 
            // labelVolume
            // 
            this.labelVolume.AutoSize = true;
            this.labelVolume.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVolume.ForeColor = System.Drawing.Color.White;
            this.labelVolume.Location = new System.Drawing.Point(2, 18);
            this.labelVolume.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelVolume.Name = "labelVolume";
            this.labelVolume.Size = new System.Drawing.Size(63, 18);
            this.labelVolume.TabIndex = 63;
            this.labelVolume.Text = "Volume";
            // 
            // labelPlayTime
            // 
            this.labelPlayTime.AutoSize = true;
            this.labelPlayTime.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlayTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.labelPlayTime.Location = new System.Drawing.Point(358, 43);
            this.labelPlayTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelPlayTime.Name = "labelPlayTime";
            this.labelPlayTime.Size = new System.Drawing.Size(38, 13);
            this.labelPlayTime.TabIndex = 114;
            this.labelPlayTime.Text = "00:00";
            // 
            // labelDurationTime
            // 
            this.labelDurationTime.AutoSize = true;
            this.labelDurationTime.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDurationTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.labelDurationTime.Location = new System.Drawing.Point(408, 43);
            this.labelDurationTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelDurationTime.Name = "labelDurationTime";
            this.labelDurationTime.Size = new System.Drawing.Size(38, 13);
            this.labelDurationTime.TabIndex = 69;
            this.labelDurationTime.Text = "00:00";
            // 
            // trackBarSongPlay
            // 
            this.trackBarSongPlay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.trackBarSongPlay.LargeChange = 1;
            this.trackBarSongPlay.Location = new System.Drawing.Point(3, 40);
            this.trackBarSongPlay.Margin = new System.Windows.Forms.Padding(2);
            this.trackBarSongPlay.Maximum = 100;
            this.trackBarSongPlay.Name = "trackBarSongPlay";
            this.trackBarSongPlay.Size = new System.Drawing.Size(316, 45);
            this.trackBarSongPlay.TabIndex = 68;
            this.trackBarSongPlay.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trackBarSongPlay.KeyDown += new System.Windows.Forms.KeyEventHandler(this.trackBarSongPlay_KeyDown);
            this.trackBarSongPlay.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trackBarSongPlay_MouseDown);
            this.trackBarSongPlay.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackBarSongPlay_MouseUp);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // PlayerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "PlayerControl";
            this.Size = new System.Drawing.Size(688, 90);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gbPlay.ResumeLayout(false);
            this.gbPlay.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSongPlay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox gbPlay;
        private System.Windows.Forms.Label labelSongName;
        private System.Windows.Forms.Button btnFileBrowser;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelCurrentVolumeLevel;
        private System.Windows.Forms.Label labelVolume;
        private System.Windows.Forms.Label labelPlayTime;
        private System.Windows.Forms.Label labelDurationTime;
        private System.Windows.Forms.TrackBar trackBarSongPlay;
        private FontAwesome.Sharp.IconButton increaseVolumeButton;
        private FontAwesome.Sharp.IconButton decreaseVolumeButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label lblStateChangeMsg;
        private FontAwesome.Sharp.IconButton playButton;
        private FontAwesome.Sharp.IconButton stopButton;
        private FontAwesome.Sharp.IconButton forwardButton;
        private FontAwesome.Sharp.IconButton rewardButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Button btnFolderBrowser;
        private System.Windows.Forms.Button btnItemListRemove;
        private System.Windows.Forms.Button btnRandom;
        private System.Windows.Forms.Button btnLoop;
    }
}
