using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TOAMediaPlayer
{
    public partial class Form1 : Form
    {
        public Loggers files = new Loggers("debugs.txt");
        public CancellationTokenSource tokenSource = new CancellationTokenSource();
        
        public Form1()
        {
            
            InitializeComponent();
            looplog();
            checkBox1.Enabled = false;
            api_run();
        }

        public void api_run()
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "currport.bat";
            p.Start();

            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            var search = output.IndexOf("LISTENING");
            if (search != -1)
            {
                checkBox1.Checked = true;
                
            }
            else
            {
                checkBox1.Checked = false;
            }
        }
        public void looplog()
        {
            listBox1.Items.Clear();
            var reads = files.Read();
            string[] stringSeparators = new string[] { "\r\n" };
            var linewrite = reads.ToString().Split(stringSeparators, StringSplitOptions.None);
            if (linewrite.Length > 0)
            {
                foreach (var msg in linewrite)
                {
                    add_items(msg);
                }
            }
        }

        public void add_items(string msg)
        {
            listBox1.Items.Add(msg);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            files.Clear();
            listBox1.Items.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tokenSource.Cancel();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            looplog();
        }
    }
}
