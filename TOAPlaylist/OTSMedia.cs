using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

using NAudio.Wave;

namespace TOAMediaPlayer.TOAPlaylist
{
    
    public class OTSMedia : IMedia
    {
        TimeSpan _duration = TimeSpan.MaxValue;
        bool _shuffle = false;
        bool _loop = false;
        bool _isIdentical = false;
        string _fileName = string.Empty;
        string _fileLocation = string.Empty;
        PlaybackState _playbackState = PlaybackState.Stopped;
        int _Id = 0;
        public bool isIdentical => _isIdentical;
          
        public string fileName {
            get => _fileName;
            set => _fileName = value;
        }
        public string fileLocation
        {
            get => _fileLocation;
            set => _fileLocation = value;
        }
        public int Id { 
            get => _Id; 
            set => _Id = value; 
        }

        public int markerCount => throw new NotImplementedException();
        public bool Shuffle
        {
            get => _shuffle;
            set => _shuffle = value;
        }
        public bool Loop
        {
            get => _loop;
            set => _loop = value;
        }
        public PlaybackState CurrentPlaybackState
        {
            get => _playbackState;
            set => _playbackState = value;
        }

        public string durationString => throw new NotImplementedException();

        public int attributeCount => throw new NotImplementedException();

        public TimeSpan duration
        {
            get => _duration;
            set => _duration = value;
        }

        public string getAttributeName(int lIndex)
        {
            throw new NotImplementedException();
        }

        public string getItemInfo(string bstrItemName)
        {
            throw new NotImplementedException();
        }

        public string getItemInfoByAtom(int lAtom)
        {
            throw new NotImplementedException();
        }

        public string getMarkerName(int MarkerNum)
        {
            throw new NotImplementedException();
        }

        public double getMarkerTime(int MarkerNum)
        {
            throw new NotImplementedException();
        }

        public bool isMemberOf(IPlaylist pPlaylist)
        {
            throw new NotImplementedException();
        }

        public bool isReadOnlyItem(string bstrItemName)
        {
            throw new NotImplementedException();
        }

        public void setItemInfo(string bstrItemName, string bstrVal)
        {
            throw new NotImplementedException();
        }
    }
}
