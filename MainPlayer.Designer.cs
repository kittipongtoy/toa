using System;
using System.Windows.Forms;

namespace TOAMediaPlayer
{
    partial class MainPlayer
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainPlayer));
            TOAMediaPlayer.TOAPlaylist.OTSMedia otsMedia1 = new TOAMediaPlayer.TOAPlaylist.OTSMedia();
            this.panel1 = new System.Windows.Forms.Panel();
            this.iconButton2 = new FontAwesome.Sharp.IconButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.iconButton1 = new FontAwesome.Sharp.IconButton();
            this.iconPictureBox1 = new FontAwesome.Sharp.IconPictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.iconWindowClose = new FontAwesome.Sharp.IconPictureBox();
            this.iconWindowMinimize = new FontAwesome.Sharp.IconPictureBox();
            this.iconWindowMaximize = new FontAwesome.Sharp.IconPictureBox();
            this.iconLogo = new FontAwesome.Sharp.IconButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.iconButtonPlayer8 = new FontAwesome.Sharp.IconButton();
            this.iconButtonPlayer7 = new FontAwesome.Sharp.IconButton();
            this.iconButtonPlayer6 = new FontAwesome.Sharp.IconButton();
            this.iconButtonPlayer5 = new FontAwesome.Sharp.IconButton();
            this.iconButtonPlayer4 = new FontAwesome.Sharp.IconButton();
            this.iconButtonPlayer3 = new FontAwesome.Sharp.IconButton();
            this.iconButtonPlayer2 = new FontAwesome.Sharp.IconButton();
            this.iconButtonPlayer1 = new FontAwesome.Sharp.IconButton();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblPlayerId = new System.Windows.Forms.Label();
            this.labelSelectedSong = new System.Windows.Forms.Label();
            this.btnPlaylistFileImport = new FontAwesome.Sharp.IconPictureBox();
            this.btnPlaylistLoop = new FontAwesome.Sharp.IconPictureBox();
            this.btnPlaylistSave = new FontAwesome.Sharp.IconPictureBox();
            this.btnPlaylistShuffle = new FontAwesome.Sharp.IconPictureBox();
            this.btnPlaylistRemove = new FontAwesome.Sharp.IconPictureBox();
            this.btnPlaylistAddFile = new FontAwesome.Sharp.IconPictureBox();
            this.btnPlaylistAddFolder = new FontAwesome.Sharp.IconPictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.nPlayer8 = new TOAMediaPlayer.NAudioOutput.NPlayer();
            this.nPlayer7 = new TOAMediaPlayer.NAudioOutput.NPlayer();
            this.nPlayer6 = new TOAMediaPlayer.NAudioOutput.NPlayer();
            this.nPlayer5 = new TOAMediaPlayer.NAudioOutput.NPlayer();
            this.nPlayer4 = new TOAMediaPlayer.NAudioOutput.NPlayer();
            this.nPlayer3 = new TOAMediaPlayer.NAudioOutput.NPlayer();
            this.nPlayer2 = new TOAMediaPlayer.NAudioOutput.NPlayer();
            this.nPlayer1 = new TOAMediaPlayer.NAudioOutput.NPlayer();
            this.myListView = new ListViewCustomReorder.ListViewEx();
            this.colSeq = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSongName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLength = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconWindowClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconWindowMinimize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconWindowMaximize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnPlaylistFileImport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPlaylistLoop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPlaylistSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPlaylistShuffle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPlaylistRemove)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPlaylistAddFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPlaylistAddFolder)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DimGray;
            this.panel1.Controls.Add(this.iconButton2);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.iconButton1);
            this.panel1.Controls.Add(this.iconPictureBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.iconWindowClose);
            this.panel1.Controls.Add(this.iconWindowMinimize);
            this.panel1.Controls.Add(this.iconWindowMaximize);
            this.panel1.Controls.Add(this.iconLogo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1451, 26);
            this.panel1.TabIndex = 8;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            // 
            // iconButton2
            // 
            this.iconButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iconButton2.IconChar = FontAwesome.Sharp.IconChar.None;
            this.iconButton2.IconColor = System.Drawing.Color.Black;
            this.iconButton2.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconButton2.Location = new System.Drawing.Point(1284, 4);
            this.iconButton2.Name = "iconButton2";
            this.iconButton2.Size = new System.Drawing.Size(75, 21);
            this.iconButton2.TabIndex = 8;
            this.iconButton2.Text = "Timer";
            this.iconButton2.UseVisualStyleBackColor = true;
            this.iconButton2.Click += new System.EventHandler(this.iconButton2_Click_1);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::TOAMediaPlayer.Properties.Resources.TOA_Icon__WO__01;
            this.pictureBox1.Location = new System.Drawing.Point(9, 3);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(22, 23);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // iconButton1
            // 
            this.iconButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iconButton1.IconChar = FontAwesome.Sharp.IconChar.None;
            this.iconButton1.IconColor = System.Drawing.Color.Black;
            this.iconButton1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconButton1.Location = new System.Drawing.Point(1146, 4);
            this.iconButton1.Name = "iconButton1";
            this.iconButton1.Size = new System.Drawing.Size(75, 19);
            this.iconButton1.TabIndex = 6;
            this.iconButton1.Text = "ทดสอบ";
            this.iconButton1.UseVisualStyleBackColor = true;
            this.iconButton1.Visible = false;
            this.iconButton1.Click += new System.EventHandler(this.iconLogo_Click);
            // 
            // iconPictureBox1
            // 
            this.iconPictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iconPictureBox1.BackColor = System.Drawing.Color.DimGray;
            this.iconPictureBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.iconPictureBox1.IconChar = FontAwesome.Sharp.IconChar.Hammer;
            this.iconPictureBox1.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.iconPictureBox1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconPictureBox1.IconSize = 21;
            this.iconPictureBox1.Location = new System.Drawing.Point(1255, 4);
            this.iconPictureBox1.Name = "iconPictureBox1";
            this.iconPictureBox1.Size = new System.Drawing.Size(25, 21);
            this.iconPictureBox1.TabIndex = 5;
            this.iconPictureBox1.TabStop = false;
            this.iconPictureBox1.UseGdi = true;
            this.iconPictureBox1.Visible = false;
            this.iconPictureBox1.Click += new System.EventHandler(this.iconPictureBox1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkOrange;
            this.label1.Location = new System.Drawing.Point(34, 4);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 19);
            this.label1.TabIndex = 3;
            this.label1.Text = "TOA Multi-Track Player";
            // 
            // iconWindowClose
            // 
            this.iconWindowClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iconWindowClose.BackColor = System.Drawing.Color.DimGray;
            this.iconWindowClose.ForeColor = System.Drawing.SystemColors.Window;
            this.iconWindowClose.IconChar = FontAwesome.Sharp.IconChar.WindowClose;
            this.iconWindowClose.IconColor = System.Drawing.SystemColors.Window;
            this.iconWindowClose.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconWindowClose.IconSize = 21;
            this.iconWindowClose.Location = new System.Drawing.Point(1423, 2);
            this.iconWindowClose.Margin = new System.Windows.Forms.Padding(2);
            this.iconWindowClose.Name = "iconWindowClose";
            this.iconWindowClose.Size = new System.Drawing.Size(25, 21);
            this.iconWindowClose.TabIndex = 4;
            this.iconWindowClose.TabStop = false;
            this.iconWindowClose.Click += new System.EventHandler(this.iconWindowClose_Click);
            // 
            // iconWindowMinimize
            // 
            this.iconWindowMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iconWindowMinimize.BackColor = System.Drawing.Color.DimGray;
            this.iconWindowMinimize.ForeColor = System.Drawing.SystemColors.Window;
            this.iconWindowMinimize.IconChar = FontAwesome.Sharp.IconChar.WindowMinimize;
            this.iconWindowMinimize.IconColor = System.Drawing.SystemColors.Window;
            this.iconWindowMinimize.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconWindowMinimize.IconSize = 20;
            this.iconWindowMinimize.Location = new System.Drawing.Point(1364, 5);
            this.iconWindowMinimize.Margin = new System.Windows.Forms.Padding(2);
            this.iconWindowMinimize.Name = "iconWindowMinimize";
            this.iconWindowMinimize.Size = new System.Drawing.Size(25, 20);
            this.iconWindowMinimize.TabIndex = 1;
            this.iconWindowMinimize.TabStop = false;
            this.iconWindowMinimize.Click += new System.EventHandler(this.iconWindowMinimize_Click);
            // 
            // iconWindowMaximize
            // 
            this.iconWindowMaximize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iconWindowMaximize.BackColor = System.Drawing.Color.DimGray;
            this.iconWindowMaximize.ForeColor = System.Drawing.SystemColors.Window;
            this.iconWindowMaximize.IconChar = FontAwesome.Sharp.IconChar.WindowMaximize;
            this.iconWindowMaximize.IconColor = System.Drawing.SystemColors.Window;
            this.iconWindowMaximize.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconWindowMaximize.IconSize = 21;
            this.iconWindowMaximize.Location = new System.Drawing.Point(1394, 2);
            this.iconWindowMaximize.Margin = new System.Windows.Forms.Padding(2);
            this.iconWindowMaximize.Name = "iconWindowMaximize";
            this.iconWindowMaximize.Size = new System.Drawing.Size(25, 21);
            this.iconWindowMaximize.TabIndex = 3;
            this.iconWindowMaximize.TabStop = false;
            this.iconWindowMaximize.Click += new System.EventHandler(this.iconWindowMaximize_Click);
            // 
            // iconLogo
            // 
            this.iconLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iconLogo.BackColor = System.Drawing.Color.Black;
            this.iconLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.iconLogo.IconChar = FontAwesome.Sharp.IconChar.Weebly;
            this.iconLogo.IconColor = System.Drawing.Color.LawnGreen;
            this.iconLogo.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconLogo.IconSize = 20;
            this.iconLogo.Location = new System.Drawing.Point(1225, 3);
            this.iconLogo.Margin = new System.Windows.Forms.Padding(2);
            this.iconLogo.Name = "iconLogo";
            this.iconLogo.Size = new System.Drawing.Size(25, 20);
            this.iconLogo.TabIndex = 0;
            this.iconLogo.UseVisualStyleBackColor = false;
            this.iconLogo.Visible = false;
            this.iconLogo.Click += new System.EventHandler(this.iconLogo_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 26);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.nPlayer8);
            this.splitContainer1.Panel1.Controls.Add(this.nPlayer7);
            this.splitContainer1.Panel1.Controls.Add(this.nPlayer6);
            this.splitContainer1.Panel1.Controls.Add(this.nPlayer5);
            this.splitContainer1.Panel1.Controls.Add(this.nPlayer4);
            this.splitContainer1.Panel1.Controls.Add(this.nPlayer3);
            this.splitContainer1.Panel1.Controls.Add(this.nPlayer2);
            this.splitContainer1.Panel1.Controls.Add(this.nPlayer1);
            this.splitContainer1.Panel1.Controls.Add(this.iconButtonPlayer8);
            this.splitContainer1.Panel1.Controls.Add(this.iconButtonPlayer7);
            this.splitContainer1.Panel1.Controls.Add(this.iconButtonPlayer6);
            this.splitContainer1.Panel1.Controls.Add(this.iconButtonPlayer5);
            this.splitContainer1.Panel1.Controls.Add(this.iconButtonPlayer4);
            this.splitContainer1.Panel1.Controls.Add(this.iconButtonPlayer3);
            this.splitContainer1.Panel1.Controls.Add(this.iconButtonPlayer2);
            this.splitContainer1.Panel1.Controls.Add(this.iconButtonPlayer1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1451, 1025);
            this.splitContainer1.SplitterDistance = 961;
            this.splitContainer1.TabIndex = 9;
            // 
            // iconButtonPlayer8
            // 
            this.iconButtonPlayer8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.iconButtonPlayer8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButtonPlayer8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iconButtonPlayer8.IconChar = FontAwesome.Sharp.IconChar.None;
            this.iconButtonPlayer8.IconColor = System.Drawing.Color.Black;
            this.iconButtonPlayer8.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconButtonPlayer8.Location = new System.Drawing.Point(925, 896);
            this.iconButtonPlayer8.Name = "iconButtonPlayer8";
            this.iconButtonPlayer8.Size = new System.Drawing.Size(35, 124);
            this.iconButtonPlayer8.TabIndex = 153;
            this.iconButtonPlayer8.Text = "8";
            this.iconButtonPlayer8.UseVisualStyleBackColor = false;
            this.iconButtonPlayer8.Click += new System.EventHandler(this.iconButtonPlayer8_Click);
            // 
            // iconButtonPlayer7
            // 
            this.iconButtonPlayer7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.iconButtonPlayer7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButtonPlayer7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iconButtonPlayer7.IconChar = FontAwesome.Sharp.IconChar.None;
            this.iconButtonPlayer7.IconColor = System.Drawing.Color.Black;
            this.iconButtonPlayer7.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconButtonPlayer7.Location = new System.Drawing.Point(925, 770);
            this.iconButtonPlayer7.Name = "iconButtonPlayer7";
            this.iconButtonPlayer7.Size = new System.Drawing.Size(35, 124);
            this.iconButtonPlayer7.TabIndex = 152;
            this.iconButtonPlayer7.Text = "7";
            this.iconButtonPlayer7.UseVisualStyleBackColor = false;
            this.iconButtonPlayer7.Click += new System.EventHandler(this.iconButtonPlayer7_Click);
            // 
            // iconButtonPlayer6
            // 
            this.iconButtonPlayer6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.iconButtonPlayer6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButtonPlayer6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iconButtonPlayer6.IconChar = FontAwesome.Sharp.IconChar.None;
            this.iconButtonPlayer6.IconColor = System.Drawing.Color.Black;
            this.iconButtonPlayer6.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconButtonPlayer6.Location = new System.Drawing.Point(925, 641);
            this.iconButtonPlayer6.Name = "iconButtonPlayer6";
            this.iconButtonPlayer6.Size = new System.Drawing.Size(35, 128);
            this.iconButtonPlayer6.TabIndex = 151;
            this.iconButtonPlayer6.Text = "6";
            this.iconButtonPlayer6.UseVisualStyleBackColor = false;
            this.iconButtonPlayer6.Click += new System.EventHandler(this.iconButtonPlayer6_Click);
            // 
            // iconButtonPlayer5
            // 
            this.iconButtonPlayer5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.iconButtonPlayer5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButtonPlayer5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iconButtonPlayer5.IconChar = FontAwesome.Sharp.IconChar.None;
            this.iconButtonPlayer5.IconColor = System.Drawing.Color.Black;
            this.iconButtonPlayer5.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconButtonPlayer5.Location = new System.Drawing.Point(925, 512);
            this.iconButtonPlayer5.Name = "iconButtonPlayer5";
            this.iconButtonPlayer5.Size = new System.Drawing.Size(35, 128);
            this.iconButtonPlayer5.TabIndex = 150;
            this.iconButtonPlayer5.Text = "5";
            this.iconButtonPlayer5.UseVisualStyleBackColor = false;
            this.iconButtonPlayer5.Click += new System.EventHandler(this.iconButtonPlayer5_Click);
            // 
            // iconButtonPlayer4
            // 
            this.iconButtonPlayer4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.iconButtonPlayer4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButtonPlayer4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iconButtonPlayer4.IconChar = FontAwesome.Sharp.IconChar.None;
            this.iconButtonPlayer4.IconColor = System.Drawing.Color.Black;
            this.iconButtonPlayer4.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconButtonPlayer4.Location = new System.Drawing.Point(925, 383);
            this.iconButtonPlayer4.Name = "iconButtonPlayer4";
            this.iconButtonPlayer4.Size = new System.Drawing.Size(35, 128);
            this.iconButtonPlayer4.TabIndex = 145;
            this.iconButtonPlayer4.Text = "4";
            this.iconButtonPlayer4.UseVisualStyleBackColor = false;
            this.iconButtonPlayer4.Click += new System.EventHandler(this.iconButtonPlayer4_Click);
            // 
            // iconButtonPlayer3
            // 
            this.iconButtonPlayer3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.iconButtonPlayer3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButtonPlayer3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iconButtonPlayer3.IconChar = FontAwesome.Sharp.IconChar.None;
            this.iconButtonPlayer3.IconColor = System.Drawing.Color.Black;
            this.iconButtonPlayer3.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconButtonPlayer3.Location = new System.Drawing.Point(925, 254);
            this.iconButtonPlayer3.Name = "iconButtonPlayer3";
            this.iconButtonPlayer3.Size = new System.Drawing.Size(35, 128);
            this.iconButtonPlayer3.TabIndex = 138;
            this.iconButtonPlayer3.Text = "3";
            this.iconButtonPlayer3.UseVisualStyleBackColor = false;
            this.iconButtonPlayer3.Click += new System.EventHandler(this.iconButtonPlayer3_Click);
            // 
            // iconButtonPlayer2
            // 
            this.iconButtonPlayer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.iconButtonPlayer2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButtonPlayer2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iconButtonPlayer2.IconChar = FontAwesome.Sharp.IconChar.None;
            this.iconButtonPlayer2.IconColor = System.Drawing.Color.Black;
            this.iconButtonPlayer2.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconButtonPlayer2.Location = new System.Drawing.Point(925, 125);
            this.iconButtonPlayer2.Name = "iconButtonPlayer2";
            this.iconButtonPlayer2.Size = new System.Drawing.Size(35, 128);
            this.iconButtonPlayer2.TabIndex = 11;
            this.iconButtonPlayer2.Text = "2";
            this.iconButtonPlayer2.UseVisualStyleBackColor = false;
            this.iconButtonPlayer2.Click += new System.EventHandler(this.iconButtonPlayer2_Click);
            // 
            // iconButtonPlayer1
            // 
            this.iconButtonPlayer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.iconButtonPlayer1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButtonPlayer1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iconButtonPlayer1.IconChar = FontAwesome.Sharp.IconChar.None;
            this.iconButtonPlayer1.IconColor = System.Drawing.Color.LawnGreen;
            this.iconButtonPlayer1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconButtonPlayer1.Location = new System.Drawing.Point(925, -4);
            this.iconButtonPlayer1.Name = "iconButtonPlayer1";
            this.iconButtonPlayer1.Size = new System.Drawing.Size(35, 128);
            this.iconButtonPlayer1.TabIndex = 10;
            this.iconButtonPlayer1.Text = "1";
            this.iconButtonPlayer1.UseVisualStyleBackColor = false;
            this.iconButtonPlayer1.Click += new System.EventHandler(this.iconButtonPlayer1_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.panel2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.myListView);
            this.splitContainer2.Panel2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.splitContainer2.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer2_Panel2_Paint);
            this.splitContainer2.Size = new System.Drawing.Size(486, 1025);
            this.splitContainer2.SplitterDistance = 31;
            this.splitContainer2.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblPlayerId);
            this.panel2.Controls.Add(this.labelSelectedSong);
            this.panel2.Controls.Add(this.btnPlaylistFileImport);
            this.panel2.Controls.Add(this.btnPlaylistLoop);
            this.panel2.Controls.Add(this.btnPlaylistSave);
            this.panel2.Controls.Add(this.btnPlaylistShuffle);
            this.panel2.Controls.Add(this.btnPlaylistRemove);
            this.panel2.Controls.Add(this.btnPlaylistAddFile);
            this.panel2.Controls.Add(this.btnPlaylistAddFolder);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(486, 31);
            this.panel2.TabIndex = 2;
            // 
            // lblPlayerId
            // 
            this.lblPlayerId.AutoSize = true;
            this.lblPlayerId.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblPlayerId.Location = new System.Drawing.Point(186, 4);
            this.lblPlayerId.Name = "lblPlayerId";
            this.lblPlayerId.Size = new System.Drawing.Size(17, 20);
            this.lblPlayerId.TabIndex = 9;
            this.lblPlayerId.Text = "x";
            // 
            // labelSelectedSong
            // 
            this.labelSelectedSong.AutoSize = true;
            this.labelSelectedSong.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.labelSelectedSong.ForeColor = System.Drawing.Color.White;
            this.labelSelectedSong.Location = new System.Drawing.Point(226, 4);
            this.labelSelectedSong.Name = "labelSelectedSong";
            this.labelSelectedSong.Size = new System.Drawing.Size(13, 20);
            this.labelSelectedSong.TabIndex = 8;
            this.labelSelectedSong.Text = ".";
            // 
            // btnPlaylistFileImport
            // 
            this.btnPlaylistFileImport.BackColor = System.Drawing.Color.Gray;
            this.btnPlaylistFileImport.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnPlaylistFileImport.IconChar = FontAwesome.Sharp.IconChar.FileImport;
            this.btnPlaylistFileImport.IconColor = System.Drawing.SystemColors.ControlText;
            this.btnPlaylistFileImport.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnPlaylistFileImport.IconSize = 24;
            this.btnPlaylistFileImport.Location = new System.Drawing.Point(158, 2);
            this.btnPlaylistFileImport.Name = "btnPlaylistFileImport";
            this.btnPlaylistFileImport.Size = new System.Drawing.Size(25, 24);
            this.btnPlaylistFileImport.TabIndex = 6;
            this.btnPlaylistFileImport.TabStop = false;
            this.btnPlaylistFileImport.UseGdi = true;
            this.btnPlaylistFileImport.Click += new System.EventHandler(this.btnPlaylistFileImport_Click);
            this.btnPlaylistFileImport.MouseHover += new System.EventHandler(this.btnPlaylistFileImport_MouseHover);
            // 
            // btnPlaylistLoop
            // 
            this.btnPlaylistLoop.BackColor = System.Drawing.Color.Gray;
            this.btnPlaylistLoop.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPlaylistLoop.BackgroundImage")));
            this.btnPlaylistLoop.ForeColor = System.Drawing.Color.Black;
            this.btnPlaylistLoop.IconChar = FontAwesome.Sharp.IconChar.Repeat;
            this.btnPlaylistLoop.IconColor = System.Drawing.Color.Black;
            this.btnPlaylistLoop.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnPlaylistLoop.IconSize = 24;
            this.btnPlaylistLoop.Location = new System.Drawing.Point(106, 2);
            this.btnPlaylistLoop.Name = "btnPlaylistLoop";
            this.btnPlaylistLoop.Size = new System.Drawing.Size(25, 24);
            this.btnPlaylistLoop.TabIndex = 4;
            this.btnPlaylistLoop.TabStop = false;
            this.btnPlaylistLoop.UseGdi = true;
            this.btnPlaylistLoop.Click += new System.EventHandler(this.btnPlaylistLoop_Click);
            this.btnPlaylistLoop.MouseHover += new System.EventHandler(this.btnPlaylistLoop_MouseHover);
            // 
            // btnPlaylistSave
            // 
            this.btnPlaylistSave.BackColor = System.Drawing.Color.Gray;
            this.btnPlaylistSave.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnPlaylistSave.IconChar = FontAwesome.Sharp.IconChar.FloppyDisk;
            this.btnPlaylistSave.IconColor = System.Drawing.SystemColors.ControlText;
            this.btnPlaylistSave.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnPlaylistSave.IconSize = 24;
            this.btnPlaylistSave.Location = new System.Drawing.Point(132, 2);
            this.btnPlaylistSave.Name = "btnPlaylistSave";
            this.btnPlaylistSave.Size = new System.Drawing.Size(25, 24);
            this.btnPlaylistSave.TabIndex = 5;
            this.btnPlaylistSave.TabStop = false;
            this.btnPlaylistSave.UseGdi = true;
            this.btnPlaylistSave.Click += new System.EventHandler(this.btnPlaylistSave_Click);
            this.btnPlaylistSave.MouseHover += new System.EventHandler(this.btnPlaylistSave_MouseHover);
            // 
            // btnPlaylistShuffle
            // 
            this.btnPlaylistShuffle.BackColor = System.Drawing.Color.Gray;
            this.btnPlaylistShuffle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnPlaylistShuffle.IconChar = FontAwesome.Sharp.IconChar.Shuffle;
            this.btnPlaylistShuffle.IconColor = System.Drawing.SystemColors.ControlText;
            this.btnPlaylistShuffle.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnPlaylistShuffle.IconSize = 24;
            this.btnPlaylistShuffle.Location = new System.Drawing.Point(80, 2);
            this.btnPlaylistShuffle.Name = "btnPlaylistShuffle";
            this.btnPlaylistShuffle.Size = new System.Drawing.Size(25, 24);
            this.btnPlaylistShuffle.TabIndex = 3;
            this.btnPlaylistShuffle.TabStop = false;
            this.btnPlaylistShuffle.UseGdi = true;
            this.btnPlaylistShuffle.Click += new System.EventHandler(this.btnPlaylistShuffle_Click);
            this.btnPlaylistShuffle.MouseHover += new System.EventHandler(this.btnPlaylistShuffle_MouseHover);
            // 
            // btnPlaylistRemove
            // 
            this.btnPlaylistRemove.BackColor = System.Drawing.Color.Gray;
            this.btnPlaylistRemove.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnPlaylistRemove.IconChar = FontAwesome.Sharp.IconChar.CircleMinus;
            this.btnPlaylistRemove.IconColor = System.Drawing.SystemColors.ControlText;
            this.btnPlaylistRemove.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnPlaylistRemove.IconSize = 24;
            this.btnPlaylistRemove.Location = new System.Drawing.Point(54, 2);
            this.btnPlaylistRemove.Name = "btnPlaylistRemove";
            this.btnPlaylistRemove.Size = new System.Drawing.Size(25, 24);
            this.btnPlaylistRemove.TabIndex = 2;
            this.btnPlaylistRemove.TabStop = false;
            this.btnPlaylistRemove.UseGdi = true;
            this.btnPlaylistRemove.Click += new System.EventHandler(this.btnPlaylistRemove_Click);
            this.btnPlaylistRemove.MouseHover += new System.EventHandler(this.btnPlaylistRemove_MouseHover);
            // 
            // btnPlaylistAddFile
            // 
            this.btnPlaylistAddFile.BackColor = System.Drawing.Color.Gray;
            this.btnPlaylistAddFile.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnPlaylistAddFile.IconChar = FontAwesome.Sharp.IconChar.PlusSquare;
            this.btnPlaylistAddFile.IconColor = System.Drawing.SystemColors.ControlText;
            this.btnPlaylistAddFile.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnPlaylistAddFile.IconSize = 24;
            this.btnPlaylistAddFile.Location = new System.Drawing.Point(28, 2);
            this.btnPlaylistAddFile.Name = "btnPlaylistAddFile";
            this.btnPlaylistAddFile.Size = new System.Drawing.Size(25, 24);
            this.btnPlaylistAddFile.TabIndex = 1;
            this.btnPlaylistAddFile.TabStop = false;
            this.btnPlaylistAddFile.UseGdi = true;
            this.btnPlaylistAddFile.Click += new System.EventHandler(this.btnPlaylistAddFile_Click);
            this.btnPlaylistAddFile.MouseHover += new System.EventHandler(this.btnPlaylistAddFile_MouseHover);
            // 
            // btnPlaylistAddFolder
            // 
            this.btnPlaylistAddFolder.BackColor = System.Drawing.Color.Gray;
            this.btnPlaylistAddFolder.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnPlaylistAddFolder.IconChar = FontAwesome.Sharp.IconChar.FolderPlus;
            this.btnPlaylistAddFolder.IconColor = System.Drawing.SystemColors.ControlText;
            this.btnPlaylistAddFolder.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnPlaylistAddFolder.IconSize = 24;
            this.btnPlaylistAddFolder.Location = new System.Drawing.Point(2, 2);
            this.btnPlaylistAddFolder.Name = "btnPlaylistAddFolder";
            this.btnPlaylistAddFolder.Size = new System.Drawing.Size(25, 24);
            this.btnPlaylistAddFolder.TabIndex = 0;
            this.btnPlaylistAddFolder.TabStop = false;
            this.btnPlaylistAddFolder.UseGdi = true;
            this.btnPlaylistAddFolder.Click += new System.EventHandler(this.btnPlaylistAddFolder_Click);
            this.btnPlaylistAddFolder.MouseHover += new System.EventHandler(this.btnPlaylistAddFolder_MouseHover);
            // 
            // nPlayer8
            // 
            this.nPlayer8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.nPlayer8.CurrentMedia = null;
            this.nPlayer8.CurrentPlaylist = null;
            this.nPlayer8.Location = new System.Drawing.Point(0, 896);
            this.nPlayer8.MediaPlaybackState = NAudio.Wave.PlaybackState.Stopped;
            this.nPlayer8.Name = "nPlayer8";
            this.nPlayer8.PlayedFileName = null;
            this.nPlayer8.PlayerName = null;
            this.nPlayer8.Size = new System.Drawing.Size(926, 124);
            this.nPlayer8.TabIndex = 0;
            this.nPlayer8.Volume = 0F;
            // 
            // nPlayer7
            // 
            this.nPlayer7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.nPlayer7.CurrentMedia = null;
            this.nPlayer7.CurrentPlaylist = null;
            this.nPlayer7.Location = new System.Drawing.Point(0, 770);
            this.nPlayer7.MediaPlaybackState = NAudio.Wave.PlaybackState.Stopped;
            this.nPlayer7.Name = "nPlayer7";
            this.nPlayer7.PlayedFileName = null;
            this.nPlayer7.PlayerName = null;
            this.nPlayer7.Size = new System.Drawing.Size(926, 124);
            this.nPlayer7.TabIndex = 1;
            this.nPlayer7.Volume = 0F;
            // 
            // nPlayer6
            // 
            this.nPlayer6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.nPlayer6.CurrentMedia = null;
            this.nPlayer6.CurrentPlaylist = null;
            this.nPlayer6.Location = new System.Drawing.Point(0, 641);
            this.nPlayer6.MediaPlaybackState = NAudio.Wave.PlaybackState.Stopped;
            this.nPlayer6.Name = "nPlayer6";
            this.nPlayer6.PlayedFileName = null;
            this.nPlayer6.PlayerName = null;
            this.nPlayer6.Size = new System.Drawing.Size(926, 128);
            this.nPlayer6.TabIndex = 2;
            this.nPlayer6.Volume = 0F;
            // 
            // nPlayer5
            // 
            this.nPlayer5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.nPlayer5.CurrentMedia = null;
            this.nPlayer5.CurrentPlaylist = null;
            this.nPlayer5.Location = new System.Drawing.Point(1, 512);
            this.nPlayer5.MediaPlaybackState = NAudio.Wave.PlaybackState.Stopped;
            this.nPlayer5.Name = "nPlayer5";
            this.nPlayer5.PlayedFileName = null;
            this.nPlayer5.PlayerName = null;
            this.nPlayer5.Size = new System.Drawing.Size(926, 128);
            this.nPlayer5.TabIndex = 3;
            this.nPlayer5.Volume = 0F;
            // 
            // nPlayer4
            // 
            this.nPlayer4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.nPlayer4.CurrentMedia = null;
            this.nPlayer4.CurrentPlaylist = null;
            this.nPlayer4.Location = new System.Drawing.Point(0, 383);
            this.nPlayer4.MediaPlaybackState = NAudio.Wave.PlaybackState.Stopped;
            this.nPlayer4.Name = "nPlayer4";
            this.nPlayer4.PlayedFileName = null;
            this.nPlayer4.PlayerName = null;
            this.nPlayer4.Size = new System.Drawing.Size(926, 128);
            this.nPlayer4.TabIndex = 4;
            this.nPlayer4.Volume = 0F;
            // 
            // nPlayer3
            // 
            this.nPlayer3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.nPlayer3.CurrentMedia = null;
            this.nPlayer3.CurrentPlaylist = null;
            this.nPlayer3.Location = new System.Drawing.Point(0, 254);
            this.nPlayer3.MediaPlaybackState = NAudio.Wave.PlaybackState.Stopped;
            this.nPlayer3.Name = "nPlayer3";
            this.nPlayer3.PlayedFileName = null;
            this.nPlayer3.PlayerName = null;
            this.nPlayer3.Size = new System.Drawing.Size(926, 128);
            this.nPlayer3.TabIndex = 5;
            this.nPlayer3.Volume = 0F;
            // 
            // nPlayer2
            // 
            this.nPlayer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.nPlayer2.CurrentMedia = null;
            this.nPlayer2.CurrentPlaylist = null;
            this.nPlayer2.Location = new System.Drawing.Point(0, 125);
            this.nPlayer2.MediaPlaybackState = NAudio.Wave.PlaybackState.Stopped;
            this.nPlayer2.Name = "nPlayer2";
            this.nPlayer2.PlayedFileName = null;
            this.nPlayer2.PlayerName = null;
            this.nPlayer2.Size = new System.Drawing.Size(926, 128);
            this.nPlayer2.TabIndex = 6;
            this.nPlayer2.Volume = 0F;
            // 
            // nPlayer1
            // 
            this.nPlayer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            otsMedia1.CurrentPlaybackState = NAudio.Wave.PlaybackState.Stopped;
            otsMedia1.duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            otsMedia1.fileLocation = null;
            otsMedia1.fileName = null;
            otsMedia1.Id = 0;
            otsMedia1.Loop = false;
            otsMedia1.Shuffle = false;
            this.nPlayer1.CurrentMedia = otsMedia1;
            this.nPlayer1.CurrentPlaylist = null;
            this.nPlayer1.Location = new System.Drawing.Point(2, 4);
            this.nPlayer1.Margin = new System.Windows.Forms.Padding(4);
            this.nPlayer1.MediaPlaybackState = NAudio.Wave.PlaybackState.Stopped;
            this.nPlayer1.Name = "nPlayer1";
            this.nPlayer1.PlayedFileName = null;
            this.nPlayer1.PlayerName = "nPlayer1";
            this.nPlayer1.Size = new System.Drawing.Size(925, 120);
            this.nPlayer1.TabIndex = 141;
            this.nPlayer1.Volume = 0F;
            this.nPlayer1.CurrentPlaylistChanged += new System.EventHandler(this.NPlayer1_CurrentPlaylistChanged);
            // 
            // myListView
            // 
            this.myListView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.myListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colSeq,
            this.colSongName,
            this.colLength});
            this.myListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myListView.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.myListView.ForeColor = System.Drawing.Color.White;
            this.myListView.FullRowSelect = true;
            this.myListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.myListView.HideSelection = false;
            this.myListView.LineAfter = -1;
            this.myListView.LineBefore = -1;
            this.myListView.Location = new System.Drawing.Point(0, 0);
            this.myListView.Margin = new System.Windows.Forms.Padding(4);
            this.myListView.Name = "myListView";
            this.myListView.Size = new System.Drawing.Size(486, 990);
            this.myListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.myListView.TabIndex = 2;
            this.myListView.UseCompatibleStateImageBehavior = false;
            this.myListView.View = System.Windows.Forms.View.Details;
            this.myListView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.myListView_ColumnClick);
            this.myListView.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.myListView_DrawItem);
            this.myListView.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.myListView_DrawSubItem);
            this.myListView.SelectedIndexChanged += new System.EventHandler(this.myListView_SelectedIndexChanged);
            this.myListView.Click += new System.EventHandler(this.myListView_Click);
            this.myListView.DoubleClick += new System.EventHandler(this.myListView_DoubleClick);
            this.myListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.myListView_KeyDown);
            this.myListView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.myListView_KeyUp);
            this.myListView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDown);
            this.myListView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseMove);
            this.myListView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseUp);
            // 
            // colSeq
            // 
            this.colSeq.Tag = "1";
            this.colSeq.Text = "###";
            this.colSeq.Width = 30;
            // 
            // colSongName
            // 
            this.colSongName.Text = "ชื่อเพลง";
            this.colSongName.Width = 800;
            // 
            // colLength
            // 
            this.colLength.Tag = "";
            this.colLength.Text = "เวลา(นาที) ";
            this.colLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colLength.Width = 109;
            // 
            // MainPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(1451, 1051);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "MainPlayer";
            this.Text = "TOA Multi-Track Player";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainPlayer_FormClosing);
            this.Load += new System.EventHandler(this.MainPlayer_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainPlayer_KeyDown);
            this.Resize += new System.EventHandler(this.MainPlayer_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconWindowClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconWindowMinimize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconWindowMaximize)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnPlaylistFileImport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPlaylistLoop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPlaylistSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPlaylistShuffle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPlaylistRemove)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPlaylistAddFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPlaylistAddFolder)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private FontAwesome.Sharp.IconPictureBox iconWindowClose;
        private FontAwesome.Sharp.IconPictureBox iconWindowMinimize;
        private FontAwesome.Sharp.IconPictureBox iconWindowMaximize;
        private FontAwesome.Sharp.IconButton iconLogo;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel panel2;
        private FontAwesome.Sharp.IconPictureBox btnPlaylistFileImport;
        private FontAwesome.Sharp.IconPictureBox btnPlaylistLoop;
        private FontAwesome.Sharp.IconPictureBox btnPlaylistSave;
        private FontAwesome.Sharp.IconPictureBox btnPlaylistShuffle;
        private FontAwesome.Sharp.IconPictureBox btnPlaylistRemove;
        private FontAwesome.Sharp.IconPictureBox btnPlaylistAddFile;
        private FontAwesome.Sharp.IconPictureBox btnPlaylistAddFolder;
        public ListViewCustomReorder.ListViewEx myListView;
        private System.Windows.Forms.ColumnHeader colSeq;
        private System.Windows.Forms.ColumnHeader colSongName;
        private System.Windows.Forms.ColumnHeader colLength;
        private System.Windows.Forms.Label labelSelectedSong;
        private FontAwesome.Sharp.IconButton iconButtonPlayer1;
        private FontAwesome.Sharp.IconButton iconButtonPlayer2;
        private System.Windows.Forms.Label lblPlayerId;
        private FontAwesome.Sharp.IconButton iconButtonPlayer3;
        public NAudioOutput.NPlayer nPlayer1;
        public NAudioOutput.NPlayer nPlayer2;
        public FontAwesome.Sharp.IconPictureBox iconPictureBox1;
        public NAudioOutput.NPlayer nPlayer3;
        private FontAwesome.Sharp.IconButton iconButtonPlayer8;
        private FontAwesome.Sharp.IconButton iconButtonPlayer7;
        private FontAwesome.Sharp.IconButton iconButtonPlayer6;
        private FontAwesome.Sharp.IconButton iconButtonPlayer5;
        public NAudioOutput.NPlayer nPlayer8;
        public NAudioOutput.NPlayer nPlayer7;
        public NAudioOutput.NPlayer nPlayer6;
        public NAudioOutput.NPlayer nPlayer5;
        private FontAwesome.Sharp.IconButton iconButtonPlayer4;
        public NAudioOutput.NPlayer nPlayer4;
        private FontAwesome.Sharp.IconButton iconButton1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private FontAwesome.Sharp.IconButton iconButton2;
    }
}

