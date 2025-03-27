namespace TOAMediaPlayer
{
    partial class PlayListControl
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelSelectedSong = new System.Windows.Forms.Label();
            this.iconSongDelete = new FontAwesome.Sharp.IconPictureBox();
            this.iconPictureBox1 = new FontAwesome.Sharp.IconPictureBox();
            this.iconImportFrom = new FontAwesome.Sharp.IconPictureBox();
            this.iconExportTo = new FontAwesome.Sharp.IconPictureBox();
            this.iconShuffle = new FontAwesome.Sharp.IconPictureBox();
            this.iconLoop = new FontAwesome.Sharp.IconPictureBox();
            this.iconFileAdd = new FontAwesome.Sharp.IconPictureBox();
            this.iconFolderAdd = new FontAwesome.Sharp.IconPictureBox();
            this.myListView = new System.Windows.Forms.ListView();
            this.colSeq = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSongName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colFileSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLength = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDirectoryName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconSongDelete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconImportFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconExportTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconShuffle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconLoop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconFileAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconFolderAdd)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.myListView, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.486238F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 91.51376F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(725, 436);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DimGray;
            this.panel1.Controls.Add(this.labelSelectedSong);
            this.panel1.Controls.Add(this.iconSongDelete);
            this.panel1.Controls.Add(this.iconPictureBox1);
            this.panel1.Controls.Add(this.iconImportFrom);
            this.panel1.Controls.Add(this.iconExportTo);
            this.panel1.Controls.Add(this.iconShuffle);
            this.panel1.Controls.Add(this.iconLoop);
            this.panel1.Controls.Add(this.iconFileAdd);
            this.panel1.Controls.Add(this.iconFolderAdd);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(719, 30);
            this.panel1.TabIndex = 0;
            // 
            // labelSelectedSong
            // 
            this.labelSelectedSong.AutoSize = true;
            this.labelSelectedSong.Font = new System.Drawing.Font("Georgia", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSelectedSong.Location = new System.Drawing.Point(230, 4);
            this.labelSelectedSong.Name = "labelSelectedSong";
            this.labelSelectedSong.Size = new System.Drawing.Size(50, 18);
            this.labelSelectedSong.TabIndex = 10;
            this.labelSelectedSong.Text = "ชื่อเพลง:";
            // 
            // iconSongDelete
            // 
            this.iconSongDelete.BackColor = System.Drawing.Color.DimGray;
            this.iconSongDelete.ForeColor = System.Drawing.SystemColors.ControlText;
            this.iconSongDelete.IconChar = FontAwesome.Sharp.IconChar.MinusSquare;
            this.iconSongDelete.IconColor = System.Drawing.SystemColors.ControlText;
            this.iconSongDelete.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconSongDelete.IconSize = 25;
            this.iconSongDelete.Location = new System.Drawing.Point(58, 2);
            this.iconSongDelete.Name = "iconSongDelete";
            this.iconSongDelete.Size = new System.Drawing.Size(25, 26);
            this.iconSongDelete.TabIndex = 9;
            this.iconSongDelete.TabStop = false;
            this.iconSongDelete.Click += new System.EventHandler(this.iconSongDelete_Click);
            // 
            // iconPictureBox1
            // 
            this.iconPictureBox1.BackColor = System.Drawing.Color.DimGray;
            this.iconPictureBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.iconPictureBox1.IconChar = FontAwesome.Sharp.IconChar.None;
            this.iconPictureBox1.IconColor = System.Drawing.SystemColors.ControlText;
            this.iconPictureBox1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconPictureBox1.IconSize = 25;
            this.iconPictureBox1.Location = new System.Drawing.Point(198, 2);
            this.iconPictureBox1.Name = "iconPictureBox1";
            this.iconPictureBox1.Size = new System.Drawing.Size(25, 25);
            this.iconPictureBox1.TabIndex = 8;
            this.iconPictureBox1.TabStop = false;
            // 
            // iconImportFrom
            // 
            this.iconImportFrom.BackColor = System.Drawing.Color.DimGray;
            this.iconImportFrom.ForeColor = System.Drawing.SystemColors.ControlText;
            this.iconImportFrom.IconChar = FontAwesome.Sharp.IconChar.FileImport;
            this.iconImportFrom.IconColor = System.Drawing.SystemColors.ControlText;
            this.iconImportFrom.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconImportFrom.IconSize = 25;
            this.iconImportFrom.Location = new System.Drawing.Point(170, 2);
            this.iconImportFrom.Name = "iconImportFrom";
            this.iconImportFrom.Size = new System.Drawing.Size(25, 25);
            this.iconImportFrom.TabIndex = 6;
            this.iconImportFrom.TabStop = false;
            this.iconImportFrom.Click += new System.EventHandler(this.iconImportFromXML_Click);
            // 
            // iconExportTo
            // 
            this.iconExportTo.BackColor = System.Drawing.Color.DimGray;
            this.iconExportTo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.iconExportTo.IconChar = FontAwesome.Sharp.IconChar.Save;
            this.iconExportTo.IconColor = System.Drawing.SystemColors.ControlText;
            this.iconExportTo.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconExportTo.IconSize = 25;
            this.iconExportTo.Location = new System.Drawing.Point(142, 2);
            this.iconExportTo.Name = "iconExportTo";
            this.iconExportTo.Size = new System.Drawing.Size(25, 25);
            this.iconExportTo.TabIndex = 5;
            this.iconExportTo.TabStop = false;
            this.iconExportTo.Click += new System.EventHandler(this.iconExportToXML_Click);
            // 
            // iconShuffle
            // 
            this.iconShuffle.BackColor = System.Drawing.Color.DimGray;
            this.iconShuffle.ForeColor = System.Drawing.Color.Black;
            this.iconShuffle.IconChar = FontAwesome.Sharp.IconChar.Random;
            this.iconShuffle.IconColor = System.Drawing.Color.Black;
            this.iconShuffle.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconShuffle.IconSize = 25;
            this.iconShuffle.Location = new System.Drawing.Point(86, 2);
            this.iconShuffle.Name = "iconShuffle";
            this.iconShuffle.Size = new System.Drawing.Size(25, 25);
            this.iconShuffle.TabIndex = 4;
            this.iconShuffle.TabStop = false;
            this.iconShuffle.Click += new System.EventHandler(this.iconShuffle_Click);
            // 
            // iconLoop
            // 
            this.iconLoop.BackColor = System.Drawing.Color.DimGray;
            this.iconLoop.ForeColor = System.Drawing.Color.Black;
            this.iconLoop.IconChar = FontAwesome.Sharp.IconChar.Chrome;
            this.iconLoop.IconColor = System.Drawing.Color.Black;
            this.iconLoop.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconLoop.IconSize = 25;
            this.iconLoop.Location = new System.Drawing.Point(114, 2);
            this.iconLoop.Name = "iconLoop";
            this.iconLoop.Size = new System.Drawing.Size(25, 25);
            this.iconLoop.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.iconLoop.TabIndex = 2;
            this.iconLoop.TabStop = false;
            this.iconLoop.Click += new System.EventHandler(this.iconLoop_Click);
            // 
            // iconFileAdd
            // 
            this.iconFileAdd.BackColor = System.Drawing.Color.DimGray;
            this.iconFileAdd.ForeColor = System.Drawing.SystemColors.ControlText;
            this.iconFileAdd.IconChar = FontAwesome.Sharp.IconChar.PlusSquare;
            this.iconFileAdd.IconColor = System.Drawing.SystemColors.ControlText;
            this.iconFileAdd.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconFileAdd.IconSize = 25;
            this.iconFileAdd.Location = new System.Drawing.Point(30, 2);
            this.iconFileAdd.Name = "iconFileAdd";
            this.iconFileAdd.Size = new System.Drawing.Size(25, 25);
            this.iconFileAdd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.iconFileAdd.TabIndex = 1;
            this.iconFileAdd.TabStop = false;
            this.iconFileAdd.Click += new System.EventHandler(this.iconFileAdd_Click);
            // 
            // iconFolderAdd
            // 
            this.iconFolderAdd.BackColor = System.Drawing.Color.DimGray;
            this.iconFolderAdd.ForeColor = System.Drawing.SystemColors.ControlText;
            this.iconFolderAdd.IconChar = FontAwesome.Sharp.IconChar.FolderPlus;
            this.iconFolderAdd.IconColor = System.Drawing.SystemColors.ControlText;
            this.iconFolderAdd.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconFolderAdd.IconSize = 25;
            this.iconFolderAdd.Location = new System.Drawing.Point(2, 2);
            this.iconFolderAdd.Name = "iconFolderAdd";
            this.iconFolderAdd.Size = new System.Drawing.Size(25, 25);
            this.iconFolderAdd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.iconFolderAdd.TabIndex = 0;
            this.iconFolderAdd.TabStop = false;
            this.iconFolderAdd.Click += new System.EventHandler(this.iconFolderAdd_Click);
            // 
            // myListView
            // 
            this.myListView.BackColor = System.Drawing.Color.Orange;
            this.myListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colSeq,
            this.colSongName,
            this.colFileSize,
            this.colLength,
            this.colDirectoryName});
            this.myListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myListView.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.myListView.FullRowSelect = true;
            this.myListView.GridLines = true;
            this.myListView.HideSelection = false;
            this.myListView.Location = new System.Drawing.Point(3, 39);
            this.myListView.Name = "myListView";
            this.myListView.Size = new System.Drawing.Size(719, 394);
            this.myListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.myListView.TabIndex = 1;
            this.myListView.UseCompatibleStateImageBehavior = false;
            this.myListView.View = System.Windows.Forms.View.Details;
            this.myListView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.myListView_ColumnClick);
            this.myListView.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.myListView_DrawItem);
            this.myListView.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.myListView_DrawSubItem);
            this.myListView.SelectedIndexChanged += new System.EventHandler(this.myListView_SelectedIndexChanged);
            // 
            // colSeq
            // 
            this.colSeq.Text = "###";
            this.colSeq.Width = 52;
            // 
            // colSongName
            // 
            this.colSongName.Text = "ชื่อเพลง";
            this.colSongName.Width = 227;
            // 
            // colFileSize
            // 
            this.colFileSize.Text = "ขนาดไฟล์ ";
            this.colFileSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colFileSize.Width = 120;
            // 
            // colLength
            // 
            this.colLength.Text = "เวลา(นาที) ";
            this.colLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colLength.Width = 120;
            // 
            // colDirectoryName
            // 
            this.colDirectoryName.Text = "DirectoryName";
            this.colDirectoryName.Width = 240;
            // 
            // PlayListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "PlayListControl";
            this.Size = new System.Drawing.Size(725, 436);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconSongDelete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconImportFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconExportTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconShuffle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconLoop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconFileAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconFolderAdd)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private FontAwesome.Sharp.IconPictureBox iconLoop;
        private FontAwesome.Sharp.IconPictureBox iconFileAdd;
        private FontAwesome.Sharp.IconPictureBox iconFolderAdd;
        private System.Windows.Forms.ListView myListView;
        private FontAwesome.Sharp.IconPictureBox iconShuffle;
        private FontAwesome.Sharp.IconPictureBox iconImportFrom;
        private FontAwesome.Sharp.IconPictureBox iconExportTo;
        private System.Windows.Forms.ColumnHeader colSeq;
        private System.Windows.Forms.ColumnHeader colSongName;
        private System.Windows.Forms.ColumnHeader colFileSize;
        private System.Windows.Forms.ColumnHeader colLength;
        private System.Windows.Forms.ColumnHeader colDirectoryName;
        private FontAwesome.Sharp.IconPictureBox iconSongDelete;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox1;
        private System.Windows.Forms.Label labelSelectedSong;
    }
}
