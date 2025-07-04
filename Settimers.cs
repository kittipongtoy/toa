﻿using MetroFramework;
using MetroFramework.Controls;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.Globalization;
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

            if (configsocket == null) {
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
            }

            var ggg = configsocket.GetValue(this.nametrack);
            var substring = ggg.ToString().Split(',');
            var substring1 = substring[1].ToString().Split('-');
            var countday = 0;

            this.timeEdit1.CustomFormat = "hh:mm tt";
            this.timeEdit1.ShowUpDown = true;
            this.timeEdit1.Font = new Font("Tahoma", 10F);
            //this.timeEdit1.Width = 150;
            this.timeEdit1.Width = 90;
            this.timeEdit1.Height = 180;

            this.timeEdit2.CustomFormat = "hh:mm tt";
            this.timeEdit2.Font = new Font("Tahoma", 10F);
            this.timeEdit2.ShowUpDown = true;
            this.timeEdit2.Width = 90;
            this.timeEdit2.Height = 180;

            //label3.AutoSize = true;
            //label4.AutoSize = true;

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
                    timeEdit1.Value = DateTime.ParseExact("27/08/2024 " + substring[2], "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
                    //label3.Text = "PM";
                }
                else
                {
                    //var date = DateTime.ParseExact("27/08/2024 " + substring[2], "dd/MM/yyyy HH:mm", null);   ///Convert.ToDateTime("27/08/2024 " + substring[2]);
                    //if (DateTime.TryParse("27/08/2024 10:30 AM", out date)) {
                    //    //String.Format("{0:d/MM/yyyy}", dDate);
                    //    timeEdit1.Value = DateTime.ParseExact("27/08/2024 " + substring[2], "dd/MM/yyyy hh:mm tt", null);
                    //} else {
                    //    //Console.WriteLine("Invalid"); // <-- Control flow goes here
                    //    timeEdit1.Value = DateTime.ParseExact("27/08/2024 " + substring[2], "dd/MM/yyyy HH:mm", null);
                    //}
                    DateTime tempDate = DateTime.ParseExact("27/08/2024 " + substring[2], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    string formattedTime = tempDate.ToString("hh:mm tt", CultureInfo.InvariantCulture);
                    timeEdit1.Value = DateTime.ParseExact("27/08/2024 " + formattedTime, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
                    //label3.Text = "";
                }
                if (substring[3].IndexOf("AM") != -1 || substring[3].IndexOf("PM") != -1)
                {
                    timeEdit2.Value = DateTime.ParseExact("27/08/2024 " + substring[3], "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
                    //label4.Text = "PM";
                }
                else
                {
                    //var date = DateTime.ParseExact("27/08/2024 " + substring[3], "dd/MM/yyyy HH:mm", null);   ///Convert.ToDateTime("27/08/2024 " + substring[2]);
                    //if (DateTime.TryParse("27/08/2024 10:30 AM", out date)) {
                    //    //String.Format("{0:d/MM/yyyy}", dDate);
                    //    timeEdit2.Value = DateTime.ParseExact("27/08/2024 " + substring[3], "dd/MM/yyyy hh:mm tt", null);
                    //} else {
                    //    //Console.WriteLine("Invalid"); // <-- Control flow goes here
                    //    timeEdit2.Value = DateTime.ParseExact("27/08/2024 " + substring[3], "dd/MM/yyyy HH:mm", null);
                    //}
                    DateTime tempDate = DateTime.ParseExact("27/08/2024 " + substring[3], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    string formattedTime = tempDate.ToString("hh:mm tt", CultureInfo.InvariantCulture);
                    timeEdit2.Value = DateTime.ParseExact("27/08/2024 " + formattedTime, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
                    //label4.Text = "";
                }
                checkBox12.Checked = true;
            }
            else
            {
                if (substring[2].IndexOf("AM") != -1 || substring[2].IndexOf("PM") != -1)
                {
                    timeEdit1.Value = DateTime.ParseExact("27/08/2024 " + substring[2], "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
                    if (substring[2].Substring(6, 2) == "AM") {
                        //label3.Text = "AM";
                    } else if (substring[2].Substring(6, 2) == "PM") {
                        //label3.Text = "PM";
                    }
                }
                else
                {
                    //timeEdit1.Value = DateTime.ParseExact("27/08/2024 " + substring[2], "dd/MM/yyyy hh:mm tt", null);
                    //timeEdit1.Value = DateTime.ParseExact("27/08/2024 " + substring[2], "dd/MM/yyyy HH:mm", null);
                    //var date = DateTime.ParseExact("27/08/2024 " + substring[2], "dd/MM/yyyy HH:mm", null);   ///Convert.ToDateTime("27/08/2024 " + substring[2]);
                    //if (DateTime.TryParse("27/08/2024 10:30 AM", out date)) {
                    //    //String.Format("{0:d/MM/yyyy}", dDate);
                    //    timeEdit1.Value = DateTime.ParseExact("27/08/2024 " + substring[2], "dd/MM/yyyy hh:mm tt", null);
                    //} else {
                    //    //Console.WriteLine("Invalid"); // <-- Control flow goes here
                    //    timeEdit1.Value = DateTime.ParseExact("27/08/2024 " + substring[2], "dd/MM/yyyy HH:mm", null);
                    //}
                    DateTime tempDate = DateTime.ParseExact("27/08/2024 " + substring[2], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    string formattedTime = tempDate.ToString("hh:mm tt", CultureInfo.InvariantCulture);
                    timeEdit1.Value = DateTime.ParseExact("27/08/2024 " + formattedTime, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
                    //label3.Text = "";
                }
                if (substring[3].IndexOf("AM") != -1 || substring[3].IndexOf("PM") != -1)
                {
                    timeEdit2.Value = DateTime.ParseExact("27/08/2024 " + substring[3], "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
                    if (substring[3].Substring(6,2) == "AM") {
                        //label4.Text = "AM";
                    } else if (substring[3].Substring(6, 2) == "PM") {
                        //label4.Text = "PM";
                    }
                }
                else
                {
                    //timeEdit2.Value = DateTime.ParseExact("27/08/2024 " + substring[3], "dd/MM/yyyy hh:mm tt", null);

                    //var date = DateTime.ParseExact("27/08/2024 " + substring[3], "dd/MM/yyyy HH:mm", null);   ///Convert.ToDateTime("27/08/2024 " + substring[2]);
                    //if (DateTime.TryParse("27/08/2024 10:30 AM", out date)) {
                    //    //String.Format("{0:d/MM/yyyy}", dDate);
                    //    timeEdit2.Value = DateTime.ParseExact("27/08/2024 " + substring[3], "dd/MM/yyyy hh:mm tt", null);
                    //} else {
                    //    //Console.WriteLine("Invalid"); // <-- Control flow goes here
                    //    timeEdit2.Value = DateTime.ParseExact("27/08/2024 " + substring[3], "dd/MM/yyyy HH:mm", null);
                    //}
                    DateTime tempDate = DateTime.ParseExact("27/08/2024 " + substring[3], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    string formattedTime = tempDate.ToString("hh:mm tt", CultureInfo.InvariantCulture);
                    timeEdit2.Value = DateTime.ParseExact("27/08/2024 " + formattedTime, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
                    //label4.Text = "";
                }
                checkBox12.Checked = false;
            }
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
                    data.typstime = substring[6] == "active" ? "24 hr" : "12 AM/PM";
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
                data.typstime = substring[6] == "active" ? "24 hr" : "12 AM/PM";
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
            var sub = decodedString.Split(',');
            try {
                var type24 = sub[6];
                var startDate = sub[2];
                var endDate = sub[3];

                // case unactive but send 12 hr value
                if (type24 == "unactive" && ((startDate.IndexOf("AM") != -1 && startDate.IndexOf("PM") != -1) || (endDate.IndexOf("AM") != -1 && endDate.IndexOf("PM") != -1))) {
                    
                    sub[2] = DateTime.ParseExact("27/08/2024 " + sub[2], "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    sub[3] = DateTime.ParseExact("27/08/2024 " + sub[3], "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture).ToString("hh:mm tt");
                }
                // case unactive but send 24 hr value
                if (type24 == "unactive" && ((startDate.IndexOf("AM") == -1 && startDate.IndexOf("PM") == -1) || (endDate.IndexOf("AM") == -1 && endDate.IndexOf("PM") == -1))) {
                    sub[2] = DateTime.ParseExact("27/08/2024 " + sub[2], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    sub[3] = DateTime.ParseExact("27/08/2024 " + sub[3], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture).ToString("hh:mm tt");
                }
                // case active but send 24 hr value
                if (type24 == "active" && ((startDate.IndexOf("AM") == -1 && startDate.IndexOf("PM") == -1) || (endDate.IndexOf("AM") == -1 && endDate.IndexOf("PM") == -1))) {
                    sub[2] = DateTime.ParseExact("27/08/2024 " + sub[2], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture).ToString("HH:mm");
                    sub[3] = DateTime.ParseExact("27/08/2024 " + sub[3], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture).ToString("HH:mm");
                }
                // case active but send 12 hr value
                if (type24 == "active" && ((startDate.IndexOf("AM") != -1 || startDate.IndexOf("PM") != -1) || (endDate.IndexOf("AM") != -1 || endDate.IndexOf("PM") != -1))) {
                    sub[2] = DateTime.ParseExact("27/08/2024 " + sub[2], "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture).ToString("HH:mm");
                    sub[3] = DateTime.ParseExact("27/08/2024 " + sub[3], "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture).ToString("HH:mm");
                }

                decodedString = string.Join(",", sub);

                configsocket.SetValue(name, decodedString);
                return "success";
            } catch (Exception ex) {
                return ex.Message;
            }
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
            if (checkBox12.Checked == false)
            {
                this.timeEdit1.CustomFormat = "hh:mm tt";
                //this.timeEdit1.Properties.Mask.EditMask = "hh:mm tt";
                this.timeEdit2.CustomFormat = "hh:mm tt";
                //this.timeEdit2.Properties.Mask.EditMask = "hh:mm tt";

                var timehr1 = this.timeEdit1.Text.Split(' ')[1];
                var timehr2 = this.timeEdit2.Text.Split(' ')[1];
                if (timehr1 == "AM") {
                    //this.label3.Text = "AM";
                } else if (timehr2 == "PM") {
                    //this.label3.Text = "PM";
                }
                if (timehr2 == "AM") {
                    //this.label4.Text = "AM";
                } else if (timehr2 == "PM") {
                    //this.label4.Text = "PM";
                }


            }
            else
            {
                this.timeEdit1.CustomFormat = "HH:mm";
                //this.timeEdit1.Properties.Mask.EditMask = "HH:mm";
                this.timeEdit2.CustomFormat = "HH:mm";
                //this.timeEdit2.Properties.Mask.EditMask = "HH:mm";
                //this.label3.Text = "";
                //this.label4.Text = "";
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
