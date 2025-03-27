using MetroFramework;
using MetroFramework.Controls;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.Net.Http;
using System.Windows.Forms;
using static TOAMediaPlayer.jsonWebAPI;

namespace TOAMediaPlayer
{
    public partial class Settimers : MetroFramework.Forms.MetroForm  //Form
    {
        RegistryKey HKLMSoftwareTOAConfig = Registry.CurrentUser.OpenSubKey(@"Software\TOA\Config", true);
        string nametrack = "";
        MainPlayer player;
        public Loggers log = new Loggers("debugs.txt");

        //public int CheckBoxSize { get; set; } = 24;
        //public Color BorderColor { get; set; } = Color.Black;


        public Settimers(MainPlayer p, string nametrack, PaintEventArgs e)
        {
            player = p;
            InitializeComponent();

            this.nametrack = nametrack;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            RegistryKey configsocket = this.HKLMSoftwareTOAConfig.OpenSubKey("trackname", true);
            var ggg = configsocket.GetValue(this.nametrack);
            var substring = ggg.ToString().Split(',');
            var substring1 = substring[1].ToString().Split('-');
            var countday = 0;

            this.timeEdit1.CustomFormat = "hh:mm tt";
            this.timeEdit1.ShowUpDown = true;
            //this.timeEdit1.Properties.Mask.EditMask = "hh:mm tt";
            this.timeEdit2.CustomFormat = "hh:mm tt";
            this.timeEdit2.ShowUpDown = true;
            //this.timeEdit2.Properties.Mask.EditMask = "hh:mm tt";

            metroLabel1.AutoSize = true;
            metroLabel2.AutoSize = true;

            #region Active Checkbox

            if (substring[0] == "active")
                {
                    checkBox9.Checked = true;
                }
                else
                {
                    checkBox9.Checked = false;
                }
            if (substring[4] == "active")
            {
                checkBox10.Checked = true;
            }
            else
            {
                checkBox10.Checked = false;
            }
            if (substring[5] == "active")
            {
                checkBox11.Checked = true;
            }
            else
            {
                checkBox11.Checked = false;
            }
            if (substring[6] == "active")
            {
                if (substring[2].IndexOf("AM") != -1 || substring[2].IndexOf("PM") != -1)
                {
                    timeEdit1.Value = DateTime.ParseExact("27/08/2024 " + substring[2], "dd/MM/yyyy hh:mm tt", null);
                }
                else
                {
                    timeEdit1.Value = DateTime.ParseExact("27/08/2024 " + substring[2], "dd/MM/yyyy hh:mm tt", null);
                }
                if (substring[3].IndexOf("AM") != -1 || substring[3].IndexOf("PM") != -1)
                {
                    timeEdit2.Value = DateTime.ParseExact("27/08/2024 " + substring[3], "dd/MM/yyyy hh:mm tt", null);
                }
                else
                {
                    timeEdit2.Value = DateTime.ParseExact("27/08/2024 " + substring[3], "dd/MM/yyyy hh:mm tt", null);
                }
                checkBox12.Checked = true;
            }
            else
            {
                if (substring[2].IndexOf("AM") != -1 && substring[2].IndexOf("PM") != -1)
                {
                    timeEdit1.Value = DateTime.ParseExact("27/08/2024 " + substring[2], "dd/MM/yyyy hh:mm tt", null);
                }
                else
                {
                    timeEdit1.Value = DateTime.ParseExact("27/08/2024 " + substring[2], "dd/MM/yyyy hh:mm tt", null);
                }
                if (substring[3].IndexOf("AM") != -1 && substring[3].IndexOf("PM") != -1)
                {
                    timeEdit2.Value = DateTime.ParseExact("27/08/2024 " + substring[3], "dd/MM/yyyy hh:mm tt", null);
                }
                else
                {
                    timeEdit2.Value = DateTime.ParseExact("27/08/2024 " + substring[3], "dd/MM/yyyy hh:mm tt", null);
                }
                checkBox12.Checked = false;
            }
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;
            checkBox8.Checked = false;
         
            timeEdit1.Text = "0:00";
            timeEdit2.Text = "0:00";
            foreach (var tt in substring1)
            {
                if (tt == "mon")
                {
                    countday += 1;
                    checkBox1.Checked = true;
                }
                else if (tt == "tue")
                {
                    countday += 1;
                    checkBox2.Checked = true;
                }
                else if (tt == "wed")
                {
                    countday += 1;
                    checkBox3.Checked = true;
                }
                else if (tt == "thu")
                {
                    countday += 1;
                    checkBox4.Checked = true;
                }
                else if (tt == "fri")
                {
                    countday += 1;
                    checkBox5.Checked = true;
                }
                else if (tt == "sat")
                {
                    countday += 1;
                    checkBox6.Checked = true;
                }
                else if (tt == "sun")
                {
                    countday += 1;
                    checkBox7.Checked = true;
                }
            }
            if (countday == 7)
            {
                checkBox8.Checked = true;
            }
            #endregion
        }

