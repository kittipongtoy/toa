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
    public partial class LoginAdmin : Form
    {
        private MainPlayer player;
        public LoginAdmin(MainPlayer player)
        {
            InitializeComponent();
            textBox2.PasswordChar = '*';
            textBox2.Select();
            this.player = player;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var user = this.textBox1.Text;
            var pass = this.textBox2.Text;
            if (pass != "")
            {
                if (user.ToLower() == "admin" && pass.ToLower() == "toa12345")
                {
                    this.Close();
                    SetupOutput xForm = new SetupOutput(player);
                    xForm.StartPosition = FormStartPosition.CenterParent;
                    xForm.ShowDialog();
                }
                else
                {
                    textBox2.Text = "";
                    var message = new Helper.MessageBox();
                    message.ShowCenter_DialogError("รหัสผิด กรุณาเช็ครหัสก่อน !!!", "แจ้งเตือน");
                }
            }
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }
    }
}
