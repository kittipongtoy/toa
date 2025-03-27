using MetroFramework.Controls;
using MetroFramework.Forms;
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
    public partial class MetroSplashScreen: MetroForm
    {
        public MetroSplashScreen()
        {
            InitializeComponent();
        }

        private async void MetroSplashScreen_Load(object sender, EventArgs e)
        {
            lblStatus.Text = "กำลังโหลดโปรแกรม...";

            // โหลดข้อมูลแบบ Async
            await Task.Run(async () => await LoadData());

            // ปิด Splash Screen และส่งค่า DialogResult กลับไป
            this.DialogResult = DialogResult.OK;
        }

        private async Task LoadData()
        {
            for (int i = 0; i <= 100; i += 20)
            {
                await Task.Delay(500); // จำลองโหลดข้อมูล

                // อัปเดต UI ใน Thread หลัก
                this.Invoke(new Action(() =>
                {
                    metroProgressBar1.Value = i;
                    lblStatus.Text = $"กำลังโหลด... {i}%";
                }));
            }
        }


    }
}
