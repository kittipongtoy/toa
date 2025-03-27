using NAudio.Wave;
using System.Collections.Generic;

namespace TOAMediaPlayer
{
    public class PlayControl
    {
        private Queue<string> playlist;
        //private IWavePlayer player;
        private WaveStream fileWaveStream;

        public PlayControl(List<string> startingPlaylist)
        {
            playlist = new Queue<string>(startingPlaylist);
        }

        public void PlaySong(IWavePlayer player)
        {
            if (playlist.Count < 1)
            {
                return;
            }

            if (player != null && player.PlaybackState != PlaybackState.Stopped)
            {
                player.Stop();
            }
            if (fileWaveStream != null)
            {
                fileWaveStream.Dispose();
            }
            if (player != null)
            {
                player.Dispose();
                player = null;
            }

            player = new WaveOutEvent();
            fileWaveStream = new AudioFileReader(playlist.Dequeue());
            player.Init(fileWaveStream);
            player.PlaybackStopped += (sender, evn) => { PlaySong(player); };
            player.Play();
        }
    }
}
