using Microsoft.Win32;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
//using System.Windows.Controls;
using System.Windows.Forms;
using TOAMediaPlayer.Utils;

namespace TOAMediaPlayer.NAudioOutput
{
    public partial class AudioPlaybackPanel : System.Windows.Forms.UserControl
    {

        public NPlayer pc = new NPlayer();
        private MainPlayer pl;
        private IWavePlayer wavePlayer;
        public CancellationTokenSource source = new CancellationTokenSource();
        private string fileName;
        private AudioFileReader audioFileReader;
        private Action<float> setVolumeDelegate;
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

        public static Process start = new Process();

        public AudioPlaybackPanel(MainPlayer pl)
        {
            InitializeComponent();
            LoadOutputDevicePlugins(ReflectionHelper.CreateAllInstancesOf<IOutputDevicePlugin>());
            RegistryKey configsocket = HKLMSoftwareTOAPlayer1.OpenSubKey("StatusProgram", true);
            var socket_st = configsocket.GetValue("Sockets");
            checkBox1.Checked = Convert.ToBoolean(socket_st);
            this.pl = pl;
            listView1.Scrollable = true;
            listView1.View = View.Details;
            ListViewItem item1 = new ListViewItem("Player1");
            ListViewItem item2 = new ListViewItem("Player2");
            ListViewItem item3 = new ListViewItem("Player3");
            ListViewItem item4 = new ListViewItem("Player4");
            ListViewItem item5 = new ListViewItem("Player5");
            ListViewItem item6 = new ListViewItem("Player6");
            ListViewItem item7 = new ListViewItem("Player7");
            ListViewItem item8 = new ListViewItem("Player8");


            item1.SubItems.Add(HKLMSoftwareTOAPlayer11.GetValue("DeviceName") == null ? "รอตั้งค่า" : HKLMSoftwareTOAPlayer11.GetValue("DeviceName").ToString());
            item2.SubItems.Add(HKLMSoftwareTOAPlayer12.GetValue("DeviceName") == null ? "รอตั้งค่า" : HKLMSoftwareTOAPlayer12.GetValue("DeviceName").ToString());
            item3.SubItems.Add(HKLMSoftwareTOAPlayer13.GetValue("DeviceName") == null ? "รอตั้งค่า" : HKLMSoftwareTOAPlayer13.GetValue("DeviceName").ToString());
            item4.SubItems.Add(HKLMSoftwareTOAPlayer14.GetValue("DeviceName") == null ? "รอตั้งค่า" : HKLMSoftwareTOAPlayer14.GetValue("DeviceName").ToString());
            item5.SubItems.Add(HKLMSoftwareTOAPlayer15.GetValue("DeviceName") == null ? "รอตั้งค่า" : HKLMSoftwareTOAPlayer15.GetValue("DeviceName").ToString());
            item6.SubItems.Add(HKLMSoftwareTOAPlayer16.GetValue("DeviceName") == null ? "รอตั้งค่า" : HKLMSoftwareTOAPlayer16.GetValue("DeviceName").ToString());
            item7.SubItems.Add(HKLMSoftwareTOAPlayer17.GetValue("DeviceName") == null ? "รอตั้งค่า" : HKLMSoftwareTOAPlayer17.GetValue("DeviceName").ToString());
            item8.SubItems.Add(HKLMSoftwareTOAPlayer18.GetValue("DeviceName") == null ? "รอตั้งค่า" : HKLMSoftwareTOAPlayer18.GetValue("DeviceName").ToString());



            listView1.Items.Add(item1);
            listView1.Items.Add(item2);
            listView1.Items.Add(item3);
            listView1.Items.Add(item4);
            listView1.Items.Add(item5);
            listView1.Items.Add(item6);
            listView1.Items.Add(item7);
            listView1.Items.Add(item8);
        }

        private void LoadOutputDevicePlugins(IEnumerable<IOutputDevicePlugin> outputDevicePlugins)
        {
            comboBoxOutputDevice.DisplayMember = "Name";
            comboBoxOutputDevice.SelectedIndexChanged += OnComboBoxOutputDeviceSelectedIndexChanged;
            foreach (var outputDevicePlugin in outputDevicePlugins.OrderBy(p => p.Priority))
            {
                comboBoxOutputDevice.Items.Add(outputDevicePlugin);
            }
            comboBoxOutputDevice.SelectedIndex = 0;
        }

