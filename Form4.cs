using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace TOAMediaPlayer
{
    public partial class Form4 : Form
    {
        private const int MIN_VALUE = 50;
        private const int MAX_VALUE = 93;
        RegistryKey HKLMSoftwareTOAConfig = Registry.CurrentUser.OpenSubKey(@"Software\TOA\Config", true);
        public Form4()
        {
            RegistryKey configsocket = this.HKLMSoftwareTOAConfig.OpenSubKey("trackname", true);
            InitializeComponent();
            textEdit3.Leave += TextBox_Leave;
            textEdit4.Leave += TextBox_Leave;
            textEdit5.Leave += TextBox_Leave;
            textEdit6.Leave += TextBox_Leave;
            textEdit7.Leave += TextBox_Leave;
            textEdit8.Leave += TextBox_Leave;
            textEdit9.Leave += TextBox_Leave;
            textEdit10.Leave += TextBox_Leave;
            if (configsocket.GetValue("ip") == null)
            {
                configsocket.SetValue("ip", "0.0.0.0");
                configsocket.SetValue("port", "8003");
                textEdit1.Text = configsocket.GetValue("ip").ToString();
                textEdit11.Text = configsocket.GetValue("ip1").ToString();
                textEdit2.Text = configsocket.GetValue("port").ToString();
            }
            else
            {
                textEdit1.Text = configsocket.GetValue("ip").ToString();
                textEdit11.Text = configsocket.GetValue("ip1").ToString();
                textEdit2.Text = configsocket.GetValue("port").ToString();
            }

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
                textEdit3.Text = configsocket.GetValue("dB1").ToString();
                textEdit4.Text = configsocket.GetValue("dB2").ToString();
                textEdit5.Text = configsocket.GetValue("dB3").ToString();
                textEdit6.Text = configsocket.GetValue("dB4").ToString();
                textEdit7.Text = configsocket.GetValue("dB5").ToString();
                textEdit8.Text = configsocket.GetValue("dB6").ToString();
                textEdit9.Text = configsocket.GetValue("dB7").ToString();
                textEdit10.Text = configsocket.GetValue("dB8").ToString();
            }
            else
            {
                textEdit3.Text = configsocket.GetValue("dB1").ToString();
                textEdit4.Text = configsocket.GetValue("dB2").ToString();
                textEdit5.Text = configsocket.GetValue("dB3").ToString();
                textEdit6.Text = configsocket.GetValue("dB4").ToString();
                textEdit7.Text = configsocket.GetValue("dB5").ToString();
                textEdit8.Text = configsocket.GetValue("dB6").ToString();
                textEdit9.Text = configsocket.GetValue("dB7").ToString();
                textEdit10.Text = configsocket.GetValue("dB8").ToString();
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
                if (configsocket.GetValue("adB1").ToString() == "active")
                {
                    checkBox1.Checked = true;
                }
                else
                {
                    checkBox1.Checked = false;
                }
                if (configsocket.GetValue("adB2").ToString() == "active")
                {
                    checkBox2.Checked = true;
                }
                else
                {
                    checkBox2.Checked = false;
                }
                if (configsocket.GetValue("adB3").ToString() == "active")
                {
                    checkBox3.Checked = true;
                }
                else
                {
                    checkBox3.Checked = false;
                }
                if (configsocket.GetValue("adB4").ToString() == "active")
                {
                    checkBox4.Checked = true;
                }
                else
                {
                    checkBox4.Checked = false;
                }
                if (configsocket.GetValue("adB5").ToString() == "active")
                {
                    checkBox5.Checked = true;
                }
                else
                {
                    checkBox5.Checked = false;
                }
                if (configsocket.GetValue("adB6").ToString() == "active")
                {
                    checkBox6.Checked = true;
                }
                else
                {
                    checkBox6.Checked = false;
                }
                if (configsocket.GetValue("adB7").ToString() == "active")
                {
                    checkBox7.Checked = true;
                }
                else
                {
                    checkBox7.Checked = false;
                }
                if (configsocket.GetValue("adB8").ToString() == "active")
                {
                    checkBox8.Checked = true;
                }
                else
                {
                    checkBox8.Checked = false;
                }
            }
            else
            {
                if (configsocket.GetValue("adB1").ToString() == "active")
                {
                    checkBox1.Checked = true;
                }
                else
                {
                    checkBox1.Checked = false;
                }
                if (configsocket.GetValue("adB2").ToString() == "active")
                {
                    checkBox2.Checked = true;
                }
                else
                {
                    checkBox2.Checked = false;
                }
                if (configsocket.GetValue("adB3").ToString() == "active")
                {
                    checkBox3.Checked = true;
                }
                else
                {
                    checkBox3.Checked = false;
                }
                if (configsocket.GetValue("adB4").ToString() == "active")
                {
                    checkBox4.Checked = true;
                }
                else
                {
                    checkBox4.Checked = false;
                }
                if (configsocket.GetValue("adB5").ToString() == "active")
                {
                    checkBox5.Checked = true;
                }
                else
                {
                    checkBox5.Checked = false;
                }
                if (configsocket.GetValue("adB6").ToString() == "active")
                {
                    checkBox6.Checked = true;
                }
                else
                {
                    checkBox6.Checked = false;
                }
                if (configsocket.GetValue("adB7").ToString() == "active")
                {
                    checkBox7.Checked = true;
                }
                else
                {
                    checkBox7.Checked = false;
                }
                if (configsocket.GetValue("adB8").ToString() == "active")
                {
                    checkBox8.Checked = true;
                }
                else
                {
                    checkBox8.Checked = false;
                }
            }

        }
        private void TextBox_Leave(object sender, EventArgs e)
        {
            //TextEdit textBox = sender as TextEdit;
            TextBox textBox = new TextBox();
            if (textBox == null) return;
            int value;

            // ตรวจสอบว่าค่าที่ป้อนไปเป็นตัวเลขหรือไม่
            var messagebox = new Helper.MessageBox();
            if (int.TryParse(textBox.Text, out value))
            {
                if (value < MIN_VALUE)
                {
                    messagebox.ShowCenter_DialogError($"ค่าของ {textBox.Name} น้อยเกินไป! ขั้นต่ำคือ {MIN_VALUE}", "Warning");
                    textBox.Text = MIN_VALUE.ToString();
                }
                else if (value > MAX_VALUE)
                {
                    messagebox.ShowCenter_DialogError($"ค่าของ {textBox.Name} มากเกินไป! ค่าสูงสุดคือ {MAX_VALUE}", "Warning");
                    textBox.Text = MAX_VALUE.ToString();
                }
            }
            else
            {
                // ป้อนค่าไม่ใช่ตัวเลข
                messagebox.ShowCenter_DialogError($"กรุณาป้อนตัวเลขใน {textBox.Name} เท่านั้น", "Error");
                textBox.Text = MIN_VALUE.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RegistryKey configsocket = this.HKLMSoftwareTOAConfig.OpenSubKey("trackname", true);
            configsocket.SetValue("port", textEdit2.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RegistryKey configsocket = this.HKLMSoftwareTOAConfig.OpenSubKey("trackname", true);
            configsocket.SetValue("ip", textEdit1.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RegistryKey configsocket = this.HKLMSoftwareTOAConfig.OpenSubKey("trackname", true);
            configsocket.SetValue("dB1", textEdit3.Text == "" ? "50" : textEdit3.Text);
            configsocket.SetValue("dB2", textEdit4.Text == "" ? "50" : textEdit4.Text);
            configsocket.SetValue("dB3", textEdit5.Text == "" ? "50" : textEdit5.Text);
            configsocket.SetValue("dB4", textEdit6.Text == "" ? "50" : textEdit6.Text);
            configsocket.SetValue("dB5", textEdit7.Text == "" ? "50" : textEdit7.Text);
            configsocket.SetValue("dB6", textEdit8.Text == "" ? "50" : textEdit8.Text);
            configsocket.SetValue("dB7", textEdit9.Text == "" ? "50" : textEdit9.Text);
            configsocket.SetValue("dB8", textEdit10.Text == "" ? "50" : textEdit10.Text);

            textEdit3.Text = configsocket.GetValue("dB1").ToString();
            textEdit4.Text = configsocket.GetValue("dB2").ToString();
            textEdit5.Text = configsocket.GetValue("dB3").ToString();
            textEdit6.Text = configsocket.GetValue("dB4").ToString();
            textEdit7.Text = configsocket.GetValue("dB5").ToString();
            textEdit8.Text = configsocket.GetValue("dB6").ToString();
            textEdit9.Text = configsocket.GetValue("dB7").ToString();
            textEdit10.Text = configsocket.GetValue("dB8").ToString();

            configsocket.SetValue("adB1", checkBox1.Checked == true ? "active" : "unactive");
            configsocket.SetValue("adB2", checkBox2.Checked == true ? "active" : "unactive");
            configsocket.SetValue("adB3", checkBox3.Checked == true ? "active" : "unactive");
            configsocket.SetValue("adB4", checkBox4.Checked == true ? "active" : "unactive");
            configsocket.SetValue("adB5", checkBox5.Checked == true ? "active" : "unactive");
            configsocket.SetValue("adB6", checkBox6.Checked == true ? "active" : "unactive");
            configsocket.SetValue("adB7", checkBox7.Checked == true ? "active" : "unactive");
            configsocket.SetValue("adB8", checkBox8.Checked == true ? "active" : "unactive");
            if (configsocket.GetValue("adB1").ToString() == "active")
            {
                checkBox1.Checked = true;
            }
            else
            {
                checkBox1.Checked = false;
            }
            if (configsocket.GetValue("adB2").ToString() == "active")
            {
                checkBox2.Checked = true;
            }
            else
            {
                checkBox2.Checked = false;
            }
            if (configsocket.GetValue("adB3").ToString() == "active")
            {
                checkBox3.Checked = true;
            }
            else
            {
                checkBox3.Checked = false;
            }
            if (configsocket.GetValue("adB4").ToString() == "active")
            {
                checkBox4.Checked = true;
            }
            else
            {
                checkBox4.Checked = false;
            }
            if (configsocket.GetValue("adB5").ToString() == "active")
            {
                checkBox5.Checked = true;
            }
            else
            {
                checkBox5.Checked = false;
            }
            if (configsocket.GetValue("adB6").ToString() == "active")
            {
                checkBox6.Checked = true;
            }
            else
            {
                checkBox6.Checked = false;
            }
            if (configsocket.GetValue("adB7").ToString() == "active")
            {
                checkBox7.Checked = true;
            }
            else
            {
                checkBox7.Checked = false;
            }
            if (configsocket.GetValue("adB8").ToString() == "active")
            {
                checkBox8.Checked = true;
            }
            else
            {
                checkBox8.Checked = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            RegistryKey configsocket = this.HKLMSoftwareTOAConfig.OpenSubKey("trackname", true);
            configsocket.SetValue("ip1", textEdit11.Text);
        }
    }
}
