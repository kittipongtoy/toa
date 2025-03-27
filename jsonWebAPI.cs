using System.Collections.Generic;

namespace TOAMediaPlayer
{
    public class jsonWebAPI
    {
        public class ShuffleLoopStatus
        {
            public bool Shuffle { get; set; }
            public bool Loop { get; set; }
        }
        public class ReturnPlayList
        {
            public string Seq { get; set; }
            public string Name { get; set; }
            public string DurationTime { get; set; }
        }

        public class ReturnPlayListMusic
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string DurationTimePlay { get; set; }
            public string DurationTime { get; set; }
            public int runmusic { get; set; }
        }

        public class playlist
        {
            public List<PlayListMusic> playlists { get; set; }
            public int? currentPlaylist { get; set; }
            public bool? isLoop { get; set; }
            public bool? isRandom { get; set; }
            public List<songs> songs { get; set; }
        }
        public class PlayListMusic
        {
            public string title { get; set; }
            public string namemusic { get; set; }
            public string name { get; set; }
            public string fgColor { get; set; }
            public string bgColor { get; set; }
            public bool? isPlaying { get; set; }
            public int? currentSongIndex { get; set; }
            public string currentMinute { get; set; }
            public string durationtotal { get; set; }
            public int volume { get; set; }

        }

        public class songs
        {
            public short index { get; set; }
            public string title { get; set; }
            public string duration { get; set; }
            public string path { get; set; }

            public bool status_file { get; set; }
        }

        public class PlayerList
        {
            public string PlayerID { get; set; }
            public string controltype { get; set; }
        }

        public class MusicErrorList
        {
            public string name { get; set; }
            public string location { get; set; }
            public int PlayerTrack { get; set; }
        }

        public class status_com
        {
            public string namecom { get; set; }
            public string status { get; set; }
        }


        public class stats_timer
        {
            public bool status { get; set; }
            public bool mon { get; set; }
            public bool tue { get; set; }
            public bool wed { get; set; }
            public bool thu { get; set; }
            public bool fri { get; set; }
            public bool sat { get; set; }
            public bool sun { get; set; }
            public bool active_start { get; set; }
            public bool active_end { get; set; }
            public string time_start { get; set; }
            public string time_end { get; set; }
            public string typstime { get; set; }
        }
    }
}
