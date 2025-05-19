using System;
using System.Windows.Forms;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace TOAMediaPlayer
{
    public partial class LoginAdmin : Form
    {
        private MainPlayer player;
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
                    SetupOutput xForm = new SetupOutput(player);
                    xForm.StartPosition = FormStartPosition.CenterParent;
                    xForm.ShowDialog();

                    //ShowSetting();
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
            if(e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }

        public void ShowSetting()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

            Form prompt = new Form()
            {
                Width = 850,
                Height = 600,
                Text = "Config",
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            // Top control buttons
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
            CheckBox chkOnline = new CheckBox { Text = "Online", Left = 430, Top = 170 };
            #endregion

            #region ขวา
            Label lblOutputRequestedLatency = new Label { Text = "Requested Latency", Left = 450, Top = 50, Width = 100 };
            ComboBox cbOutputRequestedLatency = new ComboBox { Left = 620, Top = 45, Width = 150 };
            cbOutputRequestedLatency.Items.AddRange(new[] { "25", "50", "100", "150", "200", "300", "400", "500" });
            cbOutputRequestedLatency.SelectedIndex = 0;

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
            TextBox txtPlaybackFormat = new TextBox { Left = 650, Top = 115, Width = 150 };
            #endregion

            #region
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
                Visible = false // ซ่อนไว้ก่อน
            };

            dgvPlayers.Columns[0].Name = "Player";
            dgvPlayers.Columns[1].Name = "Driver";
            for (int i = 1; i <= 8; i++)
            {
                dgvPlayers.Rows.Add($"Player {i}", "DVS Transmit 1-2 (Dante Virtual Soundcard)");
            }
            #endregion


            Button btnRoute = new Button { Text = "Route", Left = 20, Top = 490 };
            Button btnIPGain = new Button { Text = "IP and Gain", Left = 120, Top = 490 };
            Button btnLogs = new Button { Text = "Logs", Left = 250, Top = 490 };

            // ปุ่ม Route: แสดงตาราง
            btnRoute.Click += (s, e) =>
            {
                dgvPlayers.Visible = true;
            };

            // ปุ่มอื่น: ซ่อนตาราง
            btnIPGain.Click += (s, e) =>
            {
                dgvPlayers.Visible = false;
            };
            btnLogs.Click += (s, e) =>
            {
                dgvPlayers.Visible = false;
            };

            Button btnClose = new Button { Text = "Close", Left = 700, Top = 500 };
            btnClose.Click += (sender, e) => { prompt.Close(); };

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
                lblPlaybackFormat, txtPlaybackFormat,
                dgvPlayers,
                btnRoute, btnIPGain, btnLogs,
                btnClose
            });

            prompt.ShowDialog();
        }
    }
}
