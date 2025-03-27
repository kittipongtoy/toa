using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
//using System.Windows.Controls;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;

using Microsoft.Win32;

using static System.Net.WebRequestMethods;
using static System.Windows.Forms.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TOAMediaPlayer
{
    public partial class PlayListControl : System.Windows.Forms.UserControl
    {
        private ListViewColumnSorter lvwColumnSorter;
        public bool IsShuffle { get; set; }
        public bool IsLoop { get; set; }
        /*        
         *        
         *      int itemIndex = -1;
                string playerId = string.Empty;
                string currentMediaName = string.Empty;*/
        public double playPositionTimeCode = 0;
        public PlayListControl()
        {
            InitializeComponent();
            lvwColumnSorter = new ListViewColumnSorter();
            //Default Sort is None
            this.myListView.ListViewItemSorter = lvwColumnSorter;
        }

        #region 
        private System.Drawing.Color shuffleForeColor;
        public event EventHandler shuffleForeColorChanged;
        public System.Drawing.Color ShuffleForeColor
        {
            get
            {
                return shuffleForeColor;
            }
            set
            {
                if (shuffleForeColor != value)
                {
                    shuffleForeColor = value;
                    OnShuffleForeColorChanged(EventArgs.Empty);
                }
            }
        }
        protected virtual void OnShuffleForeColorChanged(EventArgs e)
        {
            if (shuffleForeColorChanged != null)
            {
                this.iconShuffle.ForeColor = shuffleForeColor;
                shuffleForeColorChanged(this, e);
            }
        }
        #endregion

        //Prepare for Web Request sent position Time to Play Song
        //private string currentPositionTimeCode = "[00000]00:00:00.00";
        private string currentPositionTimeCode = "00:00:00.000";
        public event EventHandler PositionTimeCodeChanged;
        public string PositionTimeCode
        {
            get
            {
                return currentPositionTimeCode;
            }
            set
            {
                //if (currentPositionTimeCode != value)
                //{}
                currentPositionTimeCode = value;
                OnPositionTimeCodeChanged(EventArgs.Empty);
            }
        }
        protected virtual void OnPositionTimeCodeChanged(EventArgs e)
        {
            if (PositionTimeCodeChanged != null)
            {
                TimeSpan interval;
                if (TimeSpan.TryParseExact(currentPositionTimeCode, @"hh\:mm\:ss\.fff", null, out interval))
                {
                    this.playPositionTimeCode = interval.TotalSeconds;
                    //wplayer.controls.currentPosition = this.playPositionTimeCode;
                    //wplayer.controls.currentPosition = double.Parse(String.Format("{0:HH-mm-ss}", this.PlayPositionTimeCode));
                    PositionTimeCodeChanged(this, e);
                }
            }
        }

        private void iconFolderAdd_Click(object sender, EventArgs e)
        {
            bool bShowNewFolderButton = Properties.Settings.Default.ShowNewFolderButton;
            bool bSetEnvironmentSpecialFolder = Properties.Settings.Default.SetEnvironmentToSpecialFolder;
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = bShowNewFolderButton;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(folderBrowserDialog.SelectedPath);
                if (dirInfo == null) return;
                FileInfo[] files = dirInfo.GetFiles();
                if (files != null)
                {
                    int seq = myListView.Items.Count;
                    foreach (FileInfo _file in files)
                    {
                        seq++;
                        System.Windows.Forms.ListViewItem item = new System.Windows.Forms.ListViewItem(seq.ToString());
                        item.SubItems.Add(_file.Name);
                        item.SubItems.Add((string.Format("{0:hh\\:mm\\:ss}", CoreLibrary.GetNAudoSongLength(_file.FullName))));
                        //item.SubItems.Add(CoreLibrary.PrettyPrintBytes(_file.Length));
                        item.SubItems.Add(_file.FullName);
                        myListView.Items.Add(item);
                    }
                }
                if (bSetEnvironmentSpecialFolder)
                {
                    Environment.SpecialFolder root = folderBrowserDialog.RootFolder;
                }
            }
        }
        private void iconFileAdd_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDlg = new System.Windows.Forms.OpenFileDialog();
            // Allow the user to select multiple images.
            openFileDlg.Multiselect = true;
            openFileDlg.Title = "Add TOA Audio support Files";

            //openFileDlg.Filter = "Wave File (*.wav)|*.wav;";
            openFileDlg.Filter = "TOA Audio Support File (*.mp3;*.wav)|*.mp3;*.wav";
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                foreach (String file in openFileDlg.FileNames)
                {
                    FileInfo _file = new FileInfo(file);
                    if (_file != null)
                    {
                        int newSeq = myListView.Items.Count + 1;
                        System.Windows.Forms.ListViewItem item = new System.Windows.Forms.ListViewItem(newSeq.ToString());
                        item.SubItems.Add(_file.Name);
                        item.SubItems.Add((string.Format("{0:hh\\:mm\\:ss}", CoreLibrary.GetNAudoSongLength(_file.FullName))));
                        //item.SubItems.Add(CoreLibrary.PrettyPrintBytes(_file.Length));
                        item.SubItems.Add(_file.FullName);
                        myListView.Items.Add(item);
                    }
                }
            }
        }
        private void iconSongDelete_Click(object sender, EventArgs e)
        {
            DeleteSong();
        }
        private void DeleteSong()
        {
            bool bRemoveItem = false;
            if (myListView.SelectedIndices.Count > 0)
            {
                var confirmation = MessageBox.Show("Delete Song", "Confirm!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmation == DialogResult.Yes)
                {
                    for (int i = myListView.SelectedIndices.Count - 1; i >= 0; i--)
                    {
                        bRemoveItem = true;
                        myListView.Items.RemoveAt(myListView.SelectedIndices[i]);
                    }
                }
            }
            if (bRemoveItem && myListView.Items.Count>0) ReorderSongSequence();
        }
        private void iconShuffle_Click(object sender, EventArgs e)
        {
            if (iconShuffle.ForeColor == Color.FromArgb(255, 255, 128, 0))
            {
                IsShuffle = false;
                iconShuffle.ForeColor = Color.Black;
            }
            else {
                IsShuffle = true;
                iconShuffle.ForeColor = Color.FromArgb(255, 255, 128, 0); 
            }
            Console.WriteLine(IsShuffle);
        }
        private void iconLoop_Click(object sender, EventArgs e)
        {
            if (iconLoop.ForeColor == Color.FromArgb(255, 255, 128, 0))
            {
                IsLoop = false;
                iconLoop.ForeColor = Color.Black;
            }
            else {
                IsLoop = true;
                iconLoop.ForeColor = Color.FromArgb(255, 255, 128, 0); 
            }
            Console.WriteLine(IsLoop);
        }
        private void myListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.ListView.SelectedListViewItemCollection albums = this.myListView.SelectedItems;
            //double totalTime = 0.0;
            foreach (System.Windows.Forms.ListViewItem song in albums)
            {
                //totalTime += Double.Parse(item.SubItems[2].Text);
                string showMsgText = string.Format("[{0}] {1}", song.SubItems[0].Text, song.SubItems[1].Text);
                labelSelectedSong.Text = showMsgText;
            }
        }

        #region Private Function
        private void ReorderSongSequence()
        {
            for (int i = 0; i < myListView.Items.Count; i++)
            {
                //Console.WriteLine((myListView.Items[i].SubItems[0].Text));
                int newSeq = i + 1;
                if (int.Parse(myListView.Items[i].SubItems[0].Text) != newSeq)
                {
                    myListView.Items[i].SubItems[0].Text = newSeq.ToString();
                }
            }
        }
        #endregion

        private void myListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            if ((e.State & ListViewItemStates.Selected) != 0)
            {
                // Draw the background and focus rectangle for a selected item.
                e.Graphics.FillRectangle(Brushes.Maroon, e.Bounds);
                e.DrawFocusRectangle();
            }
            else
            {
                // Draw the background for an unselected item.
                using (LinearGradientBrush brush =
                    new LinearGradientBrush(e.Bounds, Color.Orange,
                    Color.Maroon, LinearGradientMode.Horizontal))
                {
                    e.Graphics.FillRectangle(brush, e.Bounds);
                }
            }

            // Draw the item text for views other than the Details view.
            if (myListView.View != View.Details)
            {
                e.DrawText();
            }
        }

        private void myListView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            TextFormatFlags flags = TextFormatFlags.Left;

            using (StringFormat sf = new StringFormat())
            {
                // Store the column text alignment, letting it default
                // to Left if it has not been set to Center or Right.
                switch (e.Header.TextAlign)
                {
                    case HorizontalAlignment.Center:
                        sf.Alignment = StringAlignment.Center;
                        flags = TextFormatFlags.HorizontalCenter;
                        break;
                    case HorizontalAlignment.Right:
                        sf.Alignment = StringAlignment.Far;
                        flags = TextFormatFlags.Right;
                        break;
                }

                // Draw the text and background for a subitem with a 
                // negative value. 
                double subItemValue;
                if (e.ColumnIndex > 0 && Double.TryParse(
                    e.SubItem.Text, NumberStyles.Currency,
                    NumberFormatInfo.CurrentInfo, out subItemValue) &&
                    subItemValue < 0)
                {
                    // Unless the item is selected, draw the standard 
                    // background to make it stand out from the gradient.
                    if ((e.ItemState & ListViewItemStates.Selected) == 0)
                    {
                        e.DrawBackground();
                    }

                    // Draw the subitem text in red to highlight it. 
                    e.Graphics.DrawString(e.SubItem.Text,
                        myListView.Font, Brushes.Red, e.Bounds, sf);

                    return;
                }

                // Draw normal text for a subitem with a nonnegative 
                // or nonnumerical value.
                e.DrawText(flags);
            }
        }
        private void myListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            /*          // No need to Column Click : P'SAM 2022-10-01  
             *          // Determine if clicked column is already the column that is being sorted.
                        if (e.Column == lvwColumnSorter.SortColumn)
                        {
                            // Reverse the current sort direction for this column.
                            if (lvwColumnSorter.Order == SortOrder.Ascending)
                            {
                                lvwColumnSorter.Order = SortOrder.Descending;
                            }
                            else
                            {
                                lvwColumnSorter.Order = SortOrder.Ascending;
                            }
                        }
                        else
                        {
                            // Set the column number that is to be sorted; default to ascending.
                            lvwColumnSorter.SortColumn = e.Column;
                            lvwColumnSorter.Order = SortOrder.Ascending;
                        }

                        // Perform the sort with these new sort options.
                        myListView.Sort();
            */
        }
        private void iconExportToXML_Click(object sender, EventArgs e)
        {
            using (System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog()
            {
                Filter = "Text Documents|*.txt",
                ValidateNames = true,
            })
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (TextWriter tw = new StreamWriter(new FileStream(saveFileDialog.FileName, FileMode.Create), Encoding.UTF8))
                    {
                        foreach (System.Windows.Forms.ListViewItem item in myListView.Items)
                        {
                            tw.WriteLine(item.SubItems[0].Text + "\t" + item.SubItems[1].Text + "\t" + item.SubItems[2].Text + "\t" + item.SubItems[3].Text + "\t" + item.SubItems[4].Text + "\t");
                        }
                    }
                }
            }
        }

        private void iconImportFromXML_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDlg = new System.Windows.Forms.OpenFileDialog();
            // Allow the user to select multiple images.
            openFileDlg.Multiselect = true;
            openFileDlg.Title = "Import Song List|*.txt";

            //openFileDlg.Filter = "Wave File (*.wav)|*.wav;";
            openFileDlg.Filter = "Song List|*.txt";
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                if (myListView.Items.Count>0)
                {
                    if (MessageBox.Show("ต้องการล้างรายการก่อนนำเข้าข้อมูล?(Y/N)", "Confirm Clear screen!!!", MessageBoxButtons.YesNo) == DialogResult.Yes) myListView.Items.Clear();
                }
                
                List<String> data = System.IO.File.ReadAllLines(openFileDlg.FileName).ToList();
                foreach(string _item in data)
                {
                    string[] items = _item.Split(new char[] { '\t' },StringSplitOptions.RemoveEmptyEntries);
                    //myListView.Items.Add(new System.Windows.Forms.ListViewItem(item));
                    
                    System.Windows.Forms.ListViewItem item = new System.Windows.Forms.ListViewItem(items[0]);
                    item.SubItems.Add(items[1]);
                    item.SubItems.Add(items[2]);
                    item.SubItems.Add(items[3]);
                    //item.SubItems.Add(items[4]);
                    myListView.Items.Add(item);

                }
                ReorderSongSequence();
            }

        }
    }
}
