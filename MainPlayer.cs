using Microsoft.Win32;
using NAudio.Wave;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TOAMediaPlayer.NAudioOutput;
using TOAMediaPlayer.Properties;
using TOAMediaPlayer.TOAPlaylist;
using MessageBox = System.Windows.Forms.MessageBox;

namespace TOAMediaPlayer
{
    //public partial class MainPlayer : Form
    public partial class MainPlayer : Form
    {
        private int borderSize = 2;
        private Size formSize;
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        private OTSMedia selectedMedia = new OTSMedia();
        const int HT_CAPTION = 0x2;
        const int WM_NCLBUTTONDOWN = 0xA1;
        public Logger log = LogManager.GetCurrentClassLogger();
        public static bool batchrun = false;
        public Loggers files = new Loggers("debugs.txt");
        // PlayList Prop
        private ListViewColumnSorter lvwColumnSorter;
        public bool bShuffle { get; set; }
        public bool bLoop { get; set; }
        public bool ships = false;

        public bool timersc = false;
        public bool showtimeset = false;


        public double playPositionTimeCode = 0;
        public short lastId = 1;
        private bool bServerStart = false;
        private ListViewItem heldDownItem;
        private Point heldDownPoint;

        public static bool st = false;

        public string timername = "";

        private Color ActiveBGColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
        private Color InactiveBGColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
        TOASocket server;
        AudioPlaybackPanel pls;
        //TOAHttpListener httpListener;
        /*
         * 
        */
        //private NAudio.Wave.BlockAlignReductionStream stream3 = null;
        ////Wave file format
        //private NAudio.Wave.WaveFileReader wave3 = null;
        //private NAudio.Wave.DirectSoundOut output3 = null;
        RegistryKey HKLMSoftware = Registry.CurrentUser.OpenSubKey("Software", true);
        RegistryKey HKLMSoftwareTOA = Registry.CurrentUser.OpenSubKey(@"Software\TOA", true);
        RegistryKey HKLMSoftwareTOAPlayer = Registry.CurrentUser.OpenSubKey(@"Software\TOA\Player", true);
        RegistryKey HKLMSoftwareTOAConfig = Registry.CurrentUser.OpenSubKey(@"Software\TOA\Config", true);
        RegistryKey HKLMSoftwareTOAPlayList = Registry.CurrentUser.OpenSubKey(@"Software\TOA\PlayList", true);
        RegistryKey HKLMSoftwareTOAPlayListLoop = Registry.CurrentUser.OpenSubKey(@"Software\TOA\PlayList\Loop", true);
        RegistryKey HKLMSoftwareTOAPlayListShuffle = Registry.CurrentUser.OpenSubKey(@"Software\TOA\PlayList\Shuffle", true);
        RegistryKey HKL_COLOR = Registry.CurrentUser.OpenSubKey(@"Software\TOA\Config", true);
        public CancellationTokenSource source = new CancellationTokenSource();

        private TOASocket ts;
        public delegate void updateTextBoxDelegate(Color str, int i);

        public updateTextBoxDelegate updateTextBox;

        void updateTextBox1(Color str, int i) { this.myListView.Items[i].BackColor = str; }
        
        CustomeTheme customeTheme = new CustomeTheme();

        private BackgroundWorker bgWorker = new BackgroundWorker();

        public MainPlayer()
        {
            bgWorker.DoWork += BgWorker_DoWork;
            bgWorker.RunWorkerCompleted += BgWorker_RunWorkerCompleted;

            #region เรียกใช้

            customeTheme.NewTheme();
            updateTextBox = new updateTextBoxDelegate(updateTextBox1);
            this.files.Info("START RUNNING PROGRAM..");
            bool flag = !MainPlayer.st;
            if (flag)
            {
                MainPlayer.st = true;
                this.start();
            }
            this.myListView.AllowDrop = true;
            this.myListView.MultiSelect = true;
            RegistryKey configsocket = this.HKLMSoftwareTOAConfig.OpenSubKey("trackname", true);
            if (configsocket.GetValue("port") == null)
            {
                configsocket.SetValue("port", "83");
                configsocket.SetValue("ip", "127.0.0.1");
            }
            if (configsocket.GetValue("trackoption1") == null)
            {
                configsocket.SetValue("trackoption1", "false");
                configsocket.SetValue("trackoption2", "false");
                configsocket.SetValue("trackoption3", "false");
                configsocket.SetValue("trackoption4", "false");
                configsocket.SetValue("trackoption5", "false");
                configsocket.SetValue("trackoption6", "false");
                configsocket.SetValue("trackoption7", "false");
                configsocket.SetValue("trackoption8", "false");
            }
            if (configsocket.GetValue("timers1") == null)
            {
                configsocket.SetValue("timers1", "unactive,,00:00,00:00,unactive,unactive,unactive");
                configsocket.SetValue("timers2", "unactive,,00:00,00:00,unactive,unactive,unactive");
                configsocket.SetValue("timers3", "unactive,,00:00,00:00,unactive,unactive,unactive");
                configsocket.SetValue("timers4", "unactive,,00:00,00:00,unactive,unactive,unactive");
                configsocket.SetValue("timers5", "unactive,,00:00,00:00,unactive,unactive,unactive");
                configsocket.SetValue("timers6", "unactive,,00:00,00:00,unactive,unactive,unactive");
                configsocket.SetValue("timers7", "unactive,,00:00,00:00,unactive,unactive,unactive");
                configsocket.SetValue("timers8", "unactive,,00:00,00:00,unactive,unactive,unactive");

                this.nPlayer1.setTimers("timers1", this);
                this.nPlayer2.setTimers("timers2", this);
                this.nPlayer3.setTimers("timers3", this);
                this.nPlayer4.setTimers("timers4", this);
                this.nPlayer5.setTimers("timers5", this);
                this.nPlayer6.setTimers("timers6", this);
                this.nPlayer7.setTimers("timers7", this);
                this.nPlayer8.setTimers("timers8", this);
            }
            else
            {

                this.nPlayer1.setTimers("timers1", this);
                this.nPlayer2.setTimers("timers2", this);
                this.nPlayer3.setTimers("timers3", this);
                this.nPlayer4.setTimers("timers4", this);
                this.nPlayer5.setTimers("timers5", this);
                this.nPlayer6.setTimers("timers6", this);
                this.nPlayer7.setTimers("timers7", this);
                this.nPlayer8.setTimers("timers8", this);
            }

            bool flag2 = configsocket != null;
            if (flag2)
            {
                bool flag3 = configsocket.GetValue("track1") != null;
                this.nPlayer1.setnametrack("track1", "trackC1", "trackCF1", "dB1", "adB1");
                this.nPlayer2.setnametrack("track2", "trackC2", "trackCF2", "dB2", "adB2");
                this.nPlayer3.setnametrack("track3", "trackC3", "trackCF3", "dB3", "adB3");
                this.nPlayer4.setnametrack("track4", "trackC4", "trackCF4", "dB4", "adB4");
                this.nPlayer5.setnametrack("track5", "trackC5", "trackCF5", "dB5", "adB5");
                this.nPlayer6.setnametrack("track6", "trackC6", "trackCF6", "dB6", "adB6");
                this.nPlayer7.setnametrack("track7", "trackC7", "trackCF7", "dB7", "adB7");
                this.nPlayer8.setnametrack("track8", "trackC8", "trackCF8", "dB8", "adB8");
                if (flag3)
                {
                    this.nPlayer1.setting_main(this, configsocket.GetValue("track1").ToString(), 1);
                    this.nPlayer2.setting_main(this, configsocket.GetValue("track2").ToString(), 2);
                    this.nPlayer3.setting_main(this, configsocket.GetValue("track3").ToString(), 3);
                    this.nPlayer4.setting_main(this, configsocket.GetValue("track4").ToString(), 4);
                    this.nPlayer5.setting_main(this, configsocket.GetValue("track5").ToString(), 5);
                    this.nPlayer6.setting_main(this, configsocket.GetValue("track6").ToString(), 6);
                    this.nPlayer7.setting_main(this, configsocket.GetValue("track7").ToString(), 7);
                    this.nPlayer8.setting_main(this, configsocket.GetValue("track8").ToString(), 8);
                    bool flag4 = configsocket.GetValue("trackCF1") != null;
                    if (flag4)
                    {
                        this.nPlayer1.setting_color(this, configsocket.GetValue("trackCF1").ToString());
                        this.nPlayer2.setting_color(this, configsocket.GetValue("trackCF2").ToString());
                        this.nPlayer3.setting_color(this, configsocket.GetValue("trackCF3").ToString());
                        this.nPlayer4.setting_color(this, configsocket.GetValue("trackCF4").ToString());
                        this.nPlayer5.setting_color(this, configsocket.GetValue("trackCF5").ToString());
                        this.nPlayer6.setting_color(this, configsocket.GetValue("trackCF6").ToString());
                        this.nPlayer7.setting_color(this, configsocket.GetValue("trackCF7").ToString());
                        this.nPlayer8.setting_color(this, configsocket.GetValue("trackCF8").ToString());
                    }
                    bool flag5 = configsocket.GetValue("trackC1") != null;
                    if (flag5)
                    {
                        this.nPlayer1.setting_font_color(this, configsocket.GetValue("trackC1").ToString());
                        this.nPlayer2.setting_font_color(this, configsocket.GetValue("trackC2").ToString());
                        this.nPlayer3.setting_font_color(this, configsocket.GetValue("trackC3").ToString());
                        this.nPlayer4.setting_font_color(this, configsocket.GetValue("trackC4").ToString());
                        this.nPlayer5.setting_font_color(this, configsocket.GetValue("trackC5").ToString());
                        this.nPlayer6.setting_font_color(this, configsocket.GetValue("trackC6").ToString());
                        this.nPlayer7.setting_font_color(this, configsocket.GetValue("trackC7").ToString());
                        this.nPlayer8.setting_font_color(this, configsocket.GetValue("trackC8").ToString());
                    }
                }
                else
                {
                    this.nPlayer1.setting_main(this, "track1", 1);
                    this.nPlayer2.setting_main(this, "track2", 2);
                    this.nPlayer3.setting_main(this, "track3", 3);
                    this.nPlayer4.setting_main(this, "track4", 4);
                    this.nPlayer5.setting_main(this, "track5", 5);
                    this.nPlayer6.setting_main(this, "track6", 6);
                    this.nPlayer7.setting_main(this, "track7", 7);
                    this.nPlayer8.setting_main(this, "track8", 8);
                    this.nPlayer1.setting_color(this, "Red");
                    this.nPlayer2.setting_color(this, "Red");
                    this.nPlayer3.setting_color(this, "Red");
                    this.nPlayer4.setting_color(this, "Red");
                    this.nPlayer5.setting_color(this, "Red");
                    this.nPlayer6.setting_color(this, "Red");
                    this.nPlayer7.setting_color(this, "Red");
                    this.nPlayer8.setting_color(this, "Red");
                    this.nPlayer1.setting_font_color(this, "White");
                    this.nPlayer2.setting_font_color(this, "White");
                    this.nPlayer3.setting_font_color(this, "White");
                    this.nPlayer4.setting_font_color(this, "White");
                    this.nPlayer5.setting_font_color(this, "White");
                    this.nPlayer6.setting_font_color(this, "White");
                    this.nPlayer7.setting_font_color(this, "White");
                    this.nPlayer8.setting_font_color(this, "White");
                }
            }
            else
            {
                this.nPlayer1.setting_main(this, "track1", 1);
                this.nPlayer2.setting_main(this, "track2", 2);
                this.nPlayer3.setting_main(this, "track3", 3);
                this.nPlayer4.setting_main(this, "track4", 4);
                this.nPlayer5.setting_main(this, "track5", 5);
                this.nPlayer6.setting_main(this, "track6", 6);
                this.nPlayer7.setting_main(this, "track7", 7);
                this.nPlayer8.setting_main(this, "track8", 8);
                this.nPlayer1.setting_color(this, "Red");
                this.nPlayer2.setting_color(this, "Red");
                this.nPlayer3.setting_color(this, "Red");
                this.nPlayer4.setting_color(this, "Red");
                this.nPlayer5.setting_color(this, "Red");
                this.nPlayer6.setting_color(this, "Red");
                this.nPlayer7.setting_color(this, "Red");
                this.nPlayer8.setting_color(this, "Red");
                this.nPlayer1.setting_font_color(this, "White");
                this.nPlayer2.setting_font_color(this, "White");
                this.nPlayer3.setting_font_color(this, "White");
                this.nPlayer4.setting_font_color(this, "White");
                this.nPlayer5.setting_font_color(this, "White");
                this.nPlayer6.setting_font_color(this, "White");
                this.nPlayer7.setting_font_color(this, "White");
                this.nPlayer8.setting_font_color(this, "White");
            }

            this.myListView.AllowDrop = true;
            this.ts = new TOASocket(this);
            //iconButtonPlayer1_Click(null, new EventArgs());
            #endregion
        }

        public TOASocket GetSocketInstance() {
            return server;
        }

        private void BgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(3000); // จำลองโหลดข้อมูล
        }

