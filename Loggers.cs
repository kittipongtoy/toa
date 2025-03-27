using System;
using System.IO;
using System.Text;

namespace TOAMediaPlayer
{
    public class Loggers
    {
        public string filename = "";

        public Loggers(string filename)
        {
            this.filename = filename;
            if(!File.Exists(filename))
            {
                using (FileStream fs = File.Create(filename))
                {

                }
            }

        }

        public void Info(string message)
        {
            File.AppendAllText(filename, "[Info] System : " +message+" DateTime:"+Convert.ToDateTime(DateTime.Now).ToString("dd/M/yyyy HH:mm:ss") + Environment.NewLine);
        }
        public void Error(string message)
        {
            File.AppendAllText(filename, "[Error] System : " + message+ " DateTime:" + Convert.ToDateTime(DateTime.Now).ToString("dd/M/yyyy HH:mm:ss") + Environment.NewLine);
        }
        public void Error(Exception ex)
        {
            File.AppendAllText(filename, "[Error] System : " + ex.Message + " DateTime:" + Convert.ToDateTime(DateTime.Now).ToString("dd/M/yyyy HH:mm:ss") + Environment.NewLine);
            File.AppendAllText(filename, "[Error] System : " + ex.InnerException + " DateTime:" + Convert.ToDateTime(DateTime.Now).ToString("dd/M/yyyy HH:mm:ss") + Environment.NewLine);
        }

        public string Read()
        {
            StringBuilder msg = new StringBuilder();
            using (StreamReader sr = File.OpenText(filename))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    msg.AppendLine(s);
                }
            }
            return msg.ToString();
        }

        public string get_Filename()
        {
            return filename;
        }

        public void Clear()
        {
            if (File.Exists(filename))
            {
                File.Delete(filename);
                using (FileStream fs = File.Create(filename))
                {

                }
            }
        }
    }
}