        public void setnametrack(string nametrack)
        {
            this.nametrack = nametrack;
        }

        private void Settimers_Load(object sender, EventArgs e)
        {

        }

        private void updateja(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked == true &&
                this.checkBox2.Checked == true &&
                this.checkBox3.Checked == true &&
                this.checkBox4.Checked == true &&
                this.checkBox5.Checked == true &&
                this.checkBox6.Checked == true &&
                this.checkBox7.Checked == true)
            {
                checkBox8.Checked = true;
            }
            else
            {
                checkBox8.Checked = false;
            }
        }

        private void updateja1(object sender, EventArgs e)
        {
            if (checkBox8.Checked == true)
            {
                checkBox1.Checked = true;
                checkBox2.Checked = true;
                checkBox3.Checked = true;
                checkBox4.Checked = true;
                checkBox5.Checked = true;
                checkBox6.Checked = true;
                checkBox7.Checked = true;
            }
            else
            {
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox6.Checked = false;
                checkBox7.Checked = false;
            }
        }

        private void updatedate(object sender, EventArgs e)
        {

        }

        private void updatereg()
        {
            var configs = "";
            var days = "";
            RegistryKey configsocket = this.HKLMSoftwareTOAConfig.OpenSubKey("trackname", true);
            if (checkBox9.Checked == true)
            {
                configs += "active,";
            }
            else
            {
                configs += "unactive,";
            }
            if (checkBox1.Checked == true)
            {
                if (days == "")
                {
                    days += "mon";
                }
                else
                {
                    days += "-mon";
                }
            }
            if (checkBox2.Checked == true)
            {
                if (days == "")
                {
                    days += "tue";
                }
                else
                {
                    days += "-tue";
                }
            }
            if (checkBox3.Checked == true)
            {
                if (days == "")
                {
                    days += "wed";
                }
                else
                {
                    days += "-wed";
                }
            }
            if (checkBox4.Checked == true)
            {
                if (days == "")
                {
                    days += "thu";
                }
                else
                {
                    days += "-thu";
                }
            }
            if (checkBox5.Checked == true)
            {
                if (days == "")
                {
                    days += "fri";
                }
                else
                {
                    days += "-fri";
                }
            }
            if (checkBox6.Checked == true)
            {
                if (days == "")
                {
                    days += "sat";
                }
                else
                {
                    days += "-sat";
                }
            }
            if (checkBox7.Checked == true)
            {
                if (days == "")
                {
                    days += "sun";
                }
                else
                {
                    days += "-sun";
                }
            }
            configs += days;
            var gg1 = timeEdit1.Text.Split(':');
            var gg2 = timeEdit2.Text.Split(':');

            var start = gg1[0].Length > 1 ? gg1[0] : "0" + gg1[0];
            var end = gg2[0].Length > 1 ? gg2[0] : "0" + gg2[0];

            configs += "," + start + ":" + gg1[1] + "," + end + ":" + gg2[1];
            if (checkBox10.Checked == true)
            {
                configs += ",active";
            }
            else
            {
                configs += ",unactive";
            }
            if (checkBox11.Checked == true)
            {
                configs += ",active";
            }
            else
            {
                configs += ",unactive";
            }
            if (checkBox12.Checked == true)
            {
                configs += ",active";
            }
            else
            {
                configs += ",unactive";
            }
            configsocket.SetValue(this.nametrack, configs);
        }

