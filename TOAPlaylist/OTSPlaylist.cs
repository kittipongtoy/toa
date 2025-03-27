using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOAMediaPlayer.TOAPlaylist
{
    public class OTSPlaylist : IPlaylist
    {
        private string _Name = String.Empty;
        private List<IMedia> _MediaList = new List<IMedia>();
        
        public IMedia this[int lIndex] => _MediaList[lIndex];
        public int count => _MediaList.Count;

        public string name
        {
            get => _Name;
            set => _Name = value;
        }
        //Implement later
        public int attributeCount
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        public string attributeName => throw new NotImplementedException();

        public void appendItem(IMedia pIWMPMedia)
        {
            if (pIWMPMedia == null) return;
            _MediaList.Add(pIWMPMedia);
        }

        public void clear()
        {
            _MediaList.Clear();
        }

        public void insertItem(int lIndex, IMedia pIWMPMedia)
        {
            // Insert an Media at index
            if (lIndex < 0 || lIndex >= _MediaList.Count) return;
            _MediaList.Insert(lIndex, pIWMPMedia);
        }
        public bool isIdentical(IMedia pIWMPMedia)
        {
            bool bIdentical = false;
            if (pIWMPMedia == null) return false;
            foreach (IMedia _media in _MediaList)
            {
                _media.Id.CompareTo(pIWMPMedia.Id);
                bIdentical = true;
                break;
            }
            return bIdentical;
        }
        public void moveItem(int lIndexOld, int lIndexNew)
        {
            var item = _MediaList[lIndexOld];
            _MediaList.RemoveAt(lIndexOld);
            _MediaList.Insert(lIndexNew, item);
        }

        public void removeItem(IMedia pIWMPMedia)
        {
            if (pIWMPMedia == null) return;
            _MediaList.Remove(pIWMPMedia);
        }
        #region Extra Properties
        public string getItemInfo(string bstrName)
        {
            throw new NotImplementedException();
        }
        public void setItemInfo(string bstrName, string bstrValue)
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}
