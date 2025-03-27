using Microsoft.Win32;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Windows.Forms;
using TOAMediaPlayer.TOAPlaylist;
using TOAMediaPlayer.Utils;

namespace TOAMediaPlayer.NAudioOutput
{

    public partial class NPlayer : UserControl
    {
        //NAudio
        private float _volume = 0f;
        public IWavePlayer wavePlayer;

        public EventArgs es;
        public string nametrack = "";
        public string nametrackC = "";
        public string nametrackCF = "";
        public string timertrack = "";
        public string textfont = "";
        public string fgcolor = "";
        public string bgcolor = "";

        public string dB = "";
        public string adB = "";

        public static bool selectmusic = false;
        public int runmusicindex = 0;
        public int runmusicindexOld = 0;
        public int stopindexmusic = -1;
        public MainPlayer pl;
        public bool runplaylist = false;
        public int settingplayertrack = 0;
        public string filenamerun = "";
        public bool prevnext = false;
        public bool selectmusic1 = false;
        public bool playing = false;
        public Loggers log = new Loggers("debugs.txt");

        public List<int> playbefore = new List<int>();

        public NPlayer pp;

        //private string fileName;
        public AudioFileReader audioFileReader;
        private Action<float> setVolumeDelegate;
        private RegistryKey HKLMSoftwareTOAPlayer = Registry.CurrentUser.OpenSubKey(@"Software\TOA\Player", true);
        private RegistryKey HKLMSoftwareTOAPlayer1 = Registry.CurrentUser.OpenSubKey(@"Software\TOA\Config\trackname", true);
        RegistryKey HKLMSoftwareTOAConfig = Registry.CurrentUser.OpenSubKey(@"Software\TOA\Config", true);

        private PlaybackState mediaPlaybackState = PlaybackState.Stopped;