        void OnComboBoxOutputDeviceSelectedIndexChanged(object sender, EventArgs e)
        {
            panelOutputDeviceSettings.Controls.Clear();
            Control settingsPanel;
            if (SelectedOutputDevicePlugin.IsAvailable)
            {
                settingsPanel = SelectedOutputDevicePlugin.CreateSettingsPanel();
            }
            else
            {
                settingsPanel = new Label() { Text = "This output device is unavailable on your system", Dock = DockStyle.Fill };
            }
            panelOutputDeviceSettings.Controls.Add(settingsPanel);
        }

        private IOutputDevicePlugin SelectedOutputDevicePlugin
        {
            get { return (IOutputDevicePlugin)comboBoxOutputDevice.SelectedItem; }
        }

        public void change_time_music()
        {

        }

        public TimeSpan get_time_music()
        {
            TimeSpan currentTime = (wavePlayer.PlaybackState == PlaybackState.Stopped) ? TimeSpan.Zero : audioFileReader.CurrentTime;
            return currentTime;
        }

        private void OnButtonPlayClick(object sender, EventArgs e)
        {
            if (!SelectedOutputDevicePlugin.IsAvailable)
            {
                var messagebox = new Helper.MessageBox();
                messagebox.ShowCenter_DialogError("The selected output driver is not available on this system", "แจ้งเตือน");
                return;
            }

            if (wavePlayer != null)
            {
                if (wavePlayer.PlaybackState == PlaybackState.Playing)
                {
                    return;
                }
                else if (wavePlayer.PlaybackState == PlaybackState.Paused)
                {
                    wavePlayer.Play();
                    groupBoxDriverModel.Enabled = false;
                    return;
                }
            }

            // we are in a stopped state
            // TODO: only re-initialise if necessary

            if (String.IsNullOrEmpty(fileName))
            {
                OnOpenFileClick(sender, e);
            }
            if (String.IsNullOrEmpty(fileName))
            {
                return;
            }

            try
            {
                CreateWaveOut();
            }
            catch (Exception driverCreateException)
            {
                var messagebox = new Helper.MessageBox();
                messagebox.ShowCenter_DialogError("ButtonPlayClick Error : " + driverCreateException.Message, "แจ้งเตือน");
                return;
            }

            ISampleProvider sampleProvider;
            try
            {
                sampleProvider = CreateInputStream(fileName);
            }
            catch (Exception createException)
            {
                var messagebox = new Helper.MessageBox();
                messagebox.ShowCenter_DialogError("SampleProvider Error : " + createException.Message, "แจ้งเตือน");
                return;
            }


            labelTotalTime.Text = String.Format("{0:00}:{1:00}", (int)audioFileReader.TotalTime.TotalMinutes,
                audioFileReader.TotalTime.Seconds);

            try
            {
                wavePlayer.Init(sampleProvider);
            }
            catch (Exception initException)
            {
                var messagebox = new Helper.MessageBox();
                messagebox.ShowCenter_DialogError("WavePlay Error : " + "{0}" + initException.Message, "แจ้งเตือน");
                return;
            }

            setVolumeDelegate(volumeSlider1.Volume);
            groupBoxDriverModel.Enabled = false;
            wavePlayer.Play();
        }

        private ISampleProvider CreateInputStream(string fileName)
        {
            audioFileReader = new AudioFileReader(fileName);
            textBoxCurrentFile.Text = $"{Path.GetFileName(fileName)}\r\n{audioFileReader.WaveFormat}";

            var sampleChannel = new SampleChannel(audioFileReader, true);
            sampleChannel.PreVolumeMeter += OnPreVolumeMeter;
            setVolumeDelegate = vol => sampleChannel.Volume = vol;
            var postVolumeMeter = new MeteringSampleProvider(sampleChannel);
            postVolumeMeter.StreamVolume += OnPostVolumeMeter;

            return postVolumeMeter;
        }

