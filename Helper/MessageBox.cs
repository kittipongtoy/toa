using Microsoft.Win32;
using NLog.Fluent;
using System;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using TOAMediaPlayer.NAudioOutput;

namespace TOAMediaPlayer.Helper
{
    public class MessageBox
    {
        public Form Form;
        // โชว์ error ปกติ ตรงกลาง
        public string ShowCenter_DialogError(string message, string title)
        {
            // 🔹 หาฟอร์มหลักของโปรแกรม
            Form mainForm = Application.OpenForms[0];

            // 🔹 สร้าง Dialog
            Form = new Form
            {
                FormBorderStyle = FormBorderStyle.None, // ไม่มีขอบหน้าต่าง
                StartPosition = FormStartPosition.Manual,
                Size = new Size(400, 150),
                BackColor = Color.LightGray,
                Name = "Warning",
                Owner = mainForm // 🔥 ล็อกให้อยู่ในโปรแกรม
            };

            // 🔹 คำนวณตำแหน่งให้แสดงที่มุมล่างขวาของหน้าต่างหลัก
            var screen = mainForm.Bounds;
            Form.Left = screen.Left + (screen.Width - Form.Width) / 2;
            Form.Top = screen.Top + (screen.Height - Form.Height) / 2;

            // 🔹 Panel Header (สีดำ)
            Panel headerPanel = new Panel
            {
                BackColor = Color.Black,
                Dock = DockStyle.Top,
                Height = 40
            };
            Form.Controls.Add(headerPanel);

            // 🔹 Icon วงกลมสีแดง
            PictureBox icon = new PictureBox
            {
                Size = new Size(20, 20),
                Location = new Point(10, 10),
                BackColor = Color.Red
            };
            headerPanel.Controls.Add(icon);

            // 🔹 Label "Connection Error"
            Label titleLabel = new Label
            {
                Text = title,
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(40, 10),
                AutoSize = true
            };
            headerPanel.Controls.Add(titleLabel);

            // 🔹 Label ข้อความแจ้งเตือน
            Label messageLabel = new Label
            {
                Text = message,
                Font = new Font("Arial", 10),
                Location = new Point(20, 60),
                AutoSize = true
            };
            Form.Controls.Add(messageLabel);

            // 🔹 ปุ่ม "ปิดโปรแกรม"
            Button closeButton = new Button
            {
                Text = "Ok",
                BackColor = Color.Orange,
                ForeColor = Color.Black,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Size = new Size(100, 30),
                Location = new Point(260, 100)
            };
            closeButton.Click += (s, e) => Form.Close(); // ปิด Dialog เมื่อกดปุ่ม
            Form.Controls.Add(closeButton);
            NPlayer nPlayer = new NPlayer();
            nPlayer.trigger_warning_url(message);
            // 🔥 ล็อก Dialog ไม่ให้ไปนอกโปรแกรม
            return Form.ShowDialog(mainForm) == DialogResult.OK ? message : "";
        }

        // โชว์ error connection ขวาล่าง 
        public string ShowRight_DialogError(string message, string title)
        {
            // 🔹 หาฟอร์มหลักของโปรแกรม
            Form mainForm = Application.OpenForms[0];

            // 🔹 สร้าง Dialog
            Form = new Form
            {
                FormBorderStyle = FormBorderStyle.None, // ไม่มีขอบหน้าต่าง
                StartPosition = FormStartPosition.Manual,
                Size = new Size(400, 150),
                BackColor = Color.LightGray,
                Name = "Warning",
                Owner = mainForm // 🔥 ล็อกให้อยู่ในโปรแกรม
            };

            // 🔹 คำนวณตำแหน่งให้แสดงที่มุมล่างขวาของหน้าต่างหลัก
            var screen = mainForm.Bounds;
            Form.Left = screen.Right - Form.Width;
            Form.Top = screen.Bottom - Form.Height;

            // 🔹 Panel Header (สีดำ)
            Panel headerPanel = new Panel
            {
                BackColor = Color.Black,
                Dock = DockStyle.Top,
                Height = 40
            };
            Form.Controls.Add(headerPanel);

            // 🔹 Icon วงกลมสีแดง
            PictureBox icon = new PictureBox
            {
                Size = new Size(20, 20),
                Location = new Point(10, 10),
                BackColor = Color.Red
            };
            headerPanel.Controls.Add(icon);

            // 🔹 Label "Connection Error"
            Label titleLabel = new Label
            {
                Text = title,
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(40, 10),
                AutoSize = true
            };
            headerPanel.Controls.Add(titleLabel);

            // 🔹 Label ข้อความแจ้งเตือน
            Label messageLabel = new Label
            {
                Text = message,
                Font = new Font("Arial", 10),
                Location = new Point(20, 60),
                AutoSize = true
            };
            Form.Controls.Add(messageLabel);

            // 🔹 ปุ่ม "ปิดโปรแกรม"
            Button closeButton = new Button
            {
                Text = "ปิดโปรแกรม",
                BackColor = Color.Orange,
                ForeColor = Color.Black,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Size = new Size(100, 30),
                Location = new Point(260, 100)
            };
            closeButton.Click += (s, e) => Form.Close(); // ปิด Dialog เมื่อกดปุ่ม
            Form.Controls.Add(closeButton);

            // 🔥 ล็อก Dialog ไม่ให้ไปนอกโปรแกรม
            return Form.ShowDialog(mainForm) == DialogResult.OK ? message : "";
        }
    }
}
