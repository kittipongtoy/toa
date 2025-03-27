using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TOAMediaPlayer.TOAPlaylist
{
    public  interface IMedia
    {
        bool isIdentical { get;}
        string fileName { get; set; }
        string fileLocation { get; set; }
        int Id { get; set; }
        int markerCount { get; }
        TimeSpan duration { get; set; }
        string durationString { get; }
        int attributeCount { get; }
        bool Shuffle { get; set; }
        bool Loop { get; set; }

        double getMarkerTime(int MarkerNum);
        string getMarkerName(int MarkerNum);
        string getAttributeName(int lIndex);
        string getItemInfo(string bstrItemName);
        void setItemInfo(string bstrItemName, string bstrVal);
        string getItemInfoByAtom(int lAtom);
        bool isMemberOf(IPlaylist pPlaylist);
        bool isReadOnlyItem(string bstrItemName);

        //int Id { get; set; }
        //string TimeLength { get; set; }
        //string FileName { get; set; }
        //string FileLocation { get; set; }
        //bool NowPlaying { get; set; }
        //bool bShuffle { get; set; }
        //bool bLoop { get; set; }
    }
}
