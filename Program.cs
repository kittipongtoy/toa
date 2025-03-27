using System;
using System.Globalization;
using System.Windows.Forms;

namespace TOAMediaPlayer
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (MetroSplashScreen splash = new MetroSplashScreen())
            {
                if (splash.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new MainPlayer());
                }
            }

            //// แสดง SplashScreen ก่อน
            //SplashScreenForm splash = new SplashScreenForm();
            //splash.Show();
            //Application.DoEvents();  // ให้ UI อัปเดตทันที
            //Application.Run(new MainPlayer());


            ////Application.Run(new ErrorMusic());
        }
    }
}