        void OnPreVolumeMeter(object sender, StreamVolumeEventArgs e)
        {
            // we know it is stereo
            waveformPainter1.AddMax(e.MaxSampleValues[0]);
            waveformPainter2.AddMax(e.MaxSampleValues[1]);
        }

        void OnPostVolumeMeter(object sender, StreamVolumeEventArgs e)
        {
            // we know it is stereo
            volumeMeter1.Amplitude = e.MaxSampleValues[0];
            volumeMeter2.Amplitude = e.MaxSampleValues[1];
        }

        private void CreateWaveOut()
        {
            CloseWaveOut();
            var latency = (int)comboBoxLatency.SelectedItem;
            wavePlayer = SelectedOutputDevicePlugin.CreateDevice(latency);
            textBoxPlaybackFormat.Text = $"{wavePlayer.OutputWaveFormat}";
            wavePlayer.PlaybackStopped += OnPlaybackStopped;
        }

        void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            groupBoxDriverModel.Enabled = true;
            if (e.Exception != null)
            {
                var messagebox = new Helper.MessageBox();
                messagebox.ShowCenter_DialogError(e.Exception.Message, "Playback Device Error");
            }
            if (audioFileReader != null)
            {
                audioFileReader.Position = 0;
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

        private void OnFormLoad(object sender, EventArgs e)
        {
            comboBoxLatency.Items.Add(25);
            comboBoxLatency.Items.Add(50);
            comboBoxLatency.Items.Add(100);
            comboBoxLatency.Items.Add(150);
            comboBoxLatency.Items.Add(200);
            comboBoxLatency.Items.Add(300);
            comboBoxLatency.Items.Add(400);
            comboBoxLatency.Items.Add(500);
            comboBoxLatency.SelectedIndex = 7;
        }

        private void OnButtonPauseClick(object sender, EventArgs e)
        {
            if (wavePlayer != null)
            {
                if (wavePlayer.PlaybackState == PlaybackState.Playing)
                {
                    wavePlayer.Pause();
                }
            }
        }

        private void OnVolumeSliderChanged(object sender, EventArgs e)
        {
            setVolumeDelegate?.Invoke(volumeSlider1.Volume);
        }

        private void OnButtonStopClick(object sender, EventArgs e) => wavePlayer?.Stop();

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (wavePlayer != null && audioFileReader != null)
            {
                TimeSpan currentTime = (wavePlayer.PlaybackState == PlaybackState.Stopped) ? TimeSpan.Zero : audioFileReader.CurrentTime;
                trackBarPosition.Value = Math.Min(trackBarPosition.Maximum, (int)(100 * currentTime.TotalSeconds / audioFileReader.TotalTime.TotalSeconds));
                labelCurrentTime.Text = String.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds);
            }
            else
            {
                trackBarPosition.Value = 0;
            }
        }

        private void trackBarPosition_Scroll(object sender, EventArgs e)
        {
            if (wavePlayer != null)
            {
                audioFileReader.CurrentTime = TimeSpan.FromSeconds(audioFileReader.TotalTime.TotalSeconds * trackBarPosition.Value / 100.0);
            }
        }

