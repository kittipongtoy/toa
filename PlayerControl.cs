using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;

using Microsoft.Win32;

using WMPLib;

using static System.Environment;
using static System.Net.WebRequestMethods;

using Color = System.Drawing.Color;
using File = System.IO.File;

namespace TOAMediaPlayer
{
    public partial class PlayerControl : System.Windows.Forms.UserControl
    {
        private WindowsMediaPlayer wplayer;
        string CurrentMediaName = string.Empty;
        public double PlayPositionTimeCode = 0;
        bool bNowPlaying = false;
        private bool bRandomMode = false;
        
        public IWMPPlaylist playlist;
        public IWMPPlaylistArray wMPPlaylistArray;
        public IWMPPlaylistCollection playlistCollection;
        public IWMPPlaylistCtrl playlistCtrl;

        public IWMPControls CtrlControl
        {
            get { return wplayer.controls; }
        }

        public IWMPNetwork Network
        {
            get { return wplayer.network; }
        }
        public IWMPMedia currentMedia { get; set; }
        public IWMPPlaylist currentPlaylist { get; set; }
        public PlayerControl()
        {
            InitializeComponent();
            wplayer = new WMPLib.WindowsMediaPlayer();
            wplayer.PlayStateChange += WindowsMediaPlayer_PlayStateChange;
            wplayer.PositionChange += WindowsMediaPlayer_PositionChange;
            wplayer.Buffering += WindowsMediaPlayer_Buffering;
            wplayer.CurrentItemChange += Player_CurrentItemChange;
            wplayer.CurrentPlaylistChange += Player_CurrentPlaylistChange;
            wplayer.CurrentPlaylistItemAvailable += Player_CurrentPlaylistItemAvailable;
            wplayer.MediaError += Player_MediaError;
            
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog.FileOk += OpenFileDialog_FileOk;
            // Allow the user to select multiple images.
            this.openFileDialog.Multiselect = true;
            this.openFileDialog.Title = "กรุณาเลือก";

            this.listBox1.DrawItem += ListBox1_DrawItem;
            timer.Interval = 100;

        }
        int itemIndex = -1;
        private void ListBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            Graphics g = e.Graphics;
            if (e.Index == itemIndex)
            {
                g.FillRectangle(new SolidBrush(Color.Pink), e.Bounds);
            }
            else
            {
                g.FillRectangle(new SolidBrush(Color.White), e.Bounds);
            }
            System.Windows.Forms.ListBox lb = (System.Windows.Forms.ListBox)sender;
            g.DrawString(listBox1.Items[e.Index].ToString(), e.Font,
                  new SolidBrush(Color.Black), new PointF(e.Bounds.X, e.Bounds.Y));
            e.DrawFocusRectangle();
        }

        private void Player_MediaError(object pMediaObject)
        {
            MessageBox.Show(String.Format("Cannot play media file. {0}", (string)pMediaObject));
        }

        private void Player_CurrentPlaylistItemAvailable(string bstrItemName)
        {
            MessageBox.Show(String.Format("Player_CurrentPlaylistItemAvailable {0}", bstrItemName));
        }

        private void Player_CurrentPlaylistChange(WMPPlaylistChangeEventType change)
        {
            switch (change)
            {
                // Only update the list for the move, delete, insert, and append event types.
                case WMPLib.WMPPlaylistChangeEventType.wmplcMove:    // Value = 3
                case WMPLib.WMPPlaylistChangeEventType.wmplcDelete:  // Value = 4
                case WMPLib.WMPPlaylistChangeEventType.wmplcInsert:  // Value = 5
                case WMPLib.WMPPlaylistChangeEventType.wmplcAppend:  // Value = 6

                    // Create a string array large enough to hold all of the media item names.
                    int count = wplayer.currentPlaylist.count;
                    string[] mediaItems = new string[count];

                    // Clear any previous contents of the text box.
                    //$EDIT:WIKORN
                    //playlistItems.Clear();

                    // Loop through the playlist and store each media item name.
                    for (int i = 0; i < count; i++)
                    {
                        mediaItems[i] = wplayer.currentPlaylist.get_Item(i).name;
                    }

                    // Display the media item names.
                    //$EDIT:WIKORN
                    //playlistItems.Lines = mediaItems;
                    break;

                default:
                    break;
            }
        }

