using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TOAMediaPlayer
{
    public partial class Form3 : Form
    {
        public MainPlayer player;
        private RegistryKey HKLMSoftwareTOAPlayer1 = Registry.CurrentUser.OpenSubKey(@"Software\TOA\Config", true);
        public Form3(MainPlayer pl)
        {
            InitializeComponent();
            player = pl;
            RegistryKey cHKLMPlayer = HKLMSoftwareTOAPlayer1.OpenSubKey("trackname");
            
            textBox1.Text = cHKLMPlayer.GetValue("trackC1").ToString();
            textBox4.Text = cHKLMPlayer.GetValue("trackC2").ToString();
            textBox8.Text = cHKLMPlayer.GetValue("trackC3").ToString();
            textBox6.Text = cHKLMPlayer.GetValue("trackC4").ToString();
            textBox12.Text = cHKLMPlayer.GetValue("trackC5").ToString();
            textBox10.Text = cHKLMPlayer.GetValue("trackC6").ToString();
            textBox16.Text = cHKLMPlayer.GetValue("trackC7").ToString();
            textBox14.Text = cHKLMPlayer.GetValue("trackC8").ToString();
            textBox2.Text = cHKLMPlayer.GetValue("trackCF1").ToString();
            textBox3.Text = cHKLMPlayer.GetValue("trackCF2").ToString();
            textBox7.Text = cHKLMPlayer.GetValue("trackCF3").ToString();
            textBox5.Text = cHKLMPlayer.GetValue("trackCF4").ToString();
            textBox11.Text = cHKLMPlayer.GetValue("trackCF5").ToString();
            textBox9.Text = cHKLMPlayer.GetValue("trackCF6").ToString();
            textBox15.Text = cHKLMPlayer.GetValue("trackCF7").ToString();
            textBox13.Text = cHKLMPlayer.GetValue("trackCF8").ToString();

            cHKLMPlayer.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ColorDialog myDialogColor = new ColorDialog();
            myDialogColor.AllowFullOpen = true;
            myDialogColor.ShowHelp = true;
            if(myDialogColor.ShowDialog() == DialogResult.OK )
            {
                var ss = myDialogColor.Color.R+","+ myDialogColor.Color.G + ","+ myDialogColor.Color.B + ","+ myDialogColor.Color.A;
                textBox1.Text = ss.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ColorDialog myDialogColor = new ColorDialog();
            myDialogColor.AllowFullOpen = true;
            myDialogColor.ShowHelp = true;
            if (myDialogColor.ShowDialog() == DialogResult.OK)
            {
                var ss = myDialogColor.Color.R + "," + myDialogColor.Color.G + "," + myDialogColor.Color.B + "," + myDialogColor.Color.A;
                textBox2.Text = ss.ToString();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ColorDialog myDialogColor = new ColorDialog();
            myDialogColor.AllowFullOpen = true;
            myDialogColor.ShowHelp = true;
            if (myDialogColor.ShowDialog() == DialogResult.OK)
            {
                var ss = myDialogColor.Color.R + "," + myDialogColor.Color.G + "," + myDialogColor.Color.B + "," + myDialogColor.Color.A;
                textBox4.Text = ss.ToString();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ColorDialog myDialogColor = new ColorDialog();
            myDialogColor.AllowFullOpen = true;
            myDialogColor.ShowHelp = true;
            if (myDialogColor.ShowDialog() == DialogResult.OK)
            {
                var ss = myDialogColor.Color.R + "," + myDialogColor.Color.G + "," + myDialogColor.Color.B + "," + myDialogColor.Color.A;
                textBox3.Text = ss.ToString();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ColorDialog myDialogColor = new ColorDialog();
            myDialogColor.AllowFullOpen = true;
            myDialogColor.ShowHelp = true;
            if (myDialogColor.ShowDialog() == DialogResult.OK)
            {
                var ss = myDialogColor.Color.R + "," + myDialogColor.Color.G + "," + myDialogColor.Color.B + "," + myDialogColor.Color.A;
                textBox8.Text = ss.ToString();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ColorDialog myDialogColor = new ColorDialog();
            myDialogColor.AllowFullOpen = true;
            myDialogColor.ShowHelp = true;
            if (myDialogColor.ShowDialog() == DialogResult.OK)
            {
                var ss = myDialogColor.Color.R + "," + myDialogColor.Color.G + "," + myDialogColor.Color.B + "," + myDialogColor.Color.A;
                textBox7.Text = ss.ToString();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ColorDialog myDialogColor = new ColorDialog();
            myDialogColor.AllowFullOpen = true;
            myDialogColor.ShowHelp = true;
            if (myDialogColor.ShowDialog() == DialogResult.OK)
            {
                var ss = myDialogColor.Color.R + "," + myDialogColor.Color.G + "," + myDialogColor.Color.B + "," + myDialogColor.Color.A;
                textBox6.Text = ss.ToString();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ColorDialog myDialogColor = new ColorDialog();
            myDialogColor.AllowFullOpen = true;
            myDialogColor.ShowHelp = true;
            if (myDialogColor.ShowDialog() == DialogResult.OK)
            {
                var ss = myDialogColor.Color.R + "," + myDialogColor.Color.G + "," + myDialogColor.Color.B + "," + myDialogColor.Color.A;
                textBox5.Text = ss.ToString();
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            ColorDialog myDialogColor = new ColorDialog();
            myDialogColor.AllowFullOpen = true;
            myDialogColor.ShowHelp = true;
            if (myDialogColor.ShowDialog() == DialogResult.OK)
            {
                var ss = myDialogColor.Color.R + "," + myDialogColor.Color.G + "," + myDialogColor.Color.B + "," + myDialogColor.Color.A;
                textBox12.Text = ss.ToString();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            ColorDialog myDialogColor = new ColorDialog();
            myDialogColor.AllowFullOpen = true;
            myDialogColor.ShowHelp = true;
            if (myDialogColor.ShowDialog() == DialogResult.OK)
            {
                var ss = myDialogColor.Color.R + "," + myDialogColor.Color.G + "," + myDialogColor.Color.B + "," + myDialogColor.Color.A;
                textBox11.Text = ss.ToString();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ColorDialog myDialogColor = new ColorDialog();
            myDialogColor.AllowFullOpen = true;
            myDialogColor.ShowHelp = true;
            if (myDialogColor.ShowDialog() == DialogResult.OK)
            {
                var ss = myDialogColor.Color.R + "," + myDialogColor.Color.G + "," + myDialogColor.Color.B + "," + myDialogColor.Color.A;
                textBox10.Text = ss.ToString();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ColorDialog myDialogColor = new ColorDialog();
            myDialogColor.AllowFullOpen = true;
            myDialogColor.ShowHelp = true;
            if (myDialogColor.ShowDialog() == DialogResult.OK)
            {
                var ss = myDialogColor.Color.R + "," + myDialogColor.Color.G + "," + myDialogColor.Color.B + "," + myDialogColor.Color.A;
                textBox9.Text = ss.ToString();
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            ColorDialog myDialogColor = new ColorDialog();
            myDialogColor.AllowFullOpen = true;
            myDialogColor.ShowHelp = true;
            if (myDialogColor.ShowDialog() == DialogResult.OK)
            {
                var ss = myDialogColor.Color.R + "," + myDialogColor.Color.G + "," + myDialogColor.Color.B + "," + myDialogColor.Color.A;
                textBox16.Text = ss.ToString();
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            ColorDialog myDialogColor = new ColorDialog();
            myDialogColor.AllowFullOpen = true;
            myDialogColor.ShowHelp = true;
            if (myDialogColor.ShowDialog() == DialogResult.OK)
            {
                var ss = myDialogColor.Color.R + "," + myDialogColor.Color.G + "," + myDialogColor.Color.B + "," + myDialogColor.Color.A;
                textBox15.Text = ss.ToString();
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            ColorDialog myDialogColor = new ColorDialog();
            myDialogColor.AllowFullOpen = true;
            myDialogColor.ShowHelp = true;
            if (myDialogColor.ShowDialog() == DialogResult.OK)
            {
                var ss = myDialogColor.Color.R + "," + myDialogColor.Color.G + "," + myDialogColor.Color.B + "," + myDialogColor.Color.A;
                textBox14.Text = ss.ToString();
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            ColorDialog myDialogColor = new ColorDialog();
            myDialogColor.AllowFullOpen = true;
            myDialogColor.ShowHelp = true;
            if (myDialogColor.ShowDialog() == DialogResult.OK)
            {
                var ss = myDialogColor.Color.R + "," + myDialogColor.Color.G + "," + myDialogColor.Color.B + "," + myDialogColor.Color.A;
                textBox13.Text = ss.ToString();
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            RegistryKey cHKLMPlayer = HKLMSoftwareTOAPlayer1.CreateSubKey("trackname");
            cHKLMPlayer.SetValue("trackC1", textBox1.Text.ToString(), RegistryValueKind.String);
            cHKLMPlayer.SetValue("trackC2", textBox4.Text.ToString(), RegistryValueKind.String);
            cHKLMPlayer.SetValue("trackC3", textBox8.Text.ToString(), RegistryValueKind.String);
            cHKLMPlayer.SetValue("trackC4", textBox6.Text.ToString(), RegistryValueKind.String);
            cHKLMPlayer.SetValue("trackC5", textBox12.Text.ToString(), RegistryValueKind.String);
            cHKLMPlayer.SetValue("trackC6", textBox10.Text.ToString(), RegistryValueKind.String);
            cHKLMPlayer.SetValue("trackC7", textBox16.Text.ToString(), RegistryValueKind.String);
            cHKLMPlayer.SetValue("trackC8", textBox14.Text.ToString(), RegistryValueKind.String);
            cHKLMPlayer.SetValue("trackCF1", textBox2.Text.ToString(), RegistryValueKind.String);
            cHKLMPlayer.SetValue("trackCF2", textBox3.Text.ToString(), RegistryValueKind.String);
            cHKLMPlayer.SetValue("trackCF3", textBox7.Text.ToString(), RegistryValueKind.String);
            cHKLMPlayer.SetValue("trackCF4", textBox5.Text.ToString(), RegistryValueKind.String);
            cHKLMPlayer.SetValue("trackCF5", textBox11.Text.ToString(), RegistryValueKind.String);
            cHKLMPlayer.SetValue("trackCF6", textBox9.Text.ToString(), RegistryValueKind.String);
            cHKLMPlayer.SetValue("trackCF7", textBox15.Text.ToString(), RegistryValueKind.String);
            cHKLMPlayer.SetValue("trackCF8", textBox13.Text.ToString(), RegistryValueKind.String);

            cHKLMPlayer.Close();
            player.updatecolor();
            this.Hide();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}
