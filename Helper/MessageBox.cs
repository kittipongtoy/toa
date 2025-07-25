﻿using Microsoft.Win32;
using NLog.Fluent;
using System;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TOAMediaPlayer.NAudioOutput;

namespace TOAMediaPlayer.Helper
{
    public class MessageBox
    {
        // โชว์ error ปกติ ตรงกลาง
        public string ShowCenter_DialogError(string message, string title)
        {
            Form mainForm = Application.OpenForms[0];

            Form form = new Form
            {
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.Manual,
                Size = new Size(400, 150),
                BackColor = Color.LightGray,
                Name = "Warning",
                Owner = mainForm
            };

            var screen = mainForm.Bounds;
            form.Left = screen.Left + (screen.Width - form.Width) / 2;
            form.Top = screen.Top + (screen.Height - form.Height) / 2;

            // ✅ Panel มี Scrollbar
            Panel scrollPanel = new Panel
            {
                AutoScroll = true,
                Dock = DockStyle.Fill,
                BackColor = Color.LightGray
            };
            form.Controls.Add(scrollPanel);

            // 🔹 Header Panel
            Panel headerPanel = new Panel
            {
                BackColor = Color.Black,
                Dock = DockStyle.Top,
                Height = 40
            };
            scrollPanel.Controls.Add(headerPanel);

            PictureBox icon = new PictureBox
            {
                Size = new Size(20, 20),
                Location = new Point(10, 10),
                BackColor = Color.Red
            };
            headerPanel.Controls.Add(icon);

            Label titleLabel = new Label
            {
                Text = title,
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(40, 10),
                AutoSize = true
            };
            headerPanel.Controls.Add(titleLabel);

            // 🔹 เนื้อหาที่อาจยาวมาก
            Label messageLabel = new Label
            {
                Text = message,
                Font = new Font("Arial", 10),
                Location = new Point(20, 60),
                MaximumSize = new Size(350, 0),  // ✅ บังคับให้ข้อความตัดบรรทัด
                AutoSize = true
            };
            scrollPanel.Controls.Add(messageLabel);

            Button closeButton = new Button
            {
                Text = "Ok",
                BackColor = Color.Orange,
                ForeColor = Color.Black,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Size = new Size(100, 30),
                Location = new Point(260, messageLabel.Bottom + 20)
            };
            closeButton.Click += (s, e) => form.Close();
            scrollPanel.Controls.Add(closeButton);

            // ✅ เรียก Trigger
            NPlayer nPlayer = new NPlayer();
            nPlayer.trigger_warning_url(message);

            Task.Run(() =>
            {
                if (mainForm.InvokeRequired)
                {
                    mainForm.Invoke(new Action(() => form.ShowDialog(mainForm)));
                }
                else
                {
                    form.ShowDialog(mainForm);
                }
            });

            return message;
        }

        // โชว์ error connection ขวาล่าง 
        public string ShowRight_DialogError(string message, string title)
        {
            // 🔹 หาฟอร์มหลักของโปรแกรม
            Form mainForm = Application.OpenForms[0];

            // 🔹 สร้าง Dialog
            Form form = new Form
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
            form.Left = screen.Right - form.Width;
            form.Top = screen.Bottom - form.Height;

            // 🔹 Panel Header (สีดำ)
            Panel headerPanel = new Panel
            {
                BackColor = Color.Black,
                Dock = DockStyle.Top,
                Height = 40
            };
            form.Controls.Add(headerPanel);

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
            form.Controls.Add(messageLabel);

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
            closeButton.Click += (s, e) => form.Close(); // ปิด Dialog เมื่อกดปุ่ม
            form.Controls.Add(closeButton);

            // 🔥 ล็อก Dialog ไม่ให้ไปนอกโปรแกรม
            return form.ShowDialog(mainForm) == DialogResult.OK ? message : "";
        }
    }
}
