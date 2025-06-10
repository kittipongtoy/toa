using Microsoft.Win32;
using NAudio.CoreAudioApi;
using NAudio.Gui;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using TOAMediaPlayer.NAudioOutput;

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
        private RegistryKey HKLMSoftwareTOAPlayer1 = Registry.CurrentUser.OpenSubKey(@"Software\TOA\Config", true);

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

        public void ShowSetting()
        {
            //var listView1 = new System.Windows.Forms.ListView();

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
            ComboBox cbOutputDriver = new ComboBox { Left = 130, Top = 45, Width = 150 };
            cbOutputDriver.Items.AddRange(new[] { "WasapiOut" });
            cbOutputDriver.SelectedIndex = 0;

            Label lblDriver = new Label { Text = "Driver", Left = 20, Top = 90, Width = 100 };
            ComboBox cbDriver = new ComboBox { Left = 130, Top = 85, Width = 200 };
            for (int i = 0; i < WaveOut.DeviceCount; i++)
            {
                var deviceInfo = WaveOut.GetCapabilities(i);
                cbDriver.Items.Add(deviceInfo.ProductName);
            }
            if (cbDriver.Items.Count > 0)
                cbDriver.SelectedIndex = 0;

            CheckBox chkEventCallback = new CheckBox { Text = "Event Callback", Left = 20, Top = 130 };
            CheckBox chkExclusive = new CheckBox { Text = "Exclusive Mode", Left = 150, Top = 130 };

            Label lblPlayer = new Label { Text = "Player", Left = 20, Top = 170 };
            ComboBox cbPlayer = new ComboBox { Left = 130, Top = 165, Width = 200 };
            cbPlayer.Items.AddRange(new[] { "nPlayer1", "nPlayer2", "nPlayer3", "nPlayer4", "nPlayer5", "nPlayer6", "nPlayer7", "nPlayer8" });
            cbPlayer.SelectedItem = "nPlayer1";

            Button btnApply = new Button { Text = "Apply", Left = 350, Top = 165 };
            CheckBox chkOnline = new CheckBox { Text = "Online", Left = 470, Top = 170, Width = 60 };

            btnApply.Click += SubmitApply;
            #endregion

            #region ขวา
            Label lblOutputRequestedLatency = new Label { Text = "Requested Latency", Left = 450, Top = 50, Width = 100 };
            ComboBox cbOutputRequestedLatency = new ComboBox { Left = 620, Top = 45, Width = 150 };
            cbOutputRequestedLatency.Items.AddRange(new[] { "25", "50", "100", "150", "200", "300", "400", "500" });
            cbOutputRequestedLatency.SelectedIndex = 7;

            Label lblVolume = new Label { Text = "Volume", Left = 450, Top = 85 };
            TrackBar tbVolume = new TrackBar
            {
                Left = 650,
                Top = 80,
                Width = 150,
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

            Label lblCurrentFile = new Label { Text = "Current File:", Left = 450, Top = 85 };
            TextBox txtCurrentFile = new TextBox { Left = 650, Top = 80, Width = 150 };

            Label lblPlaybackFormat = new Label { Text = "Playback Format:", Left = 450, Top = 120 };

            Panel pnlWaveformGraph = new Panel
            {
                Left = 540,
                Top = 140,
                Width = 350,
                Height = 60,
                BackColor = Color.Black,
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

            Panel pnlWaveform = new Panel
            {
                Left = 800,
                Top = 115,
                Width = 10,
                Height = 80, // สูงขึ้นกว่าเดิม
                BackColor = Color.Black
            };

            float level = 0;
            pnlWaveform.Paint += (sender, pe) =>
            {
                pe.Graphics.Clear(Color.Black);

                int barHeight = (int)(pnlWaveform.Height * level);
                int y = pnlWaveform.Height - barHeight; // ยิ่งเสียงดัง ยิ่งสูงขึ้นจากด้านล่าง

                pe.Graphics.FillRectangle(Brushes.Lime, 0, y, pnlWaveform.Width, barHeight);
            };

            Timer timer = new Timer { Interval = 50 };
            timer.Tick += (s, e) =>
            {
                level = device.AudioMeterInformation.MasterPeakValue;
                pnlWaveform.Invalidate();
            };
            timer.Start();
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
            DataGridView dgvPlayers = new DataGridView
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
            dgvPlayers.Columns[0].Name = "Player";
            dgvPlayers.Columns[1].Name = "Driver";

            // วนลูปเพิ่มข้อมูลลง DataGridView
            for (int i = 0; i < registryKeys.Length; i++)
            {
                string deviceName = registryKeys[i].GetValue("DeviceName")?.ToString() ?? "รอตั้งค่า";
                dgvPlayers.Rows.Add($"Player {i + 1}", deviceName);
            }

            #region btnIPGain
            Panel panelIPGain = new Panel { Left = 20, Top = 220, Width = 790, Height = 250, BorderStyle = BorderStyle.FixedSingle, BackColor = Color.LightGray, Visible = false };

            // IP
            Label lblIP = new Label { Text = "Server IP", Left = 40, Top = 20, Width = 50 };
            TextBox txtIP = new TextBox { Left = 120, Top = 15, Width = 150 };
            Button btnSetIP = new Button { Text = "SET", Left = 280, Top = 15 };

            // Port
            Label lblPort = new Label { Text = "Port", Left = 380, Top = 20, Width = 50 };
            TextBox cbPort = new TextBox { Left = 440, Top = 15, Width = 80 };
            Button btnSetPort = new Button { Text = "SET", Left = 540, Top = 15 };

            // Label Title
            panelIPGain.Controls.AddRange(new Control[] {
                 lblIP, txtIP, btnSetIP,
                 lblPort, cbPort, btnSetPort,
            });

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
            for (int i = 0; i < 8; i++)
            {
                int rowOffset = (i % 4) * 30;
                int colOffset = i < 4 ? 0 : 400;

                CheckBox chk = new CheckBox { Text = $"Player{i + 1}", Left = 10 + colOffset, Top = startY + rowOffset };
                TextBox txt = new TextBox { Text = "3 dB", Left = 90 + colOffset, Top = startY + rowOffset, Width = 80 };
                Label lblDb = new Label { Text = "dB", Left = 175 + colOffset, Top = startY + rowOffset + 5, Width = 20 };
                Button btn = new Button { Text = "SET", Left = 240 + colOffset, Top = startY + rowOffset };

                panelWhiteArea.Controls.AddRange(new Control[] { chk, txt, lblDb, btn });
            }

            // เพิ่ม panelWhiteArea ลงใน panelIPGain
            panelIPGain.Controls.Add(panelWhiteArea);

            #endregion

            Panel panelLogs = new Panel { Left = 20, Top = 220, Width = 790, Height = 250, BorderStyle = BorderStyle.FixedSingle, BackColor = Color.LightGray, Visible = false };
            RichTextBox rtbLogs = new RichTextBox { Left = 10, Top = 10, Width = 770, Height = 230, BackColor = Color.White, BorderStyle = BorderStyle.None, ReadOnly = true };
            rtbLogs.Clear(); // ล้างข้อความก่อน

            foreach (string log in systemLogs)
            {
                rtbLogs.AppendText(log + "\n");
            }
            panelLogs.Controls.Add(rtbLogs);

            Button btnRoute = new Button { Text = "Route", Left = 20, Top = 490, Width = 90, Height = 30 };
            Button btnIPGain = new Button { Text = "IP and Gain", Left = 120, Top = 490, Width = 110, Height = 30 };
            Button btnLogs = new Button { Text = "Logs", Left = 250, Top = 490, Width = 90, Height = 30 };

            btnRoute.Click += (s, e) => { dgvPlayers.Visible = true; panelIPGain.Visible = false; panelLogs.Visible = false; };
            btnIPGain.Click += (s, e) => { dgvPlayers.Visible = false; panelIPGain.Visible = true; panelLogs.Visible = false; };
            btnLogs.Click += (s, e) => { dgvPlayers.Visible = false; panelIPGain.Visible = false; panelLogs.Visible = true; };
            #endregion

            Button btnClose = new Button { Text = "Close", Left = 700, Top = 500 };
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
                    float maxLevel = Math.Max(e.MaxSampleValues[0], e.MaxSampleValues[1]);

                    prompt.BeginInvoke(new Action(() =>
                    {
                        level = maxLevel;
                        pnlWaveform.Invalidate();

                        // ✅ ส่วนที่คุณ "ลืม" ใส่:
                        waveformQueue.Enqueue(maxLevel);
                        while (waveformQueue.Count > maxWaveformPoints)
                            waveformQueue.Dequeue();

                        pnlWaveformGraph.Invalidate();
                    }));
                };

                wavePlayer = new WaveOutEvent();
                wavePlayer.Init(metering);
                // ยังไม่ Play ทันที
            }


            //void LoadAudio(string path)
            //{
            //    wavePlayer?.Stop();
            //    wavePlayer?.Dispose();
            //    reader?.Dispose();

            //    if (!File.Exists(path)) return;

            //    reader = new AudioFileReader(path);
            //    var sampleChannel = new SampleChannel(reader, true);
            //    var metering = new MeteringSampleProvider(sampleChannel);
            //    metering.StreamVolume += (s, e) =>
            //    {
            //        prompt.BeginInvoke(new Action(() =>
            //        {
            //            level = Math.Max(e.MaxSampleValues[0], e.MaxSampleValues[1]);
            //            pnlWaveform.Invalidate();
            //        }));
            //    };

            //    wavePlayer = new WaveOutEvent();
            //    wavePlayer.Init(metering);
            //    // ยังไม่ Play ทันที
            //}


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

                    //GenerateWaveformBitmap(ofd.FileName);
                    pnlWaveformGraph.Invalidate();
                }
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
                lblDriver, cbDriver,
                chkEventCallback, chkExclusive,
                lblPlayer, cbPlayer, btnApply, chkOnline,
                lblOutputRequestedLatency, cbOutputRequestedLatency,
                lblVolume, tbVolume,
                lblCurrentFile, txtCurrentFile,
                lblPlaybackFormat, pnlWaveform, //pnlWaveformGraph,
                dgvPlayers, panelIPGain, panelLogs,
                btnRoute, btnIPGain, btnLogs,
                btnClose
            });

            prompt.ShowDialog();
        }

        



        //private Bitmap waveformBitmap;
        //private Panel pnlWaveformGraph;

        //void GenerateWaveformBitmap(string filePath)
        //{
        //    using (var reader = new AudioFileReader(filePath)) // แก้ตรงนี้
        //    {
        //        int samplesToRead = (int)(reader.Length / 4);
        //        var samples = new float[samplesToRead];
        //        int read = reader.Read(samples, 0, samplesToRead);

        //        int width = pnlWaveformGraph.Width;
        //        int height = pnlWaveformGraph.Height;
        //        waveformBitmap = new Bitmap(width, height);

        //        using (var g = Graphics.FromImage(waveformBitmap))
        //        {
        //            g.Clear(Color.Black);
        //            Pen pen = Pens.Lime;

        //            float midY = height / 2f;
        //            int step = Math.Max(read / width, 1);

        //            PointF[] points = new PointF[width];
        //            for (int x = 0; x < width; x++)
        //            {
        //                int start = x * step;
        //                int end = Math.Min(start + step, read);

        //                float avg = 0;
        //                for (int i = start; i < end; i++) avg += samples[i];
        //                avg /= (end - start);

        //                float y = midY - (avg * midY);
        //                points[x] = new PointF(x, y);
        //            }

        //            g.DrawLines(pen, points);
        //        }
        //    }
        //}


        public void SubmitApply(object sender, EventArgs e)
        { 
        
        }
    }
}