        private void Player_CurrentItemChange(object pdispMedia)
        {
            CurrentMediaName = wplayer.currentMedia.name;
        }

        ~PlayerControl()
        {
            wplayer.close();
        }
        private void WindowsMediaPlayer_Buffering(bool Start)
        {
            Console.Write(String.Format("{0}", Start));
        }

        private void WindowsMediaPlayer_PositionChange(double oldPosition, double newPosition)
        {
            MessageBox.Show(String.Format("olePosition {0} , newPosition {1}", oldPosition, newPosition));
        }

        /*
         Player Id
        AlbumID
        Current SongName
        currentPositionTime
         */

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
        // VidSecs As Integer = Math.Round(GetMediaDuration(YourFilePath))'Get total seconds
        // Vidhhmmss As String = TimeSpan.FromSeconds(Math.Round(GetMediaDuration(YourFilePath))).ToString ' Format hh:mm:ss

        protected virtual void OnPositionTimeCodeChanged(EventArgs e)
        {
            if (PositionTimeCodeChanged != null)
            {
                TimeSpan interval;
                if (TimeSpan.TryParseExact(currentPositionTimeCode, @"hh\:mm\:ss\.fff", null, out interval))
                {
                    this.PlayPositionTimeCode = interval.TotalSeconds;
                    wplayer.controls.currentPosition = PlayPositionTimeCode;
                    //wplayer.controls.currentPosition = double.Parse(String.Format("{0:HH-mm-ss}", this.PlayPositionTimeCode));
                    PositionTimeCodeChanged(this, e);
                }
            }
        }


        #region Background System.Drawing.Color
        private System.Drawing.Color backgroundColor;
        public event EventHandler TOABackgroundColorChanged;
        public System.Drawing.Color TOABackgroundColor
        {
            get
            {
                return backgroundColor;
            }
            set
            {
                if (backgroundColor != value)
                {
                    backgroundColor = value;
                    TOAOnBackgroundColorChanged(EventArgs.Empty);
                }
            }
        }
        protected virtual void TOAOnBackgroundColorChanged(EventArgs e)
        {
            if (TOABackgroundColorChanged != null)
            {
                this.BackColor = backgroundColor;
                TOABackgroundColorChanged(this, e);
            }
        }
        #endregion


        private System.Drawing.Brush playerBrush = null;
        public System.Drawing.Brush BackBrush
        {
            get
            {
                if (playerBrush == null)
                {
                    playerBrush = new SolidBrush(System.Drawing.Color.Black);
                }
                return playerBrush;
            }
            set
            {
                if (playerBrush != value)
                {
                    playerBrush = value;
                    Invalidate();
                }
            }
        }

        
        private void WindowsMediaPlayer_PlayStateChange(int NewState)
        {
            if (NewState == (int)WMPPlayState.wmppsMediaEnded)
            {
                //wplayer.close();
                //wplayer = null;
                this.bNowPlaying= false;
                this.trackBarSongPlay.Value = 0;
                ResetWhenStop();
                if (this.bRandomMode)
                {
                    RandomPlay();
                }
                //try
                //{
                //    System.Diagnostics.Process[] prc = System.Diagnostics.Process.GetProcessesByName("wmplayer");
                //    if (prc.Length > 0)
                //        prc[prc.Length - 1].Kill();
                //}
                //catch (Exception)
                //{
                //}
            }
            if (NewState == (int)WMPPlayState.wmppsReady)
            {
                MessageBox.Show(Enum.GetName(typeof(WMPPlayState), NewState));
            }
            lblStateChangeMsg.Text = Enum.GetName(typeof(WMPPlayState), NewState);
        }
        private double GetAudioDuration(String mediaFileName)
        {
            double songDuration = 0;
            try
            {
                WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();
                WMPLib.IWMPMedia media = wplayer.newMedia(mediaFileName);
                songDuration = media.duration;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return songDuration;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) return;
            try
            {

                //CurrentMediaName = listBox1.SelectedValue.ToString();
                CurrentMediaName = listBox1.SelectedItem.ToString();
                if (!File.Exists(CurrentMediaName)) throw new Exception("Exception Message Here");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (!this.bNowPlaying) {
                timer.Enabled = false;
                return;
            }
            try
            {
                trackBarSongPlay.Value = (int)wplayer.controls.currentPosition;
                labelPlayTime.Text = string.Format(@"{0:HH\:mm\:ss\.fff}", wplayer.controls.currentPositionString);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {

            }
        }

        //private void btnStop_Click(object sender, EventArgs e)
        //{
        //    if (wplayer.playState == WMPPlayState.wmppsPlaying)
        //    {
        //        wplayer.controls.stop();
        //        timer.Stop();
        //    }
        //}

        private void playButton_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count <= 0) return;
            Play();

        }
        private void stopButton_Click(object sender, EventArgs e)
        {
            Stop();
        }

