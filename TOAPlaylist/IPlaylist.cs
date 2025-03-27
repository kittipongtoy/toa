using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TOAMediaPlayer.TOAPlaylist
{
    public interface IPlaylist
    {
        int count { get; }
        string name { get; set; }
        int attributeCount { get; set; }
        string attributeName { get; }
        
        IMedia this[int lIndex] { get;}
        bool isIdentical(IMedia pIWMPMedia);
        string getItemInfo(string bstrName);
        void setItemInfo(string bstrName, string bstrValue);
        void clear();
        void insertItem(int lIndex, IMedia pITOAMedia);
        void appendItem(IMedia pITOAMedia);
        void removeItem(IMedia pITOAMedia);
        void moveItem(int lIndexOld, int lIndexNew);
    }

}
