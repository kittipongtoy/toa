using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOAMediaPlayer.TOAPlaylist
{
    public class OTSPlaylistArray
    {
        List<OTSPlaylist> _PlaylistArray = new List<OTSPlaylist>();
        public OTSPlaylistArray()
        {
            
        }
        public int Count() { return _PlaylistArray.Count; }
        public string Name
        {
            get { return _PlaylistArray[0].name; }
        }
        public IPlaylist Item()
        {
            return _PlaylistArray[0];
        }

    }
}