        public void Play()
        {
            listBox1.SelectedIndex = 0;
            CurrentMediaName = listBox1.SelectedItem.ToString();
            if (CurrentMediaName == string.Empty) return;
            if (bNowPlaying && playButton.IconChar == FontAwesome.Sharp.IconChar.Pause)
            {
                if (wplayer.playState == WMPPlayState.wmppsPlaying)
                {
                    playButton.IconChar = FontAwesome.Sharp.IconChar.Play;
                    playButton.IconColor = System.Drawing.Color.Black;
                    panel1.BackColor = System.Drawing.Color.FromArgb(42, 42, 42);
                    wplayer.controls.pause();
                    bNowPlaying = false;
                    timer.Enabled = true;
                }
                else
                {
                    playButton.IconChar = FontAwesome.Sharp.IconChar.Pause;
                    playButton.IconColor = System.Drawing.Color.Orange;
                    wplayer.controls.pause();
                    panel1.BackColor = System.Drawing.Color.SaddleBrown;
                    timer.Enabled = false;
                    trackBarSongPlay.Minimum = 0;
                    trackBarSongPlay.Maximum = (int)GetAudioDuration(listBox1.SelectedItem.ToString());
                    //trackBarSongPlay.Maximum = (int)GetAudioDuration(CurrentMediaName);
                    currentPositionTimeCode = wplayer.currentMedia.durationString;
                    labelDurationTime.Text = string.Format(@"{0:hh\:mm\:ss\.fff}", wplayer.currentMedia.durationString);
                    labelSongName.Text = string.Format(@"{0}", wplayer.currentMedia.name);

                }
            }
            else
            {
                switch (wplayer.playState)
                {
                    case WMPPlayState.wmppsPaused:
                        if (!bNowPlaying && (playButton.IconChar == FontAwesome.Sharp.IconChar.Pause))
                        {
                            playButton.IconChar = FontAwesome.Sharp.IconChar.Play;
                            playButton.IconColor = System.Drawing.Color.Black;
                            panel1.BackColor = System.Drawing.Color.SaddleBrown;
                            timer.Enabled = true;
                        }
                        else
                        {
                            playButton.IconChar = FontAwesome.Sharp.IconChar.Pause;
                            playButton.IconColor = System.Drawing.Color.Orange;
                            wplayer.controls.play();
                            panel1.BackColor = System.Drawing.Color.SaddleBrown;
                            timer.Enabled = true;
                            bNowPlaying = true;
                        }
                        //lblStateChangeMsg.Text = PLAY_STATUS_PAUSE;
                        break;

                    case WMPPlayState.wmppsStopped:
                        wplayer.URL = CurrentMediaName;
                        playButton.IconChar = FontAwesome.Sharp.IconChar.Play;
                        //lblStateChangeMsg.Text = string.Empty;
                        timer.Enabled = true;
                        break;

                    case WMPPlayState.wmppsUndefined:
                    case WMPPlayState.wmppsTransitioning:
                        wplayer.URL = CurrentMediaName;
                        playButton.IconChar = FontAwesome.Sharp.IconChar.Pause;
                        playButton.IconColor = System.Drawing.Color.Orange;
                        //                    wplayer.controls.play();
                        panel1.BackColor = System.Drawing.Color.SaddleBrown;
                        bNowPlaying = true;
                        timer.Start();
                        break;
                    case WMPPlayState.wmppsReady:
                        playButton.IconChar = FontAwesome.Sharp.IconChar.Pause;
                        playButton.IconColor = System.Drawing.Color.Orange;
                        wplayer.controls.play();
                        panel1.BackColor = System.Drawing.Color.SaddleBrown;
                        timer.Enabled = true;
                        bNowPlaying = true;
                        break;
                    default:
                        break;
                }

                trackBarSongPlay.Minimum = 0;
                trackBarSongPlay.Maximum = (int)GetAudioDuration(listBox1.SelectedItem.ToString());
                //trackBarSongPlay.Maximum = (int)GetAudioDuration(CurrentMediaName);
                currentPositionTimeCode = wplayer.currentMedia.durationString;
                labelDurationTime.Text = string.Format(@"{0:hh\:mm\:ss\.fff}", wplayer.currentMedia.durationString);
                labelSongName.Text = string.Format(@"{0}", wplayer.currentMedia.name);

            }

        }
        public void Stop()
        {
            if (wplayer.playState == WMPPlayState.wmppsPlaying)
            {
                playButton.IconChar = FontAwesome.Sharp.IconChar.Play;
                wplayer.controls.stop();
                wplayer.URL = String.Empty;
                bNowPlaying = false;
                timer.Stop();
                ResetWhenStop();
            }
            //wplayer.URL = string.Empty;
            //playButton.IconChar = FontAwesome.Sharp.IconChar.Play;
            //timer.Enabled = true;
            //panel1.BackColor = System.Drawing.Color.SaddleBrown;
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedItem == null) return;
            if (e.Button == MouseButtons.Left)
            {
                //textBox1.Select(0, textBox1.Text.Length);
                try
                {
                    //CurrentMediaName = listBox1.SelectedValue.ToString();
                    CurrentMediaName = listBox1.SelectedItem.ToString();
                    if (!File.Exists(CurrentMediaName)) throw new Exception("Exception Message Here");
                    wplayer.URL = CurrentMediaName;
                    Play();
                    //timer.Start();
                    //trackBarSongPlay.Minimum = 0;
                    //trackBarSongPlay.Maximum = (int)GetAudioDuration(listBox1.SelectedItem.ToString());

                    //labelDurationTime.Text = string.Format(@"{0:hh\:mm\:ss}", wplayer.currentMedia.durationString);
                    //labelSongName.Text = string.Format(@"{0}", wplayer.currentMedia.name);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void ResetWhenStop()
        {
            playButton.IconChar = FontAwesome.Sharp.IconChar.Play;
            playButton.IconColor = System.Drawing.Color.Black;
            labelPlayTime.Text = "00:00";
            labelDurationTime.Text = "00:00";
            labelSongName.Text = String.Empty;
            panel1.BackColor = System.Drawing.Color.FromArgb(42, 42, 42);
        }

        private void rewardButton_Click(object sender, EventArgs e)
        {

            if (listBox1.SelectedItem == null) return;
            if (listBox1.SelectedIndex > 0)
            {
                listBox1.SelectedIndex = listBox1.SelectedIndex - 1;
            }
            //CurrentMediaName = listBox1.SelectedValue.ToString();
            CurrentMediaName = listBox1.SelectedItem.ToString();
            wplayer.URL = CurrentMediaName;
            Play();
        }

        private void forwardButton_Click(object sender, EventArgs e)

        {
            if (listBox1.SelectedItem == null) return;

            if (listBox1.SelectedIndex < listBox1.Items.Count - 1)
            {
                listBox1.SelectedIndex = listBox1.SelectedIndex + 1;
            }
            //CurrentMediaName = listBox1.SelectedValue.ToString();
            CurrentMediaName = listBox1.SelectedItem.ToString();
            wplayer.URL = CurrentMediaName;
            Play();
        }

        private void decreaseVolumeButton_Click(object sender, EventArgs e)
        {
            if (wplayer.settings.volume < 0)
            {
                wplayer.settings.volume = 0;
            }
            else
            {
                wplayer.settings.volume -= 1;
            }
            labelCurrentVolumeLevel.Text = String.Format("{0} %", wplayer.settings.volume);
        }

        private void increaseVolumeButton_Click(object sender, EventArgs e)
        {
            if (wplayer.settings.volume > 100)
            {
                wplayer.settings.volume = 100;
            }
            else
            {
                wplayer.settings.volume += 1;
            }
            labelCurrentVolumeLevel.Text = String.Format("{0} %", wplayer.settings.volume);
        }

        private void decreaseVolumeButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                if (wplayer.settings.volume < 0)
                {
                    wplayer.settings.volume = 0;
                }
                else
                {
                    wplayer.settings.volume -= 5;
                }
                labelCurrentVolumeLevel.Text = String.Format("{0} %", wplayer.settings.volume);
            }
        }

