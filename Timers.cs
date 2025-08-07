using Microsoft.Win32;
using TOAMediaPlayer.NAudioOutput;

namespace TOAMediaPlayer
{
    public class Timers : NPlayer
    {
        public NPlayer player;
        public string nametrack = "";
        RegistryKey HKLMSoftwareTOAConfig = Registry.CurrentUser.OpenSubKey(@"Software\TOA\Config", true);
        public Timers(NPlayer p,string track)
        {
            
            player = p;
            nametrack = track;
            //Thread t1 = new Thread(new ThreadStart(Thread1));
            //t1.Start();

        }
    }
}
