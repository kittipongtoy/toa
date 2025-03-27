using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOAMediaPlayer
{
    public class MediaAlbum
    {
        public Guid PlayListGuid { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string DirectoryName { get; set; }
        public long FileSize { get; set; }
        public int TotalTime { get; set; }
        public bool Shuffle { get; set; }
        public bool Loop { get; set; }
    }
    public class MediaFolder
    {
        //      private int FileId;
        //      private string TemporaryFilename;
        public WMPLib.WindowsMediaPlayer wplayer;
        //private byte[] SelectedCardAudioFileData = null;
        public MediaFolder()
        {
            wplayer = new WMPLib.WindowsMediaPlayer();
        }

        public void CreateDir(string wplayerID,string PlayListID,string FolderName)
        {
            int FileId;
            string TemporaryFilename;


            //Create directory if necessary
            TemporaryFilename = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + wplayerID + "\\" + PlayListID + "\\" + FolderName + "\\";
            if (!Directory.Exists(Path.GetDirectoryName(TemporaryFilename)))
                Directory.CreateDirectory(Path.GetDirectoryName(TemporaryFilename));

            //Delete any old files if they've been released finally
            for (FileId = 0; FileId < 20; FileId++)
            {
                TemporaryFilename = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + wplayerID + "\\" + PlayListID + "\\" + FolderName + "\\temp_play_file" + FileId + ".mp3";
                if (File.Exists(TemporaryFilename))
                {
                    try
                    {
                        File.SetAttributes(TemporaryFilename, FileAttributes.Normal);
                        if ((File.GetAttributes(TemporaryFilename) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                            File.SetAttributes(TemporaryFilename, FileAttributes.Normal);

                        File.Delete(TemporaryFilename);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            //Write the file
            for (FileId = 0; FileId < 20; FileId++)
            {
                TemporaryFilename = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + wplayerID + "\\" + PlayListID + "\\" + FolderName + "\\temp_play_file" + FileId + ".mp3";
                if (!File.Exists(TemporaryFilename))
                {
                    //Write the file
                    //File.WriteAllBytes(TemporaryFilename, SelectedCardAudioFileData);
                    File.SetAttributes(TemporaryFilename, FileAttributes.Normal);

                    //Play the file
                    wplayer = new WMPLib.WindowsMediaPlayer();
                    wplayer.PlayStateChange += new WMPLib._WMPOCXEvents_PlayStateChangeEventHandler(wplayer_PlayStateChange);
                    wplayer.URL = TemporaryFilename;
                    wplayer.controls.play();
                    break;
                }
            }
        }

        //********** MP3 PLAYER FINISHED PLAYING **********
        // CLEAN RESOURCES
        void wplayer_PlayStateChange(int NewState)
        {
            if (NewState == (int)WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                wplayer.close();
                wplayer = null;

                try
                {
                    System.Diagnostics.Process[] prc = System.Diagnostics.Process.GetProcessesByName("wmplayer");
                    if (prc.Length > 0)
                        prc[prc.Length - 1].Kill();
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
