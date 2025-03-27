using System;
using System.Linq;
using System.Windows.Forms;

namespace TOAMediaPlayer.NAudioOutput
{
    public class AudioPlaybackPanelPlugin : INAudioPlugin
    {
        public string Name
        {
            get { return "TOA Output Driver"; }
        }

        public Control CreatePanel(MainPlayer p)
        {
            return new AudioPlaybackPanel(p);
        }
    }
}