        //private Queue<TOATrackName> QTracks = new Queue<TOATrackName>();
        //private Queue<OTSPlaylist> QTracks = new Queue<OTSPlaylist>();
        private Queue<OTSMedia> QueueOTSMedia = new Queue<OTSMedia>();
        public List<OTSMedia> ListOTSMedia = new List<OTSMedia>();
        public List<string> playlist = new List<string>();
        //private List<OTSPlaylist> playlist = new List<OTSPlaylist>();
        private Color NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));

        private Color InactiveBGColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));

        private Color ActiveBGColorWithoutAlpha = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(255)))), ((int)(((byte)(130)))), ((int)(((byte)(0)))));
        private Color ActiveBGColorWithAlpha = System.Drawing.Color.FromArgb(20, 255, 130, 0);

        public void Thread1(NPlayer p)
        {
            Thread.Sleep(5000);
            var runfi = false;

            while (true)
            {
                if (this.timertrack != "")
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    var DD = DateTime.Now;
                    var DDSR = Convert.ToDateTime(DD).ToString("ddd", culture);
                    RegistryKey configsocket = this.HKLMSoftwareTOAConfig.OpenSubKey("trackname", true);
                    var SPD = configsocket.GetValue(this.timertrack).ToString().Split(',');
                    var days = SPD[1].Split('-');
                    var start = SPD[2].Split(':');
                    var end = SPD[3].Split(':');


                    if (SPD[0] == "active")
                    {
                        if (SPD[4] == "active")
                        {
                            var start11 = "";
                            var end11 = "";
                            if (SPD[2].IndexOf("AM") != -1 || SPD[2].IndexOf("PM") != -1)
                            {
                                DateTime dateTime = DateTime.ParseExact(SPD[2], "hh:mm tt", CultureInfo.InvariantCulture);
                                start11 = dateTime.Hour.ToString();
                                end11 = dateTime.Minute.ToString();
                            }
                            else
                            {
                                start11 = start[0];
                                end11 = start[1];
                            }
                            if (DD.Hour == Convert.ToInt32(start11) && DD.Minute == Convert.ToInt32(end11))
                            {
                                foreach (var s in days)
                                {

                                    if (DDSR.ToLower() == s.ToLower())
                                    {
                                        if (wavePlayer == null || wavePlayer.PlaybackState != PlaybackState.Playing)
                                        {
                                            this.PlayButton_Click(p, p.es);

                                        }
                                        break;
                                    }
                                }
                            }

                        }
                        if (SPD[5] == "active")
                        {
                            var start11 = "";
                            var end11 = "";
                            if (SPD[3].IndexOf("AM") != -1 || SPD[3].IndexOf("PM") != -1)
                            {
                                DateTime dateTime = DateTime.ParseExact(SPD[3], "hh:mm tt", CultureInfo.InvariantCulture);
                                start11 = dateTime.Hour.ToString();
                                end11 = dateTime.Minute.ToString();
                            }
                            else
                            {
                                start11 = end[0];
                                end11 = end[1];
                            }
                            if (DD.Hour == Convert.ToInt32(start11) && DD.Minute == Convert.ToInt32(end11))
                            {
                                foreach (var s in days)
                                {

                                    if (DDSR.ToLower() == s.ToLower())
                                    {
                                        if (wavePlayer != null)
                                        {
                                            if (wavePlayer.PlaybackState == PlaybackState.Playing)
                                            {
                                                this.StopButton_Click(p, p.es);
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                if (runfi == false)
                {
                    var kuys1 = DateTime.Now;

                    Thread.Sleep(((60 - kuys1.Second) * 1000));
                    runfi = true;

                }
                else
                {
                    Thread.Sleep(60000);

                }
            }
        }

        public void set_prevnext(bool t)
        {
            this.prevnext = t;
        }
        public void set_selectmusic(bool t)
        {
            this.selectmusic1 = t;
        }

        public string get_fgcolor()
        {
            return fgcolor;
        }

        public string get_bgcolor()
        {
            return bgcolor;
        }

        public string get_textfont()
        {
            return textfont;
        }

        public void set_runplaylist(bool t)
        {
            runplaylist = t;
        }
        public void setPeek(NPlayer p)
        {
            pp = p;
        }
        public bool get_runplaylist()
        {
            return runplaylist;
        }
        public void setnametrack(string nametrack, string nametrackC, string nametrackCF, string dB, string adB)
        {
            RegistryKey configsocket = this.HKLMSoftwareTOAConfig.OpenSubKey("trackname", true);
            this.nametrack = nametrack;
            this.nametrackC = nametrackC;
            this.nametrackCF = nametrackCF;
            this.adB = adB;
            this.dB = dB;
            if (configsocket.GetValue("dB1") == null)
            {
                configsocket.SetValue("dB1", "0");
                configsocket.SetValue("dB2", "0");
                configsocket.SetValue("dB3", "0");
                configsocket.SetValue("dB4", "0");
                configsocket.SetValue("dB5", "0");
                configsocket.SetValue("dB6", "0");
                configsocket.SetValue("dB7", "0");
                configsocket.SetValue("dB8", "0");

            }


            if (configsocket.GetValue("adB1") == null)
            {
                configsocket.SetValue("adB1", "unactive");
                configsocket.SetValue("adB2", "unactive");
                configsocket.SetValue("adB3", "unactive");
                configsocket.SetValue("adB4", "unactive");
                configsocket.SetValue("adB5", "unactive");
                configsocket.SetValue("adB6", "unactive");
                configsocket.SetValue("adB7", "unactive");
                configsocket.SetValue("adB8", "unactive");

            }
        }

        public void setTimers(string timertrack, MainPlayer pps)
        {
            this.timertrack = timertrack;
            this.pl = pps;
        }

        public void setMainPlay(MainPlayer pps)
        {
            this.pl = pps;
        }
        //===================================
        public delegate void updateTextBoxDelegate(String tetBoxString);
        public delegate void updateTextBoxDelegate1(Color tetBoxString);
        public updateTextBoxDelegate updateTextBox;
        public updateTextBoxDelegate updateTextBox11;
        public updateTextBoxDelegate updateTextBox22;

        public updateTextBoxDelegate1 updateTextBox33;

        void updateTextBox1(string str) { lblVolumeLevel.Text = str; }

        void updateTextBox2(string str) { labelTotalTime.Text = str; }
        void updateTextBox3(string str) { lblPlaySongName.Text = str; }
        void updateTextBox4(Color str) { trackBarPosition.BackColor = str; }
        //===================================
        public NPlayer()
        {
            InitializeComponent();
            QueueOTSMedia.Clear();
            ListOTSMedia.Clear();
            updateTextBox = new updateTextBoxDelegate(updateTextBox1);
            updateTextBox11 = new updateTextBoxDelegate(updateTextBox2);
            updateTextBox22 = new updateTextBoxDelegate(updateTextBox3);
            updateTextBox33 = new updateTextBoxDelegate1(updateTextBox4);
            Thread t1 = new Thread(new ThreadStart(delegate { Thread1(this); }));
            t1.Start();
        }

        public void set_runorder(int id)
        {
            this.runmusicindex = id;
        }

        public void setreg(string name, string value)
        {

        }

        public void setting_main(MainPlayer pls, string name, int playertrack)
        {
            this.pl = pls;
            this.label2.Text = name;
            this.textfont = name;
            this.settingplayertrack = playertrack;
        }
        public void setting_color(MainPlayer pls, string name)
        {
            bool flag = name != "" && name != " ";
            this.bgcolor = name;
            if (flag)
            {
                bool flag2 = name.IndexOf(",") == -1;
                if (flag2)
                {
                    this.panel1.BackColor = Color.FromName(name);
                }
                else
                {
                    string[] subtext = name.Split(new char[]
                    {
                        ','
                    });
                    this.panel1.BackColor = Color.FromArgb(Convert.ToInt32(subtext[3]), Convert.ToInt32(subtext[0]), Convert.ToInt32(subtext[1]), Convert.ToInt32(subtext[2]));
                }
            }
        }
        public void setting_font_color(MainPlayer pls, string name)
        {
            bool flag = name != "" && name != " ";
            this.fgcolor = name;
            if (flag)
            {
                bool flag2 = name.IndexOf(",") == -1;
                if (flag2)
                {
                    this.label2.ForeColor = Color.FromName(name);
                }
                else
                {
                    string[] subtext = name.Split(new char[]
                    {
                        ','
                    });
                    this.label2.ForeColor = Color.FromArgb(Convert.ToInt32(subtext[3]), Convert.ToInt32(subtext[0]), Convert.ToInt32(subtext[1]), Convert.ToInt32(subtext[2]));
                }
            }
        }
        public void set_color_back(string color)
        {
            var colors = color.Split(',');
            this.panel1.BackColor = Color.FromArgb(Convert.ToInt32(colors[0]), Convert.ToInt32(colors[1]), Convert.ToInt32(colors[2]), Convert.ToInt32(colors[3]));
        }
        public void set_color_text(string color)
        {
            var colors = color.Split(',');
            this.label2.ForeColor = Color.FromArgb(Convert.ToInt32(colors[0]), Convert.ToInt32(colors[1]), Convert.ToInt32(colors[2]), Convert.ToInt32(colors[3]));
        }
        public void set_text(string text)
        {
            this.label2.Text = text;
        }

        #region Player Name
        private String playerName;
        public event EventHandler PlayerNameChanged;
        public string PlayerName
        {
            get { return this.playerName; }
            set
            {
                if (this.playerName != value)
                {
                    this.playerName = value;
                    //LoadOutputDevicePlugins(ReflectionHelper.CreateAllInstancesOf<IOutputDevicePlugin>());
                    OnPlayerNameChanged(EventArgs.Empty);
                }
            }
        }
        protected virtual void OnPlayerNameChanged(EventArgs e)
        {
            if (PlayerNameChanged != null)
            {
                //Add Output Device
                // Load Output Device per Player 
                // Extend
                PlayerNameChanged(this, e);
            }
        }
        #endregion

        public PlaybackState MediaPlaybackState
        {
            get => this.mediaPlaybackState;
            set => mediaPlaybackState = value;
        }
        public float Volume
        {
            get => _volume;
            set => _volume = value;
        }

        public string _playedFileName;
        public string PlayedFileName
        {
            get => _playedFileName;
            set => _playedFileName = value;
        }
        #region CurrentMedia
        private OTSMedia currentMedia;
        public event EventHandler CurrentMediaChanged;
        public OTSMedia CurrentMedia
        {
            get
            {
                return this.currentMedia;
            }
            set
            {
                currentMedia = value;
                OnCurrentMediaChanged(EventArgs.Empty);
            }
        }
        protected virtual void OnCurrentMediaChanged(EventArgs e)
        {
            if (CurrentMediaChanged != null)
            {
                this.QueueOTSMedia.Enqueue(currentMedia);
                PlayButton_Click(null, e);
                CurrentMediaChanged(this, e);
            }
        }
        #endregion 
        //private TOATrackName track = new TOATrackName();
        //public event EventHandler TrackChanged;
        //public TOATrackName SongTrack
        //{
        //    get
        //    {
        //        return track;
        //    }
        //    set
        //    {
        //        track = value;
        //        OnTrackChanged(EventArgs.Empty);
        //    }
        //}
        //protected virtual void OnTrackChanged(EventArgs e)
        //{
        //    if (TrackChanged != null)
        //    {
        //        this.QTracks.Enqueue(track);
        //        PlayButton_Click(null, e);
        //        TrackChanged(this, e);
        //    }
        //}
        public void set_stop(int id)
        {
            stopindexmusic = id;
        }
        public void stop_music_name(int id)
        {
            if (wavePlayer != null)
            {
                if ((id + 1) == runmusicindex)
                {
                    stopindexmusic = id;
                }
            }
        }

        public void clear_music()
        {
            if (wavePlayer != null)
            {
                if (wavePlayer.PlaybackState == PlaybackState.Playing || wavePlayer.PlaybackState == PlaybackState.Paused)
                {
                    wavePlayer.Stop();
                }
                ListOTSMedia.Clear();
                playlist.Clear();
            }
            else
            {
                ListOTSMedia.Clear();
                playlist.Clear();
            }
        }

        private OTSPlaylist _CurrentPlaylist;
        public event EventHandler CurrentPlaylistChanged;
        public OTSPlaylist CurrentPlaylist
        {
            get
            {
                return this._CurrentPlaylist;
            }
            set
            {
                if (value != null)
                {
                    playlist.Clear();
                    _CurrentPlaylist = value;
                    OnCurrentPlaylistChanged(EventArgs.Empty);
                }
                if (stopindexmusic != -1)
                {
                    PlayerControlStop();
                    stopindexmusic = -1;
                }
            }
        }
        protected virtual void OnCurrentPlaylistChanged(EventArgs e)
        {

            if (CurrentPlaylistChanged != null)
            {
                //this.QTracks.Enqueue(_CurrentPlaylist);
                //PlayButton_Click(null, e);
                if (_CurrentPlaylist.count > 0)
                {
                    ListOTSMedia.Clear();
                    //Select Media In PlayList
                    for (int i = 0; i < _CurrentPlaylist.count; i++)
                    {
                        ListOTSMedia.Add((OTSMedia)_CurrentPlaylist[i]);
                    }
                }
                if (ListOTSMedia.Count() > 0)
                {

                    foreach (var xc in ListOTSMedia)
                    {
                        playlist.Add(xc.fileLocation);
                    }
                }
                if (_CurrentPlaylist.count == 0)
                {
                    ListOTSMedia.Clear();
                    playlist.Clear();
                }
                CurrentPlaylistChanged(this, e);
            }
            if (get_runplaylist() == false)
            {
                playlist.Clear();
                if (wavePlayer != null)
                {
                    if (ListOTSMedia.Count() > 0)
                    {

                        foreach (var xc in ListOTSMedia)
                        {
                            playlist.Add(xc.fileLocation);
                        }
                    }
                }

            }
            else
            {
                if (ListOTSMedia.Count() > 0)
                {
                    List<string> playlist1 = new List<string>();
                    foreach (var cf in playlist)
                    {
                        var cc = ListOTSMedia.Where(x => x.fileLocation.Equals(cf)).FirstOrDefault();
                        if (cc == null)
                        {
                            playlist1.Add(cf);
                        }
                    }
                    foreach (var ff in playlist1)
                    {
                        playlist.Remove(ff);
                    }
                }
            }
        }

        //List<IOutputDevicePlugin> OutputDeviceList =new List<IOutputDevicePlugin>();
        #region NAudio Call Funcation
        //private IOutputDevicePlugin SelectedOutputDevicePlugin
        //{
        //    get { 

        //        return (IOutputDevicePlugin)comboBoxOutputDevice.SelectedItem; 
        //    }
        //}
        #endregion


        //private IOutputDevicePlugin SelectedOutputDevicePlugin
        //{
        //    get { return (IOutputDevicePlugin)comboBoxOutputDevice.SelectedItem; }
        //}

        void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {

        }

        private void BackwardButton_Click(object sender, EventArgs e)
        {
            //bool bOK = false;
            //if (playlist.Count > 0) bOK = true;
            if (this.playing == true) return;
            if (playlist.Count() != null && playlist.Count() != 0)
            {
                if (wavePlayer.PlaybackState == PlaybackState.Playing || wavePlayer.PlaybackState == PlaybackState.Paused)
                {
                    if ((runmusicindex - 1) < 1)
                    {
                        runmusicindex = playlist.Count();
                    }
                    else
                    {
                        runmusicindex--;
                    }
                    if (pl.bShuffle == true)
                    {
                        prevnext = true;
                    }
                    playing = true;
                    wavePlayer.Stop();
                }
            }
            else
            {
                var messagebox = new Helper.MessageBox();
                messagebox.ShowCenter_DialogError("กรุณาเล่นเพลงก่อน", "แจ้งเตือน");
            }
            this.trigger_url();
            stop_ran++;
        }

        private void ForwardButton_Click(object sender, EventArgs e)
        {
            //bool bOK = false;
            //if (playlist.Count > 0) bOK = true;
            if (this.playing == true) return;
            if (playlist.Count() != null && playlist.Count() != 0)
            {
                if (wavePlayer.PlaybackState == PlaybackState.Playing || wavePlayer.PlaybackState == PlaybackState.Paused)
                {
                    //Stop Cuurent Player to be play next Song in player list
                    if ((runmusicindex + 1) > playlist.Count())
                    {
                        runmusicindex = 1;
                    }
                    else
                    {
                        runmusicindex++;
                    }
                    //prevnext = true;
                    this.playing = true;
                    wavePlayer.Stop();
                }
            }
            else
            {
                var messagebox = new Helper.MessageBox();
                messagebox.ShowCenter_DialogError("กรุณาเล่นเพลงก่อน", "แจ้งเตือน");
            }

            this.trigger_url();
        }

        //HashSet<int> uniqueNumbers = new HashSet<int>();
        string ran = "";
        int stop_ran = 0;
        int next_ran = 0;
        private int GenerateRandomString(int length)
        {
            System.Random rand = new System.Random();       //Creates seed value from system clock if no seed supplied
            //int iTemp = rand.Next(0, (length));				//Min possible value, max possible value+1
            //return (iTemp);

            int num = rand.Next(1, length);
            if (stop_ran == 0)
            {
                if (ran != "")
                {
                    var aa = ran.Split(',');
                    if (!aa.Contains(num.ToString())) // ตรวจสอบว่ามีตัวเลขนี้ในลิสต์ไหม
                    {
                        ran += "," + num; // ถ้าไม่มี ให้เพิ่มเข้าไป
                    }
                    else
                    {
                        num = rand.Next(1, length);
                        var bb = ran.Split(',');
                        if (bb.Count() != length)
                        {
                            while (bb.Contains(num.ToString()))
                            {
                                num = rand.Next(1, length + 1);
                            }
                            ran += "," + num;
                        }
                        else
                        {
                            ran = "";
                        }
                    }
                }
                else
                {
                    ran += num;
                }
            }
            else
            {
                if (next_ran == 0)
                {
                    var aa = ran.Split(',');
                    var sdsd = aa.Take((aa.Length + 1) - stop_ran).ToList();
                    num = Convert.ToInt32(sdsd.LastOrDefault());
                    next_ran++;
                }
                else
                {
                    if (next_ran == stop_ran)
                    {
                        var aa = ran.Split(',');
                        if (!aa.Contains(num.ToString())) // ตรวจสอบว่ามีตัวเลขนี้ในลิสต์ไหม
                        {
                            ran += "," + num; // ถ้าไม่มี ให้เพิ่มเข้าไป
                        }
                        else
                        {
                            num = rand.Next(1, length);
                            var bb = ran.Split(',');
                            if (bb.Count() != length)
                            {
                                while (bb.Contains(num.ToString()))
                                {
                                    num = rand.Next(1, length + 1);
                                }
                                ran += "," + num;
                            }
                            else
                            {
                                ran = "";
                            }
                        }
                        stop_ran = 0;
                        next_ran = 0;
                    }
                    else
                    {
                        var aa = ran.Split(',');
                        var sdsd = aa.Take((aa.Length + 1) - (stop_ran - next_ran)).ToList();
                        num = Convert.ToInt32(sdsd.LastOrDefault());
                        next_ran++;
                    }
                }
            }
            return num;
        }

        public void add_music_play(string name)
        {
            foreach (var ac in ListOTSMedia)
            {
                if (ac.fileName == name)
                {
                    var va = playlist.Where(x => x.Equals(ac.fileLocation)).FirstOrDefault();
                    if (va == null)
                    {
                        playlist.Add(ac.fileLocation);
                    }
                }
            }
        }

        public void delete_music_play(string name)
        {
            foreach (var ac in ListOTSMedia)
            {
                if (ac.fileName == name)
                {
                    var va = playlist.Where(x => x.Equals(ac.fileLocation)).FirstOrDefault();
                    if (va == null)
                    {
                        playlist.Remove(ac.fileLocation);
                        break;
                    }
                }
            }
        }

        public bool returnRunMusic()
        {
            if (wavePlayer != null)
            {
                if (wavePlayer.PlaybackState == PlaybackState.Playing)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public void run_music_play()
        {
            if (wavePlayer != null)
            {
                if (wavePlayer.PlaybackState == PlaybackState.Playing) return;
                wavePlayer.Play();
                this.PlayButton.IconChar = FontAwesome.Sharp.IconChar.PauseCircle;
                this.PlayButton.IconColor = System.Drawing.Color.Orange;
            }
            else
            {
                PlayButton_Click(this, es);
            }
        }

        public void stop_music_play()
        {
            if (wavePlayer != null)
            {
                if (wavePlayer.PlaybackState == PlaybackState.Stopped) return;
                PlayerControlStop();
            }
        }

        public void pause_music_play()
        {
            if (wavePlayer != null)
            {
                if (wavePlayer.PlaybackState == PlaybackState.Paused) return;
                wavePlayer.Pause();
                this.PlayButton.IconChar = FontAwesome.Sharp.IconChar.PlayCircle;
                this.PlayButton.IconColor = System.Drawing.Color.LightGray;
            }
        }

        public int get_runorder()
        {
            return runmusicindex;
        }

        public string get_status_music()
        {
            if (wavePlayer != null)
            {
                if (wavePlayer.PlaybackState == PlaybackState.Playing)
                {
                    return "Play";
                }
                else if (wavePlayer.PlaybackState == PlaybackState.Paused)
                {
                    return "Paused";
                }
            }
            return "ยังไม่เปิดเครื่องเล่น";
        }

        public void forward_music_play()
        {
            ForwardButton_Click(this, es);
        }

        public void backward_music_play()
        {
            BackwardButton_Click(this, es);
        }

        public string get_loopandshuffle()
        {
            if (pl.bLoop == true)
            {
                return "เล่นแบบตามลำดับ";
            }
            else if (pl.bShuffle == true)
            {
                return "เล่นแบบสุ่ม";
            }
            return "";
        }

        public bool get_isRamdom(short id)
        {
            var ggg = pl.LoadShuffleLoopState1(id);
            return ggg.Item1;
        }

        public bool get_isLoop(short id)
        {
            var ggg = pl.LoadShuffleLoopState1(id);
            return ggg.Item2;
        }

        public string set_loopandshuffle(short ids, string typec)
        {
            if (pl.lastId == ids)
            {
                if (typec.ToLower().Trim() == "2".ToLower().Trim())
                {
                    var shuffle = this.get_isRamdom(ids);
                    pl.change_loop_shuffle(ids, false, shuffle == true ? false : true);

                    return "เล่นแบบตามสุ่ม";
                }
                else if (typec.ToLower().Trim() == "1".ToLower().Trim())
                {
                    var shuffle = this.get_isLoop(ids);
                    pl.change_loop_shuffle(ids, shuffle == true ? false : true, false);

                    return "เล่นแบบตามลำดับ";
                }
            }
            else
            {
                //var ggg1 = pl.LoadShuffleLoopState1(ids);
                if (typec.ToLower().Trim() == "2".ToLower().Trim())
                {
                    var shuffle = this.get_isRamdom(ids);
                    pl.change_loop_shuffle(ids, false, shuffle == true ? false : true);

                    return "เล่นแบบตามสุ่ม";
                }
                else if (typec.ToLower().Trim() == "1".ToLower().Trim())
                {
                    var shuffle = this.get_isLoop(ids);
                    pl.change_loop_shuffle(ids, shuffle == true ? false : true, false);

                    return "เล่นแบบตามลำดับ";
                }

            }
            return "";
        }

        public string select_music_for_web(short id, string namemusic)
        {
            try
            {
                pl.mylistView_doubleclick(id, namemusic);
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.InnerException.ToString();
            }

        }

        public void select_music(string id)
        {
            ran = "";
            next_ran = 0;
            stop_ran = 0;
            ran += id;

            selectmusic = true;
            runmusicindex = Convert.ToInt32(id);

            if (playlist.Count == 0)
            {
                foreach (var xc in ListOTSMedia)
                {
                    playlist.Add(xc.fileLocation);
                }
            }
            if (wavePlayer == null)
            {

                PlaySongOnPlayList(this, es);
            }
            else
            {
                wavePlayer.Stop();
            }
        }

        public void PlayButton_Click(object sender, EventArgs e)
        {
            /*
             * Control
             * - Play
             * - Pause
            */
            #region 1) Check Player PlaybackState
            if (wavePlayer != null)
            {

                if (ListOTSMedia.Count() == 0)
                {
                    playlist.Clear();
                    wavePlayer.Stop();
                }
                else if (QueueOTSMedia.Count > 0)
                {
                    foreach (var xc in QueueOTSMedia)
                    {
                        select_music(xc.Id.ToString());
                        QueueOTSMedia.Clear();
                        break;
                    }
                }

                else
                {
                    // Play
                    switch (wavePlayer.PlaybackState)
                    {
                        case PlaybackState.Playing:
                            wavePlayer.Pause();
                            MediaPlaybackState = PlaybackState.Paused;
                            SetMediaPlayState();
                            //return;
                            break;
                        case PlaybackState.Paused:
                            wavePlayer.Play();
                            MediaPlaybackState = PlaybackState.Playing;

                            SetMediaPlayState();
                            break;

                        default:
                            // code block
                            break;
                    }
                }

            }
            #endregion
            //if (QueueOTSMedia.Count > 0)
            //{
            //    PlayMedia(QueueOTSMedia.Dequeue());
            //    return;
            //}
            //Shuffle, Loop
            //$EDIT : 2022-12-01 
            else
            {
                if (ListOTSMedia.Count > 0)
                {
                    // (QueueOTSMedia.Peek()).Loop
                    var check = get_runplaylist();
                    foreach (var track in ListOTSMedia)
                    {


                        //if (wavePlayer != null)
                        //{
                        //    if (wavePlayer.PlaybackState == PlaybackState.Playing)
                        //    {
                        //        //Stop Cuurent Player to be play next Song in player list
                        //        playlist.Add(track.fileLocation);
                        //    }
                        //}
                        //else
                        //{

                        //    if ((QueueOTSMedia.Peek()).fileName == track.fileName)
                        //    {
                        //        PlayMedia(QueueOTSMedia.Peek()); //QueueOTSMedia.Dequeue()

                        //        return;
                        //    }
                        //}

                        playlist.Add(track.fileLocation);


                    }
                    if (check == false)
                    {

                        PlaySongOnPlayList(this, e);
                    }
                    else
                    {
                        if (QueueOTSMedia.Count > 0)
                        {
                            foreach (var ca in QueueOTSMedia)
                            {
                                select_music(ca.Id.ToString());
                                QueueOTSMedia.Clear();
                                break;
                            }
                        }
                    }
                }
                else
                {
                    var messagebox = new Helper.MessageBox();
                    messagebox.ShowCenter_DialogError("กรุณาเพิ่มเพลงก่อน !", "แจ้งเตือน");
                }



                //    /// OR ||
                //    //int i = 0;
                //    //while (i< playlist.Count)
                //    //{
                //    //    PlaySongOnPlayList();
                //    //    i++;
                //    //}
            }
            this.trigger_url();

        }

        private void NPlayer1_CurrentMediaChanged(object sender, EventArgs e)
        {
            var messagebox = new Helper.MessageBox();
            messagebox.ShowCenter_DialogError("NPlayer1_CurrentMediaChanged", "แจ้งเตือน");
        }

        private void SetMediaPlayState()
        {
            if (wavePlayer == null) return;
            switch (MediaPlaybackState)
            {
                case PlaybackState.Playing:
                    MediaPlaybackState = PlaybackState.Paused; //Play then Pause
                    this.PlayButton.IconChar = FontAwesome.Sharp.IconChar.PauseCircle;
                    this.PlayButton.IconColor = System.Drawing.Color.Orange;
                    this.BackColor = ActiveBGColorWithAlpha;
                    if (this.trackBarPosition.InvokeRequired)
                    {
                        this.trackBarPosition.Invoke(updateTextBox33, NormalColor);
                    }
                    else
                    {
                        this.trackBarPosition.BackColor = NormalColor;
                    }
                    break;
                case PlaybackState.Paused:
                    MediaPlaybackState = PlaybackState.Playing; //Pause then Play
                    this.PlayButton.IconChar = FontAwesome.Sharp.IconChar.PlayCircle;
                    this.PlayButton.IconColor = Color.LightGray;
                    this.BackColor = ActiveBGColorWithoutAlpha;
                    if (this.trackBarPosition.InvokeRequired)
                    {
                        this.trackBarPosition.Invoke(updateTextBox33, InactiveBGColor);
                    }
                    else
                    {
                        this.trackBarPosition.BackColor = InactiveBGColor;
                    }
                    break;

                default:
                    break;
            }
        }

        private void SetPlayerBackGround()
        {
            if (wavePlayer.PlaybackState == PlaybackState.Stopped)
            {
                MediaPlaybackState = PlaybackState.Playing;
                PlayButton.IconChar = FontAwesome.Sharp.IconChar.PlayCircle;
                PlayButton.IconColor = System.Drawing.Color.LightGray;
                //PlayButton.BackColor= System.Drawing.Color.Orange;
                //PlayButton.ForeColor= System.Drawing.Color.White;
                this.BackColor = NormalColor;
                if (lblPlaySongName.InvokeRequired)
                {
                    lblPlaySongName.Invoke(updateTextBox22, string.Empty);
                }
                else
                {
                    lblPlaySongName.Text = string.Empty;

                }
                if (lblVolumeLevel.InvokeRequired)
                {
                    lblVolumeLevel.Invoke(updateTextBox, "");
                }
                else
                {
                    lblVolumeLevel.Text = "";

                }
                wavePlayer = null;
                //labelTotalTime.Text = string.Empty;
                //this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            }
        }

        public void PlayMedia(OTSMedia mediaToPlay)
        {
            if (mediaToPlay == null) return;
            if (mediaToPlay.Id == 0) return;
            if (wavePlayer == null)
            {
                #region Init Output Device
                LoadOutputDevicePlugins(ReflectionHelper.CreateAllInstancesOf<IOutputDevicePlugin>());
                #endregion
                #region Create Input Stream
                ISampleProvider sampleProvider;
                try
                {
                    sampleProvider = CreateInputStream(mediaToPlay.fileLocation);
                    lblPlaySongName.Text = mediaToPlay.fileName.Replace(".wav", "").Replace("mp3", ""); ;
                }
                catch (Exception createException)
                {
                    var messagebox = new Helper.MessageBox();
                    messagebox.ShowCenter_DialogError("PlayMedia Error : " + String.Format("{0}", createException.Message), "แจ้งเตือน");
                    return;
                }
                #endregion

                labelTotalTime.Text = String.Format("{0:00}:{1:00}", (int)audioFileReader.TotalTime.TotalMinutes, audioFileReader.TotalTime.Seconds);
                try
                {
                    wavePlayer.Init(sampleProvider);
                }
                catch (Exception initException)
                {
                    var messagebox = new Helper.MessageBox();
                    messagebox.ShowCenter_DialogError("PlayMedia WavePlayer Error  : " + String.Format("{0}", initException.Message), "Error Initializing Output");
                    return;
                }

                wavePlayer.PlaybackStopped += (sender, evn) =>
                {
                    SetPlayerBackGround();
                    //timer1.Stop();
                };
                if (wavePlayer.PlaybackState == PlaybackState.Stopped)
                {
                    wavePlayer.Play();
                    MediaPlaybackState = PlaybackState.Playing;
                    SetMediaPlayState();
                }
            }
        }

        public void PlaySongOnPlayList(object sender, EventArgs e)
        {

            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        if (playlist.Count == 0)
                        {
                            return;
                        }
                        int x = 0;
                        if (pl.bShuffle == true && prevnext == false && selectmusic1 == false)
                        {
                            x = GenerateRandomString(playlist.Count);
                            if (x == 0) x = 1;
                            runmusicindex = x;
                        }
                        else if (pl.bLoop == true)
                        {
                            if (runmusicindex == 0)
                            {
                                runmusicindex = 1;
                            }
                        }
                        this.selectmusic1 = false;

                        if (prevnext == true)
                        {
                            if (playbefore.Count > 0)
                            {
                                runmusicindex = playbefore.LastOrDefault();
                                var gg = playbefore.Where(xx => xx == runmusicindex).FirstOrDefault();
                                playbefore.Remove(gg);

                            }
                            else
                            {
                                runmusicindex = runmusicindexOld;
                            }
                        }

                        try
                        {
                            if (this.InvokeRequired)
                            {
                                this.Invoke(new Action(() =>
                                {
                                    LoadOutputDevicePlugins(ReflectionHelper.CreateAllInstancesOf<IOutputDevicePlugin>());

                                }));
                            }
                            else
                            {
                                LoadOutputDevicePlugins(ReflectionHelper.CreateAllInstancesOf<IOutputDevicePlugin>());
                            }
                        }
                        catch (Exception ex)
                        {
                            var messagebox = new Helper.MessageBox();
                            messagebox.ShowCenter_DialogError("LoadOutputDevicePlugings Error : " + ex.Message + "inner : " + ex.InnerException, "แจ้งเตือน");
                        }

                        List<string> fileNames = new List<string>();
                        if (selectmusic == false)
                        {
                            if (pl.bShuffle != true)
                            {
                                if (pl.bShuffle == false && pl.bLoop == false && runmusicindexOld == playlist.Count())
                                {
                                    PlayerControlStop();
                                    return;
                                }
                                else if (runmusicindex == runmusicindexOld)
                                {
                                    if ((runmusicindex + 1) > playlist.Count())
                                    {
                                        runmusicindex = 1;
                                    }
                                    else
                                    {
                                        runmusicindex += 1;
                                    }
                                    if (runmusicindexOld != 0)
                                    {
                                        if (prevnext != true) playbefore.Add(runmusicindexOld);
                                    }
                                    runmusicindexOld = runmusicindex;

                                    fileNames.Add(playlist[runmusicindex - 1]);
                                }
                                else
                                {
                                    if (runmusicindexOld != 0)
                                    {
                                        if (prevnext != true) playbefore.Add(runmusicindexOld);
                                    }
                                    runmusicindexOld = runmusicindex;
                                    fileNames.Add(playlist[runmusicindex - 1]);
                                    playbefore.Add(runmusicindex);
                                }
                            }
                            else
                            {
                                if (runmusicindexOld != 0)
                                {
                                    if (prevnext != true) playbefore.Add(runmusicindexOld);
                                }
                                runmusicindexOld = runmusicindex;
                                fileNames.Add(playlist[runmusicindex - 1]);
                            }
                        }
                        else
                        {
                            playbefore.Clear();
                            //if(runmusicindexOld != 0)
                            //{
                            //    if (prevnext != true) playbefore.Add(runmusicindexOld);
                            //}
                            runmusicindexOld = runmusicindex;

                            fileNames.Add(playlist[runmusicindex - 1]);
                            selectmusic = false;
                        }
                        set_filename(playlist[runmusicindex - 1]);
                        prevnext = false;
                        Queue<string> queuePlay = new Queue<string>(fileNames);
                        string fileName = queuePlay.Peek();

                        ISampleProvider sampleProvider;
                        if (!File.Exists(fileName))
                        {
                            if (pl.bLoop == true)
                            {
                                var gggg = 1;
                                do
                                {
                                    log.Error(String.Format("{0} ที่อยู่เพลง: {1}", "ไม่พบไฟล์เพลง", fileName));
                                    pl.change_file_kag(fileName);
                                    if (playlist.Count == runmusicindexOld)
                                    {
                                        runmusicindexOld = 1;
                                        runmusicindex = 1;
                                        fileNames.Clear();
                                        fileNames.Add(playlist[runmusicindex - 1]);
                                        queuePlay.Clear();
                                        queuePlay = new Queue<string>(fileNames);
                                        fileName = queuePlay.Peek();
                                    }
                                    else
                                    {
                                        runmusicindexOld += 1;
                                        runmusicindex += 1;
                                        fileNames.Clear();
                                        fileNames.Add(playlist[runmusicindex - 1]);
                                        queuePlay.Clear();
                                        queuePlay = new Queue<string>(fileNames);
                                        fileName = queuePlay.Peek();
                                    }
                                    if (gggg == playlist.Count)
                                    {
                                        this.PlayerControlStop();
                                        return;
                                    }
                                    gggg++;

                                } while (!File.Exists(fileName));

                            }
                            else
                            {
                                if (playlist.Count == runmusicindexOld)
                                {
                                    log.Error(String.Format("{0} ที่อยู่เพลง: {1}", "ไม่พบไฟล์เพง", fileName));
                                    pl.change_file_kag(fileName);
                                    this.PlayerControlStop();
                                    return;
                                }
                                var gggg = 1;
                                do
                                {
                                    log.Error(String.Format("{0} ที่อยู่เพลง: {1}", "ไม่พบไฟล์เพง", fileName));
                                    pl.change_file_kag(fileName);
                                    if (playlist.Count == runmusicindexOld)
                                    {
                                        runmusicindexOld = 1;
                                        runmusicindex = 1;
                                        fileNames.Clear();
                                        fileNames.Add(playlist[runmusicindex - 1]);
                                        queuePlay.Clear();
                                        queuePlay = new Queue<string>(fileNames);
                                        fileName = queuePlay.Peek();
                                    }
                                    else
                                    {
                                        runmusicindexOld += 1;
                                        runmusicindex += 1;
                                        fileNames.Clear();
                                        fileNames.Add(playlist[runmusicindex - 1]);
                                        queuePlay.Clear();
                                        queuePlay = new Queue<string>(fileNames);
                                        fileName = queuePlay.Peek();
                                    }
                                    if (gggg == playlist.Count)
                                    {
                                        this.PlayerControlStop();
                                        return;
                                    }
                                    gggg++;
                                } while (!File.Exists(fileName));
                            }
                        }

                        try
                        {
                            sampleProvider = CreateInputStream(fileName);
                        }
                        catch (Exception createException)
                        {
                            var messagebox = new Helper.MessageBox();
                            messagebox.ShowCenter_DialogError("CreateInputStream Error : " + String.Format("{0}", createException.Message), "แจ้งเตือน");
                            return;
                        }
                        //Key currentPlay Media
                        PlayedFileName = fileName;

                        //Make sure before remove
                        //playlist.Remove(fileName);
                        if (labelTotalTime.InvokeRequired)
                        {
                            this.labelTotalTime.Invoke(this.updateTextBox11, String.Format("{0:00}:{1:00}", (int)audioFileReader.TotalTime.TotalMinutes, audioFileReader.TotalTime.Seconds));
                        }
                        else
                        {
                            this.labelTotalTime.Text = String.Format("{0:00}:{1:00}", (int)audioFileReader.TotalTime.TotalMinutes, audioFileReader.TotalTime.Seconds);
                        }

                        wavePlayer.Init(sampleProvider);

                        var namemusic = fileName.Split('\\');
                        if (lblPlaySongName.InvokeRequired)
                        {
                            this.lblVolumeLevel.Invoke(this.updateTextBox22, namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", ""));
                        }
                        else
                        {
                            this.lblPlaySongName.Text = namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", "");

                        }
                        if (wavePlayer.PlaybackState == PlaybackState.Stopped)
                        {
                            //pl.change_namemedia(pl.Index_item(namemusic[namemusic.Length - 1]));
                            pl.change_namemedia(runmusicindex, settingplayertrack);
                        }
                        wavePlayer.PlaybackStopped += (senders, evn) =>
                        {

                            PlaySongOnPlayList(senders, evn);
                            //timer1.Stop();
                            SetPlayerBackGround();


                        };
                        wavePlayer.Play();

                        if (wavePlayer.PlaybackState == PlaybackState.Playing)
                        {
                            PlayButton.IconChar = FontAwesome.Sharp.IconChar.PauseCircle;
                            PlayButton.IconColor = System.Drawing.Color.Orange;
                            this.BackColor = Color.FromArgb(20, 255, 130, 0);
                            //PlayButton.IconChar = FontAwesome.Sharp.IconChar.PlayCircle;
                            //SAVE INFO TO IsoStorage
                            MediaPlaybackState = PlaybackState.Playing;
                            //PlayButton.IconColor = System.Drawing.Color.Orange;
                            //PlayButton.IconColor = Color.LightGray;
                            this.playing = false;
                            this.trigger_url();
                            return;
                        }
                        this.trigger_url();
                    }));
                 } else
                {
                    if (playlist.Count == 0)
                {
                    return;
                }
                int x = 0;
                if (pl.bShuffle == true && prevnext == false && selectmusic1 == false)
                {


                    x = GenerateRandomString(playlist.Count);
                    if (x == 0) x = 1;
                    runmusicindex = x;
                    //if (playbefore.Where(xx => xx == runmusicindex).FirstOrDefault() == 0) break;

                }
                else if (pl.bLoop == true)
                {
                    if (runmusicindex == 0)
                    {



                        runmusicindex = 1;
                    }
                }
                this.selectmusic1 = false;

                if (prevnext == true)
                {
                    if (playbefore.Count > 0)
                    {
                        runmusicindex = playbefore.LastOrDefault();
                        var gg = playbefore.Where(xx => xx == runmusicindex).FirstOrDefault();
                        playbefore.Remove(gg);

                    }
                    else
                    {
                        runmusicindex = runmusicindexOld;
                    }
                }

                //Check on Playing meaning to IconChar was PauseCircle
                //if (PlayButton.IconChar == FontAwesome.Sharp.IconChar.PauseCircle)
                //{
                //    wavePlayer.Play();
                //    PlayButton.IconChar = FontAwesome.Sharp.IconChar.PlayCircle;
                //    PlayButton.IconColor = System.Drawing.Color.Orange;
                //    this.BackColor = Color.FromArgb(20, 255, 130, 0);
                //    return;
                //}
                try
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() =>
                        {
                            LoadOutputDevicePlugins(ReflectionHelper.CreateAllInstancesOf<IOutputDevicePlugin>());

                        }));
                    }
                    else
                    {
                        LoadOutputDevicePlugins(ReflectionHelper.CreateAllInstancesOf<IOutputDevicePlugin>());

                    }
                }
                catch (Exception ex)
                {
                        //throw;
                        var messagebox = new Helper.MessageBox();
                        messagebox.ShowCenter_DialogError("LoadOutputDevicePlugins Error : " + ex.Message + "inner " + ex.InnerException, "แจ้งเตือน");

                    }
                    List<string> fileNames = new List<string>();
                if (selectmusic == false)
                {
                    if (pl.bShuffle != true)
                    {
                        if (pl.bShuffle == false && pl.bLoop == false && runmusicindexOld == playlist.Count())
                        {
                            PlayerControlStop();
                            return;
                        }
                        else if (runmusicindex == runmusicindexOld)
                        {
                            if ((runmusicindex + 1) > playlist.Count())
                            {
                                runmusicindex = 1;
                            }
                            else
                            {
                                runmusicindex += 1;
                            }
                            if (runmusicindexOld != 0)
                            {
                                if (prevnext != true) playbefore.Add(runmusicindexOld);
                            }
                            runmusicindexOld = runmusicindex;

                            fileNames.Add(playlist[runmusicindex - 1]);
                        }
                        else
                        {
                            if (runmusicindexOld != 0)
                            {
                                if (prevnext != true) playbefore.Add(runmusicindexOld);
                            }
                            runmusicindexOld = runmusicindex;
                            fileNames.Add(playlist[runmusicindex - 1]);
                            playbefore.Add(runmusicindex);
                        }
                    }
                    else
                    {
                        if (runmusicindexOld != 0)
                        {
                            if (prevnext != true) playbefore.Add(runmusicindexOld);
                        }
                        runmusicindexOld = runmusicindex;
                        fileNames.Add(playlist[runmusicindex - 1]);
                    }
                }
                else
                {
                    playbefore.Clear();
                    //if(runmusicindexOld != 0)
                    //{
                    //    if (prevnext != true) playbefore.Add(runmusicindexOld);
                    //}
                    runmusicindexOld = runmusicindex;

                    fileNames.Add(playlist[runmusicindex - 1]);
                    selectmusic = false;
                }
                set_filename(playlist[runmusicindex - 1]);
                prevnext = false;
                Queue<string> queuePlay = new Queue<string>(fileNames);
                string fileName = queuePlay.Peek();

                ISampleProvider sampleProvider;
                if (!File.Exists(fileName))
                {
                    //return;
                    if (pl.bLoop == true)
                    {
                        var gggg = 1;
                        do
                        {
                            log.Error(String.Format("{0} ที่อยู่เพลง: {1}", "ไม่พบไฟล์เพลง", fileName));
                            pl.change_file_kag(fileName);
                            if (playlist.Count == runmusicindexOld)
                            {
                                runmusicindexOld = 1;
                                runmusicindex = 1;
                                fileNames.Clear();
                                fileNames.Add(playlist[runmusicindex - 1]);
                                queuePlay.Clear();
                                queuePlay = new Queue<string>(fileNames);
                                fileName = queuePlay.Peek();
                            }
                            else
                            {
                                runmusicindexOld += 1;
                                runmusicindex += 1;
                                fileNames.Clear();
                                fileNames.Add(playlist[runmusicindex - 1]);
                                queuePlay.Clear();
                                queuePlay = new Queue<string>(fileNames);
                                fileName = queuePlay.Peek();
                            }
                            if (gggg == playlist.Count)
                            {
                                this.PlayerControlStop();
                                return;
                            }
                            gggg++;

                        } while (!File.Exists(fileName));

                    }
                    else
                    {
                        if (playlist.Count == runmusicindexOld)
                        {
                            log.Error(String.Format("{0} ที่อยู่เพลง: {1}", "ไม่พบไฟล์เพง", fileName));
                            pl.change_file_kag(fileName);
                            this.PlayerControlStop();
                            return;
                        }
                        var gggg = 1;
                        do
                        {
                            log.Error(String.Format("{0} ที่อยู่เพลง: {1}", "ไม่พบไฟล์เพง", fileName));
                            pl.change_file_kag(fileName);
                            if (playlist.Count == runmusicindexOld)
                            {
                                runmusicindexOld = 1;
                                runmusicindex = 1;
                                fileNames.Clear();
                                fileNames.Add(playlist[runmusicindex - 1]);
                                queuePlay.Clear();
                                queuePlay = new Queue<string>(fileNames);
                                fileName = queuePlay.Peek();
                            }
                            else
                            {
                                runmusicindexOld += 1;
                                runmusicindex += 1;
                                fileNames.Clear();
                                fileNames.Add(playlist[runmusicindex - 1]);
                                queuePlay.Clear();
                                queuePlay = new Queue<string>(fileNames);
                                fileName = queuePlay.Peek();
                            }
                            if (gggg == playlist.Count)
                            {
                                this.PlayerControlStop();
                                return;
                            }
                            gggg++;
                        } while (!File.Exists(fileName));
                    }
                }
                try
                {
                    sampleProvider = CreateInputStream(fileName);
                }
                catch (Exception createException)
                {
                    var messagebox = new Helper.MessageBox();
                    messagebox.ShowCenter_DialogError("CreateInputStream Error : " + String.Format("{0}", createException.Message), "Error Loading File");
                    return;
                }
                //Key currentPlay Media
                PlayedFileName = fileName;

                //Make sure before remove
                //playlist.Remove(fileName);
                if (labelTotalTime.InvokeRequired)
                {
                    this.labelTotalTime.Invoke(this.updateTextBox11, String.Format("{0:00}:{1:00}", (int)audioFileReader.TotalTime.TotalMinutes, audioFileReader.TotalTime.Seconds));
                }
                else
                {
                    this.labelTotalTime.Text = String.Format("{0:00}:{1:00}", (int)audioFileReader.TotalTime.TotalMinutes, audioFileReader.TotalTime.Seconds);
                }

                wavePlayer.Init(sampleProvider);

                var namemusic = fileName.Split('\\');
                if (lblPlaySongName.InvokeRequired)
                {
                    this.lblVolumeLevel.Invoke(this.updateTextBox22, namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", ""));
                }
                else
                {
                    this.lblPlaySongName.Text = namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", "");

                }
                if (wavePlayer.PlaybackState == PlaybackState.Stopped)
                {
                    //pl.change_namemedia(pl.Index_item(namemusic[namemusic.Length - 1]));
                    pl.change_namemedia(runmusicindex, settingplayertrack);
                }
                wavePlayer.PlaybackStopped += (senders, evn) =>
                {

                    PlaySongOnPlayList(senders, evn);
                    //timer1.Stop();
                    SetPlayerBackGround();


                };
                wavePlayer.Play();

                if (wavePlayer.PlaybackState == PlaybackState.Playing)
                {
                    PlayButton.IconChar = FontAwesome.Sharp.IconChar.PauseCircle;
                    PlayButton.IconColor = System.Drawing.Color.Orange;
                    this.BackColor = Color.FromArgb(20, 255, 130, 0);
                    //PlayButton.IconChar = FontAwesome.Sharp.IconChar.PlayCircle;
                    //SAVE INFO TO IsoStorage
                    MediaPlaybackState = PlaybackState.Playing;
                    //PlayButton.IconColor = System.Drawing.Color.Orange;
                    //PlayButton.IconColor = Color.LightGray;
                    this.playing = false;
                    this.trigger_url();
                    return;
                }
                this.trigger_url();
                }
                
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                MessageBox.Show("Output Device เปลื่ยนไป กรุณาตั้งค่าใหม่ \n (ติดต่อเจ้าหน้าที่ดูแลระบบ)");
            }
        }

        //public void PlaySong()
        //{
        //    WaveStream fileWaveStream = null;
        //    if (playlist.Count < 1)
        //    {
        //        return;
        //    }
        //    Queue<string> queuePlay = new Queue<string>(playlist);
        //    if (wavePlayer != null && wavePlayer.PlaybackState != PlaybackState.Stopped)
        //    {
        //        wavePlayer.Stop();
        //    }
        //    if (fileWaveStream != null)
        //    {
        //        fileWaveStream.Dispose();
        //    }
        //    if (wavePlayer != null)
        //    {
        //        wavePlayer.Dispose();
        //        wavePlayer = null;
        //    }
        //    wavePlayer = new WaveOutEvent();
        //    fileWaveStream = new AudioFileReader(queuePlay.Dequeue());
        //    wavePlayer.Init(fileWaveStream);
        //    wavePlayer.PlaybackStopped += (sender, evn) => { PlaySong(); };
        //    wavePlayer.Play();
        //}

        private ISampleProvider CreateInputStream(string fileName)
        {
            RegistryKey HKLMSoftwareTOAConfig = Registry.CurrentUser.OpenSubKey(@"Software\TOA\Config", true);
            RegistryKey configsocket = HKLMSoftwareTOAConfig.OpenSubKey("trackname", true);
            var get_adB = configsocket.GetValue(adB);
            var get_dB = Convert.ToDouble(configsocket.GetValue(dB)) == 0.0 ? 1.0 : Convert.ToDouble(configsocket.GetValue(dB));
            try
            {

                //ProcessStartInfo processStartInfo = new ProcessStartInfo()
                //{
                //    FileName = "cmd.exe", // หรือ "powershell.exe" ขึ้นอยู่กับว่าคุณใช้ Command Line แบบไหน
                //    Arguments = "python gainmp3.py " + fileName + " --dbm " + get_dB, // /C ใช้เพื่อให้ cmd.exe รันคำสั่งแล้วปิด
                //    RedirectStandardOutput = true, // รับข้อมูลจาก Standard Output
                //    UseShellExecute = false, // ห้ามใช้ shell execute
                //    CreateNoWindow = true // ไม่แสดงหน้าต่าง cmd
                //};

                //// สร้าง Process และเริ่มต้นการรัน
                //Process process = Process.Start(processStartInfo);
                //process.WaitForExit();
                //// อ่านผลลัพธ์จาก Standard Output
                //string output = process.StandardOutput.ReadToEnd();
                if (get_adB.ToString() == "active")
                {
                    string command = "python gainmp3.py \"" + fileName + "\" --dbm " + get_dB + " --play " + nametrack; // ตัวอย่างคำสั่งที่ต้องการรัน
                    ProcessStartInfo processStartInfo = new ProcessStartInfo()
                    {
                        FileName = "cmd.exe", // หรือ "powershell.exe" ขึ้นอยู่กับคำสั่งที่ใช้
                        Arguments = "/C " + command, // /C คือให้ cmd.exe รันคำสั่งแล้วปิด
                        RedirectStandardOutput = true, // สำหรับดึงข้อมูลจาก Standard Output
                        UseShellExecute = false, // ไม่ใช้ Shell Execute
                        CreateNoWindow = true // ไม่แสดงหน้าต่าง cmd
                    };

                    Process process = Process.Start(processStartInfo);
                    string output = process.StandardOutput.ReadToEnd(); // รับข้อมูลจาก output
                    process.WaitForExit(); // รอให้โปรเซสเสร็จสิ้น
                    Console.WriteLine(output);

                }
                //else
                //{
                //    string command = "python gainmp3.py \"" + fileName + "\" --dbm 89 --play " + nametrack; // ตัวอย่างคำสั่งที่ต้องการรัน
                //    ProcessStartInfo processStartInfo = new ProcessStartInfo()
                //    {
                //        FileName = "cmd.exe", // หรือ "powershell.exe" ขึ้นอยู่กับคำสั่งที่ใช้
                //        Arguments = "/C " + command, // /C คือให้ cmd.exe รันคำสั่งแล้วปิด
                //        RedirectStandardOutput = true, // สำหรับดึงข้อมูลจาก Standard Output
                //        UseShellExecute = false, // ไม่ใช้ Shell Execute
                //        CreateNoWindow = true // ไม่แสดงหน้าต่าง cmd
                //    };

                //    Process process = Process.Start(processStartInfo);
                //    string output = process.StandardOutput.ReadToEnd(); // รับข้อมูลจาก output
                //    process.WaitForExit(); // รอให้โปรเซสเสร็จสิ้น
                //    Console.WriteLine(output);
                //}
                // แสดงผลลัพธ์
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }


            audioFileReader = new AudioFileReader(fileName);
            var sampleChannel = new SampleChannel(audioFileReader, true);

            double targetDecibel = get_adB.ToString() != "active" ? 1.0f : get_dB;
            float volumeFactor = (float)Math.Pow(10, targetDecibel / 20);
            //sampleChannel.Volume = volumeFactor;
            //setVolumeDelegate = vol => sampleChannel.Volume = vol;
            var postVolumeMeter = new MeteringSampleProvider(sampleChannel);
            //postVolumeMeter.StreamVolume += OnStreamVolume;
            return postVolumeMeter;
        }

        private static void OnStreamVolume(object sender, StreamVolumeEventArgs e)
        {
            // e.MaxSampleValues เก็บค่า Peak Level ของแต่ละ Channel
            foreach (var sample in e.MaxSampleValues)
            {
                // แปลง Amplitude เป็น dBFS
                double dB = 20 * Math.Log10(sample);
                Console.WriteLine($"Current Volume: {dB:F2} dBFS");
            }
        }

        //private void AddFileName()
        //{
        //    if (QueueOTSMedia.Count > 0)
        //    {
        //        OTSMedia _track = QueueOTSMedia.Peek();
        //        if (_track.Id == 0) return;
        //        fileName = _track.fileLocation;
        //        lblPlaySongName.Text = _track.fileName;
        //        QueueOTSMedia.Dequeue();
        //    }
        //    else
        //    {
        //        if (CurrentPlaylist.count > 0)
        //        {
        //            //Select Media In PlayList
        //            for (int i = 0; i < CurrentPlaylist.count; i++)
        //            {
        //                songList.Add(CurrentPlaylist[i].fileLocation);
        //            }

        //            PlayControl play = new PlayControl(songList);
        //            play.PlaySong();
        //        }
        //    }
        //    //lblPlaySongName.Text = this.playerName;
        //}

        private void CreateWaveOut()
        {
            //CloseWaveOut();
            //NAudio.Wave.WaveStream pcm = NAudio.Wave.WaveFormatConversionStream.CreatePcmStream(new NAudio.Wave.Mp3FileReader(fileName));
            //NAudio.Wave.BlockAlignReductionStream stream = new NAudio.Wave.BlockAlignReductionStream(pcm);

            //Play|Pause
            //var latency = (int)comboBoxLatency.SelectedItem;
            //wavePlayer = SelectedOutputDevicePlugin.CreateDevice(latency);
            //wavePlayer = SelectedOutputDevicePlugin.CreateDevice(latency);
            //textBoxPlaybackFormat.Text = $"{wavePlayer.OutputWaveFormat}";

            //lblPlaySongName.Text = this.playerName;
        }

        //private AudioFileReader CreateInputStream(string fileName)
        //{
        //    audioFileReader = new AudioFileReader(fileName);
        //    return audioFileReader;
        //}

        private void LoadOutputDevicePlugins(IEnumerable<IOutputDevicePlugin> outputDevicePlugins)
        {
            CloseWaveOut();
            int Latency = 500;
            float startupVolume = 0.1f; //-10 dB
            string DriverName = string.Empty;
            bool IsAvailable = false;
            var OutputID = string.Empty;
            int Priority = 1;
            bool UseEventCallback = false;
            AudioClientShareMode ExclusiveMode = AudioClientShareMode.Shared;
            //RegistryKey key = HKLMSoftwareTOAPlayer.OpenSubKey("nPlayer1", RegistryKeyPermissionCheck.ReadWriteSubTree);
            //if (key != null)
            //{
            //using (RegistryKey playerKey = key.OpenSubKey(this.playerName, false)
            using (RegistryKey playerId = HKLMSoftwareTOAPlayer.OpenSubKey(this.playerName, false))
            {
                if (playerId != null)
                {
                    DriverName = (string)playerId.GetValue("DriverName");
                    IsAvailable = Boolean.Parse((string)playerId.GetValue("IsAvailable"));
                    OutputID = (string)playerId.GetValue("OutputID");
                    Priority = (int)playerId.GetValue("Priority");
                    Latency = (int)playerId.GetValue("Latency", 500, RegistryValueOptions.DoNotExpandEnvironmentNames);
                    string _volString = (string)playerId.GetValue("Volume", startupVolume.ToString(), RegistryValueOptions.DoNotExpandEnvironmentNames);
                    startupVolume = (float.Parse(_volString));

                    foreach (var outputDevicePlugin in outputDevicePlugins.OrderBy(p => p.Priority))
                    {
                        try
                        {
                            if ((DriverName == "WasapiOut") && ((outputDevicePlugin.Name == "WasapiOut") && (outputDevicePlugin.IsAvailable == true)))
                            {
                                bool bExclusiveMode = Boolean.Parse((string)playerId.GetValue("ExclusiveMode"));
                                if (bExclusiveMode) ExclusiveMode = AudioClientShareMode.Exclusive;
                                UseEventCallback = Boolean.Parse((string)playerId.GetValue("UseEventCallback"));
                                MMDevice device = GetAudioEndpoint(OutputID);
                                //IWavePlayer waveOut = new WasapiOut(device, AudioClientShareMode.Shared, false, latency);
                                wavePlayer = new WasapiOut(device, ExclusiveMode, UseEventCallback, Latency);
                                wavePlayer.Volume = startupVolume;
                                _volume = startupVolume;
                                DisplayVolumeLabel(wavePlayer.Volume);
                                break;
                            }
                            // $EDIT : Volume should be set in Wave
                            /* Message=
                             * setting volume not supported on directsoundout, 
                             * adjust the volume on your waveprovider instead
                            */
                            // Exclude DirectSoundOut from Project : 2022-11-03
                            else if (DriverName == "DirectSound")
                            {
                                Guid newGuid = Guid.Parse(OutputID);

                                wavePlayer = new DirectSoundOut(newGuid, Latency);
                                _volume = startupVolume;
                                DisplayVolumeLabel(startupVolume);
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            var messagebox = new Helper.MessageBox();
                            messagebox.ShowCenter_DialogError("Output Device เปลื่ยนไป กรุณาตั้งค่าใหม่ \n (ติดต่อเจ้าหน้าที่ดูแลระบบ)", "แจ้งเตือน");
                        }
                    }
                }
            }
        }

        //private void btnDeviceList_Click(object sender, EventArgs e)
        //{
        //    int latency = 300;
        //    MMDevice device = GetAudioEndpoint("{0.0.0.00000000}.{354976d5-33d6-46de-a472-a123e6ad2051}");
        //    IWavePlayer waveOut = new WasapiOut(device, AudioClientShareMode.Shared, false, latency);
        //}

        static MMDevice GetAudioEndpoint(string Id)
        {
            if (Environment.OSVersion.Version.Major < 6)
            {
                throw new NotSupportedException("WASAPI supported only on Windows Vista and above");
            }
            var enumerator = new MMDeviceEnumerator();
            //for (int i = 0; i < WaveOut.DeviceCount; i++)
            //{
            //    var cap = WaveOut.GetCapabilities(i);
            //    Console.WriteLine("{0}: {1}", i, cap.ProductName);
            //    var device = enumerator.GetDevice(Id);
            //}
            return enumerator.GetDevice(Id);
        }

        public void StopButton_Click(object sender, EventArgs e)
        {
            PlayerControlStop();
        }

        public void PlayerControlStop()
        {
            try
            {
                ran = "";
                next_ran = 0;
                stop_ran = 0;
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        playlist.Clear();
                        QueueOTSMedia.Clear();
                        MediaPlaybackState = PlaybackState.Stopped;
                        wavePlayer?.Stop();

                        PlayButton.IconChar = FontAwesome.Sharp.IconChar.PlayCircle;
                        PlayButton.IconColor = Color.LightGray;
                        pl.clear_namemedia();
                        runmusicindex = 0;
                        runmusicindexOld = 0;
                        set_runplaylist(false);
                        this.trigger_url();

                    }));
                }
                else
                {
                    playlist.Clear();
                    QueueOTSMedia.Clear();
                    MediaPlaybackState = PlaybackState.Stopped;
                    wavePlayer?.Stop();

                    PlayButton.IconChar = FontAwesome.Sharp.IconChar.PlayCircle;
                    PlayButton.IconColor = Color.LightGray;
                    pl.clear_namemedia();
                    runmusicindex = 0;
                    runmusicindexOld = 0;
                    set_runplaylist(false);
                    this.trigger_url();

                }
            }
            catch (Exception ex)
            {
                var messagebox = new Helper.MessageBox();
                messagebox.ShowCenter_DialogError("PlayerControlStop Error : " + ex.Message + "inner : " + ex.InnerException, "แจ้งเตือน");
            }
        }

        private void VolumeLevel(float level)
        {

        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (wavePlayer != null && audioFileReader != null)
            {
                TimeSpan currentTime = (wavePlayer.PlaybackState == PlaybackState.Stopped) ? TimeSpan.Zero : audioFileReader.CurrentTime;
                trackBarPosition.Value = Math.Min(trackBarPosition.Maximum, (int)(100 * currentTime.TotalSeconds / audioFileReader.TotalTime.TotalSeconds));
                labelTotalTime.Text = String.Format("{0:00}:{1:00} / {2:00}:{3:00}", (int)currentTime.TotalMinutes, currentTime.Seconds, audioFileReader.TotalTime.Minutes, audioFileReader.TotalTime.Seconds);
            }
            else
            {
                trackBarPosition.Value = 0;
                labelTotalTime.Text = String.Format("00:00 / 00:00");
            }
        }

        public void DecreaseVolumeButton_Click()
        {
            DecreaseVolumeButton_Click(this, es);
        }

        private void DecreaseVolumeButton_Click(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    if (wavePlayer == null) return;
                    if ((float)NSNumber.RoundAmount(wavePlayer.Volume, 2) > 0.01f)
                    {
                        wavePlayer.Volume -= 0.01f;
                    }
                    else
                    {
                        wavePlayer.Volume = 0.0f;
                    }
                    DisplayVolumeLabel(wavePlayer.Volume);
                    this.trigger_url();

                }));

            }
            else
            {
                if (wavePlayer == null) return;
                if ((float)NSNumber.RoundAmount(wavePlayer.Volume, 2) > 0.01f)
                {
                    wavePlayer.Volume -= 0.01f;
                }
                else
                {
                    wavePlayer.Volume = 0.0f;
                }
                DisplayVolumeLabel(wavePlayer.Volume);
                this.trigger_url();

            }
        }

        public void IncreaseVolumeButton_Click()
        {
            IncreaseVolumeButton_Click(this, es);
        }

        private void IncreaseVolumeButton_Click(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    if (wavePlayer == null) return;
                    if ((float)NSNumber.RoundAmount(wavePlayer.Volume, 2) < 1.00f)
                    {
                        wavePlayer.Volume += 0.01f;
                    }
                    else
                    {
                        wavePlayer.Volume = 1.00f;
                    }
                    DisplayVolumeLabel(wavePlayer.Volume);
                    this.trigger_url();

                }));

            }
            else
            {
                if (wavePlayer == null) return;
                if ((float)NSNumber.RoundAmount(wavePlayer.Volume, 2) < 1.00f)
                {
                    wavePlayer.Volume += 0.01f;
                }
                else
                {
                    wavePlayer.Volume = 1.00f;
                }
                DisplayVolumeLabel(wavePlayer.Volume);
                this.trigger_url();

            }
        }

        private void CloseWaveOut()
        {
            if (wavePlayer != null)
            {
                wavePlayer.Stop();
            }
            if (audioFileReader != null)
            {
                // this one really closes the file and ACM conversion
                audioFileReader.Dispose();
                setVolumeDelegate = null;
                audioFileReader = null;
            }
            if (wavePlayer != null)
            {
                wavePlayer.Dispose();
                wavePlayer = null;
            }
        }

        private void DisplayVolumeLabel(float volumn)
        {
            Volume = volumn;

            //var sd = 
            //var x = this.label1;
            if (this.lblVolumeLevel.InvokeRequired)
            {
                this.lblVolumeLevel.Invoke(this.updateTextBox, String.Format("{0:F0}%", volumn * 100f));
            }
            else
            {
                this.lblVolumeLevel.Text = String.Format("{0:F0}%", volumn * 100f);
            }

            if (HKLMSoftwareTOAPlayer != null)
            {
                RegistryKey cHKLMPlayer = HKLMSoftwareTOAPlayer.CreateSubKey(this.PlayerName);
                cHKLMPlayer.SetValue("Volume", volumn.ToString(), RegistryValueKind.String);
                cHKLMPlayer.Close();
            }
        }

        private void trackBarPosition_ValueChanged(object sender, EventArgs e)
        {
            //if (audioFileReader != null)
            //{
            //    var lengthInBytes = audioFileReader.Length;
            //    var pos = (lengthInBytes / 100) * trackBarPosition.Value;
            //    audioFileReader.Position = pos;
            //}
        }

        public void scroll_music(double sec)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    var xx = sec;
                    if (wavePlayer != null)
                    {
                        TimeSpan currentTime = (wavePlayer.PlaybackState == PlaybackState.Stopped) ? TimeSpan.Zero : audioFileReader.CurrentTime;
                        trackBarPosition.Value = Math.Min(trackBarPosition.Maximum, (int)(100 * currentTime.TotalSeconds / audioFileReader.TotalTime.TotalSeconds));
                        var val = (xx * audioFileReader.TotalTime.TotalSeconds) / 100;
                        audioFileReader.CurrentTime = TimeSpan.FromSeconds(val);
                    }
                }));

            }
            else
            {
                var xx = sec;
                if (wavePlayer != null)
                {
                    TimeSpan currentTime = (wavePlayer.PlaybackState == PlaybackState.Stopped) ? TimeSpan.Zero : audioFileReader.CurrentTime;
                    trackBarPosition.Value = Math.Min(trackBarPosition.Maximum, (int)(100 * currentTime.TotalSeconds / audioFileReader.TotalTime.TotalSeconds));
                    var val = (xx * audioFileReader.TotalTime.TotalSeconds) / 100;
                    audioFileReader.CurrentTime = TimeSpan.FromSeconds(val);
                }
            }
        }

        private void trackBarPosition_Scroll(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    var xx = trackBarPosition.Value;
                    if (wavePlayer != null)
                    {
                        TimeSpan currentTime = (wavePlayer.PlaybackState == PlaybackState.Stopped) ? TimeSpan.Zero : audioFileReader.CurrentTime;
                        trackBarPosition.Value = Math.Min(trackBarPosition.Maximum, (int)(100 * currentTime.TotalSeconds / audioFileReader.TotalTime.TotalSeconds));
                        var val = (xx * audioFileReader.TotalTime.TotalSeconds) / 100;
                        audioFileReader.CurrentTime = TimeSpan.FromSeconds(val);
                        this.trigger_url();
                    }
                }));

            }
            else
            {
                var xx = trackBarPosition.Value;
                if (wavePlayer != null)
                {
                    TimeSpan currentTime = (wavePlayer.PlaybackState == PlaybackState.Stopped) ? TimeSpan.Zero : audioFileReader.CurrentTime;
                    trackBarPosition.Value = Math.Min(trackBarPosition.Maximum, (int)(100 * currentTime.TotalSeconds / audioFileReader.TotalTime.TotalSeconds));
                    var val = (xx * audioFileReader.TotalTime.TotalSeconds) / 100;
                    audioFileReader.CurrentTime = TimeSpan.FromSeconds(val);
                    this.trigger_url();

                }
            }
        }

        private void labelTotalTime_Click(object sender, EventArgs e)
        {

        }

        private void BackwardButton_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Prev", BackwardButton);
        }

        private void PlayButton_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Play/Pause", PlayButton);
        }

        private void StopButton_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Stop", StopButton);
        }

        private void ForwardButton_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Next", ForwardButton);
        }

        private void DecreaseVolumeButton_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Decrease Volume Level", DecreaseVolumeButton);
        }

        public string get_filename()
        {
            return this.filenamerun;
        }

        public void set_filename(string name)
        {
            this.filenamerun = name;
        }

        private void IncreaseVolumeButton_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Increase Volume Level", IncreaseVolumeButton);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            string proptValue = Prompt.ShowDialog("ใส่ข้อความ", "ตั้งชื่อ Track", nametrack, nametrackC, nametrackCF);
            if (proptValue != null && proptValue != "" && proptValue != " ")
            {
                var substring = proptValue.Split('+');
                RegistryKey configsocket = HKLMSoftwareTOAConfig.OpenSubKey("trackname", true);
                if (substring[0] != "")
                {
                    configsocket.SetValue(nametrack, substring[0]);
                    this.label2.Text = substring[0];
                    this.textfont = substring[0];
                }
                if (substring[1] != "")
                {
                    configsocket.SetValue(nametrackC, substring[1]);
                    bool flag = substring[1] != "" && substring[1] != " ";
                    if (flag)
                    {
                        bool flag2 = substring[1].IndexOf(",") == -1;
                        if (flag2)
                        {
                            this.label2.ForeColor = Color.FromName(substring[1]);
                        }
                        else
                        {
                            string[] subtext = substring[1].Split(new char[]
                            {
                        ','
                            });
                            this.label2.ForeColor = Color.FromArgb(Convert.ToInt32(subtext[3]), Convert.ToInt32(subtext[0]), Convert.ToInt32(subtext[1]), Convert.ToInt32(subtext[2]));
                        }
                        this.fgcolor = substring[1];
                    }
                }
                if (substring[2] != "")
                {
                    configsocket.SetValue(nametrackCF, substring[2]);
                    bool flag = substring[2] != "" && substring[2] != " ";
                    if (flag)
                    {
                        bool flag2 = substring[2].IndexOf(",") == -1;
                        if (flag2)
                        {
                            this.panel1.BackColor = Color.FromName(substring[2]);
                        }
                        else
                        {
                            string[] subtext = substring[2].Split(new char[]
                            {
                        ','
                            });
                            this.panel1.BackColor = Color.FromArgb(Convert.ToInt32(subtext[3]), Convert.ToInt32(subtext[0]), Convert.ToInt32(subtext[1]), Convert.ToInt32(subtext[2]));
                        }
                        this.bgcolor = substring[2];
                    }
                }

            }
            this.trigger_url();
        }

        public async void trigger_url()
        {
            RegistryKey configsocket = HKLMSoftwareTOAConfig.OpenSubKey("trackname", true);
            var stringurl = "http://" + configsocket.GetValue("ip").ToString() + "/api/updateData?ip_address=" + configsocket.GetValue("ip1") + "&port=" + configsocket.GetValue("port");
            //HttpClient client = new HttpClient();
            //try
            //{
            //    HttpResponseMessage response = await client.GetAsync(stringurl);

            //    // ตรวจสอบสถานะ HTTP
            //    response.EnsureSuccessStatusCode();

            //    string responseBody = await response.Content.ReadAsStringAsync();
            //    Console.WriteLine(responseBody);
            //}
            //catch (HttpRequestException e)
            //{
            //    Console.WriteLine($"Request error: {e.Message}");
            //}
            try
            {
                var client = new HttpClient();
                //var request = new HttpRequestMessage(HttpMethod.Get, "http://192.168.22.37/toa/api/updateData?ip_address=192.168.22.37&port=83");
                var request = new HttpRequestMessage(HttpMethod.Get, stringurl);
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                log.Info("Success Trigger");
            }
            catch (Exception ex)
            {
                log.Error("Error: " + ex.Message + " , Inner: " + ex.InnerException);
            }


        }

        public void change_text_color(string text, string bg, string fg)
        {
            RegistryKey configsocket = HKLMSoftwareTOAConfig.OpenSubKey("trackname", true);
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    if (text != "")
                    {
                        configsocket.SetValue(nametrack, text);
                        this.label2.Text = text;
                        this.textfont = text;
                    }
                    if (fg != "")
                    {
                        configsocket.SetValue(nametrackC, fg);
                        bool flag = fg != "" && fg != " ";
                        if (flag)
                        {
                            bool flag2 = fg.IndexOf(",") == -1;
                            if (flag2)
                            {
                                this.label2.ForeColor = Color.FromName(fg);
                            }
                            else
                            {
                                string[] subtext = fg.Split(new char[]
                                {
                        ','
                                });
                                this.label2.ForeColor = Color.FromArgb(Convert.ToInt32(subtext[3]), Convert.ToInt32(subtext[0]), Convert.ToInt32(subtext[1]), Convert.ToInt32(subtext[2]));
                            }
                            this.fgcolor = fg;
                        }
                    }
                    if (bg != "")
                    {
                        configsocket.SetValue(nametrackCF, bg);
                        bool flag = bg != "" && bg != " ";
                        if (flag)
                        {
                            bool flag2 = bg.IndexOf(",") == -1;
                            if (flag2)
                            {
                                this.panel1.BackColor = Color.FromName(bg);
                            }
                            else
                            {
                                string[] subtext = bg.Split(new char[]
                                {
                        ','
                                });
                                this.panel1.BackColor = Color.FromArgb(Convert.ToInt32(subtext[3]), Convert.ToInt32(subtext[0]), Convert.ToInt32(subtext[1]), Convert.ToInt32(subtext[2]));
                            }
                            this.bgcolor = bg;
                        }
                    }
                    log.Info("Trigger To URL");
                    this.trigger_url();
                }));
            }
            else
            {
                if (text != "")
                {
                    configsocket.SetValue(nametrack, text);
                    this.label2.Text = text;
                    this.textfont = text;
                }
                if (fg != "")
                {
                    configsocket.SetValue(nametrackC, fg);
                    bool flag = fg != "" && fg != " ";
                    if (flag)
                    {
                        bool flag2 = fg.IndexOf(",") == -1;
                        if (flag2)
                        {
                            this.label2.ForeColor = Color.FromName(fg);
                        }
                        else
                        {
                            string[] subtext = fg.Split(new char[]
                            {
                        ','
                            });
                            this.label2.ForeColor = Color.FromArgb(Convert.ToInt32(subtext[3]), Convert.ToInt32(subtext[0]), Convert.ToInt32(subtext[1]), Convert.ToInt32(subtext[2]));
                        }
                        this.fgcolor = fg;
                    }
                }
                if (bg != "")
                {
                    configsocket.SetValue(nametrackCF, bg);
                    bool flag = bg != "" && bg != " ";
                    if (flag)
                    {
                        bool flag2 = bg.IndexOf(",") == -1;
                        if (flag2)
                        {
                            this.panel1.BackColor = Color.FromName(bg);
                        }
                        else
                        {
                            string[] subtext = bg.Split(new char[]
                            {
                        ','
                            });
                            this.panel1.BackColor = Color.FromArgb(Convert.ToInt32(subtext[3]), Convert.ToInt32(subtext[0]), Convert.ToInt32(subtext[1]), Convert.ToInt32(subtext[2]));
                        }
                        this.bgcolor = bg;
                    }
                }
                log.Info("Trigger To URL");
                this.trigger_url();
            }

        }

    }

    public static class Prompt
    {
        public static string ShowDialog(string text, string caption, string nametrack, string nametrackC, string nametrackCF)
        {
            RegistryKey HKLMSoftwareTOAConfig = Registry.CurrentUser.OpenSubKey(@"Software\TOA\Config", true);
            RegistryKey configsocket = HKLMSoftwareTOAConfig.OpenSubKey("trackname", true);
            var text1 = configsocket.GetValue(nametrack);
            var text2 = configsocket.GetValue(nametrackC);
            var text3 = configsocket.GetValue(nametrackCF);

            Form prompt = new Form()
            {
                Width = 650,
                Height = 120,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Track Setting",
                StartPosition = FormStartPosition.CenterScreen,
                BackColor= ColorTranslator.FromHtml("#D9D9D9")
            };

            var textLabel = new Label() { Left = 20, Top = 20, Text = "Track Setting" };
            textLabel.ForeColor = ColorTranslator.FromHtml("#5A5A5A");

            var textBox = new TextBox() { Left = 110, Top = 18, Width = 500, Text = text1.ToString() };

            #region color 1
            var textBox1 = new TextBox() { Left = 170, Top = 50, Width = 80, Text = text2.ToString() };
            var subsc1 = text2.ToString().Split(',');
            var textlabel1 = new TextBox() { Left = 110, Top = 50, Width = 40, BackColor = Color.FromArgb(Convert.ToInt32(subsc1[3]), Convert.ToInt32(subsc1[0]), Convert.ToInt32(subsc1[1]), Convert.ToInt32(subsc1[2])) };
            #endregion

            #region color 2
            var textBox2 = new TextBox() { Left = 405, Top = 50, Width = 80, Text = text3.ToString() };
            var subsc2 = text3.ToString().Split(',');
            var textlabel2 = new TextBox() { Left = 345, Top = 50, Width = 40, BackColor = Color.FromArgb(Convert.ToInt32(subsc2[3]), Convert.ToInt32(subsc2[0]), Convert.ToInt32(subsc2[1]), Convert.ToInt32(subsc2[2])) };
            #endregion

            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            var colorback = new Button() { Text = "Track Color", Left = 260, Width = 75, Height = 23, Top = 50 };
            var colorfont = new Button() { Text = "Font Color", Left = 20, Width = 75, Height = 23, Top = 50 };
            colorback.ForeColor = Color.White;
            colorfont.ForeColor = Color.White;
            colorback.BackColor = ColorTranslator.FromHtml("#B0B0B0");
            colorfont.BackColor = ColorTranslator.FromHtml("#B0B0B0");

            var confirmation = new Button() { Text = "Ok", Left = 520, Width = 80, Height = 30, Top = 45, DialogResult = DialogResult.OK };
            confirmation.ForeColor = Color.White;
            confirmation.BackColor = ColorTranslator.FromHtml("#FFA500");  //ColorTranslator.FromHtml("#5A5A5A");
            confirmation.Click += (sender, e) => { prompt.Close(); };

            colorback.Click += (sender, e) =>
            {
                ColorDialog myDialogColor = new ColorDialog();
                myDialogColor.AllowFullOpen = true;
                myDialogColor.ShowHelp = true;
                if (myDialogColor.ShowDialog() == DialogResult.OK)
                {
                    var ss = myDialogColor.Color.R + "," + myDialogColor.Color.G + "," + myDialogColor.Color.B + "," + myDialogColor.Color.A;
                    textBox2.Text = ss.ToString();
                    textlabel2.BackColor = Color.FromArgb(Convert.ToInt32(myDialogColor.Color.A), Convert.ToInt32(myDialogColor.Color.R), Convert.ToInt32(myDialogColor.Color.G), Convert.ToInt32(myDialogColor.Color.B));
                }
            };

            colorfont.Click += (sender, e) =>
            {
                ColorDialog myDialogColor = new ColorDialog();
                myDialogColor.AllowFullOpen = true;
                myDialogColor.ShowHelp = true;
                if (myDialogColor.ShowDialog() == DialogResult.OK)
                {
                    var ss = myDialogColor.Color.R + "," + myDialogColor.Color.G + "," + myDialogColor.Color.B + "," + myDialogColor.Color.A;
                    textBox1.Text = ss.ToString();
                    textlabel1.BackColor = Color.FromArgb(Convert.ToInt32(myDialogColor.Color.A), Convert.ToInt32(myDialogColor.Color.R), Convert.ToInt32(myDialogColor.Color.G), Convert.ToInt32(myDialogColor.Color.B));
                }
            };

            prompt.Controls.Add(textBox);

            prompt.Controls.Add(textBox1);
            prompt.Controls.Add(textlabel1);

            prompt.Controls.Add(textBox2);
            prompt.Controls.Add(textlabel2);

            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(colorback);
            prompt.Controls.Add(colorfont);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text + "+" + textBox1.Text + "+" + textBox2.Text : "";
        }
    }

}

