using System;
using System.Windows.Forms;

namespace TOAMediaPlayer.NAudioOutput
{
    public interface INAudioPlugin
    {
        string Name { get; }
        Control CreatePanel(MainPlayer player);
    }
}