        public stats_timer get_timer(string truckname)
        {
            if (this.InvokeRequired)
            {
                stats_timer data = new stats_timer();

                this.Invoke(new Action(() =>
                {
                    RegistryKey configsocket = this.HKLMSoftwareTOAConfig.OpenSubKey("trackname", true);
                    var ggg = configsocket.GetValue(truckname);
                    var substring = ggg.ToString().Split(',');
                    var substring1 = substring[1].ToString().Split('-');
                    var countday = 0;
                    data.status = substring[0] == "active" ? true : false;
                    data.time_start = substring[2];
                    data.time_end = substring[3];
                    data.typstime = substring[6] == "active" ? "12 AM/PM" : "24 hr";
                    data.active_start = substring[4] == "active" ? true : false;
                    data.active_end = substring[5] == "active" ? true : false;
                    foreach (var tt in substring1)
                    {
                        if (tt == "mon")
                        {
                            data.mon = true;
                        }
                        else if (tt == "tue")
                        {
                            data.tue = true;
                        }
                        else if (tt == "wed")
                        {
                            data.wed = true;
                        }
                        else if (tt == "thu")
                        {
                            data.thu = true;
                        }
                        else if (tt == "fri")
                        {
                            data.fri = true;
                        }
                        else if (tt == "sat")
                        {
                            data.sat = true;
                        }
                        else if (tt == "sun")
                        {
                            data.sun = true;
                        }
                    }
                }));
                return data;

            }
            else
            {
                stats_timer data = new stats_timer();
                RegistryKey configsocket = this.HKLMSoftwareTOAConfig.OpenSubKey("trackname", true);
                var ggg = configsocket.GetValue(truckname);
                var substring = ggg.ToString().Split(',');
                var substring1 = substring[1].ToString().Split('-');
                var countday = 0;
                data.status = substring[0] == "active" ? true : false;
                data.time_start = substring[2];
                data.time_end = substring[3];
                data.typstime = substring[6] == "active" ? "12 AM/PM" : "24 hr";
                data.active_start = substring[4] == "active" ? true : false;
                data.active_end = substring[5] == "active" ? true : false;
                foreach (var tt in substring1)
                {
                    if (tt == "mon")
                    {
                        data.mon = true;
                    }
                    else if (tt == "tue")
                    {
                        data.tue = true;
                    }
                    else if (tt == "wed")
                    {
                        data.wed = true;
                    }
                    else if (tt == "thu")
                    {
                        data.thu = true;
                    }
                    else if (tt == "fri")
                    {
                        data.fri = true;
                    }
                    else if (tt == "sat")
                    {
                        data.sat = true;
                    }
                    else if (tt == "sun")
                    {
                        data.sun = true;
                    }
                }
                return data;
            }
        }

        public string updatereg(string configs, string name)
        {
            //var configs = "";
            var days = "";
            RegistryKey configsocket = this.HKLMSoftwareTOAConfig.OpenSubKey("trackname", true);
            string decodedString = Uri.UnescapeDataString(configs);
            configsocket.SetValue(name, decodedString);
            return "success";
        }

        public Tuple<bool, string> cksetting()
        {
            if (checkBox9.Checked == true)
            {
                if (checkBox1.Checked == false && checkBox2.Checked == false && checkBox3.Checked == false && checkBox4.Checked == false && checkBox5.Checked == false && checkBox6.Checked == false && checkBox7.Checked == false)
                {
                    //return Tuple.Create(true, "กรุณากำหนดวันก่อน!!!");
                    return Tuple.Create(true, "กรุณาตรวจสอบการตั้งวันอีกครั้งหนึ่ง");
                }
                if (checkBox10.Checked == false && checkBox11.Checked == false)
                {
                    //return Tuple.Create(true, "กรุณากำหนดเวลาเปิดหรือเวลาปิดก่อน!!!");
                    return Tuple.Create(true, "กรุณาตรวจสอบการตั้งเวลา เปิด หรือ ปิด อีกครั้งหนึ่ง");
                }
            }
            return Tuple.Create(false, "ไม่พบปัญหา");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var ggg = this.cksetting();
            if (ggg.Item1 == false)
            {
                player.timersc = false;
                player.showtimeset = false;
                updatereg();
                this.Hide();
            }
            else
            {
                MessageBox.Show(ggg.Item2);
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

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox12.Checked == true)
            {
                this.timeEdit1.CustomFormat = "hh:mm tt";
                //this.timeEdit1.Properties.Mask.EditMask = "hh:mm tt";
                this.timeEdit2.CustomFormat = "hh:mm tt";
                //this.timeEdit2.Properties.Mask.EditMask = "hh:mm tt";
            }
            else
            {
                this.timeEdit1.CustomFormat = "HH:mm";
                //this.timeEdit1.Properties.Mask.EditMask = "HH:mm";
                this.timeEdit2.CustomFormat = "HH:mm";
                //this.timeEdit2.Properties.Mask.EditMask = "HH:mm";
            }
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            //this.checkBox9.s
        }

        private void metroLabel1_Click(object sender, EventArgs e)
        {

        }
    }
}
