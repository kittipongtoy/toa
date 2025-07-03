using Microsoft.Win32;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.Compression;
using NAudio.Wave.SampleProviders;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace TOAMediaPlayer
{
    public partial class LoginAdmin : Form
    {
        private MainPlayer player;

        private RegistryKey HKLMSoftwareTOAPlayer = Registry.CurrentUser.OpenSubKey(@"Software\TOA\Player", true);
        private RegistryKey HKLMSoftwareTOAPlayer11 = Registry.CurrentUser.OpenSubKey(@"Software\TOA\Player\nPlayer1", true);
        private RegistryKey HKLMSoftwareTOAPlayer12 = Registry.CurrentUser.OpenSubKey(@"Software\TOA\Player\nPlayer2", true);
        private RegistryKey HKLMSoftwareTOAPlayer13 = Registry.CurrentUser.OpenSubKey(@"Software\TOA\Player\nPlayer3", true);
        private RegistryKey HKLMSoftwareTOAPlayer14 = Registry.CurrentUser.OpenSubKey(@"Software\TOA\Player\nPlayer4", true);
        private RegistryKey HKLMSoftwareTOAPlayer15 = Registry.CurrentUser.OpenSubKey(@"Software\TOA\Player\nPlayer5", true);
        private RegistryKey HKLMSoftwareTOAPlayer16 = Registry.CurrentUser.OpenSubKey(@"Software\TOA\Player\nPlayer6", true);
        private RegistryKey HKLMSoftwareTOAPlayer17 = Registry.CurrentUser.OpenSubKey(@"Software\TOA\Player\nPlayer7", true);
        private RegistryKey HKLMSoftwareTOAPlayer18 = Registry.CurrentUser.OpenSubKey(@"Software\TOA\Player\nPlayer8", true);
        private RegistryKey HKLMSoftwareTOAConfig = Registry.CurrentUser.OpenSubKey(@"Software\TOA\Config", true);
        private Loggers files = new Loggers("debugs.txt");

        public LoginAdmin(MainPlayer player)
        {
            InitializeComponent();
            textBox2.PasswordChar = '*';
            textBox2.Select();
            this.player = player;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var user = this.textBox1.Text;
            var pass = this.textBox2.Text;
            if (pass != "")
            {
                if (user.ToLower() == "admin" && pass.ToLower() == "toa12345")
                {
                    this.Close();
                    //SetupOutput xForm = new SetupOutput(player);
                    //xForm.StartPosition = FormStartPosition.CenterParent;
                    //xForm.ShowDialog();

                    ShowSetting();
                }
                else
                {
                    textBox2.Text = "";
                    var message = new Helper.MessageBox();
                    message.ShowCenter_DialogError("รหัสผิด กรุณาเช็ครหัสก่อน !!!", "แจ้งเตือน");
                }
            }
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }

        private List<string> systemLogs = new List<string>();

        private Queue<float> waveformQueue = new Queue<float>();
        private int maxWaveformPoints = 350; // เท่าความกว้าง panel

        private ComboBox cbOutputDriver;
        private ComboBox cbOutputDeviceName;
        private ComboBox cbOutputRequestedLatency;
        private TrackBar tbVolume;
        private ComboBox cbPlayer;
    
        private TextBox txtWebIP;
        private TextBox txtIP;
        private TextBox cbPort;

        private TextBox txtbox;
        private CheckBox chk;

        private TextBox txtbox2;
        private CheckBox chk2;

        private TextBox txtbox3;
        private CheckBox chk3;

        private TextBox txtbox4;
        private CheckBox chk4;

        private TextBox txtbox5;
        private CheckBox chk5;

        private TextBox txtbox6;
        private CheckBox chk6;

        private TextBox txtbox7;
        private CheckBox chk7;

        private TextBox txtbox8;
        private CheckBox chk8;
        DataGridView dgvPlayers2 = new DataGridView
        {
            Left = 20,
            Top = 220,
            Width = 790,
            Height = 250,
            ReadOnly = true,
            ColumnCount = 2,
            AllowUserToAddRows = false,
            RowHeadersVisible = false,
            Visible = true,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };

        public void ShowSetting()
        {
            Form prompt = new Form
            {
                Width = 850,
                Height = 600,
                Text = "Config",
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            #region บนหัว
            Button btnOpen = new Button { Text = "📂", Left = 10, Top = 10, Width = 30 }; // open
            Button btnPlay = new Button { Text = "▶", Left = 45, Top = 10, Width = 30 }; // play
            Button btnPause = new Button { Text = "⏸", Left = 80, Top = 10, Width = 30 }; // pause
            Button btnStop = new Button { Text = "⏹", Left = 115, Top = 10, Width = 30 }; // stop
            #endregion

            #region ซ้าย
            Label lblCurrentTime = new Label { Text = "Current Time: 00:00", Left = 160, Top = 15, AutoSize = true };
            Label lblTotalTime = new Label { Text = "Total Time: 00:00", Left = 300, Top = 15, AutoSize = true };

            Label lblOutputDriver = new Label { Text = "Output Driver", Left = 20, Top = 50, Width = 100 };
            cbOutputDriver = new ComboBox { Left = 130, Top = 45, Width = 150 };
            cbOutputDriver.Items.AddRange(new[] { "WasapiOut" });
            cbOutputDriver.SelectedIndex = 0;

            Label lblDriver = new Label { Text = "Driver", Left = 20, Top = 90, Width = 100 };
            cbOutputDeviceName = new ComboBox { Left = 130, Top = 85, Width = 200 };
            for (int i = 0; i < WaveOut.DeviceCount; i++)
            {
                var deviceInfo = WaveOut.GetCapabilities(i);
                cbOutputDeviceName.Items.Add(deviceInfo.ProductName);
            }
            if (cbOutputDeviceName.Items.Count > 0)
                cbOutputDeviceName.SelectedIndex = 0;

            CheckBox chkEventCallback = new CheckBox { Text = "Event Callback", Left = 20, Top = 130 };
            CheckBox chkExclusive = new CheckBox { Text = "Exclusive Mode", Left = 150, Top = 130 };

            Label lblPlayer = new Label { Text = "Player", Left = 20, Top = 170 };
            cbPlayer = new ComboBox { Left = 130, Top = 165, Width = 200 };
            cbPlayer.Items.AddRange(new[] { "nPlayer1", "nPlayer2", "nPlayer3", "nPlayer4", "nPlayer5", "nPlayer6", "nPlayer7", "nPlayer8" });
            cbPlayer.SelectedItem = "nPlayer1";

            Button btnApply = new Button { Text = "Apply", Left = 350, Top = 165 };
            CheckBox chkOnline = new CheckBox { Text = "Online", Left = 470, Top = 180, Width = 60 };

            btnApply.Click += SubmitApply;
            #endregion

            #region ขวา
            Label lblOutputRequestedLatency = new Label { Text = "Requested Latency", Left = 450, Top = 50, Width = 100 };
            cbOutputRequestedLatency = new ComboBox { Left = 620, Top = 45, Width = 150 };
            cbOutputRequestedLatency.Items.AddRange(new[] { "25", "50", "100", "150", "200", "300", "400", "500" });
            cbOutputRequestedLatency.SelectedIndex = 7;

            Label lblVolume = new Label { Text = "Volume", Left = 450, Top = 85 };

            Label lblCurrentFile = new Label { Text = "Current File:", Left = 450, Top = 130, Width = 80 };

            Label lblPlaybackFormat = new Label { Text = "Playback Format:", Left = 450, Top = 160 };

            tbVolume = new TrackBar
            {
                Left = 615,
                Top = 80,
                Width = 140,
                Height = 5,
                Minimum = 0,
                Maximum = 100,
                TickFrequency = 10
            };

            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            MMDevice device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            float currentVolume = device.AudioEndpointVolume.MasterVolumeLevelScalar;
            tbVolume.Value = (int)(currentVolume * 100);

            tbVolume.Scroll += (s, e) =>
            {
                device.AudioEndpointVolume.MasterVolumeLevelScalar = tbVolume.Value / 100f;
            };

            #region กราฟ เสียง 1
            Panel pnlWaveform = new Panel
            {
                Left = 800,
                Top = 130,
                Width = 10,
                Height = 80,
                BackColor = Color.Black
            };

            float level = 0;
            pnlWaveform.Paint += (sender, pe) =>
            {
                pe.Graphics.Clear(Color.Black);
                int barHeight = (int)(pnlWaveform.Height * level);
                int y = pnlWaveform.Height - barHeight;
                pe.Graphics.FillRectangle(Brushes.Lime, 0, y, pnlWaveform.Width, barHeight);
            };

            Timer timer = new Timer { Interval = 50 };
            timer.Tick += (s, e) =>
            {
                pnlWaveform.Invalidate();
            };
            timer.Start();

            #endregion

            #region กราฟเส้น เสียง 
            Panel pnlWaveformGraph = new Panel
            {
                Left = 590,
                Top = 160,
                Width = 180,
                Height = 50,
                BackColor = Color.Yellow,
                BorderStyle = BorderStyle.FixedSingle
            };

            pnlWaveformGraph.Paint += (sender, pe) =>
            {
                pe.Graphics.Clear(Color.Black);

                var levels = waveformQueue.ToArray();
                if (levels.Length < 2) return;

                PointF[] points = new PointF[levels.Length];
                float widthPerPoint = pnlWaveformGraph.Width / (float)maxWaveformPoints;
                float height = pnlWaveformGraph.Height;
                float midY = height / 2;

                for (int i = 0; i < levels.Length; i++)
                {
                    float x = i * widthPerPoint;
                    float y = midY - levels[i] * midY; // เส้น waveform
                    points[i] = new PointF(x, y);
                }

                pe.Graphics.DrawLines(Pens.Lime, points);
            };
            #endregion

            #region กราฟ เสียง 2
            Panel pnlWaveform2 = new Panel
            {
                Left = 780,
                Top = 130,
                Width = 10,
                Height = 80,
                BackColor = Color.Black
            };

            float level2 = 0;
            pnlWaveform2.Paint += (sender, pe) =>
            {
                pe.Graphics.Clear(Color.Black);
                int barHeight = (int)(pnlWaveform2.Height * level2);
                int y = pnlWaveform2.Height - barHeight;
                pe.Graphics.FillRectangle(Brushes.Lime, 0, y, pnlWaveform2.Width, barHeight);
            };

            Timer timer2 = new Timer { Interval = 50 };
            timer2.Tick += (s, e) =>
            {
                pnlWaveform2.Invalidate();
            };
            timer2.Start();

            #endregion
            #endregion

            var registryKeys = new[]
            {
                HKLMSoftwareTOAPlayer11,
                HKLMSoftwareTOAPlayer12,
                HKLMSoftwareTOAPlayer13,
                HKLMSoftwareTOAPlayer14,
                HKLMSoftwareTOAPlayer15,
                HKLMSoftwareTOAPlayer16,
                HKLMSoftwareTOAPlayer17,
                HKLMSoftwareTOAPlayer18
            };

            #region ปุ่ม Route/IP/Logs
            dgvPlayers2.Columns[0].Name = "Player";
            dgvPlayers2.Columns[1].Name = "Driver";

            // วนลูปเพิ่มข้อมูลลง DataGridView
            for (int i = 0; i < registryKeys.Length; i++)
            {
                string deviceName = registryKeys[i].GetValue("DeviceName")?.ToString() ?? "รอตั้งค่า";
                dgvPlayers2.Rows.Add($"Player {i + 1}", deviceName);
            }

            #region btnIPGain
            Panel panelIPGain = new Panel { Left = 20, Top = 220, Width = 790, Height = 250, BorderStyle = BorderStyle.FixedSingle, BackColor = Color.LightGray, Visible = false };

            // Web IP
            Label lblWebIP = new Label { Text = "Web IP", Left = 20, Top = 20, Width = 40 };
            txtWebIP = new TextBox { Left = 60, Top = 15, Width = 120 };
            Button btnWebSetIP = new Button { Text = "SET", Left = 190, Top = 15 };
            btnWebSetIP.Click += SubmitWebSetIP;

            //// IP
            Label lblIP = new Label { Text = "Server IP", Left = 280, Top = 20, Width = 60 };
            txtIP = new TextBox { Left = 350, Top = 15, Width = 120 };
            Button btnSetIP = new Button { Text = "SET", Left = 480, Top = 15 };
            btnSetIP.Click += SubmitSetIP;

            // Port
            Label lblPort = new Label { Text = "Port", Left = 570, Top = 20, Width = 40 };
            cbPort = new TextBox { Left = 620, Top = 15, Width = 50 };
            Button btnSetPort = new Button { Text = "SET", Left = 690, Top = 15 };
            btnSetPort.Click += SubmitSetPort;

            // Label Title
            panelIPGain.Controls.AddRange(new Control[] {
                 lblWebIP, txtWebIP, btnWebSetIP,
                 lblIP, txtIP, btnSetIP,
                 lblPort, cbPort, btnSetPort,
            });

            #region Auto Gain Control
            // พื้นขาวเฉพาะกลุ่ม Auto Gain Control + Player 1-4
            Panel panelWhiteArea = new Panel
            {
                Left = 10,
                Top = 45,
                Width = 770,
                Height = 190,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None // หรือ FixedSingle ถ้าอยากให้มีกรอบ
            };

            // Auto Gain Label
            Label lblAutoGain = new Label { Text = "Auto Gain Control (default 89.0)", Left = 10, Top = 10, Width = 300 };
            panelWhiteArea.Controls.Add(lblAutoGain);

            // วน Player 1–8 ฝั่งซ้าย + ขวา
            int startY = 30;
            int rowOffset = (0 % 4) * 30;
            int colOffset = 0 < 4 ? 0 : 400;

            chk = new CheckBox { Text = $"Player1", Left = 10 + colOffset, Top = startY + rowOffset, Width = 62 };
            txtbox = new TextBox { Text = "3 dB", Left = 90 + colOffset, Top = startY + rowOffset, Width = 80 };
            Label lblDb = new Label { Text = "dB", Left = 175 + colOffset, Top = startY + rowOffset + 5, Width = 20 };
            Button btn = new Button { Text = "SET", Left = 240 + colOffset, Top = startY + rowOffset };
            btn.Click += Submitbtnplayer;

            int rowOffset2 = (1 % 4) * 30;
            int colOffset2 = 1 < 4 ? 0 : 400;
            chk2 = new CheckBox { Text = $"Player2", Left = 10 + colOffset2, Top = startY + rowOffset2, Width = 62 };
            txtbox2 = new TextBox { Text = "3 dB", Left = 90 + colOffset2, Top = startY + rowOffset2, Width = 80 };
            Label lblDb2 = new Label { Text = "dB", Left = 175 + colOffset2, Top = startY + rowOffset2 + 5, Width = 20 };
            Button btn2 = new Button { Text = "SET", Left = 240 + colOffset2, Top = startY + rowOffset2 };
            btn2.Click += Submitbtnplayer2;

            int rowOffset3 = (2 % 4) * 30;
            int colOffset3 = 2 < 4 ? 0 : 400;
            chk3 = new CheckBox { Text = $"Player3", Left = 10 + colOffset3, Top = startY + rowOffset3, Width = 62 };
            txtbox3 = new TextBox { Text = "3 dB", Left = 90 + colOffset3, Top = startY + rowOffset3, Width = 80 };
            Label lblDb3 = new Label { Text = "dB", Left = 175 + colOffset3, Top = startY + rowOffset3 + 5, Width = 20 };
            Button btn3 = new Button { Text = "SET", Left = 240 + colOffset3, Top = startY + rowOffset3 };
            btn3.Click += Submitbtnplayer3;

            int rowOffset4 = (3 % 4) * 30;
            int colOffset4 = 3 < 4 ? 0 : 400;
            chk4 = new CheckBox { Text = $"Player4", Left = 10 + colOffset4, Top = startY + rowOffset4, Width = 62 };
            txtbox4 = new TextBox { Text = "3 dB", Left = 90 + colOffset4, Top = startY + rowOffset4, Width = 80 };
            Label lblDb4 = new Label { Text = "dB", Left = 175 + colOffset4, Top = startY + rowOffset4 + 5, Width = 20 };
            Button btn4 = new Button { Text = "SET", Left = 240 + colOffset4, Top = startY + rowOffset4 };
            btn4.Click += Submitbtnplayer4;

            int rowOffset5 = (4 % 4) * 30;
            int colOffset5 = 4 < 4 ? 0 : 400;
            chk5 = new CheckBox { Text = $"Player5", Left = 10 + colOffset5, Top = startY + rowOffset5, Width = 62 };
            txtbox5 = new TextBox { Text = "3 dB", Left = 90 + colOffset5, Top = startY + rowOffset5, Width = 80 };
            Label lblDb5 = new Label { Text = "dB", Left = 175 + colOffset5, Top = startY + rowOffset5 + 5, Width = 20 };
            Button btn5 = new Button { Text = "SET", Left = 240 + colOffset5, Top = startY + rowOffset5 };
            btn5.Click += Submitbtnplayer5;

            int rowOffset6 = (5 % 4) * 30;
            int colOffset6 = 5 < 4 ? 0 : 400;
            chk6 = new CheckBox { Text = $"Player6", Left = 10 + colOffset6, Top = startY + rowOffset6, Width = 62 };
            txtbox6 = new TextBox { Text = "3 dB", Left = 90 + colOffset6, Top = startY + rowOffset6, Width = 80 };
            Label lblDb6 = new Label { Text = "dB", Left = 175 + colOffset6, Top = startY + rowOffset6 + 5, Width = 20 };
            Button btn6 = new Button { Text = "SET", Left = 240 + colOffset6, Top = startY + rowOffset6 };
            btn6.Click += Submitbtnplayer6;

            int rowOffset7 = (6 % 4) * 30;
            int colOffset7 = 6 < 4 ? 0 : 400;
            chk7 = new CheckBox { Text = $"Player7", Left = 10 + colOffset7, Top = startY + rowOffset7, Width = 62 };
            txtbox7 = new TextBox { Text = "3 dB", Left = 90 + colOffset7, Top = startY + rowOffset7, Width = 80 };
            Label lblDb7 = new Label { Text = "dB", Left = 175 + colOffset7, Top = startY + rowOffset7 + 5, Width = 20 };
            Button btn7 = new Button { Text = "SET", Left = 240 + colOffset7, Top = startY + rowOffset7 };
            btn7.Click += Submitbtnplayer7;

            int rowOffset8 = (7 % 4) * 30;
            int colOffset8 = 7 < 4 ? 0 : 400;
            chk8 = new CheckBox { Text = $"Player8", Left = 10 + colOffset8, Top = startY + rowOffset8, Width = 62 };
            txtbox8 = new TextBox { Text = "3 dB", Left = 90 + colOffset8, Top = startY + rowOffset8, Width = 80 };
            Label lblDb8 = new Label { Text = "dB", Left = 175 + colOffset8, Top = startY + rowOffset8 + 5, Width = 20 };
            Button btn8 = new Button { Text = "SET", Left = 240 + colOffset8, Top = startY + rowOffset8 };
            btn8.Click += Submitbtnplayer8;

            panelWhiteArea.Controls.AddRange(new Control[] { 
                chk, txtbox, lblDb, btn,
                chk2, txtbox2, lblDb2, btn2,
                chk3, txtbox3, lblDb3, btn3,
                chk4, txtbox4, lblDb4, btn4,
                chk5, txtbox5, lblDb5, btn5,
                chk6, txtbox6, lblDb6, btn6,
                chk7, txtbox7, lblDb7, btn7,
                chk8, txtbox8, lblDb8, btn8
            });

            // เพิ่ม panelWhiteArea ลงใน panelIPGain
            panelIPGain.Controls.Add(panelWhiteArea);
            #endregion
            #endregion

            Panel panelLogs = new Panel { Left = 20, Top = 220, Width = 790, Height = 250, BorderStyle = BorderStyle.FixedSingle, BackColor = Color.LightGray, Visible = false };
            RichTextBox rtbLogs = new RichTextBox { Left = 10, Top = 10, Width = 770, Height = 230, BackColor = Color.White, BorderStyle = BorderStyle.None, ReadOnly = true };
            rtbLogs.Clear(); // ล้างข้อความก่อน
            var reads = files.Read();
            string[] stringSeparators = new string[] { "\r\n" };
            var linewrite = reads.ToString().Split(stringSeparators, StringSplitOptions.None);
            if (linewrite.Length > 0)
            {
                foreach (var msg in linewrite)
                {
                    //foreach (string log in systemLogs)
                    //{
                        rtbLogs.AppendText(msg + "\n");
                    //}
                }
            }
            panelLogs.Controls.Add(rtbLogs);

            Button btnRoute = new Button { Text = "Route", Left = 20, Top = 490, Width = 90, Height = 30 };
            Button btnIPGain = new Button { Text = "IP and Gain", Left = 120, Top = 490, Width = 110, Height = 30 };
            Button btnLogs = new Button { Text = "Logs", Left = 250, Top = 490, Width = 90, Height = 30 };

            btnRoute.Click += (s, e) => { dgvPlayers2.Visible = true; panelIPGain.Visible = false; panelLogs.Visible = false; };
            btnIPGain.Click += (s, e) => { dgvPlayers2.Visible = false; panelIPGain.Visible = true; panelLogs.Visible = false; };
            btnLogs.Click += (s, e) => { dgvPlayers2.Visible = false; panelIPGain.Visible = false; panelLogs.Visible = true; };
            #endregion

            Button btnClose = new Button { Text = "Close", Left = 700, Top = 495, Width = 90, Height = 30 };
            btnClose.Click += (sender, e) => { prompt.Close(); };

            // Playback engine
            IWavePlayer wavePlayer = null;
            AudioFileReader reader = null;

            void LoadAudio(string path)
            {
                wavePlayer?.Stop();
                wavePlayer?.Dispose();
                reader?.Dispose();

                if (!File.Exists(path)) return;

                reader = new AudioFileReader(path);
                var sampleChannel = new SampleChannel(reader, true);
                var metering = new MeteringSampleProvider(sampleChannel);

                metering.StreamVolume += (s, e) =>
                {
                    float levelLeft = e.MaxSampleValues[0];   // ซ้าย 
                    float levelRight = e.MaxSampleValues[1];  // ขวา

                    if (prompt != null && prompt.IsHandleCreated && !prompt.IsDisposed)
                    {
                        prompt.BeginInvoke(new Action(() =>
                        {
                            level = levelLeft;
                            pnlWaveform.Invalidate();
                            level2 = levelRight;
                            pnlWaveform2.Invalidate();

                            float avg = (levelLeft + levelRight) / 2f;
                            waveformQueue.Enqueue(avg);
                            while (waveformQueue.Count > maxWaveformPoints)
                                waveformQueue.Dequeue();

                            pnlWaveformGraph.Invalidate();
                        }));
                    }
                };

                wavePlayer = new WaveOutEvent();
                wavePlayer.Init(metering);
                // ยังไม่ Play ทันที
            }

            Label namefile = new Label { Text = "", Left = 590, Top = 134, Width = 180 };
            namefile.Font = new Font("Segoe UI", 5, FontStyle.Regular);

            btnOpen.Click += (s, e) =>
            {
                var ofd = new System.Windows.Forms.OpenFileDialog
                {
                    Filter = "Audio Files (*.wav;*.mp3)|*.wav;*.mp3|All files (*.*)|*.*",
                    Title = "Select audio file"
                };
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Registry.SetValue("HKEY_CURRENT_USER\\Software\\AudioPlayback", "LastAudioPath", ofd.FileName);
                    LoadAudio(ofd.FileName);

                    GenerateWaveformBitmap(ofd.FileName);
                    //pnlWaveformGraph.Invalidate();

                    // ✅ แสดงชื่อไฟล์บนหน้าจอ               
                    namefile.Text = Path.GetFileName(ofd.FileName);
                    // ✅ เพิ่มบรรทัดนี้ให้เสียงเล่นทันที
                    wavePlayer?.Play();
                }
            };

            btnPlay.Click += (s, e) =>
            {
                // ✅ เพิ่มบรรทัดนี้ให้เสียงเล่นทันที
                wavePlayer?.Play();
            };

            btnPause.Click += (s, e) =>
            {
                // ✅ เพิ่มบรรทัดนี้ให้เสียงเล่นทันที
                wavePlayer?.Pause();
            };

            btnStop.Click += (s, e) =>
            {
                // ✅ เพิ่มบรรทัดนี้ให้เสียงเล่นทันที
                wavePlayer?.Stop();
                reader.Position = 0;
            };

            string lastPath = (string)Registry.GetValue("HKEY_CURRENT_USER\\Software\\AudioPlayback", "LastAudioPath", null);
            if (!string.IsNullOrEmpty(lastPath) && File.Exists(lastPath))
            {
                LoadAudio(lastPath); // โหลดไว้ แต่ยังไม่เล่น
            }

            prompt.Controls.AddRange(new Control[] {
                btnOpen, btnPlay, btnPause, btnStop,
                lblCurrentTime, lblTotalTime,
                lblOutputDriver, cbOutputDriver,
                lblDriver, cbOutputDeviceName,
                chkEventCallback, chkExclusive,
                lblPlayer, cbPlayer, btnApply, chkOnline,
                lblOutputRequestedLatency, cbOutputRequestedLatency,
                lblVolume, tbVolume,
                lblCurrentFile, namefile,
                lblPlaybackFormat, pnlWaveform, pnlWaveform2, pnlWaveformGraph,
                dgvPlayers2, panelIPGain, panelLogs,
                btnRoute, btnIPGain, btnLogs,
                btnClose
            });

            // แสดงค่าที่เก็บไว้
            RegistryKey configsocket = this.HKLMSoftwareTOAConfig.OpenSubKey("trackname", true);
            if (configsocket.GetValue("ip") == null)
            {
                configsocket.SetValue("ip", "0.0.0.0");
                configsocket.SetValue("port", "8003");
                txtWebIP.Text = configsocket.GetValue("ip").ToString();
                txtIP.Text = configsocket.GetValue("ip1").ToString();
                cbPort.Text = configsocket.GetValue("port").ToString();
            }else
            {
                txtWebIP.Text = configsocket.GetValue("ip").ToString();
                txtIP.Text = configsocket.GetValue("ip1").ToString();
                cbPort.Text = configsocket.GetValue("port").ToString();
            }

            prompt.ShowDialog();
        }

        private Bitmap waveformBitmap;
        private Panel pnlWaveformGraph;

        void GenerateWaveformBitmap(string filePath)
        {
            using (var reader = new AudioFileReader(filePath))
            {
                int samplesToRead = (int)(reader.Length / 4);
                var samples = new float[samplesToRead];
                int read = reader.Read(samples, 0, samplesToRead);              
            }
        }

        public void SubmitApply(object sender, EventArgs e)
        {
            // ดึงค่า Driver จากตัวเลือก
            string selectedPluginName = cbOutputDriver.SelectedItem?.ToString() ?? "WASAPI";

            // ดึงค่า latency
            int latency = int.TryParse(cbOutputRequestedLatency.SelectedItem?.ToString(), out int lat) ? lat : 100;

            // ดึงค่าระดับเสียง (volume)
            float volume = tbVolume.Value / 100f;

            // ดึง Player เช่น Player1, Player2...
            string playerName = cbPlayer.SelectedItem?.ToString();

            // ดึง Output Device จาก Panel
            string DeviceName = cbOutputDeviceName.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(playerName) && HKLMSoftwareTOAPlayer != null)
            {
                DeleteSubKeyTree(playerName, selectedPluginName);

                RegistryKey cHKLMPlayer = HKLMSoftwareTOAPlayer.CreateSubKey(playerName);
                cHKLMPlayer.SetValue("DriverName", selectedPluginName, RegistryValueKind.String);
                cHKLMPlayer.SetValue("Latency", latency, RegistryValueKind.DWord);
                cHKLMPlayer.SetValue("Volume", volume.ToString("0.00"), RegistryValueKind.String);
                cHKLMPlayer.SetValue("DeviceName", DeviceName, RegistryValueKind.String);

                MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
                var allDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
                // หา device ที่ชื่อเหมือนกัน
                var selectedDevice = allDevices.FirstOrDefault(d => d.FriendlyName == DeviceName);
                if (selectedDevice != null)
                {
                    cHKLMPlayer.SetValue("OutputID", selectedDevice.ID, RegistryValueKind.String);
                }
                else
                {
                    cHKLMPlayer.SetValue("OutputID", Guid.NewGuid().ToString(), RegistryValueKind.String);
                }
                cHKLMPlayer.SetValue("Priority", 1, RegistryValueKind.DWord);
                cHKLMPlayer.SetValue("IsAvailable", true.ToString(), RegistryValueKind.String);
                cHKLMPlayer.Close();
            }

            dgvPlayers2.Rows.Clear(); // ล้างแถวเก่า

            var registryKeys = new[]
            {
                HKLMSoftwareTOAPlayer11,
                HKLMSoftwareTOAPlayer12,
                HKLMSoftwareTOAPlayer13,
                HKLMSoftwareTOAPlayer14,
                HKLMSoftwareTOAPlayer15,
                HKLMSoftwareTOAPlayer16,
                HKLMSoftwareTOAPlayer17,
                HKLMSoftwareTOAPlayer18
            };

            for (int i = 0; i < registryKeys.Length; i++)
            {
                string deviceName = registryKeys[i].GetValue("DeviceName")?.ToString() ?? "รอตั้งค่า";
                dgvPlayers2.Rows.Add($"Player {i + 1}", deviceName);
            }
        }

        public void SubmitWebSetIP(object sender, EventArgs e)
        {
            RegistryKey configsocket = this.HKLMSoftwareTOAConfig.OpenSubKey("trackname", true);
            configsocket.SetValue("ip", txtWebIP.Text);
            api_run();
            
            var message = new Helper.MessageBox();
            message.ShowCenter_DialogError("บันทึก IP Addess Web เรียบร้อยแล้ว", "แจ้งเตือน");
        }

        public void SubmitSetIP(object sender, EventArgs e)
        {
            RegistryKey configsocket = this.HKLMSoftwareTOAConfig.OpenSubKey("trackname", true);
            configsocket.SetValue("ip1", txtIP.Text);
            api_run();

            var message = new Helper.MessageBox();
            message.ShowCenter_DialogError("บันทึก IP Addess Server เรียบร้อยแล้ว", "แจ้งเตือน");
        }

        public void SubmitSetPort(object sender, EventArgs e)
        {
            RegistryKey configsocket = this.HKLMSoftwareTOAConfig.OpenSubKey("trackname", true);
            configsocket.SetValue("port", cbPort.Text);
            api_run();

            var message = new Helper.MessageBox();
            message.ShowCenter_DialogError("บันทึก Port เรียบร้อยแล้ว", "แจ้งเตือน");
        }

        public void Submitbtnplayer(object sender, EventArgs e)
        {
            var txt = "";
            int value;
            var messagebox = new Helper.MessageBox();
            if (int.TryParse(txtbox.Text, out value))
            {
                RegistryKey configsocket = this.HKLMSoftwareTOAConfig.OpenSubKey("trackname", true);
                int val = Convert.ToInt32(txtbox.Text);
                if (val >= 93)
                {
                    txt = "ค่าของ dB มากเกินไป ตัวเลขสูงสุด คือ 93";
                    messagebox.ShowCenter_DialogError(txt, "Error");
                }
                else if (val <= 50)
                {
                    txt = "ค่าของ dB น้อยเกินไป ตัวเลขขั้นต่ำ คือ 50";
                    messagebox.ShowCenter_DialogError(txt, "Error");
                }
                else
                {
                    if (configsocket.GetValue("adB1").ToString() == "active")
                    {
                        chk.Checked = true;
                    }
                    else
                    {
                        chk.Checked = false;
                    }
                    configsocket.SetValue("dB1", txtbox.Text == "" ? "50" : txtbox.Text);
                    configsocket.SetValue("adB1", chk.Checked == true ? "active" : "unactive");
                }
            }
            else
            {
                messagebox.ShowCenter_DialogError($"กรุณาป้อนตัวเลข เท่านั้น", "Error");
            }
        }

        public void Submitbtnplayer2(object sender, EventArgs e)
        {
            var txt = "";
            int value;
            var messagebox = new Helper.MessageBox();
            if (int.TryParse(txtbox2.Text, out value))
            {
                RegistryKey configsocket = this.HKLMSoftwareTOAConfig.OpenSubKey("trackname", true);
                int val = Convert.ToInt32(txtbox2.Text);
                if (val >= 93)
                {
                    txt = "ค่าของ dB มากเกินไป ตัวเลขสูงสุด คือ 93";
                    messagebox.ShowCenter_DialogError(txt, "Error");
                }
                else if (val <= 50)
                {
                    txt = "ค่าของ dB น้อยเกินไป ตัวเลขขั้นต่ำ คือ 50";
                    messagebox.ShowCenter_DialogError(txt, "Error");
                }
                else
                {
                    if (configsocket.GetValue("adB1").ToString() == "active")
                    {
                        chk2.Checked = true;
                    }
                    else
                    {
                        chk2.Checked = false;
                    }
                    configsocket.SetValue("dB1", txtbox2.Text == "" ? "50" : txtbox.Text);
                    configsocket.SetValue("adB1", chk2.Checked == true ? "active" : "unactive");
                }
            }
            else
            {
                messagebox.ShowCenter_DialogError($"กรุณาป้อนตัวเลข เท่านั้น", "Error");
            }
        }

        public void Submitbtnplayer3(object sender, EventArgs e)
        {
            var txt = "";
            int value;
            var messagebox = new Helper.MessageBox();
            if (int.TryParse(txtbox3.Text, out value))
            {
                RegistryKey configsocket = this.HKLMSoftwareTOAConfig.OpenSubKey("trackname", true);
                int val = Convert.ToInt32(txtbox3.Text);
                if (val >= 93)
                {
                    txt = "ค่าของ dB มากเกินไป ตัวเลขสูงสุด คือ 93";
                    messagebox.ShowCenter_DialogError(txt, "Error");
                }
                else if (val <= 50)
                {
                    txt = "ค่าของ dB น้อยเกินไป ตัวเลขขั้นต่ำ คือ 50";
                    messagebox.ShowCenter_DialogError(txt, "Error");
                }
                else
                {
                    if (configsocket.GetValue("adB1").ToString() == "active")
                    {
                        chk3.Checked = true;
                    }
                    else
                    {
                        chk3.Checked = false;
                    }
                    configsocket.SetValue("dB1", txtbox3.Text == "" ? "50" : txtbox.Text);
                    configsocket.SetValue("adB1", chk3.Checked == true ? "active" : "unactive");
                }
            }
            else
            {
                messagebox.ShowCenter_DialogError($"กรุณาป้อนตัวเลข เท่านั้น", "Error");
            }
        }

        public void Submitbtnplayer4(object sender, EventArgs e)
        {
            var txt = "";
            int value;
            var messagebox = new Helper.MessageBox();
            if (int.TryParse(txtbox4.Text, out value))
            {
                RegistryKey configsocket = this.HKLMSoftwareTOAConfig.OpenSubKey("trackname", true);
                int val = Convert.ToInt32(txtbox4.Text);
                if (val >= 93)
                {
                    txt = "ค่าของ dB มากเกินไป ตัวเลขสูงสุด คือ 93";
                    messagebox.ShowCenter_DialogError(txt, "Error");
                }
                else if (val <= 50)
                {
                    txt = "ค่าของ dB น้อยเกินไป ตัวเลขขั้นต่ำ คือ 50";
                    messagebox.ShowCenter_DialogError(txt, "Error");
                }
                else
                {
                    if (configsocket.GetValue("adB1").ToString() == "active")
                    {
                        chk4.Checked = true;
                    }
                    else
                    {
                        chk4.Checked = false;
                    }
                    configsocket.SetValue("dB1", txtbox4.Text == "" ? "50" : txtbox.Text);
                    configsocket.SetValue("adB1", chk4.Checked == true ? "active" : "unactive");
                }
            }
            else
            {
                messagebox.ShowCenter_DialogError($"กรุณาป้อนตัวเลข เท่านั้น", "Error");
            }
        }

        public void Submitbtnplayer5(object sender, EventArgs e)
        {
            var txt = "";
            int value;
            var messagebox = new Helper.MessageBox();
            if (int.TryParse(txtbox5.Text, out value))
            {
                RegistryKey configsocket = this.HKLMSoftwareTOAConfig.OpenSubKey("trackname", true);
                int val = Convert.ToInt32(txtbox5.Text);
                if (val >= 93)
                {
                    txt = "ค่าของ dB มากเกินไป ตัวเลขสูงสุด คือ 93";
                    messagebox.ShowCenter_DialogError(txt, "Error");
                }
                else if (val <= 50)
                {
                    txt = "ค่าของ dB น้อยเกินไป ตัวเลขขั้นต่ำ คือ 50";
                    messagebox.ShowCenter_DialogError(txt, "Error");
                }
                else
                {
                    if (configsocket.GetValue("adB1").ToString() == "active")
                    {
                        chk5.Checked = true;
                    }
                    else
                    {
                        chk5.Checked = false;
                    }
                    configsocket.SetValue("dB1", txtbox5.Text == "" ? "50" : txtbox.Text);
                    configsocket.SetValue("adB1", chk5.Checked == true ? "active" : "unactive");
                }
            }
            else
            {
                messagebox.ShowCenter_DialogError($"กรุณาป้อนตัวเลข เท่านั้น", "Error");
            }
        }

        public void Submitbtnplayer6(object sender, EventArgs e)
        {
            var txt = "";
            int value;
            var messagebox = new Helper.MessageBox();
            if (int.TryParse(txtbox6.Text, out value))
            {
                RegistryKey configsocket = this.HKLMSoftwareTOAConfig.OpenSubKey("trackname", true);
                int val = Convert.ToInt32(txtbox6.Text);
                if (val >= 93)
                {
                    txt = "ค่าของ dB มากเกินไป ตัวเลขสูงสุด คือ 93";
                    messagebox.ShowCenter_DialogError(txt, "Error");
                }
                else if (val <= 50)
                {
                    txt = "ค่าของ dB น้อยเกินไป ตัวเลขขั้นต่ำ คือ 50";
                    messagebox.ShowCenter_DialogError(txt, "Error");
                }
                else
                {
                    if (configsocket.GetValue("adB1").ToString() == "active")
                    {
                        chk6.Checked = true;
                    }
                    else
                    {
                        chk6.Checked = false;
                    }
                    configsocket.SetValue("dB1", txtbox6.Text == "" ? "50" : txtbox.Text);
                    configsocket.SetValue("adB1", chk6.Checked == true ? "active" : "unactive");
                }
            }
            else
            {
                messagebox.ShowCenter_DialogError($"กรุณาป้อนตัวเลข เท่านั้น", "Error");
            }
        }

        public void Submitbtnplayer7(object sender, EventArgs e)
        {
            var txt = "";
            int value;
            var messagebox = new Helper.MessageBox();
            if (int.TryParse(txtbox7.Text, out value))
            {
                RegistryKey configsocket = this.HKLMSoftwareTOAConfig.OpenSubKey("trackname", true);
                int val = Convert.ToInt32(txtbox7.Text);
                if (val >= 93)
                {
                    txt = "ค่าของ dB มากเกินไป ตัวเลขสูงสุด คือ 93";
                    messagebox.ShowCenter_DialogError(txt, "Error");
                }
                else if (val <= 50)
                {
                    txt = "ค่าของ dB น้อยเกินไป ตัวเลขขั้นต่ำ คือ 50";
                    messagebox.ShowCenter_DialogError(txt, "Error");
                }
                else
                {
                    if (configsocket.GetValue("adB1").ToString() == "active")
                    {
                        chk7.Checked = true;
                    }
                    else
                    {
                        chk7.Checked = false;
                    }
                    configsocket.SetValue("dB1", txtbox7.Text == "" ? "50" : txtbox.Text);
                    configsocket.SetValue("adB1", chk7.Checked == true ? "active" : "unactive");
                }
            }
            else
            {
                messagebox.ShowCenter_DialogError($"กรุณาป้อนตัวเลข เท่านั้น", "Error");
            }
        }

        public void Submitbtnplayer8(object sender, EventArgs e)
        {
            var txt = "";
            int value;
            var messagebox = new Helper.MessageBox();
            if (int.TryParse(txtbox8.Text, out value))
            {
                RegistryKey configsocket = this.HKLMSoftwareTOAConfig.OpenSubKey("trackname", true);
                int val = Convert.ToInt32(txtbox8.Text);
                if (val >= 93)
                {
                    txt = "ค่าของ dB มากเกินไป ตัวเลขสูงสุด คือ 93";
                    messagebox.ShowCenter_DialogError(txt, "Error");
                }
                else if (val <= 50)
                {
                    txt = "ค่าของ dB น้อยเกินไป ตัวเลขขั้นต่ำ คือ 50";
                    messagebox.ShowCenter_DialogError(txt, "Error");
                }
                else
                {
                    if (configsocket.GetValue("adB1").ToString() == "active")
                    {
                        chk8.Checked = true;
                    }
                    else
                    {
                        chk8.Checked = false;
                    }
                    configsocket.SetValue("dB1", txtbox8.Text == "" ? "50" : txtbox.Text);
                    configsocket.SetValue("adB1", chk8.Checked == true ? "active" : "unactive");
                }
            }
            else
            {
                messagebox.ShowCenter_DialogError($"กรุณาป้อนตัวเลข เท่านั้น", "Error");
            }
        }

        // สร้างฟังก์ชัน DeleteSubKeyTree
        private void DeleteSubKeyTree(string player, string plugin)
        {
            string path = $@"Software\TOAPlayer\{player}";
            try
            {
                Registry.LocalMachine.DeleteSubKeyTree(path, false); // false = ไม่ throw error ถ้าไม่มี key
            }
            catch { }
        }

        public void api_run()
        {
            Task.Run(() =>
            {
                Process p = new Process();
                p.StartInfo.FileName = "currport.bat";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.Start();

                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();

                // อัปเดต UI ต้อง Invoke
                if (this.IsHandleCreated && !this.IsDisposed)
                {
                    this.BeginInvoke((MethodInvoker)(() =>
                    {
                        label1.Text = output.Contains("LISTENING") ? "เปิดอยู่" : "ไม่มี";
                    }));
                }
            });
        }

    }
}