        private void OnOpenFileClick(object sender, EventArgs e)
        {
            var openFileDialog = new System.Windows.Forms.OpenFileDialog();
            string allExtensions = "*.wav;*.aiff;*.mp3;*.aac";
            openFileDialog.Filter = String.Format("All Supported Files|{0}|All Files (*.*)|*.*", allExtensions);
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = openFileDialog.FileName;
                textBoxCurrentFile.Text = $"{Path.GetFileName(fileName)}";
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            MMDevice wasapiSoundOutput = null;
            bool wasapiUseEventCallback = false;
            bool wasapiExclusiveMode = false;

            Guid directSoundOutputGuid = Guid.Empty;
            string soundOutputGuid = string.Empty;
            string soundOutputModuleName = string.Empty;
            foreach (System.Windows.Forms.Control panel in this.panelOutputDeviceSettings.Controls)
            {
                foreach (System.Windows.Forms.Control control in panel.Controls)
                {
                    if (control.Name == "comboBoxWaspai")
                    {
                        wasapiSoundOutput = (MMDevice)((ComboBox)control).SelectedValue;
                    }

                    if (control.Name == "checkBoxWasapiEventCallback")
                    {
                        wasapiUseEventCallback = ((CheckBox)control).Checked;
                    }
                    if (control.Name == "checkBoxWasapiExclusiveMode")
                    {
                        wasapiExclusiveMode = ((CheckBox)control).Checked;
                    }
                    if (control.Name == "comboBoxDirectSound")
                    {
                        directSoundOutputGuid = (Guid)((ComboBox)control).SelectedValue;
                    }
                }
            }
            if (HKLMSoftwareTOAPlayer != null)
            {
                var latency = (int)comboBoxLatency.SelectedItem;
                var settupVolume = volumeSlider1.Volume;
                DeleteSubKeyTree((string)cboMediaPlayer.SelectedItem, (string)SelectedOutputDevicePlugin.Name);
                RegistryKey cHKLMPlayer = HKLMSoftwareTOAPlayer.CreateSubKey((string)cboMediaPlayer.SelectedItem);
                //IsShuffle = (bool)regKeyTOASubKey.GetValue("Shuffle");
                //Drivers

                cHKLMPlayer.SetValue("DriverName", SelectedOutputDevicePlugin.Name, RegistryValueKind.String);
                cHKLMPlayer.SetValue("Latency", latency, RegistryValueKind.DWord);
                cHKLMPlayer.SetValue("Volume", settupVolume, RegistryValueKind.String);

                if (wasapiSoundOutput != null)
                {
                    cHKLMPlayer.SetValue("OutputID", wasapiSoundOutput.ID, RegistryValueKind.String);
                    cHKLMPlayer.SetValue("ExclusiveMode", wasapiExclusiveMode, RegistryValueKind.String);
                    cHKLMPlayer.SetValue("UseEventCallback", wasapiUseEventCallback, RegistryValueKind.String);
                    cHKLMPlayer.SetValue("DeviceName", wasapiSoundOutput.FriendlyName, RegistryValueKind.String);
                }
                if (directSoundOutputGuid != Guid.Empty)
                {
                    cHKLMPlayer.SetValue("OutputID", directSoundOutputGuid, RegistryValueKind.String);
                }
                cHKLMPlayer.SetValue("Priority", SelectedOutputDevicePlugin.Priority, RegistryValueKind.DWord);
                cHKLMPlayer.SetValue("IsAvailable", SelectedOutputDevicePlugin.IsAvailable, RegistryValueKind.String);
                //Device
                //using (RegistryKey
                //    CH1 = cHKLMPlayer.CreateSubKey("CH-1"),
                //    CH2 = cHKLMPlayer.CreateSubKey("CH-2"))
                //{
                //    CH1.SetValue("MultipleStringValue", new string[] { "One", "Two", "Three" }, RegistryValueKind.MultiString);
                //    //Loop.Close();

                //    CH2.SetValue("BinaryValue", new byte[] { 10, 43, 44, 45, 14, 255 }, RegistryValueKind.Binary);
                //    //Shuffle.Close();
                //}

                //HKLMSoftwareTOAPlayer.Close();
                listView1.Items.Clear();
                ListViewItem item1 = new ListViewItem("Player1");
                ListViewItem item2 = new ListViewItem("Player2");
                ListViewItem item3 = new ListViewItem("Player3");
                ListViewItem item4 = new ListViewItem("Player4");
                ListViewItem item5 = new ListViewItem("Player5");
                ListViewItem item6 = new ListViewItem("Player6");
                ListViewItem item7 = new ListViewItem("Player7");
                ListViewItem item8 = new ListViewItem("Player8");


                item1.SubItems.Add(HKLMSoftwareTOAPlayer11.GetValue("DeviceName") == null ? "รอตั้งค่า" : HKLMSoftwareTOAPlayer11.GetValue("DeviceName").ToString());
                item2.SubItems.Add(HKLMSoftwareTOAPlayer12.GetValue("DeviceName") == null ? "รอตั้งค่า" : HKLMSoftwareTOAPlayer12.GetValue("DeviceName").ToString());
                item3.SubItems.Add(HKLMSoftwareTOAPlayer13.GetValue("DeviceName") == null ? "รอตั้งค่า" : HKLMSoftwareTOAPlayer13.GetValue("DeviceName").ToString());
                item4.SubItems.Add(HKLMSoftwareTOAPlayer14.GetValue("DeviceName") == null ? "รอตั้งค่า" : HKLMSoftwareTOAPlayer14.GetValue("DeviceName").ToString());
                item5.SubItems.Add(HKLMSoftwareTOAPlayer15.GetValue("DeviceName") == null ? "รอตั้งค่า" : HKLMSoftwareTOAPlayer15.GetValue("DeviceName").ToString());
                item6.SubItems.Add(HKLMSoftwareTOAPlayer16.GetValue("DeviceName") == null ? "รอตั้งค่า" : HKLMSoftwareTOAPlayer16.GetValue("DeviceName").ToString());
                item7.SubItems.Add(HKLMSoftwareTOAPlayer17.GetValue("DeviceName") == null ? "รอตั้งค่า" : HKLMSoftwareTOAPlayer17.GetValue("DeviceName").ToString());
                item8.SubItems.Add(HKLMSoftwareTOAPlayer18.GetValue("DeviceName") == null ? "รอตั้งค่า" : HKLMSoftwareTOAPlayer18.GetValue("DeviceName").ToString());



                listView1.Items.Add(item1);
                listView1.Items.Add(item2);
                listView1.Items.Add(item3);
                listView1.Items.Add(item4);
                listView1.Items.Add(item5);
                listView1.Items.Add(item6);
                listView1.Items.Add(item7);
                listView1.Items.Add(item8);
            }
        }

