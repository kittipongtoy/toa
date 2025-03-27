using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

using Microsoft.Win32;

using TOAMediaPlayer.NAudioOutput;
using TOAMediaPlayer.Utils;

namespace TOAMediaPlayer
{
    public partial class SetupOutput : Form
    {
        private INAudioPlugin currentPlugin;
        private MainPlayer mainPlayer;
        public SetupOutput(MainPlayer mainPlayer)
        {
            this.mainPlayer = mainPlayer;
            var demos = ReflectionHelper.CreateAllInstancesOf<INAudioPlugin>().OrderBy(d => d.Name);
            InitializeComponent();
            listBox1.DisplayMember = "Name";
            foreach (var demo in demos)
            {
                listBox1.Items.Add(demo);
            }
            listBox1.SelectedIndex = 0;

            var arch = Environment.Is64BitProcess ? "x64" : "x86";
            var framework = ((TargetFrameworkAttribute)(Assembly.GetEntryAssembly().GetCustomAttributes(typeof(TargetFrameworkAttribute), true).ToArray()[0])).FrameworkName;
            this.Text = $"{this.Text} ({framework}) ({arch})";

            var plugin = (INAudioPlugin)listBox1.SelectedItem;
            if (plugin == currentPlugin) return;
            currentPlugin = plugin;
            DisposeCurrent();
            var control = plugin.CreatePanel(this.mainPlayer);
            control.Dock = DockStyle.Fill;
            panel1.Controls.Add(control);
            
        }

        private void LoadOutputSetting()
        {
            var plugin = (INAudioPlugin)listBox1.SelectedItem;
            if (plugin == currentPlugin) return;
            currentPlugin = plugin;
            DisposeCurrent();
            var control = plugin.CreatePanel(this.mainPlayer);
            control.Dock = DockStyle.Fill;
            panel1.Controls.Add(control);
        }
        private void DisposeCurrent()
        {
            if (panel1.Controls.Count <= 0) return;
            panel1.Controls[0].Dispose();
            panel1.Controls.Clear();
            GC.Collect();
        }
        private void listBox_DoubleClick(object sender, EventArgs e)
        {
            LoadOutputSetting();
        }
        private void SetupOutput_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisposeCurrent();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