        private void increaseVolumeButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                if (wplayer.settings.volume > 100)
                {
                    wplayer.settings.volume = 100;
                }
                else
                {
                    wplayer.settings.volume += 5;
                }
                labelCurrentVolumeLevel.Text = String.Format("{0} %", wplayer.settings.volume);
            }
        }

        private void trackBarSongPlay_MouseDown(object sender, MouseEventArgs e)
        {
            timer.Enabled = false;
        }

        private void trackBarSongPlay_MouseUp(object sender, MouseEventArgs e)
        {
            if (!bNowPlaying) return;
            try
            {
                timer.Enabled = false;
                wplayer.controls.currentPosition = trackBarSongPlay.Value;
                timer.Enabled = true;

            }
            catch (Exception ex)
            {
                //RaiseError(ex);
                MessageBox.Show(ex.Message);
            }
            finally
            {

            }
        }

        private void trackBarSongPlay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Left)
            {
                try
                {
                    timer.Enabled = false;
                    wplayer.controls.currentPosition -= 1;
                    timer.Enabled = true;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            if (e.KeyData == Keys.Right)
            {
                try
                {
                    timer.Enabled = false;
                    wplayer.controls.currentPosition += 1;
                    timer.Enabled = true;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void btnFolderBrowser_Click(object sender, EventArgs e)
        {
            bool bShowNewFolderButton = Properties.Settings.Default.ShowNewFolderButton;
            bool bSetEnvironmentSpecialFolder = Properties.Settings.Default.SetEnvironmentToSpecialFolder;
            this.folderBrowserDialog.ShowNewFolderButton = bShowNewFolderButton;
            if (this.folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                List<string> _files = GetListOfFiles(folderBrowserDialog.SelectedPath);
                if (_files.Count == 0) return;
                foreach (string _file in _files)
                {
                    listBox1.BeginUpdate();		//Stop painting of the ListBox as items are added
                    listBox1.Items.Add(_file);
                    listBox1.EndUpdate();
                }
                //listBox1.DataSource = _files;
                //listBox1.DisplayMember = "MediaFiles";
                if (bSetEnvironmentSpecialFolder)
                {
                    Environment.SpecialFolder root = folderBrowserDialog.RootFolder;
                }

            }
        }

        private void OpenFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            string[] files = openFileDialog.FileNames;
            // Open each file and display the image in pictureBox1.
            // Call Application.DoEvents to force a repaint after each
            // file is read.        
            string fileNameInfo = string.Empty;
            ushort fileCount = 0;
            //int lCount = listBox1.Items.Count;
            foreach (string file in files)
            {
                fileCount += 1;
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(file);
                //System.IO.FileStream fileStream = fileInfo.OpenRead();
                fileInfo.Attributes = FileAttributes.Normal;
                fileNameInfo += fileCount + ". " + fileInfo.Name + "\r\n";
                listBox1.BeginUpdate();
                listBox1.Items.Add(file);
                listBox1.EndUpdate();
            }
            MessageBox.Show("Your Selected: \r\n" + fileNameInfo);
        }

        private void btnFileBrowser_Click(object sender, EventArgs e)
        {
            string strLastMediaDirectory = Properties.Settings.Default.LastMediaDirectory;
            String fileName = string.Empty;
            //If last directory is not valid then default to My Documents (if you don't include this the catch below won't occur for null strings so the start directory is undefined)
            if (!Directory.Exists(strLastMediaDirectory))
                strLastMediaDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            openFileDialog.Filter = "MPEG-1 Audio Layer-3|*.mp3|Microsoft Windows Media Audio Format|*.wma|Waveform Audio File Format|*.wav|Advanced Audio Coding|*.aac|Free Lossless Audio Codec|*.flac|Audio Video Interleave|*.avi|Apple QuickTime Movie|*.mov|Windows Media Video|*.wmv|MPEG-4 Part 14|*.mp4|Moving Picture Experts Group Phase 1 (MPEG-1)|*.MPEG|TOA Player Support Files (MP3,WMA,WAV,AAC,FLAC,AVI,MOV,WMV,MP4,MPEG)|*.mp3;*.wma;*.wav;*.aac;*.flac;*.avi;*.mov;*.wmv;*.mp4;*.mpeg";
            openFileDialog.FilterIndex = 1;               //(First entry is 1, not 0)
            try
            {
                openFileDialog.InitialDirectory = strLastMediaDirectory;
            }
            catch (Exception)
            {
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //STORE LAST USED DIRECTORY
                if (fileName.LastIndexOf("\\") >= 0)
                    strLastMediaDirectory = fileName.Substring(0, (fileName.LastIndexOf("\\") + 1));
                Properties.Settings.Default["LastMediaDirectory"] = strLastMediaDirectory;
                Properties.Settings.Default.Save();
            }
        }
        private List<string> GetListOfFiles(string targetDirectory)
        {
            List<string> FileList = new List<string>();
            DirectoryInfo di = new DirectoryInfo(targetDirectory);

            IEnumerable<FileInfo> fileList = di.GetFiles("*.*");

            //Create the query
            IEnumerable<FileInfo> fileQuery = from file in fileList
                                              where (file.Extension.ToLower() == ".mp3" || file.Extension.ToLower() == ".wav")
                                              orderby file.LastWriteTime
                                              select file;

            foreach (System.IO.FileInfo fi in fileQuery)
            {
                fi.Attributes = FileAttributes.Normal;
                FileList.Add(fi.FullName);
            }
            return FileList;
        }

        private void btnItemListRemove_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0) return;
            listBox1.Items.Remove(listBox1.SelectedItem);
        }

        private void LoadXmlToPlayList(string filename)
        {
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(filename);

            //Remove the xml file header
            foreach (XmlNode node in XmlDoc)
            {
                if (node.NodeType == XmlNodeType.XmlDeclaration)
                {
                    XmlDoc.RemoveChild(node);
                }
            }
        }
        private int GenerateRandomString(int length)
        {
            System.Random rand = new System.Random();       //Creates seed value from system clock if no seed supplied
            int iTemp = rand.Next(0, (length));				//Min possible value, max possible value+1
            return (iTemp);
        }

        private void btnLoop_Click(object sender, EventArgs e)
        {
            wplayer.settings.setMode("loop", true);
        }

        
        private void btnRandom_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0) return;
            if (bRandomMode == false) {
                bRandomMode = true;
            }else {
                bRandomMode = false;
            }
            if (bRandomMode)
            {
                RandomPlay();
            }
        }

        private void RandomPlay()
        {
            int index = GenerateRandomString(listBox1.Items.Count);
            try
            {
                listBox1.SelectedIndex = index;
                setListBoxColor(index);
                CurrentMediaName = listBox1.SelectedItem.ToString();
                if (!File.Exists(CurrentMediaName)) throw new Exception("Exception Message Here");
                wplayer.URL = CurrentMediaName;
                Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        void setListBoxColor(int index)
        {
            itemIndex = index;
            listBox1.DrawMode = DrawMode.Normal;
            listBox1.DrawMode = DrawMode.OwnerDrawFixed;
        }

    }
}
