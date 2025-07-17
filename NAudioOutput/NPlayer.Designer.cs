using System.Drawing;

namespace TOAMediaPlayer.NAudioOutput
{
    partial class NPlayer
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.lblPlaySongName = new System.Windows.Forms.Label();
            this.gbPlayer3 = new System.Windows.Forms.GroupBox();
            this.trackBarPosition = new System.Windows.Forms.TrackBar();
            this.BackwardButton = new FontAwesome.Sharp.IconButton();
            this.PlayButton = new FontAwesome.Sharp.IconButton();
            this.StopButton = new FontAwesome.Sharp.IconButton();
            this.ForwardButton = new FontAwesome.Sharp.IconButton();
            this.lblVolumeLevel = new System.Windows.Forms.Label();
            this.IncreaseVolumeButton = new FontAwesome.Sharp.IconButton();
            this.DecreaseVolumeButton = new FontAwesome.Sharp.IconButton();
            this.label1 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.labelTotalTime = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.gbPlayer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPosition)).BeginInit();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.OnTimerTick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(957, 18);
            this.panel1.TabIndex = 140;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.Location = new System.Drawing.Point(1, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "label2";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // lblPlaySongName
            // 
            this.lblPlaySongName.AutoSize = true;
            this.lblPlaySongName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblPlaySongName.Location = new System.Drawing.Point(6, 12);
            this.lblPlaySongName.Name = "lblPlaySongName";
            this.lblPlaySongName.Size = new System.Drawing.Size(0, 13);
            this.lblPlaySongName.TabIndex = 124;
            // 
            // gbPlayer3
            // 
            this.gbPlayer3.Controls.Add(this.lblPlaySongName);
            this.gbPlayer3.Location = new System.Drawing.Point(3, 19);
            this.gbPlayer3.Name = "gbPlayer3";
            this.gbPlayer3.Size = new System.Drawing.Size(531, 41);
            this.gbPlayer3.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            this.gbPlayer3.TabIndex = 128;
            this.gbPlayer3.TabStop = false;
            // 
            // trackBarPosition
            // 
            this.trackBarPosition.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.trackBarPosition.LargeChange = 1;
            this.trackBarPosition.Location = new System.Drawing.Point(3, 65);
            this.trackBarPosition.Margin = new System.Windows.Forms.Padding(2);
            this.trackBarPosition.Maximum = 100;
            this.trackBarPosition.Name = "trackBarPosition";
            this.trackBarPosition.Size = new System.Drawing.Size(531, 45);
            this.trackBarPosition.TabIndex = 129;
            this.trackBarPosition.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trackBarPosition.Scroll += new System.EventHandler(this.trackBarPosition_Scroll);
            this.trackBarPosition.ValueChanged += new System.EventHandler(this.trackBarPosition_ValueChanged);
            // 
            // BackwardButton
            // 
            this.BackwardButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BackwardButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.BackwardButton.IconChar = FontAwesome.Sharp.IconChar.BackwardFast;
            this.BackwardButton.IconColor = System.Drawing.Color.LightGray;
            this.BackwardButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.BackwardButton.IconSize = 27;
            this.BackwardButton.Location = new System.Drawing.Point(540, 24);
            this.BackwardButton.Name = "BackwardButton";
            this.BackwardButton.Size = new System.Drawing.Size(30, 30);
            this.BackwardButton.TabIndex = 130;
            this.BackwardButton.UseVisualStyleBackColor = true;
            this.BackwardButton.Click += new System.EventHandler(this.BackwardButton_Click);
            this.BackwardButton.MouseHover += new System.EventHandler(this.BackwardButton_MouseHover);
            // 
            // PlayButton
            // 
            this.PlayButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PlayButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.PlayButton.IconChar = FontAwesome.Sharp.IconChar.PlayCircle;
            this.PlayButton.IconColor = System.Drawing.Color.LightGray;
            this.PlayButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.PlayButton.IconSize = 27;
            this.PlayButton.Location = new System.Drawing.Point(576, 24);
            this.PlayButton.Name = "PlayButton";
            this.PlayButton.Size = new System.Drawing.Size(30, 30);
            this.PlayButton.TabIndex = 131;
            this.PlayButton.UseVisualStyleBackColor = true;
            this.PlayButton.Click += new System.EventHandler(this.PlayButton_Click);
            this.PlayButton.MouseHover += new System.EventHandler(this.PlayButton_MouseHover);
            // 
            // StopButton
            // 
            this.StopButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StopButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.StopButton.IconChar = FontAwesome.Sharp.IconChar.Stop;
            this.StopButton.IconColor = System.Drawing.Color.LightGray;
            this.StopButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.StopButton.IconSize = 27;
            this.StopButton.Location = new System.Drawing.Point(612, 24);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(30, 30);
            this.StopButton.TabIndex = 132;
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            this.StopButton.MouseHover += new System.EventHandler(this.StopButton_MouseHover);
            // 
            // ForwardButton
            // 
            this.ForwardButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ForwardButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.ForwardButton.IconChar = FontAwesome.Sharp.IconChar.FastForward;
            this.ForwardButton.IconColor = System.Drawing.Color.LightGray;
            this.ForwardButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.ForwardButton.IconSize = 27;
            this.ForwardButton.Location = new System.Drawing.Point(648, 24);
            this.ForwardButton.Name = "ForwardButton";
            this.ForwardButton.Size = new System.Drawing.Size(30, 30);
            this.ForwardButton.TabIndex = 133;
            this.ForwardButton.UseVisualStyleBackColor = true;
            this.ForwardButton.Click += new System.EventHandler(this.ForwardButton_Click);
            this.ForwardButton.MouseHover += new System.EventHandler(this.ForwardButton_MouseHover);
            // 
            // lblVolumeLevel
            // 
            this.lblVolumeLevel.AutoSize = true;
            this.lblVolumeLevel.Font = new System.Drawing.Font("Lucida Bright", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVolumeLevel.ForeColor = System.Drawing.Color.White;
            this.lblVolumeLevel.Location = new System.Drawing.Point(205, 42);
            this.lblVolumeLevel.Name = "lblVolumeLevel";
            this.lblVolumeLevel.Size = new System.Drawing.Size(0, 23);
            this.lblVolumeLevel.TabIndex = 0;
            this.lblVolumeLevel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // IncreaseVolumeButton
            // 
            this.IncreaseVolumeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.IncreaseVolumeButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.IncreaseVolumeButton.IconChar = FontAwesome.Sharp.IconChar.CirclePlus;
            this.IncreaseVolumeButton.IconColor = System.Drawing.Color.White;
            this.IncreaseVolumeButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.IncreaseVolumeButton.IconSize = 45;
            this.IncreaseVolumeButton.Location = new System.Drawing.Point(130, 32);
            this.IncreaseVolumeButton.Name = "IncreaseVolumeButton";
            this.IncreaseVolumeButton.Size = new System.Drawing.Size(46, 44);
            this.IncreaseVolumeButton.TabIndex = 1;
            this.IncreaseVolumeButton.UseVisualStyleBackColor = true;
            this.IncreaseVolumeButton.Click += new System.EventHandler(this.IncreaseVolumeButton_Click);
            this.IncreaseVolumeButton.MouseHover += new System.EventHandler(this.IncreaseVolumeButton_MouseHover);
            // 
            // DecreaseVolumeButton
            // 
            this.DecreaseVolumeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DecreaseVolumeButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.DecreaseVolumeButton.IconChar = FontAwesome.Sharp.IconChar.CircleMinus;
            this.DecreaseVolumeButton.IconColor = System.Drawing.Color.White;
            this.DecreaseVolumeButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.DecreaseVolumeButton.IconSize = 45;
            this.DecreaseVolumeButton.Location = new System.Drawing.Point(80, 32);
            this.DecreaseVolumeButton.Name = "DecreaseVolumeButton";
            this.DecreaseVolumeButton.Size = new System.Drawing.Size(44, 46);
            this.DecreaseVolumeButton.TabIndex = 2;
            this.DecreaseVolumeButton.UseVisualStyleBackColor = true;
            this.DecreaseVolumeButton.Click += new System.EventHandler(this.DecreaseVolumeButton_Click);
            this.DecreaseVolumeButton.MouseHover += new System.EventHandler(this.DecreaseVolumeButton_MouseHover);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Lucida Bright", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(17, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 14);
            this.label1.TabIndex = 3;
            this.label1.Text = "Volume";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.DecreaseVolumeButton);
            this.panel4.Controls.Add(this.lblVolumeLevel);
            this.panel4.Controls.Add(this.IncreaseVolumeButton);
            this.panel4.Location = new System.Drawing.Point(687, 24);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(270, 100);
            this.panel4.TabIndex = 138;
            // 
            // labelTotalTime
            // 
            this.labelTotalTime.AutoSize = true;
            this.labelTotalTime.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTotalTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.labelTotalTime.Location = new System.Drawing.Point(561, 77);
            this.labelTotalTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelTotalTime.Name = "labelTotalTime";
            this.labelTotalTime.Size = new System.Drawing.Size(81, 13);
            this.labelTotalTime.TabIndex = 139;
            this.labelTotalTime.Text = "00:00 / 00:00";
            this.labelTotalTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelTotalTime.Click += new System.EventHandler(this.labelTotalTime_Click);
            // 
            // NPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.labelTotalTime);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.ForwardButton);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.PlayButton);
            this.Controls.Add(this.BackwardButton);
            this.Controls.Add(this.trackBarPosition);
            this.Controls.Add(this.gbPlayer3);
            this.Name = "NPlayer";
            this.Size = new System.Drawing.Size(972, 124);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gbPlayer3.ResumeLayout(false);
            this.gbPlayer3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPosition)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblPlaySongName;
        private System.Windows.Forms.GroupBox gbPlayer3;
        private System.Windows.Forms.TrackBar trackBarPosition;
        private FontAwesome.Sharp.IconButton BackwardButton;
        private FontAwesome.Sharp.IconButton PlayButton;
        private FontAwesome.Sharp.IconButton StopButton;
        private FontAwesome.Sharp.IconButton ForwardButton;
        private System.Windows.Forms.Label lblVolumeLevel;
        private FontAwesome.Sharp.IconButton IncreaseVolumeButton;
        private FontAwesome.Sharp.IconButton DecreaseVolumeButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label labelTotalTime;
    }
}
