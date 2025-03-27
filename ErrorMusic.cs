using System.Collections.Generic;
using System.Windows.Forms;

namespace TOAMediaPlayer
{
    public partial class ErrorMusic : MetroFramework.Forms.MetroForm  //Form
    {
        public ErrorMusic(List<jsonWebAPI.MusicErrorList> data)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // ให้อยู่กลางจอ
            //this.FormBorderStyle = FormBorderStyle.FixedSingle;  // Fixed Size ไม่ให้ผู้ใช้ Resize
            this.MaximizeBox = false; // ปิดปุ่มขยายหน้าจอ
            LoadListView(data);
        }

        private void LoadListView(List<jsonWebAPI.MusicErrorList> data)
        {
            // สร้างคอลัมน์ให้ ListView
            listView1.View = View.Details; // โหมดแสดงผลแบบรายละเอียด

            var messagebox = new Helper.MessageBox();

            if (data == null || data.Count == 0)
            {
                messagebox.ShowCenter_DialogError("ไม่มีข้อมูลเพลงผิดพลาด", "แจ้งเตือน");
                return;
            }

            // เพิ่มข้อมูลใน ListView
            foreach (var tt in data)
            {
                if (tt == null) continue;
                ListViewItem item1 = new ListViewItem(new[] 
                { 
                    tt.PlayerTrack.ToString() ?? "", 
                    tt.name ?? "", 
                    tt.location ?? ""  
                });
                listView1.Items.Add(item1);
            }

            // ตั้งค่าการเลือกไอเท็ม
            listView1.FullRowSelect = true; // ให้เลือกทั้งแถว
        }
    }
}