        private void DeleteSubKeyTree(string SubKey, string deletKey)
        {

            RegistryKey cHKLMPlayer = HKLMSoftwareTOAPlayer.OpenSubKey(SubKey, true);
            if (cHKLMPlayer != null)
            {
                RegistryKey x = cHKLMPlayer.OpenSubKey(deletKey);
                if (x != null) cHKLMPlayer.DeleteSubKeyTree(deletKey);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                if (HKLMSoftwareTOAPlayer1 != null)
                {
                    RegistryKey Configs = HKLMSoftwareTOAPlayer1.CreateSubKey("StatusProgram");
                    Configs.SetValue("Sockets", true, RegistryValueKind.String);

                }
            }
            else
            {
                if (HKLMSoftwareTOAPlayer1 != null)
                {
                    RegistryKey Configs = HKLMSoftwareTOAPlayer1.CreateSubKey("StatusProgram");
                    Configs.SetValue("Sockets", false, RegistryValueKind.String);
                    var messagebox = new Helper.MessageBox();
                    messagebox.ShowCenter_DialogError("กรุณาปิดโปรแกรมแล้วเข้าใหม่", "แจ้งเตือน");

                }
            }

        }

        private async void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 fsa = new Form1();
            fsa.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3(this.pl);
            form3.Show();
        }

        private void textBoxCurrentFile_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            var form = new Form4(pl.GetSocketInstance());
            form.TopMost = true;
            form.Owner = this.pl;
            form.ShowDialog();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            listView1.Items.Add(textBoxCurrentFile.Text);
            string ipAddress = "127.0.0.1";  // IP ของเครื่องที่ต้องการสแกน
            int startPort = 1;               // พอร์ตเริ่มต้นที่ต้องการสแกน
            int endPort = 10;              // พอร์ตสุดท้ายที่ต้องการสแกน
            for (int port = startPort; port <= endPort; port++)
            {
                using (TcpClient tcpClient = new TcpClient())
                {
                    try
                    {
                        // พยายามเชื่อมต่อไปยัง IP และพอร์ตที่กำหนด
                        tcpClient.Connect(ipAddress, port);
                        ListViewItem item1 = new ListViewItem($"Port {port} is open.");
                        item1.SubItems.Add($"Port {port} is open.");
                        listView1.Items.Add(item1);
                    }
                    catch (Exception)
                    {
                        // ถ้าไม่สามารถเชื่อมต่อได้ พอร์ตนั้นจะถือว่าปิดอยู่
                        ListViewItem item1 = new ListViewItem($"Port {port} is closed.");
                        item1.SubItems.Add($"Port {port} is closed.");
                        listView1.Items.Add(item1);
                    }
                }
            }
        }
    }
}