        private void BgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Hide();
        }

        public void updatecolor()
        {
            if (HKL_COLOR != null)
            {

                var getColorTrack = HKL_COLOR.OpenSubKey("trackname");
                if (getColorTrack != null)
                {
                    var getColorTrack1 = getColorTrack.GetValue("trackCF1");
                    var getColorTrack2 = getColorTrack.GetValue("trackC1");
                    var getColorTrack3 = getColorTrack.GetValue("track1");
                    if (getColorTrack1 != null)
                    {
                        nPlayer1.set_color_back(getColorTrack.GetValue("trackCF1").ToString());
                        nPlayer2.set_color_back(getColorTrack.GetValue("trackCF2").ToString());
                        nPlayer3.set_color_back(getColorTrack.GetValue("trackCF3").ToString());
                        nPlayer4.set_color_back(getColorTrack.GetValue("trackCF4").ToString());
                        nPlayer5.set_color_back(getColorTrack.GetValue("trackCF5").ToString());
                        nPlayer6.set_color_back(getColorTrack.GetValue("trackCF6").ToString());
                        nPlayer7.set_color_back(getColorTrack.GetValue("trackCF7").ToString());
                        nPlayer8.set_color_back(getColorTrack.GetValue("trackCF8").ToString());

                    }
                    if (getColorTrack2 != null)
                    {
                        nPlayer1.set_color_text(getColorTrack.GetValue("trackC1").ToString());
                        nPlayer2.set_color_text(getColorTrack.GetValue("trackC2").ToString());
                        nPlayer3.set_color_text(getColorTrack.GetValue("trackC3").ToString());
                        nPlayer4.set_color_text(getColorTrack.GetValue("trackC4").ToString());
                        nPlayer5.set_color_text(getColorTrack.GetValue("trackC5").ToString());
                        nPlayer6.set_color_text(getColorTrack.GetValue("trackC6").ToString());
                        nPlayer7.set_color_text(getColorTrack.GetValue("trackC7").ToString());
                        nPlayer8.set_color_text(getColorTrack.GetValue("trackC8").ToString());
                    }
                    if (getColorTrack3 != null)
                    {
                        nPlayer1.set_text(getColorTrack.GetValue("track1").ToString());
                        nPlayer2.set_text(getColorTrack.GetValue("track2").ToString());
                        nPlayer3.set_text(getColorTrack.GetValue("track3").ToString());
                        nPlayer4.set_text(getColorTrack.GetValue("track4").ToString());
                        nPlayer5.set_text(getColorTrack.GetValue("track5").ToString());
                        nPlayer6.set_text(getColorTrack.GetValue("track6").ToString());
                        nPlayer7.set_text(getColorTrack.GetValue("track7").ToString());
                        nPlayer8.set_text(getColorTrack.GetValue("track8").ToString());
                    }
                }
            }
        }

        public void start()
        {
            InitializeComponent();
            PrepareRegistryKey();
            //playerControl1.TOABackgroundColorChanged += PlayerControl1_TOABackgroundColorChanged;
            //playerControl1.TOABackgroundColor = Color.FromArgb(66, 66, 66);
            //playerControl1.PositionTimeCodeChanged += PlayerControl1_PositionTimeCodeChanged;
            server = new TOASocket(this);
            //httpListener = new TOAHttpListener();
            lvwColumnSorter = new ListViewColumnSorter();
            //Default Sort is None
            this.myListView.ListViewItemSorter = lvwColumnSorter;
            splitContainer1.SplitterDistance = (iconButtonPlayer1.Width + nPlayer1.Width) + 2;
            splitContainer2.SplitterDistance = panel2.Height;
            for (short i = 2; i < 9; i++) {
                this.lblPlayerId.Text = i.ToString();
                LoadDefaultPlaylist(i);
            }
            iconButtonPlayer1_Click(null, new EventArgs());

            //RunProcess("noderun.bat", source.Token);

            RegistryKey configsocket = HKLMSoftwareTOAConfig.OpenSubKey("StatusProgram", true);
            if (HKLMSoftwareTOAConfig == null)
            {
                HKLMSoftware.CreateSubKey("TOA");
            }
            if (configsocket != null)
            {
                var socket_st = configsocket.GetValue("Sockets");
                if (Convert.ToBoolean(socket_st) == true)
                {
                    try
                    {
                        log.Info("START SOCKET AND BAT");
                        //Process p = new Process();
                        //p.StartInfo.UseShellExecute = false;
                        //p.StartInfo.RedirectStandardOutput = true;
                        //p.StartInfo.FileName = "currport.bat";
                        //p.Start();

                        //string output = p.StandardOutput.ReadToEnd();
                        //p.WaitForExit();
                        //var search = output.IndexOf("LISTENING");
                        //if(search != -1)
                        //{
                        //    var substrings = output.Substring(search, output.Length-search);
                        //    string[] stringSeparators = new string[] { "\r\n" };
                        //    var linewrite = substrings.ToString().Split(stringSeparators, StringSplitOptions.None);
                        //    var get_PID = linewrite[0].Replace(" ", "").Replace("LISTENING", "");
                        //    System.Diagnostics.Process.Start("CMD.exe", "taskkill /F /PID "+get_PID);
                        //}
                        server.Start();
                        //RunProcess("websockets.bat", source.Token);
                        //RunProcess("noderun.bat", source.Token);
                    }
                    catch (Exception ex)
                    {
                        log.Error("start Error : " + ex);
                    }
                }
            }
            else
            {
                RegistryKey Configs = HKLMSoftwareTOAConfig.CreateSubKey("StatusProgram");
                Configs.SetValue("Sockets", false, RegistryValueKind.String);
            }

            bServerStart = true;

        }

        private void PrepareRegistryKey()
        {

            if (HKLMSoftware != null)
            {
                RegistryKey cHKLMSoftwareTOA = HKLMSoftware.CreateSubKey("TOA");
                //bShuffle = (bool)regKeyTOASubKey.GetValue("Shuffle");
                using (RegistryKey
                    Player = cHKLMSoftwareTOA.CreateSubKey("Player"),
                    Config = cHKLMSoftwareTOA.CreateSubKey("Config"),
                    PlayList = cHKLMSoftwareTOA.CreateSubKey("PlayList"))
                {
                    Player.SetValue("Player-Id", 8, RegistryValueKind.DWord);
                    //Player.SetValue("Player-Count", 8, RegistryValueKind.DWord);
                    //PlayList.SetValue("PlayList-Count", 8,RegistryValueKind.DWord);
                    using (RegistryKey
                        Loop = PlayList.CreateSubKey("Loop"),
                        Shuffle = PlayList.CreateSubKey("Shuffle"))
                    {
                        Loop.SetValue("MultipleStringValue", new string[] { "One", "Two", "Three" }, RegistryValueKind.MultiString);
                        //Loop.Close();

                        Shuffle.SetValue("BinaryValue", new byte[] { 10, 43, 44, 45, 14, 255 }, RegistryValueKind.Binary);
                        //Shuffle.Close();
                    }

                    //Shuffle.SetValue("ID", 123);
                }
                //cHKLMSoftwareTOA.Close();
            }
            //regKeyTOASubKey.SetValue("Shuffle", bShuffle, RegistryValueKind.String);

        }

        private void iconWindowMinimize_Click(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Minimized;
            this.WindowState = FormWindowState.Minimized;
        }
        private void iconWindowMaximize_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                formSize = this.ClientSize;
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                this.formSize.Width = this.ClientSize.Width;
                this.formSize.Height = this.ClientSize.Height;
                this.Size = formSize;
            }
        }
        private void iconWindowClose_Click(object sender, EventArgs e)
        {

            //EDIT : WIKORN
            //Must Dispose Resourcs before Exit Programming
            // DO Later
            MainPlayer.batchrun = true;
            //DisposeWave();
            server.Stop();
            //Thread.Sleep(1000);
            //Thread.Sleep(3000);
            //starts.Kill();
            //source.Cancel();
            //Application.Exit();
            Environment.Exit(0);
        }
        private void MainPlayer_Resize(object sender, EventArgs e)
        {
            AdjustForm();
        }
        private void AdjustForm()
        {
            switch (this.WindowState)
            {
                case FormWindowState.Maximized: //Maximized form (After)
                    this.Padding = new Padding(8, 8, 8, 8);

                    break;
                case FormWindowState.Normal: //Restored form (After)
                    if (this.Padding.Top != borderSize)
                        this.Padding = new Padding(borderSize);
                    break;
            }
            //playerControl2.Padding = this.Padding;
            splitContainer1.SplitterDistance = (iconButtonPlayer1.Width + nPlayer1.Width) + 2;// (iconButtonPlayer1.Width + playerControl1.Width);
            splitContainer2.SplitterDistance = panel2.Height;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                if (e.Clicks > 1)
                {
                    // do something with double-click

                }
                else
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
            }
        }

        #region กระบวนการ เล่นเพลง 
        protected override void WndProc(ref Message m)
        {
            const int WM_NCCALCSIZE = 0x0083;//Standar Title Bar - Snap Window
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_MINIMIZE = 0xF020; //Minimize form (Before)
            const int SC_RESTORE = 0xF120; //Restore form (Before)
            const int WM_NCHITTEST = 0x0084;//Win32, Mouse Input Notification: Determine what part of the window corresponds to a point, allows to resize the form.
            const int resizeAreaSize = 10;
            #region Form Resize
            // Resize/WM_NCHITTEST values
            const int HTCLIENT = 1; //Represents the client area of the window
            const int HTLEFT = 10;  //Left border of a window, allows resize horizontally to the left
            const int HTRIGHT = 11; //Right border of a window, allows resize horizontally to the right
            const int HTTOP = 12;   //Upper-horizontal border of a window, allows resize vertically up
            const int HTTOPLEFT = 13;//Upper-left corner of a window border, allows resize diagonally to the left
            const int HTTOPRIGHT = 14;//Upper-right corner of a window border, allows resize diagonally to the right
            const int HTBOTTOM = 15; //Lower-horizontal border of a window, allows resize vertically down
            const int HTBOTTOMLEFT = 16;//Lower-left corner of a window border, allows resize diagonally to the left
            const int HTBOTTOMRIGHT = 17;//Lower-right corner of a window border, allows resize diagonally to the right

            if (m.Msg == WM_NCHITTEST)
            {
                base.WndProc(ref m);
                if (this.WindowState == FormWindowState.Normal)
                {
                    if ((int)m.Result == HTCLIENT)
                    {
                        Point screenPoint = new Point(m.LParam.ToInt32());
                        Point clientPoint = this.PointToClient(screenPoint);
                        if (clientPoint.Y <= resizeAreaSize)
                        {
                            if (clientPoint.X <= resizeAreaSize)
                                m.Result = (IntPtr)HTTOPLEFT;
                            else if (clientPoint.X < (this.Size.Width - resizeAreaSize))
                                m.Result = (IntPtr)HTTOP;
                            else
                                m.Result = (IntPtr)HTTOPRIGHT;
                        }
                        else if (clientPoint.Y <= (this.Size.Height - resizeAreaSize))
                        {
                            if (clientPoint.X <= resizeAreaSize)
                                m.Result = (IntPtr)HTLEFT;
                            else if (clientPoint.X > (this.Width - resizeAreaSize))
                                m.Result = (IntPtr)HTRIGHT;
                        }
                        else
                        {
                            if (clientPoint.X <= resizeAreaSize)
                                m.Result = (IntPtr)HTBOTTOMLEFT;
                            else if (clientPoint.X < (this.Size.Width - resizeAreaSize))
                                m.Result = (IntPtr)HTBOTTOM;
                            else
                                m.Result = (IntPtr)HTBOTTOMRIGHT;
                        }
                    }
                }
                return;
            }
            #endregion

            if (m.Msg == WM_NCCALCSIZE && m.WParam.ToInt32() == 1)
            {
                return;
            }

            if (m.Msg == WM_SYSCOMMAND)
            {

                int wParam = (m.WParam.ToInt32() & 0xFFF0);
                if (wParam == SC_MINIMIZE)  //Before
                    formSize = this.ClientSize;
                if (wParam == SC_RESTORE)// Restored form(Before)
                    this.Size = formSize;
            }
            base.WndProc(ref m);
        }
        #endregion

        private void MainPlayer_Load(object sender, EventArgs e)
        {
            nPlayer1.PlayerName = "nPlayer1";
            nPlayer2.PlayerName = "nPlayer2";
            nPlayer3.PlayerName = "nPlayer3";
            nPlayer4.PlayerName = "nPlayer4";
            nPlayer5.PlayerName = "nPlayer5";
            nPlayer6.PlayerName = "nPlayer6";
            nPlayer7.PlayerName = "nPlayer7";
            nPlayer8.PlayerName = "nPlayer8";
        }

        private void MainPlayer_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisposeWave();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        #region PlayList Button
        private void btnPlaylistAddFolder_Click(object sender, EventArgs e)
        {
            List<jsonWebAPI.MusicErrorList> dd = new List<jsonWebAPI.MusicErrorList>();
            bool bShowNewFolderButton = Settings.Default.ShowNewFolderButton;
            bool bSetEnvironmentSpecialFolder = Settings.Default.SetEnvironmentToSpecialFolder;
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = bShowNewFolderButton;
            bool flag = folderBrowserDialog.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(folderBrowserDialog.SelectedPath);
                bool flag2 = dirInfo == null;
                if (!flag2)
                {
                    FileInfo[] files = dirInfo.GetFiles();
                    bool flag3 = files != null;
                    if (flag3)
                    {
                        int seq = this.myListView.Items.Count;
                        foreach (FileInfo _file in files)
                        {
                            var ckfi = checkfile_gg(_file.FullName);
                            if (ckfi.Item1 == true)
                            {
                                dd.Add(new jsonWebAPI.MusicErrorList
                                {
                                    name = _file.Name,
                                    location = _file.FullName,
                                    PlayerTrack = ckfi.Item2
                                });
                                continue;
                            }
                            int checkfile = _file.Name.IndexOf(".mp3");
                            //int checkfile = (_file.Name.IndexOf(".wav") == -1) ? _file.Name.IndexOf(".mp3") : _file.Name.IndexOf(".wav");
                            bool flag4 = checkfile != -1;
                            if (flag4)
                            {
                                bool flag5 = CoreLibrary.check_audio_file(_file.FullName);
                                if (flag5)
                                {
                                    seq++;
                                    var ggg = _file.FullName.Split('\\');
                                    //string strCmdText;
                                    //strCmdText = "ffmpeg -i "+ _file.FullName + " -filter:a loudnorm output.mp3";
                                    //System.Diagnostics.Process.Start("CMD.exe", strCmdText);
                                    ListViewItem item = new ListViewItem(seq.ToString());
                                    item.SubItems.Add(_file.Name);
                                    item.SubItems.Add(string.Format("{0:hh\\:mm\\:ss}", CoreLibrary.GetNAudoSongLength(_file.FullName)));
                                    item.SubItems.Add(_file.FullName);
                                    this.myListView.Items.Add(item);
                                }
                                else
                                {
                                    string messagemusc = "ไฟล์เสีย ชื่อ " + _file.FullName;
                                    var messagebox = new Helper.MessageBox();
                                    messagebox.ShowCenter_DialogError(messagemusc, "แจ้งเตือน");
                                }
                            }
                        }
                        this.ReOrderSequence();
                        short idsa = short.Parse(this.lblPlayerId.Text.Trim());
                        this.SaveDefaultPlayList(idsa);
                        this.LoadDefaultPlaylist(idsa);
                    }
                    bool flag6 = bSetEnvironmentSpecialFolder;
                    if (flag6)
                    {
                        Environment.SpecialFolder root = folderBrowserDialog.RootFolder;
                    }
                }
                if (dd.Count > 0)
                {
                    //this.trigger_errormusic_url(dd);
                    ErrorMusic fms = new ErrorMusic(dd);
                    fms.TopMost = true;
                    fms.Name = "ErrorMusic";
                    //fms.Owner = this;
                    fms.ShowDialog();
                }
            }
            this.trigger_url();
        }

        public void change_file_kag(string name)
        {
            for (var i = 0; i < this.myListView.Items.Count; i++)
            {
                if (this.myListView.Items[i].SubItems[3].Text.ToString() == name)
                {
                    this.myListView.Items[i].BackColor = Color.Red;
                }
            }
        }

        private void btnPlaylistAddFile_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDlg = new System.Windows.Forms.OpenFileDialog();
            List<jsonWebAPI.MusicErrorList> dd = new List<jsonWebAPI.MusicErrorList>();

            openFileDlg.Multiselect = true;
            openFileDlg.Title = "Add TOA Audio support Files";
            openFileDlg.Filter = "TOA Audio Support File (*.mp3)|*.mp3";
            bool flag = openFileDlg.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                foreach (string file in openFileDlg.FileNames)
                {
                    FileInfo _file = new FileInfo(file);
                    bool flag2 = _file != null;
                    if (flag2)
                    {
                        bool flag3 = CoreLibrary.check_audio_file(_file.FullName);
                        var ckfi = checkfile_gg(_file.FullName);
                        if (ckfi.Item1 == true)
                        {
                            dd.Add(new jsonWebAPI.MusicErrorList
                            {
                                name = _file.Name,
                                location = _file.FullName,
                                PlayerTrack = ckfi.Item2
                            });

                            continue;
                        }
                        if (flag3)
                        {
                            ListViewItem item = new ListViewItem((this.myListView.Items.Count + 1).ToString());
                            item.SubItems.Add(_file.Name);
                            item.SubItems.Add(string.Format("{0:hh\\:mm\\:ss}", CoreLibrary.GetNAudoSongLength(_file.FullName)));
                            item.SubItems.Add(_file.FullName);
                            this.myListView.Items.Add(item);
                        }
                        else
                        {
                            string messagemusc = "ไฟล์เสียชื่อ " + _file.FullName;
                            var messagebox = new Helper.MessageBox();
                            messagebox.ShowCenter_DialogError(messagemusc, "แจ้งเตือน");
                        }
                    }
                }

                this.ReOrderSequence();
                short idsa = short.Parse(this.lblPlayerId.Text.Trim());
                this.SaveDefaultPlayList(idsa);
                this.LoadDefaultPlaylist(idsa);
            }
            if (dd.Count > 0)
            {
                //this.trigger_errormusic_url(dd);
                ErrorMusic fms = new ErrorMusic(dd);
                fms.TopMost = true;
                //fms.Owner = this;

                if (fms == null || fms.IsDisposed)
                {
                    fms = new ErrorMusic(dd);
                }
                fms.Name = "ErrorMusic";
                fms.ShowDialog();            
            }
            this.trigger_url();
        }
        
        private void btnPlaylistRemove_Click(object sender, EventArgs e)
        {
            this.PlaylistRemove();
        }
        
        private void PlaylistRemove()
        {
            bool bRemoveItem = false;
            bool flag = this.myListView.SelectedIndices.Count > 0;
            if (flag)
            {
                //var messagebox = new Helper.MessageBox();
                //messagebox.ShowCenter_DialogError("ไม่มีข้อมูลเพลงผิดพลาด", "แจ้งเตือน");

                DialogResult confirmation = MessageBox.Show("Delete Song", "Confirm!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                bool flag2 = confirmation == DialogResult.Yes;
                if (flag2)
                {
                    string[] removeIndex = new string[this.myListView.SelectedIndices.Count];
                    for (int i = this.myListView.SelectedIndices.Count - 1; i >= 0; i--)
                    {
                        this.stop_music_id(this.myListView.SelectedItems[i].Index);
                        removeIndex[i] = this.myListView.SelectedIndices[i].ToString();
                        this.myListView.Items.RemoveAt(this.myListView.SelectedIndices[i]);
                    }
                    bRemoveItem = true;

                    int flag11 = int.Parse(this.lblPlayerId.Text.Trim());
                    
                    string fileName = String.Format("{0}\\{1}-List.txt", System.Environment.CurrentDirectory, flag11);
                    this.getDeleteFileApi(flag11,fileName, removeIndex);
                    SavePlaylistToFile(fileName);
                }
            }
            bool flag3 = bRemoveItem && this.myListView.Items.Count > 0;
            if (flag3)
            {
                this.ReOrderSequence();
            }
            else
            {
                bool flag4 = this.myListView.Items.Count == 0;
                if (flag4)
                {
                    this.clear_music();
                    short idsa = short.Parse(this.lblPlayerId.Text.Trim());
                    this.SaveDefaultPlayList(idsa, true);
                    this.LoadDefaultPlaylist(idsa);
                }
            }
            this.trigger_url();
        }

        private void SavePlaylistToFile(string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                foreach (ListViewItem item in myListView.Items)
                {
                    // ตรวจสอบให้แน่ใจว่า SubItems มีครบ 4 คอลัมน์ก่อนบันทึก
                    string id = item.Text;
                    string title = item.SubItems.Count > 1 ? item.SubItems[1].Text : "";
                    string artist = item.SubItems.Count > 2 ? item.SubItems[2].Text : "";
                    string filePathData = item.SubItems.Count > 3 ? item.SubItems[3].Text : "";
                    sw.WriteLine($"{id}\t{title}\t{artist}\t{filePathData}\t");
                }
            }

            //using (StreamWriter sw = new StreamWriter(filePath))
            //{
            //    foreach (ListViewItem item in myListView.Items)
            //    {
            //        sw.WriteLine($"{item.Text},{item.SubItems[1].Text},{item.SubItems[2].Text}");
            //    }
            //}
        }

        public void change_music_list(string old, int news)
        {
            bool flag = int.Parse(this.lblPlayerId.Text.Trim()) == 1;
            if (flag)
            {
                this.SaveDefaultPlayList(this.lastId);
                this.LoadDefaultPlaylist(1);
                string ns = this.nPlayer1.get_filename();
                string[] namemusic = ns.Split(new char[]
                {
                    '\\'
                });
                this.change_namemedia(this.Index_item(namemusic[namemusic.Length - 1]), 1);
            }
            bool flag2 = int.Parse(this.lblPlayerId.Text.Trim()) == 2;
            if (flag2)
            {
                this.SaveDefaultPlayList(this.lastId);
                this.LoadDefaultPlaylist(2);
                string ns2 = this.nPlayer2.get_filename();
                string[] namemusic2 = ns2.Split(new char[]
                {
                    '\\'
                });
                this.change_namemedia(this.Index_item(namemusic2[namemusic2.Length - 1]), 2);
            }
            bool flag3 = int.Parse(this.lblPlayerId.Text.Trim()) == 3;
            if (flag3)
            {
                this.SaveDefaultPlayList(this.lastId);
                this.LoadDefaultPlaylist(3);
                string ns3 = this.nPlayer3.get_filename();
                string[] namemusic3 = ns3.Split(new char[]
                {
                    '\\'
                });
                this.change_namemedia(this.Index_item(namemusic3[namemusic3.Length - 1]), 3);
            }
            bool flag4 = int.Parse(this.lblPlayerId.Text.Trim()) == 4;
            if (flag4)
            {
                this.SaveDefaultPlayList(this.lastId);
                this.LoadDefaultPlaylist(4);
                string ns4 = this.nPlayer4.get_filename();
                string[] namemusic4 = ns4.Split(new char[]
                {
                    '\\'
                });
                this.change_namemedia(this.Index_item(namemusic4[namemusic4.Length - 1]), 4);
            }
            bool flag5 = int.Parse(this.lblPlayerId.Text.Trim()) == 5;
            if (flag5)
            {
                this.SaveDefaultPlayList(this.lastId);
                this.LoadDefaultPlaylist(5);
                string ns5 = this.nPlayer5.get_filename();
                string[] namemusic5 = ns5.Split(new char[]
                {
                    '\\'
                });
                this.change_namemedia(this.Index_item(namemusic5[namemusic5.Length - 1]), 5);
            }
            bool flag6 = int.Parse(this.lblPlayerId.Text.Trim()) == 6;
            if (flag6)
            {
                this.SaveDefaultPlayList(this.lastId);
                this.LoadDefaultPlaylist(6);
                string ns6 = this.nPlayer6.get_filename();
                string[] namemusic6 = ns6.Split(new char[]
                {
                    '\\'
                });
                this.change_namemedia(this.Index_item(namemusic6[namemusic6.Length - 1]), 6);
            }
            bool flag7 = int.Parse(this.lblPlayerId.Text.Trim()) == 7;
            if (flag7)
            {
                this.SaveDefaultPlayList(this.lastId);
                this.LoadDefaultPlaylist(7);
                string ns7 = this.nPlayer7.get_filename();
                string[] namemusic7 = ns7.Split(new char[]
                {
                    '\\'
                });
                this.change_namemedia(this.Index_item(namemusic7[namemusic7.Length - 1]), 7);
            }
            bool flag8 = int.Parse(this.lblPlayerId.Text.Trim()) == 8;
            if (flag8)
            {
                this.SaveDefaultPlayList(this.lastId);
                this.LoadDefaultPlaylist(8);
                string ns8 = this.nPlayer8.get_filename();
                string[] namemusic8 = ns8.Split(new char[]
                {
                    '\\'
                });
                this.change_namemedia(this.Index_item(namemusic8[namemusic8.Length - 1]), 8);
            }
        }

        public void update_position_music(int playerstrack, int index)
        {
            bool flag = playerstrack == 1;
            if (flag)
            {
                this.nPlayer1.set_runorder(index);
            }
            bool flag2 = playerstrack == 2;
            if (flag2)
            {
                this.nPlayer2.set_runorder(index);
            }
            bool flag3 = playerstrack == 3;
            if (flag3)
            {
                this.nPlayer3.set_runorder(index);
            }
            bool flag4 = playerstrack == 4;
            if (flag4)
            {
                this.nPlayer4.set_runorder(index);
            }
            bool flag5 = playerstrack == 5;
            if (flag5)
            {
                this.nPlayer5.set_runorder(index);
            }
            bool flag6 = playerstrack == 6;
            if (flag6)
            {
                this.nPlayer6.set_runorder(index);
            }
            bool flag7 = playerstrack == 7;
            if (flag7)
            {
                this.nPlayer7.set_runorder(index);
            }
            bool flag8 = playerstrack == 8;
            if (flag8)
            {
                this.nPlayer8.set_runorder(index);
            }
        }

        string list = "";

        #region เปลื่ยนเพลงถัดไป
        //List<randommusic> randomlist = new List<randommusic>();
        public void change_namemedia(int index, int playertrack)
        {
            int eq = this.myListView.Items.Count;
            bool flag = int.Parse(this.lblPlayerId.Text.Trim()) == playertrack;
            if (flag)
            {
                //if(randommusic)
                

                for (int i = 0; i < this.myListView.Items.Count; i++)
                {
                    bool flag2 = i == index - 1;
                    if (flag2)
                    {
                        if (this.InvokeRequired)
                        {
                            this.myListView.Invoke(new Action(() =>
                            {
                                this.myListView.Items[i].BackColor = Color.Green;
                            }));

                        }
                        else
                        {
                            this.myListView.Items[i].BackColor = Color.Green;
                            //if (list == "")
                            //{
                            //    list += i;
                            //}
                            //else
                            //{
                            //    list += "," + i;
                            //}
                            ////string.Join();

                            //var sp = list == "" ? null : list.Split(',').Where(x=>x == i.ToString()).ToList();
                            //if (sp.Count() == 0)
                            //{

                            //}
                        }

                        this.update_position_music(playertrack, index);
                    }
                    else
                    {
                        //string items = this.myListView.Items[i].SubItems[3].Text.ToString();
                        if (this.myListView.Items[i].BackColor != Color.Red)
                        {
                            if (this.myListView.InvokeRequired)
                            {
                                this.myListView.Invoke(new Action(() =>
                                {
                                    this.myListView.Items[i].BackColor = Color.Empty;
                                }));
                            }
                            else
                            {
                                this.myListView.Items[i].BackColor = Color.Empty;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        public void stop_music_id(int id)
        {
            if (int.Parse(lblPlayerId.Text.Trim()) == 1)
            {
                //nPlayer1.CurrentPlaylistChanged += NPlayer1_CurrentPlaylistChanged; ;
                nPlayer1.stop_music_name(id);
                //nPlayer1.CurrentPlaylistChanged -= NPlayer1_CurrentPlaylistChanged;
            }
            if (int.Parse(lblPlayerId.Text.Trim()) == 2)
            {
                //nPlayer2.CurrentPlaylistChanged += NPlayer2_CurrentPlaylistChanged;
                nPlayer2.stop_music_name(id);
                //nPlayer2.CurrentPlaylistChanged -= NPlayer2_CurrentPlaylistChanged;
            }
            if (int.Parse(lblPlayerId.Text.Trim()) == 3)
            {
                //nPlayer3.CurrentPlaylistChanged += NPlayer3_CurrentPlaylistChanged;
                nPlayer3.stop_music_name(id);
                //nPlayer3.CurrentPlaylistChanged -= NPlayer3_CurrentPlaylistChanged;
            }
            if (int.Parse(lblPlayerId.Text.Trim()) == 4)
            {
                //nPlayer4.CurrentPlaylistChanged += NPlayer4_CurrentPlaylistChanged;
                nPlayer4.stop_music_name(id);
                //nPlayer4.CurrentPlaylistChanged -= NPlayer4_CurrentPlaylistChanged;
            }
            if (int.Parse(lblPlayerId.Text.Trim()) == 5)
            {
                //nPlayer5.CurrentPlaylistChanged += NPlayer5_CurrentPlaylistChanged; ;
                nPlayer5.stop_music_name(id);
                //nPlayer5.CurrentPlaylistChanged -= NPlayer5_CurrentPlaylistChanged;
            }
            if (int.Parse(lblPlayerId.Text.Trim()) == 6)
            {
                //nPlayer6.CurrentPlaylistChanged += NPlayer6_CurrentPlaylistChanged;
                nPlayer6.stop_music_name(id);
                //nPlayer6.CurrentPlaylistChanged -= NPlayer6_CurrentPlaylistChanged;
            }
            if (int.Parse(lblPlayerId.Text.Trim()) == 7)
            {
                //nPlayer7.CurrentPlaylistChanged += NPlayer7_CurrentPlaylistChanged;
                nPlayer7.stop_music_name(id);
                //nPlayer7.CurrentPlaylistChanged -= NPlayer7_CurrentPlaylistChanged;
            }
            if (int.Parse(lblPlayerId.Text.Trim()) == 8)
            {
                //nPlayer8.CurrentPlaylistChanged += NPlayer8_CurrentPlaylistChanged;
                nPlayer8.stop_music_name(id);
                //nPlayer8.CurrentPlaylistChanged -= NPlayer8_CurrentPlaylistChanged;
            }
        }
        
        public void clear_music()
        {
            OTSPlaylist _PlayList = new OTSPlaylist();
            bool flag = int.Parse(this.lblPlayerId.Text.Trim()) == 1;
            if (flag)
            {
                this.nPlayer1.clear_music();
            }
            bool flag2 = int.Parse(this.lblPlayerId.Text.Trim()) == 2;
            if (flag2)
            {
                this.nPlayer2.clear_music();
            }
            bool flag3 = int.Parse(this.lblPlayerId.Text.Trim()) == 3;
            if (flag3)
            {
                this.nPlayer3.clear_music();
            }
            bool flag4 = int.Parse(this.lblPlayerId.Text.Trim()) == 4;
            if (flag4)
            {
                this.nPlayer4.clear_music();
            }
            bool flag5 = int.Parse(this.lblPlayerId.Text.Trim()) == 5;
            if (flag5)
            {
                this.nPlayer5.clear_music();
            }
            bool flag6 = int.Parse(this.lblPlayerId.Text.Trim()) == 6;
            if (flag6)
            {
                this.nPlayer6.clear_music();
            }
            bool flag7 = int.Parse(this.lblPlayerId.Text.Trim()) == 7;
            if (flag7)
            {
                this.nPlayer7.clear_music();
            }
            bool flag8 = int.Parse(this.lblPlayerId.Text.Trim()) == 8;
            if (flag8)
            {
                this.nPlayer8.clear_music();
            }
        }
        
        public void change_loop_shuffle(short track, bool t1, bool t2)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    if (track == lastId)
                    {
                        if (t1 == true)
                        {
                            this.btnPlaylistLoop.ForeColor = Color.FromArgb(255, 255, 128, 0);
                        }
                        else
                        {
                            this.btnPlaylistLoop.ForeColor = Color.Black;
                        }
                        if (t2 == true)
                        {
                            this.btnPlaylistShuffle.ForeColor = Color.FromArgb(255, 255, 128, 0);
                        }
                        else
                        {
                            this.btnPlaylistShuffle.ForeColor = Color.Black;
                        }
                    }
                }));
            }
            else
            {
                if (track == lastId)
                {
                    if (t1 == true)
                    {
                        this.btnPlaylistLoop.ForeColor = Color.FromArgb(255, 255, 128, 0);
                    }
                    else
                    {
                        this.btnPlaylistLoop.ForeColor = Color.Black;
                    }
                    if (t2 == true)
                    {
                        this.btnPlaylistShuffle.ForeColor = Color.FromArgb(255, 255, 128, 0);
                    }
                    else
                    {
                        this.btnPlaylistShuffle.ForeColor = Color.Black;
                    }
                }
            }
            SaveShuffleLoopState1(track, t1, t2);

        }
        
        private void btnPlaylistShuffle_Click(object sender, EventArgs e)
        {
            bool flag = this.btnPlaylistShuffle.ForeColor == Color.FromArgb(255, 255, 128, 0);
            if (flag)
            {
                this.bShuffle = false;
                this.btnPlaylistShuffle.ForeColor = Color.Black;
            }
            else
            {
                bool flag2 = this.btnPlaylistLoop.ForeColor == Color.FromArgb(255, 255, 128, 0);
                if (flag2)
                {
                    this.bLoop = false;
                    this.btnPlaylistLoop.ForeColor = Color.Black;
                }
                this.bShuffle = true;
                this.btnPlaylistShuffle.ForeColor = Color.FromArgb(255, 255, 128, 0);
            }
            short idsa = short.Parse(this.lblPlayerId.Text.Trim());
            this.SaveShuffleLoopState(idsa);
            this.trigger_url();
        }
        
        private void btnPlaylistLoop_Click(object sender, EventArgs e)
        {

            bool flag = this.btnPlaylistLoop.ForeColor == Color.FromArgb(255, 255, 128, 0);
            if (flag)
            {
                this.bLoop = false;
                this.btnPlaylistLoop.ForeColor = Color.Black;
            }
            else
            {
                bool flag2 = this.btnPlaylistShuffle.ForeColor == Color.FromArgb(255, 255, 128, 0);
                if (flag2)
                {
                    this.bShuffle = false;
                    this.btnPlaylistShuffle.ForeColor = Color.Black;
                }
                this.bLoop = true;
                this.btnPlaylistLoop.ForeColor = Color.FromArgb(255, 255, 128, 0);
            }
            short idsa = short.Parse(this.lblPlayerId.Text.Trim());
            this.SaveShuffleLoopState(idsa);
            this.trigger_url();

        }

        private void btnPlaylistSave_Click(object sender, EventArgs e)
        {
            using (System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog
            {
                Filter = "Text Documents|*.txt",
                ValidateNames = true,
                FileName = string.Format("{0}\\{1}.txt", Environment.CurrentDirectory, this.lblPlayerId.Text)
            })
            {
                bool flag = saveFileDialog.ShowDialog() == DialogResult.OK;
                if (flag)
                {
                    using (TextWriter tw = new StreamWriter(new FileStream(saveFileDialog.FileName, FileMode.Create), Encoding.UTF8))
                    {
                        foreach (object obj in this.myListView.Items)
                        {
                            ListViewItem item = (ListViewItem)obj;
                            tw.WriteLine(string.Concat(new string[]
                            {
                                item.SubItems[0].Text,
                                "\t",
                                item.SubItems[1].Text,
                                "\t",
                                item.SubItems[2].Text,
                                "\t",
                                item.SubItems[3].Text,
                                "\t"
                            }));
                        }
                    }
                }
            }
        }
        
        public string export_file(string playlistId)
        {
            string pathFile = String.Format("{0}\\{1}-List.txt", System.Environment.CurrentDirectory, playlistId);

            if (File.Exists(pathFile))
            {
                Byte[] file = File.ReadAllBytes(pathFile);
                String base64file = Convert.ToBase64String(file);
                return base64file;
            } 
            else
            {
                return "";
            }
        }

        public bool import_file(string playlistId, string base64file)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        mylistView_doubleclick1(short.Parse(playlistId));
                        List<jsonWebAPI.MusicErrorList> dd = new List<jsonWebAPI.MusicErrorList>();
                        string pathFile = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "import_" + playlistId + ".txt");
                        Byte[] bytes = Convert.FromBase64String(base64file);
                        File.WriteAllBytes(pathFile, bytes);

                        // Step 2: Read imported data
                        List<string> data = File.ReadAllLines(pathFile).ToList();
                        // Step 3: Collect all existing item[3] values from 1-List.txt to 8-List.txt
                        HashSet<string> set = new HashSet<string>();

                        for (int i = 1; i < 9; i++) {
                            string fileName = Path.Combine(Environment.CurrentDirectory, $"{i}-List.txt");

                            if (File.Exists(fileName)) {
                                var itemsFromFile = File.ReadAllLines(fileName)
                                    .Select(x => x.Split('\t'))
                                    .Where(parts => parts.Length > 3)
                                    .Select(parts => parts[3]);

                                foreach (var item in itemsFromFile) {
                                    set.Add(item); // accumulate all items
                                }
                            }
                        }

                        // Step 4: Process imported data
                        foreach (string _item in data) {
                            string[] items = _item.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);

                            if (items.Length > 3) {
                                if (set.Contains(items[3])) {
                                    dd.Add(new jsonWebAPI.MusicErrorList {
                                        name = items[1],
                                        location = items[3],
                                        PlayerTrack = short.Parse(playlistId)
                                    });
                                    continue;
                                }

                                set.Add(items[3]); // mark as seen

                                // Add to ListView
                                ListViewItem item = new ListViewItem(items[0]);
                                item.SubItems.Add(items[1]);
                                item.SubItems.Add(items[2]);
                                item.SubItems.Add(items[3]);
                                this.myListView.Items.Add(item);
                            }
                        }

                        //using (TextWriter tw = new StreamWriter(new FileStream(fileName, FileMode.Create), Encoding.UTF8))
                        //{
                        //    foreach (string _item in data)
                        //    {

                        //string[] items = _item.Split(new char[]
                        //{
                        //    '\t'
                        //}, StringSplitOptions.RemoveEmptyEntries);

                        //        if (set.Contains(items[3])) {
                        //             dd.Add(new jsonWebAPI.MusicErrorList {
                        //                name = items[1],
                        //                location = items[3],
                        //                PlayerTrack = Convert.ToInt16(items[0])
                        //            });
                        //            continue;
                        //        }

                        //        set.Add(items[3]);

                        //        //if (ckfi.Item1 == true) {
                        //        //    dd.Add(new jsonWebAPI.MusicErrorList {
                        //        //        name = items[1],
                        //        //        location = items[3],
                        //        //        PlayerTrack = ckfi.Item2
                        //        //    });
                        //        //    continue;
                        //        //}
                        //        tw.WriteLine(items[0] + "\t" + items[1] + "\t" + items[2] + "\t" + items[3] + "\t");
                        //    }
                        //}
                        if (dd.Count > 0) {
                            Task.Run(() => {
                                this.trigger_errormusic_url(dd);
                                ErrorMusic fms = new ErrorMusic(dd);
                                fms.TopMost = true;
                                //fms.Owner = this;
                                fms.Name = "ErrorMusic";
                                fms.ShowDialog();
                            });
                        }
                        //if (int.Parse(playlistId) == 1)
                        //{
                        //    this.nPlayer1.CurrentPlaylist = _PlayList;
                        //}
                        //else if (int.Parse(playlistId) == 2)
                        //{
                        //    this.nPlayer2.CurrentPlaylist = _PlayList;
                        //}
                        //else if (int.Parse(playlistId) == 3)
                        //{
                        //    this.nPlayer3.CurrentPlaylist = _PlayList;
                        //}
                        //else if (int.Parse(playlistId) == 4)
                        //{
                        //    this.nPlayer4.CurrentPlaylist = _PlayList;
                        //}
                        //else if (int.Parse(playlistId) == 5)
                        //{
                        //    this.nPlayer5.CurrentPlaylist = _PlayList;
                        //}
                        //else if (int.Parse(playlistId) == 6)
                        //{
                        //    this.nPlayer6.CurrentPlaylist = _PlayList;
                        //}
                        //else if (int.Parse(playlistId) == 7)
                        //{
                        //    this.nPlayer7.CurrentPlaylist = _PlayList;
                        //}
                        //else
                        //{
                        //    this.nPlayer8.CurrentPlaylist = _PlayList;
                        //}
                        //if (lastId == short.Parse(playlistId))
                        //{
                        //    this.LoadDefaultPlaylist(short.Parse(playlistId));
                        //}
                        //this.trigger_url();
                        short idsa = short.Parse(this.lblPlayerId.Text.Trim());
                        this.SaveDefaultPlayList(idsa);
                        this.LoadDefaultPlaylist(idsa);
                        this.trigger_url();
                    }));
                    return true;

                }
                else
                {
                    mylistView_doubleclick1(short.Parse(playlistId));
                    List<jsonWebAPI.MusicErrorList> dd = new List<jsonWebAPI.MusicErrorList>();
                    string pathFile = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "import_" + playlistId + ".txt");
                    Byte[] bytes = Convert.FromBase64String(base64file);
                    File.WriteAllBytes(pathFile, bytes);
                    //OTSPlaylist _PlayList = new OTSPlaylist();

                    //foreach (string _item in data)
                    //{
                    //    string[] items = _item.Split(new char[]
                    //    {
                    //        '\t'
                    //    }, StringSplitOptions.RemoveEmptyEntries);

                    //    _PlayList.appendItem(new OTSMedia
                    //    {
                    //        Shuffle = this.bShuffle,
                    //        Loop = this.bLoop,
                    //        Id = int.Parse(items[0]),
                    //        fileName = items[1],
                    //        duration = TimeSpan.Parse(items[2]),
                    //        fileLocation = items[3]
                    //    });

                    //}

                    // Step 2: Read imported data
                    List<string> data = File.ReadAllLines(pathFile).ToList();
                    // Step 3: Collect all existing item[3] values from 1-List.txt to 8-List.txt
                    HashSet<string> set = new HashSet<string>();

                    for (int i = 1; i < 9; i++) {
                        string fileName = Path.Combine(Environment.CurrentDirectory, $"{i}-List.txt");

                        if (File.Exists(fileName)) {
                            var itemsFromFile = File.ReadAllLines(fileName)
                                .Select(x => x.Split('\t'))
                                .Where(parts => parts.Length > 3)
                                .Select(parts => parts[3]);

                            foreach (var item in itemsFromFile) {
                                set.Add(item); // accumulate all items
                            }
                        }
                    }

                    // Step 4: Process imported data
                    foreach (string _item in data) {
                        string[] items = _item.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);

                        if (items.Length > 3) {
                            if (set.Contains(items[3])) {
                                dd.Add(new jsonWebAPI.MusicErrorList {
                                    name = items[1],
                                    location = items[3],
                                    PlayerTrack = short.Parse(playlistId)
                                });
                                continue;
                            }

                            set.Add(items[3]); // mark as seen

                            // Add to ListView
                            ListViewItem item = new ListViewItem(items[0]);
                            item.SubItems.Add(items[1]);
                            item.SubItems.Add(items[2]);
                            item.SubItems.Add(items[3]);
                            this.myListView.Items.Add(item);
                        }
                    }
                    //using (TextWriter tw = new StreamWriter(new FileStream(fileName, FileMode.Create), Encoding.UTF8))
                    //{
                    //    foreach (string _item in data)
                    //    {
                    //        string[] items = _item.Split(new char[]
                    //        {
                    //        '\t'
                    //        }, StringSplitOptions.RemoveEmptyEntries);
                    //        if (set.Contains(items[3])) {
                    //            dd.Add(new jsonWebAPI.MusicErrorList {
                    //                name = items[1],
                    //                location = items[3],
                    //                PlayerTrack = Convert.ToInt16(items[0])
                    //            });
                    //            continue;
                    //        }
                    //        set.Add(items[3]);
                    //        tw.WriteLine(items[0] + "\t" + items[1] + "\t" + items[2] + "\t" + items[3] + "\t");
                    //    }
                    //}
                    if (dd.Count > 0) {
                        Task.Run(() => {
                            this.trigger_errormusic_url(dd);
                            ErrorMusic fms = new ErrorMusic(dd);
                            fms.TopMost = true;
                            //fms.Owner = this;
                            fms.Name = "ErrorMusic";
                            fms.ShowDialog();
                        });
                    }
                    //if (int.Parse(playlistId) == 1)
                    //{
                    //    this.nPlayer1.CurrentPlaylist = _PlayList;
                    //}
                    //else if (int.Parse(playlistId) == 2)
                    //{
                    //    this.nPlayer2.CurrentPlaylist = _PlayList;
                    //}
                    //else if (int.Parse(playlistId) == 3)
                    //{
                    //    this.nPlayer3.CurrentPlaylist = _PlayList;
                    //}
                    //else if (int.Parse(playlistId) == 4)
                    //{
                    //    this.nPlayer4.CurrentPlaylist = _PlayList;
                    //}
                    //else if (int.Parse(playlistId) == 5)
                    //{
                    //    this.nPlayer5.CurrentPlaylist = _PlayList;
                    //}
                    //else if (int.Parse(playlistId) == 6)
                    //{
                    //    this.nPlayer6.CurrentPlaylist = _PlayList;
                    //}
                    //else if (int.Parse(playlistId) == 7)
                    //{
                    //    this.nPlayer7.CurrentPlaylist = _PlayList;
                    //}
                    //else
                    //{
                    //    this.nPlayer8.CurrentPlaylist = _PlayList;
                    //}

                    ////if (lastId == short.Parse(playlistId))
                    ////{
                    ////    this.LoadDefaultPlaylist(short.Parse(playlistId));
                    ////}
                    ////this.trigger_url();
                    short idsa = short.Parse(this.lblPlayerId.Text.Trim());
                    this.SaveDefaultPlayList(idsa);
                    this.LoadDefaultPlaylist(idsa);
                    this.trigger_url();
                    return true;
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Import Error : " + ex);
                return false;
                throw;
            }
        }

        public bool reorder_playlist(string playlistId, List<PlaylistItem> playlistData)
        {
            if (this.InvokeRequired)
            {
                List<jsonWebAPI.MusicErrorList> dd = new List<jsonWebAPI.MusicErrorList>();
                if (playlistData.Count == 0) return false;

                this.Invoke(new Action(() =>
                {

                    string fileName = String.Format("{0}\\{1}-List.txt", System.Environment.CurrentDirectory, playlistId);
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }

                    using (TextWriter tw = new StreamWriter(new FileStream(fileName, FileMode.Create), Encoding.UTF8))
                    {
                        foreach (var item in playlistData)
                        {
                            tw.WriteLine(item.newIndex + "\t" + item.name + "\t" + item.duration + "\t" + item.path + "\t");
                        }
                    }
                    if (lastId == short.Parse(playlistId))
                    {
                        myListView.Items.Clear();
                        this.LoadDefaultPlaylist(short.Parse(playlistId));
                        this.change_music_list("data", 1);
                    }
                    this.trigger_url();
                }));
                return true;
            } else
            {
                List<jsonWebAPI.MusicErrorList> dd = new List<jsonWebAPI.MusicErrorList>();
                if (playlistData.Count == 0) return false;

                string fileName = String.Format("{0}\\{1}-List.txt", System.Environment.CurrentDirectory, playlistId);

                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                using (TextWriter tw = new StreamWriter(new FileStream(fileName, FileMode.Create), Encoding.UTF8))
                {

                    foreach (var item in playlistData)
                    {
                        tw.WriteLine(item.newIndex + "\t" + item.name + "\t" + item.duration + "\t" + item.path + "\t");
                    }
                }
                if (lastId == short.Parse(playlistId))
                {
                    myListView.Items.Clear();
                    this.LoadDefaultPlaylist(short.Parse(playlistId));
                }
                this.trigger_url();
            }
            return true;
        }

        public bool upload_file(string playlistId, string base64Array) {
            var isSuccess = true;
            try {
                if (this.InvokeRequired) {
                    this.Invoke(new Action(() => {
                        try {
                            List<jsonWebAPI.MusicErrorList> dd = new List<jsonWebAPI.MusicErrorList>();
                            byte[] dataByte = Convert.FromBase64String(base64Array);
                            string json = Encoding.UTF8.GetString(dataByte);
                            List<string> fileList = JsonConvert.DeserializeObject<List<string>>(json).Select(x => AppDomain.CurrentDomain.BaseDirectory + "music\\" + playlistId + "\\" + x).ToList();
                            //List<string> matchingFolders = new List<string>();

                            //foreach (DriveInfo drive in DriveInfo.GetDrives()) {
                            //    if (drive.IsReady) {
                            //        Console.WriteLine($"Scanning drive: {drive.Name}");
                            //        ScanDirectory(drive.RootDirectory.FullName, matchingFolders);
                            //    }
                            //}

                            string fileName = String.Format("{0}\\{1}-List.txt", System.Environment.CurrentDirectory, playlistId);
                            List<string> data = File.ReadAllLines(fileName).ToList<string>();

                            //using (TextWriter tw = new StreamWriter(new FileStream(fileName, FileMode.Create), Encoding.UTF8)) {
                                HashSet<string> set = data.Select(x => {
                                    string[] items = x.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                                    return items[3];
                                }).ToHashSet<string>();
                            //foreach (string _item in data) {
                            //    string[] items = _item.Split(new char[]
                            //    {
                            //'\t'
                            //    }, StringSplitOptions.RemoveEmptyEntries);
                            //    tw.WriteLine(items[0] + "\t" + items[1] + "\t" + items[2] + "\t" + items[3] + "\t");
                            //}

                            // auto select when api trigger at this status
                            if (playlistId == "1") {
                                this.iconButtonPlayer1_Click(null, null);
                            } else if (playlistId == "2") {
                                this.iconButtonPlayer2_Click(null, null);
                            } else if (playlistId == "3") {
                                this.iconButtonPlayer3_Click(null, null);
                            } else if (playlistId == "4") {
                                this.iconButtonPlayer4_Click(null, null);
                            } else if (playlistId == "5") {
                                this.iconButtonPlayer5_Click(null, null);
                            } else if (playlistId == "6") {
                                this.iconButtonPlayer6_Click(null, null);
                            } else if (playlistId == "7") {
                                this.iconButtonPlayer7_Click(null, null);
                            } else if (playlistId == "8") {
                                this.iconButtonPlayer8_Click(null, null);
                            }

                            int seq = this.myListView.Items.Count;
                            foreach (var file in fileList) {
                                FileInfo _file = new FileInfo(file);
                                string decodedName = Uri.UnescapeDataString(_file.Name);
                                string decodedPath = Uri.UnescapeDataString(_file.FullName);
                                if (set.Contains(decodedPath)) {
                                    dd.Add(new jsonWebAPI.MusicErrorList {
                                        name = decodedName,
                                        location = decodedPath,
                                        PlayerTrack = Convert.ToInt16(playlistId)
                                    });
                                    continue;
                                }

                                //int checkfile = decodedName.IndexOf(".mp3");
                                ////int checkfile = (_file.Name.IndexOf(".wav") == -1) ? _file.Name.IndexOf(".mp3") : _file.Name.IndexOf(".wav");
                                //bool flag4 = checkfile != -1;
                                //if (flag4) {
                                bool flag5 = CoreLibrary.check_audio_file(_file.FullName);
                                if (flag5) {
                                    seq++;
                                    var ggg = _file.FullName.Split('\\');
                                    //string strCmdText;
                                    //strCmdText = "ffmpeg -i "+ _file.FullName + " -filter:a loudnorm output.mp3";
                                    //System.Diagnostics.Process.Start("CMD.exe", strCmdText);
                                    ListViewItem item = new ListViewItem(seq.ToString());
                                    item.SubItems.Add(_file.Name);
                                    item.SubItems.Add(string.Format("{0:hh\\:mm\\:ss}", CoreLibrary.GetNAudoSongLength(_file.FullName)));
                                    item.SubItems.Add(_file.FullName);
                                    this.myListView.Items.Add(item);
                                } else {
                                    string messagemusc = "ไฟล์เสีย ชื่อ " + _file.Name;
                                    var messagebox = new Helper.MessageBox();
                                    messagebox.ShowCenter_DialogError(messagemusc, "แจ้งเตือน");
                                    isSuccess = false;
                                    if (_file.Exists) {
                                        File.Delete(_file.FullName);
                                    }
                                }
                                //}
                            }
                            if (dd.Count > 0) {
                                Task.Run(() => {
                                    this.trigger_errormusic_url(dd);
                                    ErrorMusic fms = new ErrorMusic(dd);
                                    fms.TopMost = true;
                                    //fms.Owner = this;
                                    fms.Name = "ErrorMusic";
                                    fms.ShowDialog();
                                });
                            }
                            this.ReOrderSequence();
                            short idsa = short.Parse(playlistId);
                            this.SaveDefaultPlayList(idsa);
                            this.LoadDefaultPlaylist(idsa);
                            //if (lastId == short.Parse(playlistId)) {
                            //    this.LoadDefaultPlaylist(short.Parse(playlistId));
                            //}
                            this.trigger_url();
                        } catch (Exception ex) {
                            isSuccess = false;
                            //var messagebox = new Helper.MessageBox();
                            //messagebox.ShowCenter_DialogError(ex.Message, "เกิดข้อผิดพลาด");
                            log.Error("upload_file Error: " + ex.Message + " , trigger_url Inner: " + ex.InnerException);
                        }
                    }));
                } else {
                    List<jsonWebAPI.MusicErrorList> dd = new List<jsonWebAPI.MusicErrorList>();
                    byte[] dataByte = Convert.FromBase64String(base64Array);
                    string json = Encoding.UTF8.GetString(dataByte);
                    List<string> fileList = JsonConvert.DeserializeObject<List<string>>(json).Select(x => AppDomain.CurrentDomain.BaseDirectory + "music\\" + playlistId + "\\" + x).ToList();
                    //List<string> matchingFolders = new List<string>();

                    //foreach (DriveInfo drive in DriveInfo.GetDrives()) {
                    //    if (drive.IsReady) {
                    //        Console.WriteLine($"Scanning drive: {drive.Name}");
                    //        ScanDirectory(drive.RootDirectory.FullName, matchingFolders);
                    //    }
                    //}

                    string fileName = String.Format("{0}\\{1}-List.txt", System.Environment.CurrentDirectory, playlistId);
                    List<string> data = File.ReadAllLines(fileName).ToList<string>();

                    //using (TextWriter tw = new StreamWriter(new FileStream(fileName, FileMode.Create), Encoding.UTF8)) {
                    HashSet<string> set = data.Select(x => {
                        string[] items = x.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        return items[3];
                    }).ToHashSet<string>();
                    //foreach (string _item in data) {
                    //    string[] items = _item.Split(new char[]
                    //    {
                    //'\t'
                    //    }, StringSplitOptions.RemoveEmptyEntries);
                    //    tw.WriteLine(items[0] + "\t" + items[1] + "\t" + items[2] + "\t" + items[3] + "\t");
                    //}

                    // auto select when api trigger at this status
                    if (playlistId == "1") {
                        this.iconButtonPlayer1_Click(null, null);
                    } else if (playlistId == "2") {
                        this.iconButtonPlayer2_Click(null, null);
                    } else if (playlistId == "3") {
                        this.iconButtonPlayer3_Click(null, null);
                    } else if (playlistId == "4") {
                        this.iconButtonPlayer4_Click(null, null);
                    } else if (playlistId == "5") {
                        this.iconButtonPlayer5_Click(null, null);
                    } else if (playlistId == "6") {
                        this.iconButtonPlayer6_Click(null, null);
                    } else if (playlistId == "7") {
                        this.iconButtonPlayer7_Click(null, null);
                    } else if (playlistId == "8") {
                        this.iconButtonPlayer8_Click(null, null);
                    }

                    int seq = this.myListView.Items.Count;
                    foreach (var file in fileList) {
                        FileInfo _file = new FileInfo(file);
                        string decodedName = Uri.UnescapeDataString(_file.Name);
                        string decodedPath = Uri.UnescapeDataString(_file.FullName);
                        if (set.Contains(decodedPath)) {
                            dd.Add(new jsonWebAPI.MusicErrorList {
                                name = decodedName,
                                location = decodedPath,
                                PlayerTrack = Convert.ToInt16(playlistId)
                            });
                            continue;
                        }

                        //int checkfile = decodedName.IndexOf(".mp3");
                        ////int checkfile = (_file.Name.IndexOf(".wav") == -1) ? _file.Name.IndexOf(".mp3") : _file.Name.IndexOf(".wav");
                        //bool flag4 = checkfile != -1;
                        //if (flag4) {
                        bool flag5 = CoreLibrary.check_audio_file(_file.FullName);
                        if (flag5) {
                            seq++;
                            var ggg = _file.FullName.Split('\\');
                            //string strCmdText;
                            //strCmdText = "ffmpeg -i "+ _file.FullName + " -filter:a loudnorm output.mp3";
                            //System.Diagnostics.Process.Start("CMD.exe", strCmdText);
                            ListViewItem item = new ListViewItem(seq.ToString());
                            item.SubItems.Add(_file.Name);
                            item.SubItems.Add(string.Format("{0:hh\\:mm\\:ss}", CoreLibrary.GetNAudoSongLength(_file.FullName)));
                            item.SubItems.Add(_file.FullName);
                            this.myListView.Items.Add(item);
                        } else {
                            string messagemusc = "ไฟล์เสีย ชื่อ " + _file.Name;
                            var messagebox = new Helper.MessageBox();
                            messagebox.ShowCenter_DialogError(messagemusc, "แจ้งเตือน");
                            isSuccess = false;
                            if (_file.Exists) {
                                File.Delete(_file.FullName);
                            }
                        }
                        //}
                    }
                    if (dd.Count > 0) {
                        Task.Run(() => {
                            this.trigger_errormusic_url(dd);
                            ErrorMusic fms = new ErrorMusic(dd);
                            fms.TopMost = true;
                            //fms.Owner = this;
                            fms.Name = "ErrorMusic";
                            fms.ShowDialog();
                        });
                    }
                    this.ReOrderSequence();
                    short idsa = short.Parse(playlistId);
                    this.SaveDefaultPlayList(idsa);
                    this.LoadDefaultPlaylist(idsa);
                    //if (lastId == short.Parse(playlistId)) {
                    //    this.LoadDefaultPlaylist(short.Parse(playlistId));
                    //}
                    this.trigger_url();
                }
            } catch (Exception ex) {
                isSuccess = false;
                //var messagebox = new Helper.MessageBox();
                //messagebox.ShowCenter_DialogError(ex.Message, "เกิดข้อผิดพลาด");
                log.Error("upload_file Error: " + ex.Message + " , trigger_url Inner: " + ex.InnerException);
            }
            return isSuccess;
        }

        public bool close_form() {
            try {
                // Make a copy of the open forms to avoid modifying the collection during iteration
                List<Form> openForms = Application.OpenForms.Cast<Form>().ToList();

                foreach (Form form in openForms) {
                    if (form.Name == "Warning") {
                        if (form.InvokeRequired) {
                            form.Invoke(new Action(() => {
                                if (!form.IsDisposed)
                                    form.Close();
                            }));
                        } else {
                            if (!form.IsDisposed)
                                form.Close();
                        }
                    }
                }
            } catch (Exception ex) {
                log.Error("close_form Error: " + ex.Message + " , trigger_url Inner: " + ex.InnerException);
                return false;
            }
            return true;
        }

        public bool close_musicerror() {
            try {
                // Make a copy of the open forms to avoid modifying the collection during iteration
                List<Form> openForms = Application.OpenForms.Cast<Form>().ToList();

                foreach (Form form in openForms) {
                    if (form.Name == "ErrorMusic") {
                        if (form.InvokeRequired) {
                            form.Invoke(new Action(() => {
                                if (!form.IsDisposed)
                                    form.Close();
                            }));
                        } else {
                            if (!form.IsDisposed)
                                form.Close();
                        }
                    }
                }
            } catch (Exception ex) {
                log.Error("close_form Error: " + ex.Message + " , trigger_url Inner: " + ex.InnerException);
                return false;
            }
            return true;
        }

        private void btnPlaylistFileImport_Click(object sender, EventArgs e)
        {
            List<jsonWebAPI.MusicErrorList> dd = new List<jsonWebAPI.MusicErrorList>();
            System.Windows.Forms.OpenFileDialog openFileDlg = new System.Windows.Forms.OpenFileDialog();
            openFileDlg.Multiselect = true;
            openFileDlg.Title = "Import Song List|*.txt";
            openFileDlg.Filter = "Song List|*.txt";
            bool flag = openFileDlg.ShowDialog() == DialogResult.OK;

            if (flag)
            {
                bool flag2 = this.myListView.Items.Count > 0;
                if (flag2)
                {
                    bool flag3 = MessageBox.Show("ต้องการล้างรายการก่อนนำเข้าข้อมูล?(Y/N)", "Confirm Clear screen!!!", MessageBoxButtons.YesNo) == DialogResult.Yes;
                    if (flag3)
                    {
                        this.myListView.Items.Clear();
                    }
                }
                List<string> data = File.ReadAllLines(openFileDlg.FileName).ToList<string>();
                foreach (string _item in data)
                {
                    string[] items = _item.Split(new char[]
                    {
                    '\t'
                    }, StringSplitOptions.RemoveEmptyEntries);
                    var ckfi = checkfile_gg(items[3]);
                    if (ckfi.Item1 == true) {
                        dd.Add(new jsonWebAPI.MusicErrorList {
                            name = items[1],
                            location = items[3],
                            PlayerTrack = ckfi.Item2
                        });
                        continue;
                    }
                    ListViewItem item = new ListViewItem(items[0]);
                    item.SubItems.Add(items[1]);
                    item.SubItems.Add(items[2]);
                    item.SubItems.Add(items[3]);
                    this.myListView.Items.Add(item);
                }
            }
            if (dd.Count > 0) {
                //this.trigger_errormusic_url(dd);
                ErrorMusic fms = new ErrorMusic(dd);
                fms.TopMost = true;
                fms.Name = "ErrorMusic";
                //fms.Owner = this;
                fms.ShowDialog();
            }
            //this.ReOrderSequence();
            short idsa = short.Parse(this.lblPlayerId.Text.Trim());
            this.SaveDefaultPlayList(idsa);
            this.LoadDefaultPlaylist(idsa);
            this.trigger_url();

        }
        #endregion
        
        #region Private Function
        private void ReOrderSequence()
        {
            OTSPlaylist _PlayList = new OTSPlaylist();
            for (int i = 0; i < this.myListView.Items.Count; i++)
            {
                int newSeq = i + 1;
                bool flag = int.Parse(this.myListView.Items[i].SubItems[0].Text) != newSeq;
                if (flag)
                {
                    this.myListView.Items[i].SubItems[0].Text = newSeq.ToString();
                }
            }
            for (int j = 0; j < this.myListView.Items.Count; j++)
            {
                _PlayList.appendItem(new OTSMedia
                {
                    Shuffle = this.bShuffle,
                    Loop = this.bLoop,
                    Id = int.Parse(this.myListView.Items[j].SubItems[0].Text),
                    fileName = this.myListView.Items[j].SubItems[1].Text,
                    duration = TimeSpan.Parse(this.myListView.Items[j].SubItems[2].Text),
                    fileLocation = this.myListView.Items[j].SubItems[3].Text
                });
            }
            bool flag2 = int.Parse(this.lblPlayerId.Text.Trim()) == 1;
            if (flag2)
            {
                this.nPlayer1.CurrentPlaylist = _PlayList;
            }
            bool flag3 = int.Parse(this.lblPlayerId.Text.Trim()) == 2;
            if (flag3)
            {
                this.nPlayer2.CurrentPlaylist = _PlayList;
            }
            bool flag4 = int.Parse(this.lblPlayerId.Text.Trim()) == 3;
            if (flag4)
            {
                this.nPlayer3.CurrentPlaylist = _PlayList;
            }
            bool flag5 = int.Parse(this.lblPlayerId.Text.Trim()) == 4;
            if (flag5)
            {
                this.nPlayer4.CurrentPlaylist = _PlayList;
            }
            bool flag6 = int.Parse(this.lblPlayerId.Text.Trim()) == 5;
            if (flag6)
            {
                this.nPlayer5.CurrentPlaylist = _PlayList;
            }
            bool flag7 = int.Parse(this.lblPlayerId.Text.Trim()) == 6;
            if (flag7)
            {
                this.nPlayer6.CurrentPlaylist = _PlayList;
            }
            bool flag8 = int.Parse(this.lblPlayerId.Text.Trim()) == 7;
            if (flag8)
            {
                this.nPlayer7.CurrentPlaylist = _PlayList;
            }
            bool flag9 = int.Parse(this.lblPlayerId.Text.Trim()) == 8;
            if (flag9)
            {
                this.nPlayer8.CurrentPlaylist = _PlayList;
            }

        }
        #endregion

        public int Index_item(string name)
        {
            ListViewItem listViewItem = new ListViewItem(name);

            var ca = 0;

            foreach (ListViewItem item in this.myListView.Items)
            {
                var namec = item.SubItems[1].Text;
                if (namec == name)
                {
                    ca++;
                    break;
                }
                ca++;
            }
            return ca;
        }

        public void change_namemedia(int index)
        {

            var eq = this.myListView.Items.Count;
            for (var i = 0; i < this.myListView.Items.Count; i++)
            {
                if (i == (index - 1))
                {
                    if (this.myListView.InvokeRequired)
                    {
                        this.myListView.Invoke(updateTextBox, Color.Green, i);
                    }
                    else
                    {
                        this.myListView.Items[i].BackColor = Color.Green;
                        this.myListView.Items[i].BackColor = Color.Green;

                    }

                }
                else
                {
                    //string items = this.myListView.Items[i].SubItems[3].Text.ToString();
                    if (this.myListView.Items[i].BackColor != Color.Red)
                    {
                        if (this.myListView.InvokeRequired)
                        {
                            this.myListView.Invoke(updateTextBox, Color.Empty, i);
                        }
                        else
                        {
                            this.myListView.Items[i].BackColor = Color.Empty;

                        }
                    }
                }
            }
        }

        public void clear_namemedia()
        {

            for (int i = 0; i < this.myListView.Items.Count; i++)
            {
                if (this.myListView.Items[i].BackColor != Color.Red)
                {
                    if (this.myListView.InvokeRequired)
                    {
                        this.myListView.Invoke(updateTextBox, Color.Empty, i);
                    }
                    else
                    {
                        this.myListView.Items[i].BackColor = Color.Empty;

                    }
                }
            }
        }

        #region  CurrentPlaylistChanged
        public void NPlayer1_CurrentPlaylistChanged(object sender, EventArgs e)
        {

        }

        private void NPlayer2_CurrentPlaylistChanged(object sender, EventArgs e)
        {

        }
        private void NPlayer3_CurrentPlaylistChanged(object sender, EventArgs e)
        {

        }
        private void NPlayer4_CurrentPlaylistChanged(object sender, EventArgs e)
        {

        }
        private void NPlayer5_CurrentPlaylistChanged(object sender, EventArgs e)
        {

        }
        private void NPlayer6_CurrentPlaylistChanged(object sender, EventArgs e)
        {

        }
        private void NPlayer7_CurrentPlaylistChanged(object sender, EventArgs e)
        {

        }
        private void NPlayer8_CurrentPlaylistChanged(object sender, EventArgs e)
        {

        }

        #endregion

        #region ListView
        private void myListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            /* No need this event ColumnClick : P'SAM 2022-10-01
             * 1
             * 10
           // Determine if clicked column is already the column that is being sorted.
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
        private void myListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            bool flag = (e.State & ListViewItemStates.Selected) > (ListViewItemStates)0;
            if (flag)
            {
                e.Graphics.FillRectangle(Brushes.Maroon, e.Bounds);
                e.DrawFocusRectangle();
            }
            else
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(e.Bounds, Color.Orange, Color.Maroon, LinearGradientMode.Horizontal))
                {
                    e.Graphics.FillRectangle(brush, e.Bounds);
                }
            }
            bool flag2 = this.myListView.View != View.Details;
            if (flag2)
            {
                e.DrawText();
            }
        }
        private void myListView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            TextFormatFlags flags = TextFormatFlags.Default;
            using (StringFormat sf = new StringFormat())
            {
                HorizontalAlignment textAlign = e.Header.TextAlign;
                HorizontalAlignment horizontalAlignment = textAlign;
                if (horizontalAlignment != HorizontalAlignment.Right)
                {
                    if (horizontalAlignment == HorizontalAlignment.Center)
                    {
                        sf.Alignment = StringAlignment.Center;
                        flags = TextFormatFlags.HorizontalCenter;
                    }
                }
                else
                {
                    sf.Alignment = StringAlignment.Far;
                    flags = TextFormatFlags.Right;
                }
                double subItemValue;
                bool flag = e.ColumnIndex > 0 && double.TryParse(e.SubItem.Text, NumberStyles.Currency, NumberFormatInfo.CurrentInfo, out subItemValue) && subItemValue < 0.0;
                if (flag)
                {
                    bool flag2 = (e.ItemState & ListViewItemStates.Selected) == (ListViewItemStates)0;
                    if (flag2)
                    {
                        e.DrawBackground();
                    }
                    e.Graphics.DrawString(e.SubItem.Text, this.myListView.Font, Brushes.Red, e.Bounds, sf);
                }
                else
                {
                    e.DrawText(flags);
                }
            }
        }
        private void myListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            e.Equals(ListViewItemStates.Selected);
            bool flag = this.myListView.SelectedItems.Count == 0;
            if (flag)
            {
                this.labelSelectedSong.Text = string.Empty;
            }
            else
            {
                ListView.SelectedListViewItemCollection albums = this.myListView.SelectedItems;
                bool flag2 = albums == null;
                if (!flag2)
                {
                    OTSPlaylist _PlayList = new OTSPlaylist();
                    OTSMedia _trackName = new OTSMedia();
                    _trackName.Shuffle = this.bShuffle;
                    _trackName.Loop = this.bLoop;
                    foreach (object obj in albums)
                    {
                        ListViewItem song = (ListViewItem)obj;
                        _trackName.Id = int.Parse(song.SubItems[0].Text);
                        _trackName.fileName = song.SubItems[1].Text;
                        _trackName.duration = TimeSpan.Parse(song.SubItems[2].Text);
                        _trackName.fileLocation = song.SubItems[3].Text;
                        _PlayList.appendItem(_trackName);
                        string showMsgText = string.Format("#{0}: {1}", song.SubItems[0].Text, song.SubItems[1].Text);
                        this.labelSelectedSong.Text = showMsgText;
                    }
                }
            }
        }
        public void mylistView_doubleclick(short ids, string namemusic)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    switch (ids)
                    {
                        case 1:
                            iconButtonPlayer1_Click(null, new EventArgs());

                            break;
                        case 2:
                            iconButtonPlayer2_Click(null, new EventArgs());
                            break;
                        case 3:
                            iconButtonPlayer3_Click(null, new EventArgs());
                            break;
                        case 4:
                            iconButtonPlayer4_Click(null, new EventArgs());
                            break;
                        case 5:
                            iconButtonPlayer5_Click(null, new EventArgs());
                            break;

                        case 6:
                            iconButtonPlayer6_Click(null, new EventArgs());
                            break;

                        case 7:
                            iconButtonPlayer7_Click(null, new EventArgs());
                            break;

                        case 8:
                            iconButtonPlayer8_Click(null, new EventArgs());
                            break;

                    }
                    string decodedString = Uri.UnescapeDataString(namemusic);
                    for (var tt = 0; tt < this.myListView.Items.Count; tt++)
                    {
                        if (myListView.Items[tt].SubItems[1].Text.ToString() == decodedString)
                        {
                            myListView.Items[tt].Selected = true;
                            break;
                        }
                    }
                    myListView_DoubleClick(this, new EventArgs());
                }));

            }
            else
            {
                switch (ids)
                {
                    case 1:
                        iconButtonPlayer1_Click(null, new EventArgs());

                        break;
                    case 2:
                        iconButtonPlayer2_Click(null, new EventArgs());
                        break;
                    case 3:
                        iconButtonPlayer3_Click(null, new EventArgs());
                        break;
                    case 4:
                        iconButtonPlayer4_Click(null, new EventArgs());
                        break;
                    case 5:
                        iconButtonPlayer5_Click(null, new EventArgs());
                        break;

                    case 6:
                        iconButtonPlayer6_Click(null, new EventArgs());
                        break;

                    case 7:
                        iconButtonPlayer7_Click(null, new EventArgs());
                        break;

                    case 8:
                        iconButtonPlayer8_Click(null, new EventArgs());
                        break;

                }
                for (var tt = 0; tt < this.myListView.Items.Count; tt++)
                {
                    if (myListView.Items[tt].SubItems[1].Text.ToString() == namemusic)
                    {
                        myListView.Items[tt].Selected = true;
                        break;
                    }
                }
                myListView_DoubleClick(this, new EventArgs());
            }
        }
        private void myListView_DoubleClick(object sender, EventArgs e)
        {
            bool flag = this.myListView.SelectedItems.Count == 0;
            if (!flag)
            {
                ListView.SelectedListViewItemCollection albums = this.myListView.SelectedItems;
                bool flag2 = albums == null;
                if (!flag2)
                {
                    OTSMedia _trackName = new OTSMedia();
                    _trackName.Shuffle = this.bShuffle;
                    _trackName.Loop = this.bLoop;
                    foreach (object obj in albums)
                    {
                        ListViewItem song = (ListViewItem)obj;
                        _trackName.Id = int.Parse((song.Index + 1).ToString());
                        _trackName.fileName = song.SubItems[1].Text;
                        _trackName.duration = TimeSpan.Parse(song.SubItems[2].Text);
                        _trackName.fileLocation = song.SubItems[3].Text;
                    }
                    bool flag3 = int.Parse(this.lblPlayerId.Text.Trim()) == 1;
                    if (flag3)
                    {
                        this.nPlayer1.set_selectmusic(true);
                        this.nPlayer1.set_runplaylist(true);

                        this.nPlayer1.CurrentMediaChanged += this.NPlayer1_CurrentMediaChanged;
                        this.nPlayer1.CurrentMedia = _trackName;
                        this.nPlayer1.CurrentMediaChanged -= this.NPlayer1_CurrentMediaChanged;
                    }
                    bool flag4 = int.Parse(this.lblPlayerId.Text.Trim()) == 2;
                    if (flag4)
                    {
                        this.nPlayer2.set_selectmusic(true);
                        this.nPlayer2.set_runplaylist(true);
                        this.nPlayer2.CurrentMediaChanged += this.NPlayer2_CurrentMediaChanged;
                        this.nPlayer2.CurrentMedia = _trackName;
                        this.nPlayer2.CurrentMediaChanged -= this.NPlayer2_CurrentMediaChanged;
                    }
                    bool flag5 = int.Parse(this.lblPlayerId.Text.Trim()) == 3;
                    if (flag5)
                    {
                        this.nPlayer3.set_selectmusic(true);
                        this.nPlayer3.set_runplaylist(true);
                        this.nPlayer3.CurrentMediaChanged += this.NPlayer3_CurrentMediaChanged;
                        this.nPlayer3.CurrentMedia = _trackName;
                        this.nPlayer3.CurrentMediaChanged -= this.NPlayer3_CurrentMediaChanged;
                    }
                    bool flag6 = int.Parse(this.lblPlayerId.Text.Trim()) == 4;
                    if (flag6)
                    {
                        this.nPlayer4.set_selectmusic(true);
                        this.nPlayer4.set_runplaylist(true);
                        this.nPlayer4.CurrentMediaChanged += this.NPlayer4_CurrentMediaChanged;
                        this.nPlayer4.CurrentMedia = _trackName;
                        this.nPlayer4.CurrentMediaChanged -= this.NPlayer4_CurrentMediaChanged;
                    }
                    bool flag7 = int.Parse(this.lblPlayerId.Text.Trim()) == 5;
                    if (flag7)
                    {
                        this.nPlayer5.set_selectmusic(true);
                        this.nPlayer5.set_runplaylist(true);
                        this.nPlayer5.CurrentMediaChanged += this.NPlayer5_CurrentMediaChanged;
                        this.nPlayer5.CurrentMedia = _trackName;
                        this.nPlayer5.CurrentMediaChanged -= this.NPlayer5_CurrentMediaChanged;
                    }
                    bool flag8 = int.Parse(this.lblPlayerId.Text.Trim()) == 6;
                    if (flag8)
                    {
                        this.nPlayer6.set_selectmusic(true);
                        this.nPlayer6.set_runplaylist(true);
                        this.nPlayer6.CurrentMediaChanged += this.NPlayer6_CurrentMediaChanged;
                        this.nPlayer6.CurrentMedia = _trackName;
                        this.nPlayer6.CurrentMediaChanged -= this.NPlayer6_CurrentMediaChanged;
                    }
                    bool flag9 = int.Parse(this.lblPlayerId.Text.Trim()) == 7;
                    if (flag9)
                    {
                        this.nPlayer7.set_selectmusic(true);
                        this.nPlayer7.set_runplaylist(true);
                        this.nPlayer7.CurrentMediaChanged += this.NPlayer7_CurrentMediaChanged;
                        this.nPlayer7.CurrentMedia = _trackName;
                        this.nPlayer7.CurrentMediaChanged -= this.NPlayer7_CurrentMediaChanged;
                    }
                    bool flag10 = int.Parse(this.lblPlayerId.Text.Trim()) == 8;
                    if (flag10)
                    {
                        this.nPlayer8.set_selectmusic(true);
                        this.nPlayer8.set_runplaylist(true);
                        this.nPlayer8.CurrentMediaChanged += this.NPlayer8_CurrentMediaChanged;
                        this.nPlayer8.CurrentMedia = _trackName;
                        this.nPlayer8.CurrentMediaChanged -= this.NPlayer8_CurrentMediaChanged;
                    }
                }
            }
            this.trigger_url();

        }
        #endregion

        #region CurrentMediaChanged
        private void NPlayer1_CurrentMediaChanged(object sender, EventArgs e)
        {
            var xq = this.myListView.Items.Count;
        }
        private void NPlayer2_CurrentMediaChanged(object sender, EventArgs e)
        {

        }
        private void NPlayer3_CurrentMediaChanged(object sender, EventArgs e)
        {

        }
        private void NPlayer4_CurrentMediaChanged(object sender, EventArgs e)
        {

        }
        private void NPlayer5_CurrentMediaChanged(object sender, EventArgs e)
        {

        }
        private void NPlayer6_CurrentMediaChanged(object sender, EventArgs e)
        {

        }
        private void NPlayer7_CurrentMediaChanged(object sender, EventArgs e)
        {

        }
        private void NPlayer8_CurrentMediaChanged(object sender, EventArgs e)
        {

        }
        #endregion

        #region Playlist
        private void iconButtonPlayer1_Click(object sender, EventArgs e)
        {
            if (timersc == true)
            {
                var messagebox = new Helper.MessageBox();
                messagebox.ShowCenter_DialogError("กรุณาตั้ง timer ให้เสร็จก่อน", "แจ้งเตือน");
                return;
            }

            SelectPlayList(1);

            if (nPlayer1.get_runorder() != 0)
            {
                if (nPlayer1.wavePlayer != null)
                {
                    if (nPlayer1.wavePlayer.PlaybackState == PlaybackState.Playing)
                    {
                        var ns = nPlayer1.playlist[nPlayer1.get_runorder() - 1];
                        var namemusic = ns.Split('\\');
                        change_namemedia(Index_item(namemusic[namemusic.Length - 1]));
                    }
                }
            }
            labelSelectedSong.Text = String.Empty;
        }

        private void iconButtonPlayer2_Click(object sender, EventArgs e)
        {
            if (timersc == true)
            {
                var messagebox = new Helper.MessageBox();
                messagebox.ShowCenter_DialogError("กรุณาตั้ง timer ให้เสร็จก่อน", "แจ้งเตือน");
                return;
            }
            SelectPlayList(2);
            if (nPlayer2.get_runorder() != 0)
            {
                if (nPlayer2.wavePlayer != null)
                {

                    if (nPlayer2.wavePlayer.PlaybackState == PlaybackState.Playing)
                    {
                        var ns = nPlayer2.playlist[nPlayer2.get_runorder() - 1];
                        var namemusic = ns.Split('\\');
                        change_namemedia(Index_item(namemusic[namemusic.Length - 1]));
                    }
                }
            }
            labelSelectedSong.Text = String.Empty;
        }
        private void iconButtonPlayer3_Click(object sender, EventArgs e)
        {
            if (timersc == true)
            {
                var messagebox = new Helper.MessageBox();
                messagebox.ShowCenter_DialogError("กรุณาตั้ง timer ให้เสร็จก่อน", "แจ้งเตือน");
                return;
            }
            SelectPlayList(3);
            if (nPlayer3.get_runorder() != 0)
            {
                if (nPlayer3.wavePlayer != null)
                {
                    if (nPlayer3.wavePlayer.PlaybackState == PlaybackState.Playing)
                    {
                        var ns = nPlayer3.playlist[nPlayer3.get_runorder() - 1];
                        var namemusic = ns.Split('\\');
                        change_namemedia(Index_item(namemusic[namemusic.Length - 1]));
                    }
                }
            }
            labelSelectedSong.Text = String.Empty;

        }
        private void iconButtonPlayer4_Click(object sender, EventArgs e)
        {
            if (timersc == true)
            {
                var messagebox = new Helper.MessageBox();
                messagebox.ShowCenter_DialogError("กรุณาตั้ง timer ให้เสร็จก่อน", "แจ้งเตือน");
                return;
            }
            SelectPlayList(4);
            if (nPlayer4.get_runorder() != 0)
            {
                if (nPlayer4.wavePlayer != null)
                {
                    if (nPlayer4.wavePlayer.PlaybackState == PlaybackState.Playing)
                    {
                        var ns = nPlayer4.playlist[nPlayer4.get_runorder() - 1];
                        var namemusic = ns.Split('\\');
                        change_namemedia(Index_item(namemusic[namemusic.Length - 1]));
                    }
                }
            }
            labelSelectedSong.Text = String.Empty;
        }
        private void iconButtonPlayer5_Click(object sender, EventArgs e)
        {
            if (timersc == true)
            {
                var messagebox = new Helper.MessageBox();
                messagebox.ShowCenter_DialogError("กรุณาตั้ง timer ให้เสร็จก่อน", "แจ้งเตือน");
                return;
            }
            SelectPlayList(5);
            if (nPlayer5.get_runorder() != 0)
            {
                if (nPlayer5.wavePlayer != null)
                {
                    if (nPlayer5.wavePlayer.PlaybackState == PlaybackState.Playing)
                    {
                        var ns = nPlayer5.playlist[nPlayer5.get_runorder() - 1];
                        var namemusic = ns.Split('\\');
                        change_namemedia(Index_item(namemusic[namemusic.Length - 1]));
                    }
                }
            }
            labelSelectedSong.Text = String.Empty;
        }
        private void iconButtonPlayer6_Click(object sender, EventArgs e)
        {
            if (timersc == true)
            {
                var messagebox = new Helper.MessageBox();
                messagebox.ShowCenter_DialogError("กรุณาตั้ง timer ให้เสร็จก่อน", "แจ้งเตือน");
                return;
            }
            SelectPlayList(6);
            if (nPlayer6.get_runorder() != 0)
            {
                if (nPlayer6.wavePlayer != null)
                {
                    if (nPlayer6.wavePlayer.PlaybackState == PlaybackState.Playing)
                    {
                        var ns = nPlayer6.playlist[nPlayer6.get_runorder() - 1];
                        var namemusic = ns.Split('\\');
                        change_namemedia(Index_item(namemusic[namemusic.Length - 1]));
                    }
                }
            }
            labelSelectedSong.Text = String.Empty;
        }
        private void iconButtonPlayer7_Click(object sender, EventArgs e)
        {
            if (timersc == true)
            {
                var messagebox = new Helper.MessageBox();
                messagebox.ShowCenter_DialogError("กรุณาตั้ง timer ให้เสร็จก่อน", "แจ้งเตือน");
                return;
            }
            SelectPlayList(7);
            if (nPlayer7.get_runorder() != 0)
            {
                if (nPlayer7.wavePlayer != null)
                {
                    if (nPlayer7.wavePlayer.PlaybackState == PlaybackState.Playing)
                    {
                        var ns = nPlayer7.playlist[nPlayer7.get_runorder() - 1];
                        var namemusic = ns.Split('\\');
                        change_namemedia(Index_item(namemusic[namemusic.Length - 1]));
                    }
                }
            }
            labelSelectedSong.Text = String.Empty;
        }
        private void iconButtonPlayer8_Click(object sender, EventArgs e)
        {
            if (timersc == true)
            {
                var messagebox = new Helper.MessageBox();
                messagebox.ShowCenter_DialogError("กรุณาตั้ง timer ให้เสร็จก่อน", "แจ้งเตือน");
                return;
            }
            SelectPlayList(8);
            if (nPlayer8.get_runorder() != 0)
            {
                if (nPlayer8.wavePlayer != null)
                {
                    if (nPlayer8.wavePlayer.PlaybackState == PlaybackState.Playing)
                    {
                        var ns = nPlayer8.playlist[nPlayer8.get_runorder() - 1];
                        var namemusic = ns.Split('\\');
                        change_namemedia(Index_item(namemusic[namemusic.Length - 1]));
                    }
                }
            }
            labelSelectedSong.Text = String.Empty;
        }
        private void SelectPlayList(short id)
        {
            lblPlayerId.Text = String.Format(" {0} ", id);
            lblPlayerId.ForeColor = Color.LightGreen;
            //128, 646, 64
            lblPlayerId.BackColor = ActiveBGColor;
            //Save Playlist First
            //SaveDefaultPlayList(lastId);
            LoadDefaultPlaylist(id);
            
            switch (id)
            {
                case 1:
                    {
                        //iconButtonPlayer1.IconChar = FontAwesome.Sharp.IconChar.AngleRight;
                        //iconButtonPlayer1.IconColor = Color.LightGreen;
                        iconButtonPlayer1.ForeColor = Color.LightGreen;
                        iconButtonPlayer1.BackColor = ActiveBGColor;
                        iconButtonPlayer1.Text = "1";

                        iconButtonPlayer2.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer2.BackColor = InactiveBGColor;
                        iconButtonPlayer2.ForeColor = Color.Empty;
                        iconButtonPlayer2.Text = "";

                        iconButtonPlayer3.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer3.BackColor = InactiveBGColor;
                        iconButtonPlayer3.ForeColor = Color.Empty;
                        iconButtonPlayer3.Text = "";

                        iconButtonPlayer4.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer4.BackColor = InactiveBGColor;
                        iconButtonPlayer4.ForeColor = Color.Empty;
                        iconButtonPlayer4.Text = "";

                        iconButtonPlayer5.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer5.BackColor = InactiveBGColor;
                        iconButtonPlayer5.ForeColor = Color.Empty;
                        iconButtonPlayer5.Text = "";

                        iconButtonPlayer6.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer6.BackColor = InactiveBGColor;
                        iconButtonPlayer6.ForeColor = Color.Empty;
                        iconButtonPlayer6.Text = "";

                        iconButtonPlayer7.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer7.BackColor = InactiveBGColor;
                        iconButtonPlayer7.ForeColor = Color.Empty;
                        iconButtonPlayer7.Text = "";

                        iconButtonPlayer8.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer8.BackColor = InactiveBGColor;
                        iconButtonPlayer8.ForeColor = Color.Empty;
                        iconButtonPlayer8.Text = "";
                        timername = "timers1";
                    }
                    break;
                case 2:
                    {
                        //iconButtonPlayer2.IconChar = FontAwesome.Sharp.IconChar.AngleRight;
                        //iconButtonPlayer2.IconColor = Color.LightGreen;
                        iconButtonPlayer2.BackColor = ActiveBGColor;
                        iconButtonPlayer2.ForeColor = Color.LightGreen;
                        iconButtonPlayer2.Text = "2";


                        iconButtonPlayer1.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer1.BackColor = InactiveBGColor;
                        iconButtonPlayer1.ForeColor = Color.Empty;
                        iconButtonPlayer1.Text = "";

                        iconButtonPlayer3.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer3.BackColor = InactiveBGColor;
                        iconButtonPlayer3.ForeColor = Color.Empty;
                        iconButtonPlayer3.Text = "";

                        iconButtonPlayer4.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer4.BackColor = InactiveBGColor;
                        iconButtonPlayer4.ForeColor = Color.Empty;
                        iconButtonPlayer4.Text = "";

                        iconButtonPlayer5.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer5.BackColor = InactiveBGColor;
                        iconButtonPlayer5.ForeColor = Color.Empty;
                        iconButtonPlayer5.Text = "";

                        iconButtonPlayer6.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer6.BackColor = InactiveBGColor;
                        iconButtonPlayer6.ForeColor = Color.Empty;
                        iconButtonPlayer6.Text = "";

                        iconButtonPlayer7.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer7.BackColor = InactiveBGColor;
                        iconButtonPlayer7.ForeColor = Color.Empty;
                        iconButtonPlayer7.Text = "";

                        iconButtonPlayer8.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer8.BackColor = InactiveBGColor;
                        iconButtonPlayer8.ForeColor = Color.Empty;
                        iconButtonPlayer8.Text = "";
                        timername = "timers2";
                    }
                    break;
                case 3:
                    {
                        //iconButtonPlayer3.IconChar = FontAwesome.Sharp.IconChar.AngleRight;
                        //iconButtonPlayer3.IconColor = Color.LightGreen;
                        iconButtonPlayer3.ForeColor = Color.LightGreen;
                        iconButtonPlayer3.BackColor = ActiveBGColor;
                        iconButtonPlayer3.Text = "3";

                        iconButtonPlayer1.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer1.BackColor = InactiveBGColor;
                        iconButtonPlayer1.ForeColor = Color.Empty;
                        iconButtonPlayer1.Text = "";

                        iconButtonPlayer2.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer2.BackColor = InactiveBGColor;
                        iconButtonPlayer2.ForeColor = Color.Empty;
                        iconButtonPlayer2.Text = "";

                        iconButtonPlayer4.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer4.BackColor = InactiveBGColor;
                        iconButtonPlayer4.ForeColor = Color.Empty;
                        iconButtonPlayer4.Text = "";

                        iconButtonPlayer5.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer5.BackColor = InactiveBGColor;
                        iconButtonPlayer5.ForeColor = Color.Empty;
                        iconButtonPlayer5.Text = "";

                        iconButtonPlayer6.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer6.BackColor = InactiveBGColor;
                        iconButtonPlayer6.ForeColor = Color.Empty;
                        iconButtonPlayer6.Text = "";

                        iconButtonPlayer7.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer7.BackColor = InactiveBGColor;
                        iconButtonPlayer7.ForeColor = Color.Empty;
                        iconButtonPlayer7.Text = "";

                        iconButtonPlayer8.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer8.BackColor = InactiveBGColor;
                        iconButtonPlayer8.ForeColor = Color.Empty;
                        iconButtonPlayer8.Text = "";
                        timername = "timers3";
                    }
                    break;
                case 4:
                    {
                        //iconButtonPlayer4.IconChar = FontAwesome.Sharp.IconChar.AngleRight;
                        //iconButtonPlayer4.IconColor = Color.LightGreen;
                        iconButtonPlayer4.ForeColor = Color.LightGreen;
                        iconButtonPlayer4.BackColor = ActiveBGColor;
                        iconButtonPlayer4.Text = "4";

                        iconButtonPlayer1.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer1.BackColor = InactiveBGColor;
                        iconButtonPlayer1.ForeColor = Color.Empty;
                        iconButtonPlayer1.Text = "";

                        iconButtonPlayer2.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer2.BackColor = InactiveBGColor;
                        iconButtonPlayer2.ForeColor = Color.Empty;
                        iconButtonPlayer2.Text = "";

                        iconButtonPlayer3.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer3.BackColor = InactiveBGColor;
                        iconButtonPlayer3.ForeColor = Color.Empty;
                        iconButtonPlayer3.Text = "";

                        iconButtonPlayer5.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer5.BackColor = InactiveBGColor;
                        iconButtonPlayer5.ForeColor = Color.Empty;
                        iconButtonPlayer5.Text = "";

                        iconButtonPlayer6.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer6.BackColor = InactiveBGColor;
                        iconButtonPlayer6.ForeColor = Color.Empty;
                        iconButtonPlayer6.Text = "";

                        iconButtonPlayer7.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer7.BackColor = InactiveBGColor;
                        iconButtonPlayer7.ForeColor = Color.Empty;
                        iconButtonPlayer7.Text = "";

                        iconButtonPlayer8.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer8.BackColor = InactiveBGColor;
                        iconButtonPlayer8.ForeColor = Color.Empty;
                        iconButtonPlayer8.Text = "";
                        timername = "timers4";
                    }
                    break;
                case 5:
                    {
                        //iconButtonPlayer5.IconChar = FontAwesome.Sharp.IconChar.AngleRight;
                        //iconButtonPlayer5.IconColor = Color.LightGreen;
                        iconButtonPlayer5.BackColor = ActiveBGColor;
                        iconButtonPlayer5.ForeColor = Color.LightGreen;
                        iconButtonPlayer5.Text = "5";

                        iconButtonPlayer1.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer1.BackColor = InactiveBGColor;
                        iconButtonPlayer1.ForeColor = Color.Empty;
                        iconButtonPlayer1.Text = "";

                        iconButtonPlayer2.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer2.BackColor = InactiveBGColor;
                        iconButtonPlayer2.ForeColor = Color.Empty;
                        iconButtonPlayer2.Text = "";

                        iconButtonPlayer3.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer3.BackColor = InactiveBGColor;
                        iconButtonPlayer3.ForeColor = Color.Empty;
                        iconButtonPlayer3.Text = "";

                        iconButtonPlayer4.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer4.BackColor = InactiveBGColor;
                        iconButtonPlayer4.ForeColor = Color.Empty;
                        iconButtonPlayer4.Text = "";

                        iconButtonPlayer6.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer6.BackColor = InactiveBGColor;
                        iconButtonPlayer6.ForeColor = Color.Empty;
                        iconButtonPlayer6.Text = "";

                        iconButtonPlayer7.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer7.BackColor = InactiveBGColor;
                        iconButtonPlayer7.ForeColor = Color.Empty;
                        iconButtonPlayer7.Text = "";

                        iconButtonPlayer8.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer8.BackColor = InactiveBGColor;
                        iconButtonPlayer8.ForeColor = Color.Empty;
                        iconButtonPlayer8.Text = "";
                        timername = "timers5";
                    }
                    break;
                case 6:
                    {
                        //iconButtonPlayer6.IconChar = FontAwesome.Sharp.IconChar.AngleRight;
                        //iconButtonPlayer6.IconColor = Color.LightGreen;
                        iconButtonPlayer6.BackColor = ActiveBGColor;
                        iconButtonPlayer6.ForeColor = Color.LightGreen;
                        iconButtonPlayer6.Text = "6";

                        iconButtonPlayer1.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer1.BackColor = InactiveBGColor;
                        iconButtonPlayer1.ForeColor = Color.Empty;
                        iconButtonPlayer1.Text = "";

                        iconButtonPlayer2.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer2.BackColor = InactiveBGColor;
                        iconButtonPlayer2.ForeColor = Color.Empty;
                        iconButtonPlayer2.Text = "";

                        iconButtonPlayer3.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer3.BackColor = InactiveBGColor;
                        iconButtonPlayer3.ForeColor = Color.Empty;
                        iconButtonPlayer3.Text = "";

                        iconButtonPlayer4.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer4.BackColor = InactiveBGColor;
                        iconButtonPlayer4.ForeColor = Color.Empty;
                        iconButtonPlayer4.Text = "";

                        iconButtonPlayer5.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer5.BackColor = InactiveBGColor;
                        iconButtonPlayer5.ForeColor = Color.Empty;
                        iconButtonPlayer5.Text = "";

                        iconButtonPlayer7.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer7.BackColor = InactiveBGColor;
                        iconButtonPlayer7.ForeColor = Color.Empty;
                        iconButtonPlayer7.Text = "";

                        iconButtonPlayer8.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer8.BackColor = InactiveBGColor;
                        iconButtonPlayer8.ForeColor = Color.Empty;
                        iconButtonPlayer8.Text = "";
                        timername = "timers6";
                    }
                    break;
                case 7:
                    {
                        //iconButtonPlayer7.IconChar = FontAwesome.Sharp.IconChar.AngleRight;
                        //iconButtonPlayer7.IconColor = Color.LightGreen;
                        iconButtonPlayer7.BackColor = ActiveBGColor;
                        iconButtonPlayer7.ForeColor = Color.LightGreen;
                        iconButtonPlayer7.Text = "7";

                        iconButtonPlayer1.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer1.BackColor = InactiveBGColor;
                        iconButtonPlayer1.ForeColor = Color.Empty;
                        iconButtonPlayer1.Text = "";

                        iconButtonPlayer2.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer2.BackColor = InactiveBGColor;
                        iconButtonPlayer2.ForeColor = Color.Empty;
                        iconButtonPlayer2.Text = "";

                        iconButtonPlayer3.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer3.BackColor = InactiveBGColor;
                        iconButtonPlayer3.ForeColor = Color.Empty;
                        iconButtonPlayer3.Text = "";

                        iconButtonPlayer4.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer4.BackColor = InactiveBGColor;
                        iconButtonPlayer4.ForeColor = Color.Empty;
                        iconButtonPlayer4.Text = "";

                        iconButtonPlayer5.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer5.BackColor = InactiveBGColor;
                        iconButtonPlayer5.ForeColor = Color.Empty;
                        iconButtonPlayer5.Text = "";

                        iconButtonPlayer6.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer6.BackColor = InactiveBGColor;
                        iconButtonPlayer6.ForeColor = Color.Empty;
                        iconButtonPlayer6.Text = "";

                        iconButtonPlayer8.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer8.BackColor = InactiveBGColor;
                        iconButtonPlayer8.ForeColor = Color.Empty;
                        iconButtonPlayer8.Text = "";
                        timername = "timers7";
                    }
                    break;
                case 8:
                    {
                        //iconButtonPlayer8.IconChar = FontAwesome.Sharp.IconChar.AngleRight;
                        //iconButtonPlayer8.IconColor = Color.LightGreen;
                        iconButtonPlayer8.BackColor = ActiveBGColor;
                        iconButtonPlayer8.ForeColor = Color.LightGreen;
                        iconButtonPlayer8.Text = "8";

                        iconButtonPlayer1.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer1.BackColor = InactiveBGColor;
                        iconButtonPlayer1.ForeColor = Color.Empty;
                        iconButtonPlayer1.Text = "";

                        iconButtonPlayer2.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer2.BackColor = InactiveBGColor;
                        iconButtonPlayer2.ForeColor = Color.Empty;
                        iconButtonPlayer2.Text = "";

                        iconButtonPlayer3.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer3.BackColor = InactiveBGColor;
                        iconButtonPlayer3.ForeColor = Color.Empty;
                        iconButtonPlayer3.Text = "";

                        iconButtonPlayer4.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer4.BackColor = InactiveBGColor;
                        iconButtonPlayer4.ForeColor = Color.Empty;
                        iconButtonPlayer4.Text = "";

                        iconButtonPlayer5.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer5.BackColor = InactiveBGColor;
                        iconButtonPlayer5.ForeColor = Color.Empty;
                        iconButtonPlayer5.Text = "";

                        iconButtonPlayer6.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer6.BackColor = InactiveBGColor;
                        iconButtonPlayer6.ForeColor = Color.Empty;
                        iconButtonPlayer6.Text = "";

                        iconButtonPlayer7.IconChar = FontAwesome.Sharp.IconChar.None;
                        iconButtonPlayer7.BackColor = InactiveBGColor;
                        iconButtonPlayer7.ForeColor = Color.Empty;
                        iconButtonPlayer7.Text = "";
                        timername = "timers8";
                    }
                    break;

                default:
                    break;
            }
        }
        private void SaveDefaultPlayList(short Id, bool save = false)
        {
            //lblPlayerId.Text.Trim()
            if (lastId != Id) lastId = Id;
            if (myListView.Items.Count > 0)
            {
                string fileName = String.Format("{0}\\{1}-List.txt", System.Environment.CurrentDirectory, lastId);
                using (TextWriter tw = new StreamWriter(new FileStream(fileName, FileMode.Create), Encoding.UTF8))
                {
                    foreach (System.Windows.Forms.ListViewItem item in myListView.Items)
                    {
                        tw.WriteLine(item.SubItems[0].Text + "\t" + item.SubItems[1].Text + "\t" + item.SubItems[2].Text + "\t" + item.SubItems[3].Text + "\t");
                    }
                }
                myListView.Items.Clear();
                SaveShuffleLoopState(lastId);
            }
            else
            {
                if (save == true)
                {
                    string fileName = String.Format("{0}\\{1}-List.txt", System.Environment.CurrentDirectory, lastId);
                    using (TextWriter tw = new StreamWriter(new FileStream(fileName, FileMode.Create), Encoding.UTF8))
                    {


                    }
                    myListView.Items.Clear();
                }
            }
            SaveShuffleLoopState(lastId);


        }

        public void LoadDefaultPlaylist(short Id)
        {
            if (lastId != Id) lastId = Id;
            string fileName = String.Format("{0}\\{1}-List.txt", System.Environment.CurrentDirectory, Id);
            myListView.Items.Clear();
            if (File.Exists(fileName))
            {
                List<String> data = System.IO.File.ReadAllLines(fileName).ToList();
                foreach (string _item in data)
                {
                    string[] items = _item.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    //myListView.Items.Add(new System.Windows.Forms.ListViewItem(item));
                    System.Windows.Forms.ListViewItem item = new System.Windows.Forms.ListViewItem(items[0]);

                    item.SubItems.Add(items[1]);
                    item.SubItems.Add(items[2]);
                    item.SubItems.Add(items[3]);
                    if (CoreLibrary.check_audio_file(items[3]) == false)
                    {
                        item.BackColor = Color.Red;
                    }
                    //item.SubItems.Add(items[4]);
                    myListView.Items.Add(item);
                }
                ReOrderSequence();
            }
            LoadShuffleLoopState(Id);

        }

        public (bool, int) checkfile_gg(string filename1)
        {
            for (var i = 1; i <= 8; i++)
            {
                string fileName = String.Format("{0}\\{1}-List.txt", System.Environment.CurrentDirectory, i);
                if (File.Exists(fileName))
                {
                    List<String> data = System.IO.File.ReadAllLines(fileName).ToList();
                    foreach (string _item in data)
                    {
                        string[] items = _item.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        if (items[3] == filename1)
                        {
                            return (true, i);
                        }
                    }
                }
            }
            return (false, 0);
        }

        public bool checkfile_gg1(string filename1)
        {
            if (File.Exists(filename1))
            {
                return true;
            }
            return false;
        }

        private void SaveShuffleLoopState(short Id)
        {
            string fileName = String.Format("{0}\\{1}-State.txt", System.Environment.CurrentDirectory, Id);
            if (bShuffle == false && bLoop == false)
            {
                return;
            }
            using (TextWriter tw = new StreamWriter(new FileStream(fileName, FileMode.Create), Encoding.UTF8))
            {
                tw.WriteLine(bShuffle + "\t" + bLoop + "\t");
            }
        }

        public void SaveShuffleLoopState1(short Id, bool t1, bool t2)
        {
            string fileName = String.Format("{0}\\{1}-State.txt", System.Environment.CurrentDirectory, Id);
            using (TextWriter tw = new StreamWriter(new FileStream(fileName, FileMode.Create), Encoding.UTF8))
            {
                tw.WriteLine(t2 + "\t" + t1 + "\t");
            }
        }

        public void LoadShuffleLoopState(short Id)
        {
            string fileName = String.Format("{0}\\{1}-State.txt", System.Environment.CurrentDirectory, Id);
            if (File.Exists(fileName))
            {
                List<String> data = System.IO.File.ReadAllLines(fileName).ToList();
                foreach (string _item in data)
                {
                    string[] items = _item.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    bShuffle = bool.Parse(items[0]);
                    if (bShuffle)
                    {
                        btnPlaylistShuffle.ForeColor = Color.FromArgb(255, 255, 128, 0);
                    }
                    else
                    {
                        btnPlaylistShuffle.ForeColor = Color.Black;
                    }

                    bLoop = bool.Parse(items[1]);
                    if (bLoop)
                    {
                        btnPlaylistLoop.ForeColor = Color.FromArgb(255, 255, 128, 0);
                    }
                    else
                    {
                        btnPlaylistLoop.ForeColor = Color.Black;
                    }
                }
            }
        }

        public Tuple<bool, bool> LoadShuffleLoopState1(short Id)
        {
            string fileName = String.Format("{0}\\{1}-State.txt", System.Environment.CurrentDirectory, Id);
            if (File.Exists(fileName))
            {
                List<String> data = System.IO.File.ReadAllLines(fileName).ToList();
                foreach (string _item in data)
                {
                    string[] items = _item.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    bShuffle = bool.Parse(items[0]);
                    bLoop = bool.Parse(items[1]);
                    return Tuple.Create(bShuffle, bLoop);

                }
            }
            return Tuple.Create(false, false);
        }
        #endregion

        private void iconLogo_Click(object sender, EventArgs e)
        {
            if (bServerStart == false)
            {
                //httpListener.GetPlayer();
                iconLogo.BackColor = Color.White;
                server.Start();
                bServerStart = true;
            }
            else
            {
                iconLogo.BackColor = Color.Black;
                server.Stop();
                bServerStart = false;
            }
        }

        #region Dispose
        private void DisposeWave()
        {
            //TOAMediaPlayer.NAudioOutput.NPlayer
            if (nPlayer1 != null)
            {
                if (nPlayer1.MediaPlaybackState == NAudio.Wave.PlaybackState.Playing)
                {
                    nPlayer1.PlayerControlStop();
                    nPlayer1.Dispose();
                    nPlayer1 = null;
                }
            }
            if (nPlayer2 != null)
            {
                if (nPlayer2.MediaPlaybackState == NAudio.Wave.PlaybackState.Playing)
                {
                    nPlayer2.PlayerControlStop();
                    nPlayer2.Dispose();
                    nPlayer2 = null;
                }
            }

            if (nPlayer3 != null)
            {
                if (nPlayer3.MediaPlaybackState == NAudio.Wave.PlaybackState.Playing)
                {
                    nPlayer3.PlayerControlStop();
                    nPlayer3.Dispose();
                    nPlayer3 = null;
                }
            }
            if (nPlayer4 != null)
            {
                if (nPlayer4.MediaPlaybackState == NAudio.Wave.PlaybackState.Playing)
                {
                    nPlayer4.PlayerControlStop();
                    nPlayer4.Dispose();
                    nPlayer4 = null;
                }
            }
            if (nPlayer5 != null)
            {
                if (nPlayer5.MediaPlaybackState == NAudio.Wave.PlaybackState.Playing)
                {
                    nPlayer5.PlayerControlStop();
                    nPlayer5.Dispose();
                    nPlayer5 = null;
                }
            }
            if (nPlayer6 != null)
            {
                if (nPlayer6.MediaPlaybackState == NAudio.Wave.PlaybackState.Playing)
                {
                    nPlayer6.PlayerControlStop();
                    nPlayer6.Dispose();
                    nPlayer6 = null;
                }
            }

            if (nPlayer7 != null)
            {
                if (nPlayer7.MediaPlaybackState == NAudio.Wave.PlaybackState.Playing)
                {
                    nPlayer7.PlayerControlStop();
                    nPlayer7.Dispose();
                    nPlayer7 = null;
                }
            }
            if (nPlayer8 != null)
            {
                if (nPlayer8.MediaPlaybackState == NAudio.Wave.PlaybackState.Playing)
                {
                    nPlayer8.PlayerControlStop();
                    nPlayer8.Dispose();
                    nPlayer8 = null;
                }
            }
        }
        #endregion

        private void iconPictureBox1_Click(object sender, EventArgs e)
        {
            //SetupOutput xForm = new SetupOutput();
            //xForm.StartPosition = FormStartPosition.CenterParent;
            //xForm.ShowDialog();
            LoginAdmin xForm = new LoginAdmin(this);
            xForm.StartPosition = FormStartPosition.CenterParent;
            xForm.ShowDialog();
        }

        private void nPlayer1_Load(object sen31der, EventArgs e)
        {

        }

        private void nPlayer2_Load(object sender, EventArgs e)
        {

        }

        private void MainPlayer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Alt && e.KeyCode == Keys.C)
            {
                //SetupOutput xForm = new SetupOutput();
                //xForm.StartPosition = FormStartPosition.CenterParent;
                //xForm.ShowDialog();
                LoginAdmin xForm = new LoginAdmin(this);
                xForm.StartPosition = FormStartPosition.CenterParent;
                xForm.ShowDialog();
            }
            else if (e.KeyCode == Keys.Space)
            {
            }
        }

        private void splitContainer2_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void myListView_KeyDown(object sender, KeyEventArgs e)
        {
            bool flag = e.KeyCode == Keys.Delete;
            if (flag)
            {
                bool bRemoveItem = false;
                ListView target = (ListView)sender;
                bool flag2 = target.SelectedIndices != null && target.SelectedIndices.Count > 0;
                if (flag2)
                {
                    string deletefileName = string.Empty;
                    DialogResult confirmation = MessageBox.Show("Delete..", "Confirm!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    bool flag3 = confirmation == DialogResult.Yes;
                    if (flag3)
                    {
                        for (int i = this.myListView.SelectedIndices.Count - 1; i >= 0; i--)
                        {
                            this.stop_music_id(this.myListView.SelectedItems[i].Index);
                            this.myListView.Items.RemoveAt(this.myListView.SelectedIndices[i]);
                        }
                        bRemoveItem = true;
                    }
                }
                bool flag4 = bRemoveItem && this.myListView.Items.Count > 0;
                if (flag4)
                {
                    this.ReOrderSequence();
                }
                else
                {
                    bool flag5 = this.myListView.Items.Count == 0;
                    if (flag5)
                    {
                        this.clear_music();
                        this.ReOrderSequence();
                    }
                }
                short idsa = short.Parse(this.lblPlayerId.Text.Trim());
                this.SaveDefaultPlayList(idsa, true);
                this.LoadDefaultPlaylist(idsa);
                this.trigger_url();
            }
            bool flag6 = e.Control && e.KeyCode == Keys.A;
            if (flag6)
            {
                for (int j = 0; j < this.myListView.Items.Count; j++)
                {
                    this.myListView.Items[j].Selected = true;
                }
            }
            bool flag7 = e.KeyCode == Keys.Shift;
            if (flag7)
            {
                this.ships = true;
            }
        }

        public async void remove_music(string[] music, short ids)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    //music = music.Where(item => int.TryParse(item, out int num) && num >= 0)
                    //        .OrderByDescending(item => int.Parse(item)) // Sort in descending order
                    //        .ToArray();
                    mylistView_doubleclick1(ids);
                    bool bRemoveItem = false;
                    myListView.SelectedIndices.Clear();

                    foreach (var ig in music)
                    {
                        var id = short.Parse(ig);
                        if (myListView.Items.Count > id) {
                            myListView.SelectedIndices.Add(id);
                            this.stop_music_id(id + 1);
                        }
                        //this.myListView.Items[Convert.ToInt32(ig)].Selected = true;
                    }
                    //string fileName = String.Format("{0}\\{1}-List.txt", System.Environment.CurrentDirectory, ids);
                    //if (File.Exists(fileName)) {
                    //    List<string> data = System.IO.File.ReadAllLines(fileName).ToList();
                    //    foreach (var m in music) {
                    //        if (data.Count > 0) {
                    //            var filter = data[short.Parse(m)].Split('\t');
                    //            if (filter[4] == "api") {
                    //                File.Delete(filter[3]);
                    //            }
                    //        }
                    //    }
                    //}
                    string fileName = String.Format("{0}\\{1}-List.txt", System.Environment.CurrentDirectory, ids);
                    this.getDeleteFileApi(ids, fileName, music);

                    for (int i = this.myListView.SelectedIndices.Count - 1; i >= 0; i--)
                    {
                        //this.stop_music_id(this.myListView.SelectedItems[i].Index);
                        this.myListView.Items.RemoveAt(this.myListView.SelectedIndices[i]);
                    }
                    bRemoveItem = true;
                    bool flag4 = bRemoveItem && this.myListView.Items.Count > 0;
                    if (flag4)
                    {
                        this.ReOrderSequence();
                    }
                    else
                    {
                        bool flag5 = this.myListView.Items.Count == 0;
                        if (flag5)
                        {
                            this.clear_music();
                            this.ReOrderSequence();
                        }
                    }
                    short idsa = short.Parse(this.lblPlayerId.Text.Trim());
                    this.SaveDefaultPlayList(idsa, true);
                    this.LoadDefaultPlaylist(idsa);
                    this.trigger_url();
                }));
            }
            else
            {
                bool bRemoveItem = false;
                myListView.SelectedIndices.Clear();

                foreach (var ig in music) {
                    var id = short.Parse(ig);
                    if (myListView.Items.Count - 1 > id) {
                        myListView.SelectedIndices.Add(id);
                        this.stop_music_id(id + 1);
                    }
                    //this.myListView.Items[Convert.ToInt32(ig)].Selected = true;
                }

                //string fileName = String.Format("{0}\\{1}-List.txt", System.Environment.CurrentDirectory, ids);
                //if (File.Exists(fileName)) {
                //    List<string> data = System.IO.File.ReadAllLines(fileName).ToList();
                //    foreach (var m in music) {
                //        if (data.Count > 0) {
                //            var filter = data[short.Parse(m)].Split('\t');
                //            if (filter[4] == "api") {
                //                File.Delete(filter[3]);
                //            }
                //        }
                //    }
                //}
                string fileName = String.Format("{0}\\{1}-List.txt", System.Environment.CurrentDirectory, ids);
                this.getDeleteFileApi(ids, fileName, music);

                for (int i = this.myListView.SelectedIndices.Count - 1; i >= 0; i--) {
                    this.stop_music_id(this.myListView.SelectedItems[i].Index);
                    this.myListView.Items.RemoveAt(this.myListView.SelectedIndices[i]);
                }
                bRemoveItem = true;
                bool flag4 = bRemoveItem && this.myListView.Items.Count > 0;
                if (flag4) {
                    this.ReOrderSequence();
                } else {
                    bool flag5 = this.myListView.Items.Count == 0;
                    if (flag5) {
                        this.clear_music();
                        this.ReOrderSequence();
                    }
                }
                short idsa = short.Parse(this.lblPlayerId.Text.Trim());
                this.SaveDefaultPlayList(idsa, true);
                this.LoadDefaultPlaylist(idsa);
                this.trigger_url();
            }
        }

        public void getDeleteFileApi(int ids, string fileName, string[] music) {
            if (!File.Exists(fileName))
                return;

            List<string> data;

            // Read the file safely
            try {
                using (var reader = new StreamReader(fileName)) {
                    data = new List<string>();
                    string line;
                    while ((line = reader.ReadLine()) != null) {
                        data.Add(line);
                    }
                }
            } catch (IOException ioEx) {
                log.Error($"Error reading file: {ioEx.Message}");
                return;
            }

            var basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "music\\" + ids);
            foreach (var m in music) {
                try {
                    if (short.TryParse(m, out short index) && index >= 0 && index < data.Count) {
                        var filter = data[index].Split('\t');
                        if (filter[3].Contains(basePath)) {
                            string fileToDelete = filter[3];
                            // Log for debugging
                            log.Info("Attempting to delete: " + fileToDelete);
                            Console.WriteLine("Attempting to delete: " + fileToDelete);

                            if (File.Exists(fileToDelete)) {
                                // Try to delete safely
                                File.Delete(fileToDelete);
                                log.Info("Deleted: " + fileToDelete);

                            }
                        }
                    }
                } catch (IOException ioEx) {
                    log.Error($"Could not delete file: {ioEx.Message}");
                } catch (Exception ex) {
                    log.Error($"Unexpected error: {ex.Message}");
                }
            }

        }

        public void mylistView_doubleclick1(short ids)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    switch (ids)
                    {
                        case 1:
                            iconButtonPlayer1_Click(null, new EventArgs());

                            break;
                        case 2:
                            iconButtonPlayer2_Click(null, new EventArgs());
                            break;
                        case 3:
                            iconButtonPlayer3_Click(null, new EventArgs());
                            break;
                        case 4:
                            iconButtonPlayer4_Click(null, new EventArgs());
                            break;
                        case 5:
                            iconButtonPlayer5_Click(null, new EventArgs());
                            break;

                        case 6:
                            iconButtonPlayer6_Click(null, new EventArgs());
                            break;

                        case 7:
                            iconButtonPlayer7_Click(null, new EventArgs());
                            break;

                        case 8:
                            iconButtonPlayer8_Click(null, new EventArgs());
                            break;

                    }

                }));

            }
            else
            {
                switch (ids)
                {
                    case 1:
                        iconButtonPlayer1_Click(null, new EventArgs());

                        break;
                    case 2:
                        iconButtonPlayer2_Click(null, new EventArgs());
                        break;
                    case 3:
                        iconButtonPlayer3_Click(null, new EventArgs());
                        break;
                    case 4:
                        iconButtonPlayer4_Click(null, new EventArgs());
                        break;
                    case 5:
                        iconButtonPlayer5_Click(null, new EventArgs());
                        break;

                    case 6:
                        iconButtonPlayer6_Click(null, new EventArgs());
                        break;

                    case 7:
                        iconButtonPlayer7_Click(null, new EventArgs());
                        break;

                    case 8:
                        iconButtonPlayer8_Click(null, new EventArgs());
                        break;

                }

            }
        }

        private void btnPlaylistAddFolder_MouseHover(object sender, EventArgs e)
        {

            toolTip1.Show("Add Folder", btnPlaylistAddFolder);
        }

        private void btnPlaylistAddFile_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Add File", btnPlaylistAddFile);
        }

        private void btnPlaylistRemove_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Delete File", btnPlaylistRemove);
        }

        private void btnPlaylistShuffle_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Random", btnPlaylistShuffle);
        }

        private void btnPlaylistLoop_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Loop", btnPlaylistLoop);
        }

        private void btnPlaylistSave_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Save/Export", btnPlaylistSave);
        }

        private void btnPlaylistFileImport_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Import", btnPlaylistFileImport);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void myListView_Click(object sender, EventArgs e)
        {
        }

        private void myListView_KeyUp(object sender, KeyEventArgs e)
        {
            this.ships = false;
        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            this.heldDownItem = this.myListView.GetItemAt(e.X, e.Y);
            bool flag = this.heldDownItem != null;
            if (flag)
            {
                this.heldDownPoint = new Point(e.X - this.heldDownItem.Position.X, e.Y - this.heldDownItem.Position.Y);
            }
        }
        private void listView1_MouseMove(object sender, MouseEventArgs e)
        {
            bool flag = this.heldDownItem != null;
            if (flag)
            {
                this.heldDownItem.Position = new Point(e.Location.X - this.heldDownPoint.X, e.Location.Y - this.heldDownPoint.Y);
                this.Cursor = Cursors.Hand;
                int lastItemBottom = Math.Min(e.Y, this.myListView.Items[this.myListView.Items.Count - 1].GetBounds(ItemBoundsPortion.Entire).Bottom - 1);
                ListViewItem itemOver = this.myListView.GetItemAt(0, lastItemBottom);
                bool flag2 = itemOver == null;
                if (!flag2)
                {
                    System.Drawing.Rectangle rc = itemOver.GetBounds(ItemBoundsPortion.Entire);
                    bool flag3 = e.Y < rc.Top + rc.Height / 2;
                    if (flag3)
                    {
                        this.myListView.LineBefore = itemOver.Index;
                        this.myListView.LineAfter = -1;
                    }
                    else
                    {
                        this.myListView.LineBefore = -1;
                        this.myListView.LineAfter = itemOver.Index;
                    }
                    this.myListView.Invalidate();
                }
            }
        }

        private void listView1_MouseUp(object sender, MouseEventArgs e)
        {
            bool flag = this.heldDownItem == null;
            if (!flag)
            {
                try
                {
                    int lastItemBottom = Math.Min(e.Y, this.myListView.Items[this.myListView.Items.Count - 1].GetBounds(ItemBoundsPortion.Entire).Bottom - 1);
                    ListViewItem itemOver = this.myListView.GetItemAt(0, lastItemBottom);
                    bool flag2 = itemOver == null;
                    if (!flag2)
                    {
                        System.Drawing.Rectangle rc = itemOver.GetBounds(ItemBoundsPortion.Entire);
                        bool flag3 = e.Y < rc.Top + rc.Height / 2;
                        bool insertBefore = flag3;
                        bool flag4 = this.heldDownItem != itemOver;
                        if (flag4)
                        {
                            bool flag5 = insertBefore;
                            if (flag5)
                            {
                                this.myListView.Items.Remove(this.heldDownItem);
                                this.myListView.Items.Insert(itemOver.Index, this.heldDownItem);
                                this.change_music_list(this.heldDownItem.SubItems[3].Text, itemOver.Index);
                            }
                            else
                            {
                                this.myListView.Items.Remove(this.heldDownItem);
                                this.myListView.Items.Insert(itemOver.Index + 1, this.heldDownItem);
                                this.change_music_list(this.heldDownItem.SubItems[3].Text, itemOver.Index + 1);
                            }
                        }
                        this.myListView.LineAfter = (this.myListView.LineBefore = -1);
                        this.myListView.Invalidate();
                        this.trigger_url();

                    }
                }
                finally
                {
                    this.heldDownItem = null;
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void iconButton2_Click_1(object sender, EventArgs e)
        {
            if (!showtimeset)
            {
                showtimeset = true;
                //this.timersc = true;

                #region test ปุ่ม
                var form = new Settimers(this, this.timername, null);
                form.ShowDialog(); // ✅ ลบ Owner ออกถ้าไม่จำเป็น
                #endregion

                //var messagebox = new Helper.MessageBox();
                //messagebox.ShowDialog_Error("The selected output driver is not available on this system", "แจ้งเตือน");

                showtimeset = false;
            }
        }

        public async void trigger_url()
        {
            RegistryKey configsocket = HKLMSoftwareTOAConfig.OpenSubKey("trackname", true);
            var stringurl = "http://" + configsocket.GetValue("ip").ToString() + "/api/updateData?ip_address=" + configsocket.GetValue("ip1") + "&port=" + configsocket.GetValue("port");
            try
            {
                var client = new HttpClient();
                //var request = new HttpRequestMessage(HttpMethod.Get, "http://192.168.13.111/toa/api/updateData?ip_address=192.168.13.111&port=83");
                var request = new HttpRequestMessage(HttpMethod.Get, stringurl);
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                log.Info("Success Trigger");
            }
            catch (Exception ex)
            {
                log.Error("trigger_url Error: " + ex.Message + " , trigger_url Inner: " + ex.InnerException);
            }
        }

        public async void trigger_errormusic_url(List<jsonWebAPI.MusicErrorList> musiclist) {
            RegistryKey configsocket = HKLMSoftwareTOAConfig.OpenSubKey("trackname", true);
            string ipAddress = configsocket.GetValue("ip1")?.ToString();
            string port = configsocket.GetValue("port")?.ToString();
            string apiUrl = "http://" + configsocket.GetValue("ip").ToString() + "/api/errorMusic";

            var formattedList = musiclist.Select(m => new
            {
                source = m.PlayerTrack.ToString(),
                file = m.name
            }).ToList();
            string jsonString = JsonConvert.SerializeObject(formattedList);

            try {
                using (HttpClient client = new HttpClient()) {
                    // สร้าง Body JSON
                    var requestData = new {
                        ip_address = ipAddress,
                        port = port,
                        duplicate = jsonString
                    };

                    // แปลง JSON object เป็น string
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // สร้าง HttpRequestMessage แบบ POST
                    var request = new HttpRequestMessage(HttpMethod.Post, apiUrl) {
                        Content = content
                    };

                    var response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();

                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                    log.Info("Success Trigger");
                }
            } catch (Exception ex) {
                log.Error("Error: " + ex.Message + " , Inner: " + ex.InnerException);
            }
        }
    }
}