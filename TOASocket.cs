using Microsoft.Win32;
using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using TOAMediaPlayer.NAudioOutput;
using static TOAMediaPlayer.jsonWebAPI;

namespace TOAMediaPlayer
{
    public class PlaylistItem
    {
        public int newIndex { get; set; }
        public string name { get; set; }
        public string duration { get; set; }
        public string path { get; set; }
    }
    /// <summary>
    /// Create : 2022-10-10
    /// By : P'SAM
    /// Simple Http Server
    /// </summary>
    /// 

    public class TOASocket
    {
        private Socket _socket;
        private const int WEB_PORT = 80;
        private Thread _thread;
        private Thread _thread1;
        private const int BACKLOG_QTY = 1;
        private bool ServerStarted = false;
        public MainPlayer pl;
        public Loggers log = new Loggers("debugs.txt");
        RegistryKey HKLMSoftwareTOAConfig = Registry.CurrentUser.OpenSubKey(@"Software\TOA\Config", true);
        public TOASocket(MainPlayer pl)
        {
            this.pl = pl;

        }

        public void detect_file()
        {

            for (int i = 1; i <= 8; i++)
            {
                string folderPath = String.Format("{0}\\music\\{1}", System.Environment.CurrentDirectory, i);

                // สร้าง FileSystemWatcher
                FileSystemWatcher watcher = new FileSystemWatcher();
                watcher.Path = folderPath;

                // กำหนดชนิดของการเปลี่ยนแปลงที่ต้องการตรวจจับ
                watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size;

                // ตรวจจับการเพิ่มไฟล์ใหม่
                watcher.Created += OnNewFileDetected;
                watcher.Renamed += OnNewFileDetected;
                watcher.Changed += OnNewFileDetected;
                watcher.Deleted += OnNewFileDetected;

                // เริ่มตรวจจับ
                watcher.EnableRaisingEvents = true;
            }
        }
        private void OnNewFileDetected(object sender, FileSystemEventArgs e)
        {
            if (pl.InvokeRequired)
            {
                pl.Invoke(new Action(() =>
                {
                    String[] playlist = e.FullPath.Split(new[] { "\\" }, StringSplitOptions.None);
                    short playlistId = Convert.ToInt16(playlist[playlist.Length - 2]);

                    string fileName = String.Format("{0}\\{1}-List.txt", System.Environment.CurrentDirectory, playlistId);

                    if (!System.IO.File.Exists(fileName))
                    {
                        using (System.IO.File.Create(fileName));
                    }
                    if (System.IO.File.Exists(fileName))
                    {
                        // รายชื่อไฟล์เพลงในโฟลเดอร์ music/{playlistId}
                        List<String> textFile = System.IO.File.ReadAllLines(fileName).ToList();
                        List<PlaylistItem> textFiles = new List<PlaylistItem>();

                        List<String> pmntFiles = new List<String>();

                        string musicPath = String.Format("{0}\\music\\{1}", System.Environment.CurrentDirectory, playlistId);
                        DirectoryInfo dir = new DirectoryInfo(musicPath);

                        FileInfo[] allMusicFiles = dir.GetFiles();

                        // รายชื่อไฟล์เพลงในโฟลเดอร์ music/{playlistId}
                        List<PlaylistItem> musicFiles = new List<PlaylistItem>();

                        foreach (string _item in textFile)
                        {
                            string[] items = _item.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                            if (
                                !items[3].Contains(musicPath) ||
                                (
                                    items[3].Contains(musicPath) &&
                                    System.IO.File.Exists(items[2] + "/" + items[3])
                                )
                            )
                            {
                                PlaylistItem music_file = new PlaylistItem();
                                music_file.name = items[1];
                                music_file.duration = items[2];
                                music_file.path = items[3];
                                textFiles.Add(music_file);
                            }
                        }

                        foreach (FileInfo _item in allMusicFiles)
                        {
                            try
                            {
                                using (var fs = new FileStream(_item.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
                                {
                                    Console.WriteLine(fs);

                                    string songDuration = string.Format("{0:hh\\:mm\\:ss}", CoreLibrary.GetNAudoSongLength(_item.FullName));

                                    PlaylistItem music_file = new PlaylistItem();
                                    music_file.name = _item.Name;
                                    music_file.path = _item.FullName;
                                    music_file.duration = songDuration;

                                    if (!textFiles.Contains(music_file))
                                    {
                                        musicFiles.Add(music_file);
                                    }
                                }
                            }
                            catch
                            {
                                Thread.Sleep(1000);
                            }
                            
                        }

                        textFiles = textFiles.Concat(musicFiles).ToList();

                        if (System.IO.File.Exists(fileName))
                        {
                            System.IO.File.Delete(fileName);
                        }

                        using (TextWriter tw = new StreamWriter(new FileStream(fileName, FileMode.Create), Encoding.UTF8))
                        {
                            for (var i = 0; i < textFiles.Count; i++)
                            {
                                tw.WriteLine((i + 1) + "\t" + textFiles[i].name + "\t" + textFiles[i].duration + "\t" + textFiles[i].path + "\t");
                            }
                        }
                        if (pl.lastId == playlistId)
                        {
                            pl.myListView.Items.Clear();
                            pl.LoadDefaultPlaylist(playlistId);
                        }

                    }
                }));
            }
            else
            {
                String[] playlist = e.FullPath.Split(new[] { "\\" }, StringSplitOptions.None);
                short playlistId = Convert.ToInt16(playlist[playlist.Length - 2]);

                string fileName = String.Format("{0}\\{1}-List.txt", System.Environment.CurrentDirectory, playlistId);
                if (!System.IO.File.Exists(fileName))
                {
                    using (System.IO.File.Create(fileName));
                }
                if (System.IO.File.Exists(fileName))
                {
                    // รายชื่อไฟล์เพลงในโฟลเดอร์ music/{playlistId}
                    List<String> textFile = System.IO.File.ReadAllLines(fileName).ToList();
                    List<PlaylistItem> textFiles = new List<PlaylistItem>();

                    List<String> pmntFiles = new List<String>();

                    string musicPath = String.Format("{0}\\music\\{1}", System.Environment.CurrentDirectory, playlistId);
                    DirectoryInfo dir = new DirectoryInfo(musicPath);

                    FileInfo[] allMusicFiles = dir.GetFiles();

                    // รายชื่อไฟล์เพลงในโฟลเดอร์ music/{playlistId}
                    List<PlaylistItem> musicFiles = new List<PlaylistItem>();

                    foreach (string _item in textFile)
                    {
                        string[] items = _item.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        if (
                            !items[3].Contains(musicPath) ||
                            (
                                items[3].Contains(musicPath) &&
                                System.IO.File.Exists(items[2] + "/" + items[3])
                            )
                        )
                        {
                            PlaylistItem music_file = new PlaylistItem();
                            music_file.name = items[1];
                            music_file.duration = items[2];
                            music_file.path = items[3];
                            textFiles.Add(music_file);
                        }
                    }

                    foreach (FileInfo _item in allMusicFiles)
                    {
                        try
                        {
                            using (var fs = new FileStream(_item.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            Console.WriteLine(fs);
                            string songDuration = string.Format("{0:hh\\:mm\\:ss}", CoreLibrary.GetNAudoSongLength(_item.FullName));

                            PlaylistItem music_file = new PlaylistItem();
                            music_file.name = _item.Name;
                            music_file.path = _item.DirectoryName;
                            music_file.duration = songDuration;

                            if (!textFiles.Contains(music_file))
                            {
                                musicFiles.Add(music_file);
                            }
                        }
                        }
                        catch
                        {
                            Thread.Sleep(1000);
                        }
                    }

                    textFiles = textFiles.Concat(musicFiles).ToList();

                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    using (TextWriter tw = new StreamWriter(new FileStream(fileName, FileMode.Create), Encoding.UTF8))
                    {
                        for (var i = 0; i < textFiles.Count; i++)
                        {
                            tw.WriteLine((i + 1) + "\t" + textFiles[i].name + "\t" + textFiles[i].duration + "\t" + textFiles[i].path + "\t");
                        }
                    }
                    if (pl.lastId == playlistId)
                    {
                        pl.myListView.Items.Clear();
                        pl.LoadDefaultPlaylist(playlistId);
                    }

                }
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();

            //foreach (string _item in data)
            //{
            //string[] items = _item.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
            //string path = items[3];
            //string musicFolderPath = String.Format("{0}\\music\\{1}", System.Environment.CurrentDirectory, playlistId);

            //if (path.Contains(musicFolderPath))
            //{
            //    songInMusic.Add(items);
            //}

            //}
            //Console.WriteLine($"New file detected on folder: {playlistId}");
            //Console.WriteLine($"New file detected: {e.FullPath}");
        }
        public bool Start()
        {
            try
            {
                RegistryKey configsocket = this.HKLMSoftwareTOAConfig.OpenSubKey("trackname", true);
                var prc = new ProcManager();
                prc.KillByPort(Convert.ToInt32(configsocket.GetValue("port")));
                _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                //_socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                _thread = new Thread(new ThreadStart(ConnectionThreadMethod));
                _thread1 = new Thread(new ThreadStart(detect_file));
                _thread.Start();
                //_thread1.Start();
                ServerStarted = true;
            }
            catch (Exception ex)
            {
                NSEventLog.Write(EventLogEntryType.Information, "Start Socket 80", String.Format("Message: {0}", ex.Message), LogScope.TOA_Socket);
                _socket.Close();
            }
            return ServerStarted;
        }
        public void Stop()
        {
            if (!ServerStarted) return;
            try
            {
                _socket.Close();
                _thread.Abort();
                //_thread1.Abort();
                ServerStarted = false;
            }
            catch (Exception ex)
            {
                NSEventLog.Write(EventLogEntryType.Information, "Stop Socket Failed", String.Format("Message: {0}", ex.Message), LogScope.TOA_Socket);
            }
        }

        public void ConnectionThreadMethod()
        {
            try
            {
                RegistryKey configsocket = this.HKLMSoftwareTOAConfig.OpenSubKey("trackname", true);
                IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, Convert.ToInt32(configsocket.GetValue("port")));
                _socket.Bind(iPEndPoint);
                _socket.Listen(BACKLOG_QTY);
                StartListening();
            }
            catch (Exception ex)
            {
                NSEventLog.Write(EventLogEntryType.Information, "ConnectionThreadMethod", String.Format("Message: {0}", ex.Message), LogScope.TOA_Socket);
            }
            //TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), WEB_PORT);
            //server.Start();
            //Console.WriteLine("Server has started on 127.0.0.1:80.{0}Waiting for a connection...", Environment.NewLine);

            //TcpClient client = server.AcceptTcpClient();
            //Console.WriteLine("A client connected.");
        }
        public void StartListening()
        {
            try
            {
                while (true)
                {
                    DateTime dateTime = DateTime.Now;
                    String request = String.Empty;
                    byte[] bytes = new byte[2048];
                    Socket client = _socket.Accept();
                    //Reading the inbound connection data
                    while (true)
                    {
                        int numBytes = client.Receive(bytes);
                        request += Encoding.ASCII.GetString(bytes, 0, numBytes);
                        if (request.IndexOf("\r\n") > -1) break;
                    }
                    //GET , POST, PUT, PATCH, DELETE
                    //GET
                    string pattern = @"PlayerID=([0-8\-])";
                    string pattern1 = @"controltype=([^&]*)([\s\S]*?)\sHTTP";
                    string pattern2 = @"sec=([0-9]+(?:\.[0-9]+)?)";
                    string pattern3 = @"music=([^&]*)([\s\S]*?)\sHTTP";
                    string pattern4 = @"text=([^&]*)([\s\S]*?)\sHTTP";
                    string pattern5 = @"bgcolor=([^&]*)([\s\S]*?)\sHTTP";
                    string pattern6 = @"fgcolor=([^&]*)([\s\S]*?)\sHTTP";
                    string pattern7 = @"dmusic=([^&]*)([\s\S]*?)\sHTTP";
                    string pattern8 = @"configs=([^&]*)([\s\S]*?)\sHTTP";
                    string pattern9 = @"base64file=([^&]*)([\s\S]*?)\sHTTP";
                    string pattern10 = @"playlist=([^&]*)([\s\S]*?)\sHTTP";

                    string retJsonString = string.Empty;
                    try
                    {
                        //Match match = Regex.Match(request, pattern, RegexOptions.IgnoreCase);
                        //{
                        //    Console.WriteLine(match.Groups[1].Value);
                        //}
                        //Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
                        //// Match the regular expression pattern against a text string.
                        //Match match = r.Match(request);

                        Match match = Regex.Match(request, pattern, RegexOptions.IgnoreCase);
                        Match match1 = Regex.Match(request, pattern1, RegexOptions.IgnoreCase);
                        Match match2 = Regex.Match(request, pattern2, RegexOptions.IgnoreCase);
                        Match match3 = Regex.Match(request, pattern3, RegexOptions.IgnoreCase);
                        Match match4 = Regex.Match(request, pattern4, RegexOptions.IgnoreCase);
                        Match match5 = Regex.Match(request, pattern5, RegexOptions.IgnoreCase);
                        Match match6 = Regex.Match(request, pattern6, RegexOptions.IgnoreCase);
                        Match match7 = Regex.Match(request, pattern7, RegexOptions.IgnoreCase);
                        Match match8 = Regex.Match(request, pattern8, RegexOptions.IgnoreCase);
                        Match match9 = Regex.Match(request, pattern9, RegexOptions.IgnoreCase);
                        Match match10 = Regex.Match(request, pattern10, RegexOptions.IgnoreCase);

                        //int matchCount = 0;
                        while (match.Success)
                        {
                            //Console.WriteLine("Match" + (++matchCount));
                            for (int i = 1; i <= 1; i++)
                            {
                                Group g = match.Groups[i];
                                Group g1 = match1.Groups[i];
                                Group g2 = match2.Groups[i];
                                Group g3 = match3.Groups[i];
                                Group g4 = match4.Groups[i];
                                Group g5 = match5.Groups[i];
                                Group g6 = match6.Groups[i];
                                Group g7 = match7.Groups[i];
                                Group g8 = match8.Groups[i];
                                Group g9 = match9.Groups[i];
                                Group g10 = match10.Groups[i];

                                //dynamic g10json = JsonConvert.DeserializeObject(g10.Value);
                                // 2. แปลง JSON เป็น List<PlaylistItem>
                                List<PlaylistItem> playlistData = new List<PlaylistItem>();

                                if (g10.Value != "")
                                {
                                    string decoder = WebUtility.UrlDecode(g10.Value);
                                    playlistData = JsonConvert.DeserializeObject<List<PlaylistItem>>(decoder);
                                }

                                Console.WriteLine("Group" + 0 + "='" + g + "'");
                                retJsonString = GetReponsePlayList(
                                    g.Value, 
                                    g1.Value, 
                                    g2.Value, 
                                    g3.Value, 
                                    g4.Value, 
                                    g5.Value, 
                                    g6.Value, 
                                    g7.Value, 
                                    g8.Value, 
                                    g9.Value,
                                    playlistData
                                );
                                //CaptureCollection cc = g.Captures;
                                //for (int j = 0; j < cc.Count; j++)
                                //{
                                //    Capture c = cc[j];
                                //    //retJsonString = Tr(i);
                                //    System.Console.WriteLine("Capture" + j + "='" + c + "', Position=" + c.Index);
                                //}
                            }
                            match = match.NextMatch();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        Stop();
                        Start();
                    }
                    //if (new Regex("^GET").IsMatch(request))
                    //{
                    //    Regex _regx = new Regex(@"\b\w+GET\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    //    MatchCollection matchedsx = _regx.Matches(request);
                    //}
                    //else if (new Regex("^GET").IsMatch(request))
                    //{
                    //}
                    //Data Read
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append("\r\n\r\n");
                    stringBuilder.Append(request);
                    stringBuilder.Append("\r\n----- End of Request -----");
                    NSEventLog.Write(EventLogEntryType.Information, "Request", stringBuilder.ToString(), LogScope.TOA_Socket);

                    //HTML Response
                    //String header = "HTTP/1.1 200 TOA is Fine\nServer: TOA Multi-Player\nContent-Type: text/html; charset: UTF-8\n\n";
                    //String body = "<!doctype html>" +
                    //              "<html>" +
                    //              "<head><title>TOA Multi-Player </title></head>" +
                    //              "<body>" +
                    //              "<h4>Server Time is :" + dateTime.ToString() + "</h4>" +
                    //              "</body>" +
                    //              "</html>";

                    //JSON Reponse
                    String header = "HTTP/1.1 200\nServer: Player API\nAccept-Language: th\nContent-type:application/json;Charset: UTF-8\n\n";
                    //String header = "HTTP/1.1 200 TOA is Fine\nServer: TOA Web API\nContent-type:application/json; charset=utf-8\n";
                    String body = retJsonString;


                    String response = header + body;
                    byte[] resData = Encoding.UTF8.GetBytes(response);
                    client.SendTo(resData, client.RemoteEndPoint);
                    client.Close();
                }
            }
            catch (Exception ex)
            {
                Stop();
                Start();
                System.Diagnostics.Debug.WriteLine(ex.InnerException);

                log.Error(ex);
            }
        }


        private string GetReponsePlayList(string PlayerID, string controlType, string sec, 
                string music, string text, string bgcolor, string fgcolor, string dmusic, 
                string configs, string base64file_import, List<PlaylistItem> playlistData)
        {
            //JObject sl = JObject.Parse(@"{
            //    'Shuffle' : true,
            //    'Loop' : false
            //}");

            controlType = controlType.Replace("&", "");
            if (music != null && music != "")
            {
                music = music.Replace(" HTTP", "");
            }
            string json = string.Empty;
            string fileName = String.Format("{0}\\{1}-List.txt", System.Environment.CurrentDirectory, PlayerID);
            List<ReturnPlayList> lines = new List<ReturnPlayList>();
            List<ReturnPlayListMusic> lines1 = new List<ReturnPlayListMusic>();


            if (controlType.ToLower().Trim() == "play_list_music")
            {
                if (PlayerID == "0")
                {

                    if (pl.nPlayer1.wavePlayer == null)
                    {

                        ReturnPlayListMusic cList = new ReturnPlayListMusic();
                        cList.Id = 1;// ชื่อเพลง
                        cList.Name = "ยังไม่ได้เปิดโปรแกรมเล่นเพลง";// ชื่อเพลง
                        cList.DurationTimePlay = "00:00";// เวลา
                        cList.DurationTime = "00:00";// เวลาทั้งหมด

                        //item.SubItems.Add(items[4]);
                        lines1.Add(cList);

                    }
                    else
                    {
                        ReturnPlayListMusic cList = new ReturnPlayListMusic();

                        if (pl.nPlayer1.audioFileReader == null)
                        {
                            cList.Id = 1;// ชื่อเพลง
                            cList.Name = "รอเพลงสักครู่...";// ชื่อเพลง
                            cList.DurationTimePlay = "00:00";// เวลา
                            cList.DurationTime = "00:00";// เวลาทั้งหมด
                            cList.runmusic = 0;
                            //item.SubItems.Add(items[4]);
                            lines1.Add(cList);
                        }
                        else
                        {
                            if (pl.nPlayer1.returnRunMusic() == true)
                            {
                                cList.runmusic = 1;
                            }
                            else
                            {
                                cList.runmusic = 0;
                            }
                            TimeSpan currentTime = pl.nPlayer1.audioFileReader.CurrentTime;
                            var namemusic = pl.nPlayer1.audioFileReader.FileName.Split('\\');
                            cList.Id = 1;// ชื่อเพลง
                            cList.Name = namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", "");// ชื่อเพลง
                            cList.DurationTimePlay = String.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds);// เวลา
                            cList.DurationTime = String.Format("{0:00}:{1:00}", pl.nPlayer1.audioFileReader.TotalTime.Minutes, pl.nPlayer1.audioFileReader.TotalTime.Seconds);// เวลาทั้งหมด

                            //item.SubItems.Add(items[4]);
                            lines1.Add(cList);
                        }
                    }
                    if (pl.nPlayer2.wavePlayer == null)
                    {

                        ReturnPlayListMusic cList = new ReturnPlayListMusic();
                        cList.Id = 2;// ชื่อเพลง
                        cList.Name = "ยังไม่ได้เปิดโปรแกรมเล่นเพลง";// ชื่อเพลง
                        cList.DurationTimePlay = "00:00";// เวลา
                        cList.DurationTime = "00:00";// เวลาทั้งหมด
                        cList.runmusic = 0;
                        //item.SubItems.Add(items[4]);
                        lines1.Add(cList);

                    }
                    else
                    {
                        ReturnPlayListMusic cList = new ReturnPlayListMusic();

                        if (pl.nPlayer2.audioFileReader == null)
                        {
                            cList.Id = 2;// ชื่อเพลง
                            cList.Name = "รอเพลงสักครู่...";// ชื่อเพลง
                            cList.DurationTimePlay = "00:00";// เวลา
                            cList.DurationTime = "00:00";// เวลาทั้งหมด

                            //item.SubItems.Add(items[4]);
                            lines1.Add(cList);
                        }
                        else
                        {
                            if (pl.nPlayer2.returnRunMusic() == true)
                            {
                                cList.runmusic = 1;
                            }
                            else
                            {
                                cList.runmusic = 0;
                            }
                            TimeSpan currentTime = pl.nPlayer2.audioFileReader.CurrentTime;
                            var namemusic = pl.nPlayer2.audioFileReader.FileName.Split('\\');
                            cList.Id = 2;// ชื่อเพลง
                            cList.Name = namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", "");// ชื่อเพลง
                            cList.DurationTimePlay = String.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds);// เวลา
                            cList.DurationTime = String.Format("{0:00}:{1:00}", pl.nPlayer2.audioFileReader.TotalTime.Minutes, pl.nPlayer2.audioFileReader.TotalTime.Seconds);// เวลาทั้งหมด

                            //item.SubItems.Add(items[4]);
                            lines1.Add(cList);
                        }
                    }
                    if (pl.nPlayer3.wavePlayer == null)
                    {

                        ReturnPlayListMusic cList = new ReturnPlayListMusic();
                        cList.Id = 3;// ชื่อเพลง
                        cList.Name = "ยังไม่ได้เปิดโปรแกรมเล่นเพลง";// ชื่อเพลง
                        cList.DurationTimePlay = "00:00";// เวลา
                        cList.DurationTime = "00:00";// เวลาทั้งหมด
                        cList.runmusic = 0;
                        //item.SubItems.Add(items[4]);
                        lines1.Add(cList);

                    }
                    else
                    {
                        ReturnPlayListMusic cList = new ReturnPlayListMusic();

                        if (pl.nPlayer3.audioFileReader == null)
                        {
                            cList.Id = 3;// ชื่อเพลง
                            cList.Name = "รอเพลงสักครู่...";// ชื่อเพลง
                            cList.DurationTimePlay = "00:00";// เวลา
                            cList.DurationTime = "00:00";// เวลาทั้งหมด

                            //item.SubItems.Add(items[4]);
                            lines1.Add(cList);
                        }
                        else
                        {
                            if (pl.nPlayer3.returnRunMusic() == true)
                            {
                                cList.runmusic = 1;
                            }
                            else
                            {
                                cList.runmusic = 0;
                            }
                            TimeSpan currentTime = pl.nPlayer3.audioFileReader.CurrentTime;
                            var namemusic = pl.nPlayer3.audioFileReader.FileName.Split('\\');
                            cList.Id = 3;// ชื่อเพลง
                            cList.Name = namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", "");// ชื่อเพลง
                            cList.DurationTimePlay = String.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds);// เวลา
                            cList.DurationTime = String.Format("{0:00}:{1:00}", pl.nPlayer3.audioFileReader.TotalTime.Minutes, pl.nPlayer3.audioFileReader.TotalTime.Seconds);// เวลาทั้งหมด

                            //item.SubItems.Add(items[4]);
                            lines1.Add(cList);
                        }
                    }

                    if (pl.nPlayer4.wavePlayer == null)
                    {

                        ReturnPlayListMusic cList = new ReturnPlayListMusic();
                        cList.Id = 4;// ชื่อเพลง
                        cList.Name = "ยังไม่ได้เปิดโปรแกรมเล่นเพลง";// ชื่อเพลง
                        cList.DurationTimePlay = "00:00";// เวลา
                        cList.DurationTime = "00:00";// เวลาทั้งหมด
                        cList.runmusic = 0;
                        //item.SubItems.Add(items[4]);
                        lines1.Add(cList);

                    }
                    else
                    {
                        ReturnPlayListMusic cList = new ReturnPlayListMusic();

                        if (pl.nPlayer4.audioFileReader == null)
                        {
                            cList.Id = 4;// ชื่อเพลง
                            cList.Name = "รอเพลงสักครู่...";// ชื่อเพลง
                            cList.DurationTimePlay = "00:00";// เวลา
                            cList.DurationTime = "00:00";// เวลาทั้งหมด

                            //item.SubItems.Add(items[4]);
                            lines1.Add(cList);
                        }
                        else
                        {
                            if (pl.nPlayer4.returnRunMusic() == true)
                            {
                                cList.runmusic = 1;
                            }
                            else
                            {
                                cList.runmusic = 0;
                            }
                            TimeSpan currentTime = pl.nPlayer4.audioFileReader.CurrentTime;
                            var namemusic = pl.nPlayer4.audioFileReader.FileName.Split('\\');
                            cList.Id = 4;// ชื่อเพลง
                            cList.Name = namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", "");// ชื่อเพลง
                            cList.DurationTimePlay = String.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds);// เวลา
                            cList.DurationTime = String.Format("{0:00}:{1:00}", pl.nPlayer4.audioFileReader.TotalTime.Minutes, pl.nPlayer4.audioFileReader.TotalTime.Seconds);// เวลาทั้งหมด

                            //item.SubItems.Add(items[4]);
                            lines1.Add(cList);
                        }
                    }

                    if (pl.nPlayer5.wavePlayer == null)
                    {

                        ReturnPlayListMusic cList = new ReturnPlayListMusic();
                        cList.Id = 5;// ชื่อเพลง
                        cList.Name = "ยังไม่ได้เปิดโปรแกรมเล่นเพลง";// ชื่อเพลง
                        cList.DurationTimePlay = "00:00";// เวลา
                        cList.DurationTime = "00:00";// เวลาทั้งหมด
                        cList.runmusic = 0;
                        //item.SubItems.Add(items[4]);
                        lines1.Add(cList);

                    }
                    else
                    {
                        ReturnPlayListMusic cList = new ReturnPlayListMusic();

                        if (pl.nPlayer5.audioFileReader == null)
                        {
                            cList.Id = 5;// ชื่อเพลง
                            cList.Name = "รอเพลงสักครู่...";// ชื่อเพลง
                            cList.DurationTimePlay = "00:00";// เวลา
                            cList.DurationTime = "00:00";// เวลาทั้งหมด

                            //item.SubItems.Add(items[4]);
                            lines1.Add(cList);
                        }
                        else
                        {
                            if (pl.nPlayer5.returnRunMusic() == true)
                            {
                                cList.runmusic = 1;
                            }
                            else
                            {
                                cList.runmusic = 0;
                            }
                            TimeSpan currentTime = pl.nPlayer5.audioFileReader.CurrentTime;
                            var namemusic = pl.nPlayer5.audioFileReader.FileName.Split('\\');
                            cList.Id = 5;// ชื่อเพลง
                            cList.Name = namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", "");// ชื่อเพลง
                            cList.DurationTimePlay = String.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds);// เวลา
                            cList.DurationTime = String.Format("{0:00}:{1:00}", pl.nPlayer5.audioFileReader.TotalTime.Minutes, pl.nPlayer5.audioFileReader.TotalTime.Seconds);// เวลาทั้งหมด

                            //item.SubItems.Add(items[4]);
                            lines1.Add(cList);
                        }
                    }

                    if (pl.nPlayer6.wavePlayer == null)
                    {

                        ReturnPlayListMusic cList = new ReturnPlayListMusic();
                        cList.Id = 6;// ชื่อเพลง
                        cList.Name = "ยังไม่ได้เปิดโปรแกรมเล่นเพลง";// ชื่อเพลง
                        cList.DurationTimePlay = "00:00";// เวลา
                        cList.DurationTime = "00:00";// เวลาทั้งหมด
                        cList.runmusic = 0;
                        //item.SubItems.Add(items[4]);
                        lines1.Add(cList);

                    }
                    else
                    {
                        ReturnPlayListMusic cList = new ReturnPlayListMusic();

                        if (pl.nPlayer6.audioFileReader == null)
                        {
                            cList.Id = 6;// ชื่อเพลง
                            cList.Name = "รอเพลงสักครู่...";// ชื่อเพลง
                            cList.DurationTimePlay = "00:00";// เวลา
                            cList.DurationTime = "00:00";// เวลาทั้งหมด

                            //item.SubItems.Add(items[4]);
                            lines1.Add(cList);
                        }
                        else
                        {
                            if (pl.nPlayer6.returnRunMusic() == true)
                            {
                                cList.runmusic = 1;
                            }
                            else
                            {
                                cList.runmusic = 0;
                            }
                            TimeSpan currentTime = pl.nPlayer6.audioFileReader.CurrentTime;
                            var namemusic = pl.nPlayer6.audioFileReader.FileName.Split('\\');
                            cList.Id = 6;// ชื่อเพลง
                            cList.Name = namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", "");// ชื่อเพลง
                            cList.DurationTimePlay = String.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds);// เวลา
                            cList.DurationTime = String.Format("{0:00}:{1:00}", pl.nPlayer6.audioFileReader.TotalTime.Minutes, pl.nPlayer6.audioFileReader.TotalTime.Seconds);// เวลาทั้งหมด

                            //item.SubItems.Add(items[4]);
                            lines1.Add(cList);
                        }
                    }

                    if (pl.nPlayer7.wavePlayer == null)
                    {

                        ReturnPlayListMusic cList = new ReturnPlayListMusic();
                        cList.Id = 7;// ชื่อเพลง
                        cList.Name = "ยังไม่ได้เปิดโปรแกรมเล่นเพลง";// ชื่อเพลง
                        cList.DurationTimePlay = "00:00";// เวลา
                        cList.DurationTime = "00:00";// เวลาทั้งหมด
                        cList.runmusic = 0;
                        //item.SubItems.Add(items[4]);
                        lines1.Add(cList);

                    }
                    else
                    {
                        ReturnPlayListMusic cList = new ReturnPlayListMusic();

                        if (pl.nPlayer7.audioFileReader == null)
                        {
                            cList.Id = 7;// ชื่อเพลง
                            cList.Name = "รอเพลงสักครู่...";// ชื่อเพลง
                            cList.DurationTimePlay = "00:00";// เวลา
                            cList.DurationTime = "00:00";// เวลาทั้งหมด

                            //item.SubItems.Add(items[4]);
                            lines1.Add(cList);
                        }
                        else
                        {
                            if (pl.nPlayer7.returnRunMusic() == true)
                            {
                                cList.runmusic = 1;
                            }
                            else
                            {
                                cList.runmusic = 0;
                            }
                            TimeSpan currentTime = pl.nPlayer7.audioFileReader.CurrentTime;
                            var namemusic = pl.nPlayer7.audioFileReader.FileName.Split('\\');
                            cList.Id = 7;// ชื่อเพลง
                            cList.Name = namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", "");// ชื่อเพลง
                            cList.DurationTimePlay = String.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds);// เวลา
                            cList.DurationTime = String.Format("{0:00}:{1:00}", pl.nPlayer7.audioFileReader.TotalTime.Minutes, pl.nPlayer7.audioFileReader.TotalTime.Seconds);// เวลาทั้งหมด

                            //item.SubItems.Add(items[4]);
                            lines1.Add(cList);
                        }
                    }

                    if (pl.nPlayer8.wavePlayer == null)
                    {

                        ReturnPlayListMusic cList = new ReturnPlayListMusic();
                        cList.Id = 8;// ชื่อเพลง
                        cList.Name = "ยังไม่ได้เปิดโปรแกรมเล่นเพลง";// ชื่อเพลง
                        cList.DurationTimePlay = "00:00";// เวลา
                        cList.DurationTime = "00:00";// เวลาทั้งหมด
                        cList.runmusic = 0;
                        //item.SubItems.Add(items[4]);
                        lines1.Add(cList);

                    }
                    else
                    {
                        ReturnPlayListMusic cList = new ReturnPlayListMusic();

                        if (pl.nPlayer8.audioFileReader == null)
                        {
                            cList.Id = 8;// ชื่อเพลง
                            cList.Name = "รอเพลงสักครู่...";// ชื่อเพลง
                            cList.DurationTimePlay = "00:00";// เวลา
                            cList.DurationTime = "00:00";// เวลาทั้งหมด

                            //item.SubItems.Add(items[4]);
                            lines1.Add(cList);
                        }
                        else
                        {
                            if (pl.nPlayer8.returnRunMusic() == true)
                            {
                                cList.runmusic = 1;
                            }
                            else
                            {
                                cList.runmusic = 0;
                            }
                            TimeSpan currentTime = pl.nPlayer8.audioFileReader.CurrentTime;
                            var namemusic = pl.nPlayer8.audioFileReader.FileName.Split('\\');
                            cList.Id = 8;// ชื่อเพลง
                            cList.Name = namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", "");// ชื่อเพลง
                            cList.DurationTimePlay = String.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds);// เวลา
                            cList.DurationTime = String.Format("{0:00}:{1:00}", pl.nPlayer8.audioFileReader.TotalTime.Minutes, pl.nPlayer8.audioFileReader.TotalTime.Seconds);// เวลาทั้งหมด

                            //item.SubItems.Add(items[4]);
                            lines1.Add(cList);
                        }
                    }

                    json = JsonConvert.SerializeObject(lines1, Formatting.Indented);
                }
                else if (PlayerID == "1")
                {
                    if (pl.nPlayer1.wavePlayer == null) { json = JsonConvert.SerializeObject("ยังไม่ได้เปิดเครื่องเล่น", Formatting.Indented); return json; }
                    if (pl.nPlayer1.audioFileReader == null) { json = JsonConvert.SerializeObject("Wait", Formatting.Indented); return json; }
                    ReturnPlayListMusic cList = new ReturnPlayListMusic();
                    TimeSpan currentTime = pl.nPlayer1.audioFileReader.CurrentTime;
                    var namemusic = pl.nPlayer1.audioFileReader.FileName.Split('\\');
                    cList.Name = namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", "");// ชื่อเพลง
                    if (pl.nPlayer1.returnRunMusic() == true)
                    {
                        cList.runmusic = 1;
                    }
                    else
                    {
                        cList.runmusic = 0;
                    }
                    cList.DurationTimePlay = String.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds);// เวลา
                    cList.DurationTime = String.Format("{0:00}:{1:00}", pl.nPlayer1.audioFileReader.TotalTime.Minutes, pl.nPlayer1.audioFileReader.TotalTime.Seconds);// เวลาทั้งหมด

                    //item.SubItems.Add(items[4]);
                    lines1.Add(cList);
                    json = JsonConvert.SerializeObject(lines1, Formatting.Indented);
                }
                else if (PlayerID == "2")
                {
                    if (pl.nPlayer2.wavePlayer == null) { json = JsonConvert.SerializeObject("ยังไม่ได้เปิดเครื่องเล่น", Formatting.Indented); return json; }
                    if (pl.nPlayer2.audioFileReader == null) { json = JsonConvert.SerializeObject("Wait", Formatting.Indented); return json; }
                    ReturnPlayListMusic cList = new ReturnPlayListMusic();
                    TimeSpan currentTime = pl.nPlayer2.audioFileReader.CurrentTime;
                    var namemusic = pl.nPlayer2.audioFileReader.FileName.Split('\\');
                    cList.Name = namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", "");// ชื่อเพลง
                    cList.DurationTimePlay = String.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds);// เวลา
                    cList.DurationTime = String.Format("{0:00}:{1:00}", pl.nPlayer2.audioFileReader.TotalTime.Minutes, pl.nPlayer2.audioFileReader.TotalTime.Seconds);// เวลาทั้งหมด
                    if (pl.nPlayer2.returnRunMusic() == true)
                    {
                        cList.runmusic = 1;
                    }
                    else
                    {
                        cList.runmusic = 0;
                    }
                    //item.SubItems.Add(items[4]);
                    lines1.Add(cList);
                    json = JsonConvert.SerializeObject(lines1, Formatting.Indented);
                }
                else if (PlayerID == "3")
                {
                    if (pl.nPlayer3.wavePlayer == null) { json = JsonConvert.SerializeObject("ยังไม่ได้เปิดเครื่องเล่น", Formatting.Indented); return json; }
                    if (pl.nPlayer3.audioFileReader == null) { json = JsonConvert.SerializeObject("Wait", Formatting.Indented); return json; }
                    ReturnPlayListMusic cList = new ReturnPlayListMusic();
                    TimeSpan currentTime = pl.nPlayer3.audioFileReader.CurrentTime;
                    var namemusic = pl.nPlayer3.audioFileReader.FileName.Split('\\');
                    cList.Name = namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", "");// ชื่อเพลง
                    cList.DurationTimePlay = String.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds);// เวลา
                    cList.DurationTime = String.Format("{0:00}:{1:00}", pl.nPlayer3.audioFileReader.TotalTime.Minutes, pl.nPlayer3.audioFileReader.TotalTime.Seconds);// เวลาทั้งหมด
                    if (pl.nPlayer3.returnRunMusic() == true)
                    {
                        cList.runmusic = 1;
                    }
                    else
                    {
                        cList.runmusic = 0;
                    }
                    //item.SubItems.Add(items[4]);
                    lines1.Add(cList);
                    json = JsonConvert.SerializeObject(lines1, Formatting.Indented);
                }
                else if (PlayerID == "4")
                {
                    if (pl.nPlayer4.wavePlayer == null) { json = JsonConvert.SerializeObject("ยังไม่ได้เปิดเครื่องเล่น", Formatting.Indented); return json; }
                    if (pl.nPlayer4.audioFileReader == null) { json = JsonConvert.SerializeObject("Wait", Formatting.Indented); return json; }
                    ReturnPlayListMusic cList = new ReturnPlayListMusic();
                    TimeSpan currentTime = pl.nPlayer4.audioFileReader.CurrentTime;
                    var namemusic = pl.nPlayer4.audioFileReader.FileName.Split('\\');
                    cList.Name = namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", "");// ชื่อเพลง
                    cList.DurationTimePlay = String.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds);// เวลา
                    cList.DurationTime = String.Format("{0:00}:{1:00}", pl.nPlayer4.audioFileReader.TotalTime.Minutes, pl.nPlayer4.audioFileReader.TotalTime.Seconds);// เวลาทั้งหมด
                    if (pl.nPlayer4.returnRunMusic() == true)
                    {
                        cList.runmusic = 1;
                    }
                    else
                    {
                        cList.runmusic = 0;
                    }
                    //item.SubItems.Add(items[4]);
                    lines1.Add(cList);
                    json = JsonConvert.SerializeObject(lines1, Formatting.Indented);
                }
                else if (PlayerID == "5")
                {
                    if (pl.nPlayer5.wavePlayer == null) { json = JsonConvert.SerializeObject("ยังไม่ได้เปิดเครื่องเล่น", Formatting.Indented); return json; }
                    if (pl.nPlayer5.audioFileReader == null) { json = JsonConvert.SerializeObject("Wait", Formatting.Indented); return json; }
                    ReturnPlayListMusic cList = new ReturnPlayListMusic();
                    TimeSpan currentTime = pl.nPlayer5.audioFileReader.CurrentTime;
                    var namemusic = pl.nPlayer5.audioFileReader.FileName.Split('\\');
                    cList.Name = namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", "");// ชื่อเพลง
                    cList.DurationTimePlay = String.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds);// เวลา
                    cList.DurationTime = String.Format("{0:00}:{1:00}", pl.nPlayer5.audioFileReader.TotalTime.Minutes, pl.nPlayer5.audioFileReader.TotalTime.Seconds);// เวลาทั้งหมด
                    if (pl.nPlayer5.returnRunMusic() == true)
                    {
                        cList.runmusic = 1;
                    }
                    else
                    {
                        cList.runmusic = 0;
                    }
                    //item.SubItems.Add(items[4]);
                    lines1.Add(cList);
                    json = JsonConvert.SerializeObject(lines1, Formatting.Indented);
                }
                else if (PlayerID == "6")
                {
                    if (pl.nPlayer6.wavePlayer == null) { json = JsonConvert.SerializeObject("ยังไม่ได้เปิดเครื่องเล่น", Formatting.Indented); return json; }
                    if (pl.nPlayer6.audioFileReader == null) { json = JsonConvert.SerializeObject("Wait", Formatting.Indented); return json; }
                    ReturnPlayListMusic cList = new ReturnPlayListMusic();
                    TimeSpan currentTime = pl.nPlayer6.audioFileReader.CurrentTime;
                    var namemusic = pl.nPlayer6.audioFileReader.FileName.Split('\\');
                    cList.Name = namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", "");// ชื่อเพลง
                    cList.DurationTimePlay = String.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds);// เวลา
                    cList.DurationTime = String.Format("{0:00}:{1:00}", pl.nPlayer6.audioFileReader.TotalTime.Minutes, pl.nPlayer6.audioFileReader.TotalTime.Seconds);// เวลาทั้งหมด
                    if (pl.nPlayer6.returnRunMusic() == true)
                    {
                        cList.runmusic = 1;
                    }
                    else
                    {
                        cList.runmusic = 0;
                    }
                    //item.SubItems.Add(items[4]);
                    lines1.Add(cList);
                    json = JsonConvert.SerializeObject(lines1, Formatting.Indented);
                }
                else if (PlayerID == "7")
                {
                    if (pl.nPlayer7.wavePlayer == null) { json = JsonConvert.SerializeObject("ยังไม่ได้เปิดเครื่องเล่น", Formatting.Indented); return json; }
                    if (pl.nPlayer7.audioFileReader == null) { json = JsonConvert.SerializeObject("Wait", Formatting.Indented); return json; }
                    ReturnPlayListMusic cList = new ReturnPlayListMusic();
                    TimeSpan currentTime = pl.nPlayer7.audioFileReader.CurrentTime;
                    var namemusic = pl.nPlayer7.audioFileReader.FileName.Split('\\');
                    cList.Name = namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", "");// ชื่อเพลง
                    cList.DurationTimePlay = String.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds);// เวลา
                    cList.DurationTime = String.Format("{0:00}:{1:00}", pl.nPlayer7.audioFileReader.TotalTime.Minutes, pl.nPlayer7.audioFileReader.TotalTime.Seconds);// เวลาทั้งหมด
                    if (pl.nPlayer7.returnRunMusic() == true)
                    {
                        cList.runmusic = 1;
                    }
                    else
                    {
                        cList.runmusic = 0;
                    }
                    //item.SubItems.Add(items[4]);
                    lines1.Add(cList);
                    json = JsonConvert.SerializeObject(lines1, Formatting.Indented);
                }
                else if (PlayerID == "8")
                {
                    if (pl.nPlayer8.wavePlayer == null) { json = JsonConvert.SerializeObject("ยังไม่ได้เปิดเครื่องเล่น", Formatting.Indented); return json; }
                    if (pl.nPlayer8.audioFileReader == null) { json = JsonConvert.SerializeObject("Wait", Formatting.Indented); return json; }
                    ReturnPlayListMusic cList = new ReturnPlayListMusic();
                    TimeSpan currentTime = pl.nPlayer8.audioFileReader.CurrentTime;
                    var namemusic = pl.nPlayer8.audioFileReader.FileName.Split('\\');
                    cList.Name = namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", "");// ชื่อเพลง
                    cList.DurationTimePlay = String.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds);// เวลา
                    cList.DurationTime = String.Format("{0:00}:{1:00}", pl.nPlayer8.audioFileReader.TotalTime.Minutes, pl.nPlayer8.audioFileReader.TotalTime.Seconds);// เวลาทั้งหมด
                    if (pl.nPlayer8.returnRunMusic() == true)
                    {
                        cList.runmusic = 1;
                    }
                    else
                    {
                        cList.runmusic = 0;
                    }
                    //item.SubItems.Add(items[4]);
                    lines1.Add(cList);
                    json = JsonConvert.SerializeObject(lines1, Formatting.Indented);
                }
                else
                {
                    if (pl.nPlayer1.wavePlayer == null)
                    {

                        ReturnPlayListMusic cList = new ReturnPlayListMusic();
                        cList.Id = 1;// ชื่อเพลง
                        cList.Name = "ยังไม่ได้เปิดโปรแกรมเล่นเพลง";// ชื่อเพลง
                        cList.DurationTimePlay = "00:00";// เวลา
                        cList.DurationTime = "00:00";// เวลาทั้งหมด

                        //item.SubItems.Add(items[4]);
                        lines1.Add(cList);

                    }
                    else
                    {
                        ReturnPlayListMusic cList = new ReturnPlayListMusic();

                        if (pl.nPlayer1.audioFileReader == null)
                        {
                            cList.Id = 1;// ชื่อเพลง
                            cList.Name = "รอเพลงสักครู่...";// ชื่อเพลง
                            cList.DurationTimePlay = "00:00";// เวลา
                            cList.DurationTime = "00:00";// เวลาทั้งหมด
                            cList.runmusic = 0;
                            //item.SubItems.Add(items[4]);
                            lines1.Add(cList);
                        }
                        else
                        {
                            if (pl.nPlayer1.returnRunMusic() == true)
                            {
                                cList.runmusic = 1;
                            }
                            else
                            {
                                cList.runmusic = 0;
                            }
                            TimeSpan currentTime = pl.nPlayer1.audioFileReader.CurrentTime;
                            var namemusic = pl.nPlayer1.audioFileReader.FileName.Split('\\');
                            cList.Id = 1;// ชื่อเพลง
                            cList.Name = namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", "");// ชื่อเพลง
                            cList.DurationTimePlay = String.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds);// เวลา
                            cList.DurationTime = String.Format("{0:00}:{1:00}", pl.nPlayer1.audioFileReader.TotalTime.Minutes, pl.nPlayer1.audioFileReader.TotalTime.Seconds);// เวลาทั้งหมด

                            //item.SubItems.Add(items[4]);
                            lines1.Add(cList);
                        }
                    }
                    if (pl.nPlayer2.wavePlayer == null)
                    {

                        ReturnPlayListMusic cList = new ReturnPlayListMusic();
                        cList.Id = 2;// ชื่อเพลง
                        cList.Name = "ยังไม่ได้เปิดโปรแกรมเล่นเพลง";// ชื่อเพลง
                        cList.DurationTimePlay = "00:00";// เวลา
                        cList.DurationTime = "00:00";// เวลาทั้งหมด
                        cList.runmusic = 0;
                        //item.SubItems.Add(items[4]);
                        lines1.Add(cList);

                    }
                    else
                    {
                        ReturnPlayListMusic cList = new ReturnPlayListMusic();

                        if (pl.nPlayer2.audioFileReader == null)
                        {
                            cList.Id = 2;// ชื่อเพลง
                            cList.Name = "รอเพลงสักครู่...";// ชื่อเพลง
                            cList.DurationTimePlay = "00:00";// เวลา
                            cList.DurationTime = "00:00";// เวลาทั้งหมด

                            //item.SubItems.Add(items[4]);
                            lines1.Add(cList);
                        }
                        else
                        {
                            if (pl.nPlayer2.returnRunMusic() == true)
                            {
                                cList.runmusic = 1;
                            }
                            else
                            {
                                cList.runmusic = 0;
                            }
                            TimeSpan currentTime = pl.nPlayer2.audioFileReader.CurrentTime;
                            var namemusic = pl.nPlayer2.audioFileReader.FileName.Split('\\');
                            cList.Id = 2;// ชื่อเพลง
                            cList.Name = namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", "");// ชื่อเพลง
                            cList.DurationTimePlay = String.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds);// เวลา
                            cList.DurationTime = String.Format("{0:00}:{1:00}", pl.nPlayer2.audioFileReader.TotalTime.Minutes, pl.nPlayer2.audioFileReader.TotalTime.Seconds);// เวลาทั้งหมด

                            //item.SubItems.Add(items[4]);
                            lines1.Add(cList);
                        }
                    }
                    if (pl.nPlayer3.wavePlayer == null)
                    {

                        ReturnPlayListMusic cList = new ReturnPlayListMusic();
                        cList.Id = 3;// ชื่อเพลง
                        cList.Name = "ยังไม่ได้เปิดโปรแกรมเล่นเพลง";// ชื่อเพลง
                        cList.DurationTimePlay = "00:00";// เวลา
                        cList.DurationTime = "00:00";// เวลาทั้งหมด
                        cList.runmusic = 0;
                        //item.SubItems.Add(items[4]);
                        lines1.Add(cList);

                    }
                    else
                    {
                        ReturnPlayListMusic cList = new ReturnPlayListMusic();

                        if (pl.nPlayer3.audioFileReader == null)
                        {
                            cList.Id = 3;// ชื่อเพลง
                            cList.Name = "รอเพลงสักครู่...";// ชื่อเพลง
                            cList.DurationTimePlay = "00:00";// เวลา
                            cList.DurationTime = "00:00";// เวลาทั้งหมด

                            //item.SubItems.Add(items[4]);
                            lines1.Add(cList);
                        }
                        else
                        {
                            if (pl.nPlayer3.returnRunMusic() == true)
                            {
                                cList.runmusic = 1;
                            }
                            else
                            {
                                cList.runmusic = 0;
                            }
                            TimeSpan currentTime = pl.nPlayer3.audioFileReader.CurrentTime;
                            var namemusic = pl.nPlayer3.audioFileReader.FileName.Split('\\');
                            cList.Id = 3;// ชื่อเพลง
                            cList.Name = namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", "");// ชื่อเพลง
                            cList.DurationTimePlay = String.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds);// เวลา
                            cList.DurationTime = String.Format("{0:00}:{1:00}", pl.nPlayer3.audioFileReader.TotalTime.Minutes, pl.nPlayer3.audioFileReader.TotalTime.Seconds);// เวลาทั้งหมด

                            //item.SubItems.Add(items[4]);
                            lines1.Add(cList);
                        }
                    }

                    if (pl.nPlayer4.wavePlayer == null)
                    {

                        ReturnPlayListMusic cList = new ReturnPlayListMusic();
                        cList.Id = 4;// ชื่อเพลง
                        cList.Name = "ยังไม่ได้เปิดโปรแกรมเล่นเพลง";// ชื่อเพลง
                        cList.DurationTimePlay = "00:00";// เวลา
                        cList.DurationTime = "00:00";// เวลาทั้งหมด
                        cList.runmusic = 0;
                        //item.SubItems.Add(items[4]);
                        lines1.Add(cList);

                    }
                    else
                    {
                        ReturnPlayListMusic cList = new ReturnPlayListMusic();

                        if (pl.nPlayer4.audioFileReader == null)
                        {
                            cList.Id = 4;// ชื่อเพลง
                            cList.Name = "รอเพลงสักครู่...";// ชื่อเพลง
                            cList.DurationTimePlay = "00:00";// เวลา
                            cList.DurationTime = "00:00";// เวลาทั้งหมด

                            //item.SubItems.Add(items[4]);
                            lines1.Add(cList);
                        }
                        else
                        {
                            if (pl.nPlayer4.returnRunMusic() == true)
                            {
                                cList.runmusic = 1;
                            }
                            else
                            {
                                cList.runmusic = 0;
                            }
                            TimeSpan currentTime = pl.nPlayer4.audioFileReader.CurrentTime;
                            var namemusic = pl.nPlayer4.audioFileReader.FileName.Split('\\');
                            cList.Id = 4;// ชื่อเพลง
                            cList.Name = namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", "");// ชื่อเพลง
                            cList.DurationTimePlay = String.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds);// เวลา
                            cList.DurationTime = String.Format("{0:00}:{1:00}", pl.nPlayer4.audioFileReader.TotalTime.Minutes, pl.nPlayer4.audioFileReader.TotalTime.Seconds);// เวลาทั้งหมด

                            //item.SubItems.Add(items[4]);
                            lines1.Add(cList);
                        }
                    }

                    if (pl.nPlayer5.wavePlayer == null)
                    {

                        ReturnPlayListMusic cList = new ReturnPlayListMusic();
                        cList.Id = 5;// ชื่อเพลง
                        cList.Name = "ยังไม่ได้เปิดโปรแกรมเล่นเพลง";// ชื่อเพลง
                        cList.DurationTimePlay = "00:00";// เวลา
                        cList.DurationTime = "00:00";// เวลาทั้งหมด
                        cList.runmusic = 0;
                        //item.SubItems.Add(items[4]);
                        lines1.Add(cList);

                    }
                    else
                    {
                        ReturnPlayListMusic cList = new ReturnPlayListMusic();

                        if (pl.nPlayer5.audioFileReader == null)
                        {
                            cList.Id = 5;// ชื่อเพลง
                            cList.Name = "รอเพลงสักครู่...";// ชื่อเพลง
                            cList.DurationTimePlay = "00:00";// เวลา
                            cList.DurationTime = "00:00";// เวลาทั้งหมด

                            //item.SubItems.Add(items[4]);
                            lines1.Add(cList);
                        }
                        else
                        {
                            if (pl.nPlayer5.returnRunMusic() == true)
                            {
                                cList.runmusic = 1;
                            }
                            else
                            {
                                cList.runmusic = 0;
                            }
                            TimeSpan currentTime = pl.nPlayer5.audioFileReader.CurrentTime;
                            var namemusic = pl.nPlayer5.audioFileReader.FileName.Split('\\');
                            cList.Id = 5;// ชื่อเพลง
                            cList.Name = namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", "");// ชื่อเพลง
                            cList.DurationTimePlay = String.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds);// เวลา
                            cList.DurationTime = String.Format("{0:00}:{1:00}", pl.nPlayer5.audioFileReader.TotalTime.Minutes, pl.nPlayer5.audioFileReader.TotalTime.Seconds);// เวลาทั้งหมด

                            //item.SubItems.Add(items[4]);
                            lines1.Add(cList);
                        }
                    }

                    if (pl.nPlayer6.wavePlayer == null)
                    {

                        ReturnPlayListMusic cList = new ReturnPlayListMusic();
                        cList.Id = 6;// ชื่อเพลง
                        cList.Name = "ยังไม่ได้เปิดโปรแกรมเล่นเพลง";// ชื่อเพลง
                        cList.DurationTimePlay = "00:00";// เวลา
                        cList.DurationTime = "00:00";// เวลาทั้งหมด
                        cList.runmusic = 0;
                        //item.SubItems.Add(items[4]);
                        lines1.Add(cList);

                    }
                    else
                    {
                        ReturnPlayListMusic cList = new ReturnPlayListMusic();

                        if (pl.nPlayer6.audioFileReader == null)
                        {
                            cList.Id = 6;// ชื่อเพลง
                            cList.Name = "รอเพลงสักครู่...";// ชื่อเพลง
                            cList.DurationTimePlay = "00:00";// เวลา
                            cList.DurationTime = "00:00";// เวลาทั้งหมด

                            //item.SubItems.Add(items[4]);
                            lines1.Add(cList);
                        }
                        else
                        {
                            if (pl.nPlayer6.returnRunMusic() == true)
                            {
                                cList.runmusic = 1;
                            }
                            else
                            {
                                cList.runmusic = 0;
                            }
                            TimeSpan currentTime = pl.nPlayer6.audioFileReader.CurrentTime;
                            var namemusic = pl.nPlayer6.audioFileReader.FileName.Split('\\');
                            cList.Id = 6;// ชื่อเพลง
                            cList.Name = namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", "");// ชื่อเพลง
                            cList.DurationTimePlay = String.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds);// เวลา
                            cList.DurationTime = String.Format("{0:00}:{1:00}", pl.nPlayer6.audioFileReader.TotalTime.Minutes, pl.nPlayer6.audioFileReader.TotalTime.Seconds);// เวลาทั้งหมด

                            //item.SubItems.Add(items[4]);
                            lines1.Add(cList);
                        }
                    }

                    if (pl.nPlayer7.wavePlayer == null)
                    {

                        ReturnPlayListMusic cList = new ReturnPlayListMusic();
                        cList.Id = 7;// ชื่อเพลง
                        cList.Name = "ยังไม่ได้เปิดโปรแกรมเล่นเพลง";// ชื่อเพลง
                        cList.DurationTimePlay = "00:00";// เวลา
                        cList.DurationTime = "00:00";// เวลาทั้งหมด
                        cList.runmusic = 0;
                        //item.SubItems.Add(items[4]);
                        lines1.Add(cList);

                    }
                    else
                    {
                        ReturnPlayListMusic cList = new ReturnPlayListMusic();

                        if (pl.nPlayer7.audioFileReader == null)
                        {
                            cList.Id = 7;// ชื่อเพลง
                            cList.Name = "รอเพลงสักครู่...";// ชื่อเพลง
                            cList.DurationTimePlay = "00:00";// เวลา
                            cList.DurationTime = "00:00";// เวลาทั้งหมด

                            //item.SubItems.Add(items[4]);
                            lines1.Add(cList);
                        }
                        else
                        {
                            if (pl.nPlayer7.returnRunMusic() == true)
                            {
                                cList.runmusic = 1;
                            }
                            else
                            {
                                cList.runmusic = 0;
                            }
                            TimeSpan currentTime = pl.nPlayer7.audioFileReader.CurrentTime;
                            var namemusic = pl.nPlayer7.audioFileReader.FileName.Split('\\');
                            cList.Id = 7;// ชื่อเพลง
                            cList.Name = namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", "");// ชื่อเพลง
                            cList.DurationTimePlay = String.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds);// เวลา
                            cList.DurationTime = String.Format("{0:00}:{1:00}", pl.nPlayer7.audioFileReader.TotalTime.Minutes, pl.nPlayer7.audioFileReader.TotalTime.Seconds);// เวลาทั้งหมด

                            //item.SubItems.Add(items[4]);
                            lines1.Add(cList);
                        }
                    }

                    if (pl.nPlayer8.wavePlayer == null)
                    {

                        ReturnPlayListMusic cList = new ReturnPlayListMusic();
                        cList.Id = 8;// ชื่อเพลง
                        cList.Name = "ยังไม่ได้เปิดโปรแกรมเล่นเพลง";// ชื่อเพลง
                        cList.DurationTimePlay = "00:00";// เวลา
                        cList.DurationTime = "00:00";// เวลาทั้งหมด
                        cList.runmusic = 0;
                        //item.SubItems.Add(items[4]);
                        lines1.Add(cList);

                    }
                    else
                    {
                        ReturnPlayListMusic cList = new ReturnPlayListMusic();

                        if (pl.nPlayer8.audioFileReader == null)
                        {
                            cList.Id = 8;// ชื่อเพลง
                            cList.Name = "รอเพลงสักครู่...";// ชื่อเพลง
                            cList.DurationTimePlay = "00:00";// เวลา
                            cList.DurationTime = "00:00";// เวลาทั้งหมด

                            //item.SubItems.Add(items[4]);
                            lines1.Add(cList);
                        }
                        else
                        {
                            if (pl.nPlayer8.returnRunMusic() == true)
                            {
                                cList.runmusic = 1;
                            }
                            else
                            {
                                cList.runmusic = 0;
                            }
                            TimeSpan currentTime = pl.nPlayer8.audioFileReader.CurrentTime;
                            var namemusic = pl.nPlayer8.audioFileReader.FileName.Split('\\');
                            cList.Id = 8;// ชื่อเพลง
                            cList.Name = namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", "");// ชื่อเพลง
                            cList.DurationTimePlay = String.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds);// เวลา
                            cList.DurationTime = String.Format("{0:00}:{1:00}", pl.nPlayer8.audioFileReader.TotalTime.Minutes, pl.nPlayer8.audioFileReader.TotalTime.Seconds);// เวลาทั้งหมด

                            //item.SubItems.Add(items[4]);
                            lines1.Add(cList);
                        }
                    }

                    json = JsonConvert.SerializeObject(lines1, Formatting.Indented);
                }
            }
            else if (controlType.ToLower().Trim() == "get_status_music")
            {
                if (PlayerID == "1")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer1.get_status_music(), Formatting.Indented);
                }
                else if (PlayerID == "2")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer2.get_status_music(), Formatting.Indented);
                }
                else if (PlayerID == "3")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer3.get_status_music(), Formatting.Indented);
                }
                else if (PlayerID == "4")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer4.get_status_music(), Formatting.Indented);
                }
                else if (PlayerID == "5")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer5.get_status_music(), Formatting.Indented);
                }
                else if (PlayerID == "6")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer6.get_status_music(), Formatting.Indented);
                }
                else if (PlayerID == "7")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer7.get_status_music(), Formatting.Indented);
                }
                else if (PlayerID == "8")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer8.get_status_music(), Formatting.Indented);
                }
            }
            else if (controlType.ToLower().Trim() == "get_list_music")
            {
                if (PlayerID == "1")
                {
                    foreach (var _item in pl.nPlayer1.ListOTSMedia)
                    {
                        var namemusic = _item.fileName.Split('\\');
                        ReturnPlayList cList = new ReturnPlayList();
                        cList.Seq = _item.Id.ToString(); // ID
                        cList.Name = namemusic[namemusic.Length - 1];// ชื่อเพลง
                        cList.DurationTime = _item.duration.ToString();// เวลา
                                                                       //item.SubItems.Add(items[4]);
                        lines.Add(cList);
                    }
                    json = JsonConvert.SerializeObject(lines, Formatting.Indented);
                }
                else if (PlayerID == "2")
                {
                    foreach (var _item in pl.nPlayer2.ListOTSMedia)
                    {
                        var namemusic = _item.fileName.Split('\\');
                        ReturnPlayList cList = new ReturnPlayList();
                        cList.Seq = _item.Id.ToString(); // ID
                        cList.Name = namemusic[namemusic.Length - 1];// ชื่อเพลง
                        cList.DurationTime = _item.duration.ToString();// เวลา
                                                                       //item.SubItems.Add(items[4]);
                        lines.Add(cList);
                    }
                    json = JsonConvert.SerializeObject(lines, Formatting.Indented);
                }
                else if (PlayerID == "3")
                {
                    foreach (var _item in pl.nPlayer3.ListOTSMedia)
                    {
                        var namemusic = _item.fileName.Split('\\');
                        ReturnPlayList cList = new ReturnPlayList();
                        cList.Seq = _item.Id.ToString(); // ID
                        cList.Name = namemusic[namemusic.Length - 1];// ชื่อเพลง
                        cList.DurationTime = _item.duration.ToString();// เวลา
                                                                       //item.SubItems.Add(items[4]);
                        lines.Add(cList);
                    }
                    json = JsonConvert.SerializeObject(lines, Formatting.Indented);
                }
                else if (PlayerID == "4")
                {
                    foreach (var _item in pl.nPlayer4.ListOTSMedia)
                    {
                        var namemusic = _item.fileName.Split('\\');
                        ReturnPlayList cList = new ReturnPlayList();
                        cList.Seq = _item.Id.ToString(); // ID
                        cList.Name = namemusic[namemusic.Length - 1];// ชื่อเพลง
                        cList.DurationTime = _item.duration.ToString();// เวลา
                                                                       //item.SubItems.Add(items[4]);
                        lines.Add(cList);
                    }
                    json = JsonConvert.SerializeObject(lines, Formatting.Indented);
                }
                else if (PlayerID == "5")
                {
                    foreach (var _item in pl.nPlayer5.ListOTSMedia)
                    {
                        var namemusic = _item.fileName.Split('\\');
                        ReturnPlayList cList = new ReturnPlayList();
                        cList.Seq = _item.Id.ToString(); // ID
                        cList.Name = namemusic[namemusic.Length - 1];// ชื่อเพลง
                        cList.DurationTime = _item.duration.ToString();// เวลา
                                                                       //item.SubItems.Add(items[4]);
                        lines.Add(cList);
                    }
                    json = JsonConvert.SerializeObject(lines, Formatting.Indented);
                }
                else if (PlayerID == "6")
                {
                    foreach (var _item in pl.nPlayer6.ListOTSMedia)
                    {
                        var namemusic = _item.fileName.Split('\\');
                        ReturnPlayList cList = new ReturnPlayList();
                        cList.Seq = _item.Id.ToString(); // ID
                        cList.Name = namemusic[namemusic.Length - 1];// ชื่อเพลง
                        cList.DurationTime = _item.duration.ToString();// เวลา
                                                                       //item.SubItems.Add(items[4]);
                        lines.Add(cList);
                    }
                    json = JsonConvert.SerializeObject(lines, Formatting.Indented);
                }
                else if (PlayerID == "7")
                {
                    foreach (var _item in pl.nPlayer7.ListOTSMedia)
                    {
                        var namemusic = _item.fileName.Split('\\');
                        ReturnPlayList cList = new ReturnPlayList();
                        cList.Seq = _item.Id.ToString(); // ID
                        cList.Name = namemusic[namemusic.Length - 1];// ชื่อเพลง
                        cList.DurationTime = _item.duration.ToString();// เวลา
                                                                       //item.SubItems.Add(items[4]);
                        lines.Add(cList);
                    }
                    json = JsonConvert.SerializeObject(lines, Formatting.Indented);
                }
                else if (PlayerID == "8")
                {
                    foreach (var _item in pl.nPlayer8.ListOTSMedia)
                    {
                        var namemusic = _item.fileName.Split('\\');
                        ReturnPlayList cList = new ReturnPlayList();
                        cList.Seq = _item.Id.ToString(); // ID
                        cList.Name = namemusic[namemusic.Length - 1];// ชื่อเพลง
                        cList.DurationTime = _item.duration.ToString();// เวลา
                                                                       //item.SubItems.Add(items[4]);
                        lines.Add(cList);
                    }
                    json = JsonConvert.SerializeObject(lines, Formatting.Indented);
                }
            }
            else if (controlType.ToLower().Trim() == "play_control_music")
            {
                if (PlayerID == "1")
                {
                    pl.nPlayer1.run_music_play();
                }
                else if (PlayerID == "2")
                {
                    pl.nPlayer2.run_music_play();
                }
                else if (PlayerID == "3")
                {
                    pl.nPlayer3.run_music_play();
                }
                else if (PlayerID == "4")
                {
                    pl.nPlayer4.run_music_play();
                }
                else if (PlayerID == "5")
                {
                    pl.nPlayer5.run_music_play();
                }
                else if (PlayerID == "6")
                {
                    pl.nPlayer6.run_music_play();
                }
                else if (PlayerID == "7")
                {
                    pl.nPlayer7.run_music_play();
                }
                else if (PlayerID == "8")
                {
                    pl.nPlayer8.run_music_play();
                }
            }
            else if (controlType.ToLower().Trim() == "stop_control_music")
            {
                if (PlayerID == "1")
                {
                    pl.nPlayer1.stop_music_play();
                }
                else if (PlayerID == "2")
                {
                    pl.nPlayer2.stop_music_play();
                }
                else if (PlayerID == "3")
                {
                    pl.nPlayer3.stop_music_play();
                }
                else if (PlayerID == "4")
                {
                    pl.nPlayer4.stop_music_play();
                }
                else if (PlayerID == "5")
                {
                    pl.nPlayer5.stop_music_play();
                }
                else if (PlayerID == "6")
                {
                    pl.nPlayer6.stop_music_play();
                }
                else if (PlayerID == "7")
                {
                    pl.nPlayer7.stop_music_play();
                }
                else if (PlayerID == "8")
                {
                    pl.nPlayer8.stop_music_play();
                }
            }
            else if (controlType.ToLower().Trim() == "pause_control_music")
            {
                if (PlayerID == "1")
                {
                    pl.nPlayer1.pause_music_play();
                }
                else if (PlayerID == "2")
                {
                    pl.nPlayer2.pause_music_play();
                }
                else if (PlayerID == "3")
                {
                    pl.nPlayer3.pause_music_play();
                }
                else if (PlayerID == "4")
                {
                    pl.nPlayer4.pause_music_play();
                }
                else if (PlayerID == "5")
                {
                    pl.nPlayer5.pause_music_play();
                }
                else if (PlayerID == "6")
                {
                    pl.nPlayer6.pause_music_play();
                }
                else if (PlayerID == "7")
                {
                    pl.nPlayer7.pause_music_play();
                }
                else if (PlayerID == "8")
                {
                    pl.nPlayer8.pause_music_play();
                }
            }
            else if (controlType.ToLower().Trim() == "forward_control_music")
            {
                if (PlayerID == "1")
                {
                    pl.nPlayer1.forward_music_play();
                }
                else if (PlayerID == "2")
                {
                    pl.nPlayer2.forward_music_play();
                }
                else if (PlayerID == "3")
                {
                    pl.nPlayer3.forward_music_play();
                }
                else if (PlayerID == "4")
                {
                    pl.nPlayer4.forward_music_play();
                }
                else if (PlayerID == "5")
                {
                    pl.nPlayer5.forward_music_play();
                }
                else if (PlayerID == "6")
                {
                    pl.nPlayer6.forward_music_play();
                }
                else if (PlayerID == "7")
                {
                    pl.nPlayer7.forward_music_play();
                }
                else if (PlayerID == "8")
                {
                    pl.nPlayer8.forward_music_play();
                }
            }
            else if (controlType.ToLower().Trim() == "backward_control_music")
            {
                if (PlayerID == "1")
                {
                    pl.nPlayer1.backward_music_play();
                }
                else if (PlayerID == "2")
                {
                    pl.nPlayer2.backward_music_play();
                }
                else if (PlayerID == "3")
                {
                    pl.nPlayer3.backward_music_play();
                }
                else if (PlayerID == "4")
                {
                    pl.nPlayer4.backward_music_play();
                }
                else if (PlayerID == "5")
                {
                    pl.nPlayer5.backward_music_play();
                }
                else if (PlayerID == "6")
                {
                    pl.nPlayer6.backward_music_play();
                }
                else if (PlayerID == "7")
                {
                    pl.nPlayer7.backward_music_play();
                }
                else if (PlayerID == "8")
                {
                    pl.nPlayer8.backward_music_play();
                }
            }

            #region ปิด api
            else if (controlType.ToLower().Trim() == "increasevolume")
            {
                if (PlayerID == "1")
                {
                    pl.nPlayer1.IncreaseVolumeButton_Click();
                }
                else if (PlayerID == "2")
                {
                    pl.nPlayer2.IncreaseVolumeButton_Click();
                }
                else if (PlayerID == "3")
                {
                    pl.nPlayer3.IncreaseVolumeButton_Click();
                }
                else if (PlayerID == "4")
                {
                    pl.nPlayer4.IncreaseVolumeButton_Click();
                }
                else if (PlayerID == "5")
                {
                    pl.nPlayer5.IncreaseVolumeButton_Click();
                }
                else if (PlayerID == "6")
                {
                    pl.nPlayer6.IncreaseVolumeButton_Click();
                }
                else if (PlayerID == "7")
                {
                    pl.nPlayer7.IncreaseVolumeButton_Click();
                }
                else if (PlayerID == "8")
                {
                    pl.nPlayer8.IncreaseVolumeButton_Click();
                }
            }
            else if (controlType.ToLower().Trim() == "decreasevolume")
            {
                if (PlayerID == "1")
                {
                    pl.nPlayer1.DecreaseVolumeButton_Click();
                }
                else if (PlayerID == "2")
                {
                    pl.nPlayer2.DecreaseVolumeButton_Click();
                }
                else if (PlayerID == "3")
                {
                    pl.nPlayer3.DecreaseVolumeButton_Click();
                }
                else if (PlayerID == "4")
                {
                    pl.nPlayer4.DecreaseVolumeButton_Click();
                }
                else if (PlayerID == "5")
                {
                    pl.nPlayer5.DecreaseVolumeButton_Click();
                }
                else if (PlayerID == "6")
                {
                    pl.nPlayer6.DecreaseVolumeButton_Click();
                }
                else if (PlayerID == "7")
                {
                    pl.nPlayer7.DecreaseVolumeButton_Click();
                }
                else if (PlayerID == "8")
                {
                    pl.nPlayer8.DecreaseVolumeButton_Click();
                }
            }
            else if (controlType.ToLower().Trim() == "scrollmusic")
            {
                if (PlayerID == "1")
                {
                    pl.nPlayer1.scroll_music(Convert.ToDouble(sec));
                }
                else if (PlayerID == "2")
                {
                    pl.nPlayer2.scroll_music(Convert.ToDouble(sec));
                }
                else if (PlayerID == "3")
                {
                    pl.nPlayer3.scroll_music(Convert.ToDouble(sec));
                }
                else if (PlayerID == "4")
                {
                    pl.nPlayer4.scroll_music(Convert.ToDouble(sec));
                }
                else if (PlayerID == "5")
                {
                    pl.nPlayer5.scroll_music(Convert.ToDouble(sec));
                }
                else if (PlayerID == "6")
                {
                    pl.nPlayer6.scroll_music(Convert.ToDouble(sec));
                }
                else if (PlayerID == "7")
                {
                    pl.nPlayer7.scroll_music(Convert.ToDouble(sec));
                }
                else if (PlayerID == "8")
                {
                    pl.nPlayer8.scroll_music(Convert.ToDouble(sec));
                }
            }
            else if (controlType.ToLower().Trim() == "select_control_music")
            {
                if (PlayerID == "1")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer1.ListOTSMedia, Formatting.Indented);
                }
                else if (PlayerID == "2")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer2.ListOTSMedia, Formatting.Indented);
                }
                else if (PlayerID == "3")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer3.ListOTSMedia, Formatting.Indented);
                }
                else if (PlayerID == "4")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer4.ListOTSMedia, Formatting.Indented);
                }
                else if (PlayerID == "5")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer5.ListOTSMedia, Formatting.Indented);
                }
                else if (PlayerID == "6")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer6.ListOTSMedia, Formatting.Indented);
                }
                else if (PlayerID == "7")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer7.ListOTSMedia, Formatting.Indented);
                }
                else if (PlayerID == "8")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer8.ListOTSMedia, Formatting.Indented);
                }
            }
            else if (controlType.ToLower().Trim() == "get_loopandshuffle_control_music")
            {
                if (PlayerID == "1")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer1.get_loopandshuffle(), Formatting.Indented);
                }
                else if (PlayerID == "2")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer2.get_loopandshuffle(), Formatting.Indented);
                }
                else if (PlayerID == "3")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer3.get_loopandshuffle(), Formatting.Indented);
                }
                else if (PlayerID == "4")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer4.get_loopandshuffle(), Formatting.Indented);
                }
                else if (PlayerID == "5")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer5.get_loopandshuffle(), Formatting.Indented);
                }
                else if (PlayerID == "6")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer6.get_loopandshuffle(), Formatting.Indented);
                }
                else if (PlayerID == "7")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer7.get_loopandshuffle(), Formatting.Indented);
                }
                else if (PlayerID == "8")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer8.get_loopandshuffle(), Formatting.Indented);
                }
            }
            else if (controlType.ToLower().Trim() == "set_loopandshuffle_control_music")
            {
                if (PlayerID == "1")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer1.set_loopandshuffle(1, sec), Formatting.Indented);
                }
                else if (PlayerID == "2")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer2.set_loopandshuffle(2, sec), Formatting.Indented);
                }
                else if (PlayerID == "3")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer3.set_loopandshuffle(3, sec), Formatting.Indented);
                }
                else if (PlayerID == "4")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer4.set_loopandshuffle(4, sec), Formatting.Indented);
                }
                else if (PlayerID == "5")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer5.set_loopandshuffle(5, sec), Formatting.Indented);
                }
                else if (PlayerID == "6")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer6.set_loopandshuffle(6, sec), Formatting.Indented);
                }
                else if (PlayerID == "7")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer7.set_loopandshuffle(7, sec), Formatting.Indented);
                }
                else if (PlayerID == "8")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer8.set_loopandshuffle(8, sec), Formatting.Indented);
                }
            }
            else if (controlType.ToLower().Trim() == "select_music_web")
            {
                if (PlayerID == "1")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer1.select_music_for_web(1, music), Formatting.Indented);
                }
                else if (PlayerID == "2")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer2.select_music_for_web(2, music), Formatting.Indented);
                }
                else if (PlayerID == "3")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer3.select_music_for_web(3, music), Formatting.Indented);
                }
                else if (PlayerID == "4")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer4.select_music_for_web(4, music), Formatting.Indented);
                }
                else if (PlayerID == "5")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer5.select_music_for_web(5, music), Formatting.Indented);
                }
                else if (PlayerID == "6")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer6.select_music_for_web(6, music), Formatting.Indented);
                }
                else if (PlayerID == "7")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer7.select_music_for_web(7, music), Formatting.Indented);
                }
                else if (PlayerID == "8")
                {
                    json = JsonConvert.SerializeObject(pl.nPlayer8.select_music_for_web(8, music), Formatting.Indented);
                }
            }
            else if (controlType.ToLower().Trim() == "change_text_color")
            {
                if (PlayerID == "1")
                {
                    pl.nPlayer1.change_text_color(text, bgcolor, fgcolor);
                }
                else if (PlayerID == "2")
                {
                    pl.nPlayer2.change_text_color(text, bgcolor, fgcolor);
                }
                else if (PlayerID == "3")
                {
                    pl.nPlayer3.change_text_color(text, bgcolor, fgcolor);
                }
                else if (PlayerID == "4")
                {
                    pl.nPlayer4.change_text_color(text, bgcolor, fgcolor);
                }
                else if (PlayerID == "5")
                {
                    pl.nPlayer5.change_text_color(text, bgcolor, fgcolor);
                }
                else if (PlayerID == "6")
                {
                    pl.nPlayer6.change_text_color(text, bgcolor, fgcolor);
                }
                else if (PlayerID == "7")
                {
                    pl.nPlayer7.change_text_color(text, bgcolor, fgcolor);
                }
                else if (PlayerID == "8")
                {
                    pl.nPlayer8.change_text_color(text, bgcolor, fgcolor);
                }
                json = JsonConvert.SerializeObject("success", Formatting.Indented);
            }
            else if (controlType.ToLower().Trim() == "delete_music")
            {
                if (dmusic == null || dmusic == "" || dmusic == " ") { json = JsonConvert.SerializeObject("กรุณาใส่ Index เพลงที่จะรบ เช่น 1,2", Formatting.Indented); return json; }
                var ggg = dmusic.Split(',');
                if (PlayerID == "1")
                {
                    pl.remove_music(ggg, 1);
                }
                else if (PlayerID == "2")
                {
                    pl.remove_music(ggg, 2);
                }
                else if (PlayerID == "3")
                {
                    pl.remove_music(ggg, 3);
                }
                else if (PlayerID == "4")
                {
                    pl.remove_music(ggg, 4);
                }
                else if (PlayerID == "5")
                {
                    pl.remove_music(ggg, 5);
                }
                else if (PlayerID == "6")
                {
                    pl.remove_music(ggg, 6);
                }
                else if (PlayerID == "7")
                {
                    pl.remove_music(ggg, 7);
                }
                else if (PlayerID == "8")
                {
                    pl.remove_music(ggg, 8);
                }
                json = JsonConvert.SerializeObject("success", Formatting.Indented);
            }
            else if (controlType.ToLower().Trim() == "get_timer")
            {
                if (PlayerID == "1")
                {
                    Settimers ggg = new Settimers(pl, "timers1",null);

                    json = JsonConvert.SerializeObject(ggg.get_timer("timers1"), Formatting.Indented);
                }
                else if (PlayerID == "2")
                {
                    Settimers ggg = new Settimers(pl, "timers2",null);
                    json = JsonConvert.SerializeObject(ggg.get_timer("timers2"), Formatting.Indented);
                }
                else if (PlayerID == "3")
                {
                    Settimers ggg = new Settimers(pl, "timers3",null);
                    json = JsonConvert.SerializeObject(ggg.get_timer("timers3"), Formatting.Indented);
                }
                else if (PlayerID == "4")
                {
                    Settimers ggg = new Settimers(pl, "timers4",null);
                    json = JsonConvert.SerializeObject(ggg.get_timer("timers4"), Formatting.Indented);
                }
                else if (PlayerID == "5")
                {
                    Settimers ggg = new Settimers(pl, "timers5",null);
                    json = JsonConvert.SerializeObject(ggg.get_timer("timers5"), Formatting.Indented);
                }
                else if (PlayerID == "6")
                {
                    Settimers ggg = new Settimers(pl, "timers6",null);
                    json = JsonConvert.SerializeObject(ggg.get_timer("timers6"), Formatting.Indented);
                }
                else if (PlayerID == "7")
                {
                    Settimers ggg = new Settimers(pl, "timers7",null);
                    json = JsonConvert.SerializeObject(ggg.get_timer("timers7"), Formatting.Indented);
                }
                else if (PlayerID == "8")
                {
                    Settimers ggg = new Settimers(pl, "timers8",null);
                    json = JsonConvert.SerializeObject(ggg.get_timer("timers8"), Formatting.Indented);
                }
            }
            else if (controlType.ToLower().Trim() == "set_timer")
            {
                if (PlayerID == "1")
                {
                    Settimers ggg = new Settimers(pl, "timers1",null);

                    json = JsonConvert.SerializeObject(ggg.updatereg(configs, "timers1"), Formatting.Indented);
                }
                else if (PlayerID == "2")
                {
                    Settimers ggg = new Settimers(pl, "timers2",null);
                    json = JsonConvert.SerializeObject(ggg.updatereg(configs, "timers2"), Formatting.Indented);
                }
                else if (PlayerID == "3")
                {
                    Settimers ggg = new Settimers(pl, "timers3",null);
                    json = JsonConvert.SerializeObject(ggg.updatereg(configs, "timers3"), Formatting.Indented);
                }
                else if (PlayerID == "4")
                {
                    Settimers ggg = new Settimers(pl, "timers4",null);
                    json = JsonConvert.SerializeObject(ggg.updatereg(configs, "timers4"), Formatting.Indented);
                }
                else if (PlayerID == "5")
                {
                    Settimers ggg = new Settimers(pl, "timers5",null);
                    json = JsonConvert.SerializeObject(ggg.updatereg(configs, "timers5"), Formatting.Indented);
                }
                else if (PlayerID == "6")
                {
                    Settimers ggg = new Settimers(pl, "timers6",null);
                    json = JsonConvert.SerializeObject(ggg.updatereg(configs, "timers6"), Formatting.Indented);
                }
                else if (PlayerID == "7")
                {
                    Settimers ggg = new Settimers(pl, "timers7", null);
                    json = JsonConvert.SerializeObject(ggg.updatereg(configs, "timers7"), Formatting.Indented);
                }
                else if (PlayerID == "8")
                {
                    Settimers ggg = new Settimers(pl, "timers8", null);
                    json = JsonConvert.SerializeObject(ggg.updatereg(configs, "timers8"), Formatting.Indented);
                }
            }
            else if (controlType.ToLower().Trim() == "get_status_program")
            {
                if (pl.InvokeRequired)
                {
                    pl.Invoke(new Action(() =>
                    {
                        status_com status_Com = new status_com();
                        status_Com.namecom = Dns.GetHostName();
                        status_Com.status = "Success";
                        json = JsonConvert.SerializeObject(status_Com, Formatting.Indented);
                    }));

                }
                else
                {
                    status_com status_Com = new status_com();
                    status_Com.namecom = Dns.GetHostName();
                    status_Com.status = "Success";
                    json = JsonConvert.SerializeObject(status_Com, Formatting.Indented);
                }
            }
            else if (controlType.ToLower().Trim() == "getmusiclist")
            {
                List<playlist> listc = new List<playlist>();
                List<PlayListMusic> plays = new List<PlayListMusic>();
                NPlayer plsc = null;
                short id = 0;
                if (PlayerID != "0")
                {
                    if (pl.nPlayer1.wavePlayer != null)
                    {
                        if (pl.nPlayer1.audioFileReader == null) { json = JsonConvert.SerializeObject("Wait", Formatting.Indented); return json; }
                        TimeSpan currentTime = pl.nPlayer1.audioFileReader.CurrentTime;
                        var namemusic = pl.nPlayer1.audioFileReader.FileName.Split('\\');
                        plays.Add(new PlayListMusic
                        {
                            namemusic = namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", ""),// ชื่อเพลง
                            title = "PlayList1",
                            name = pl.nPlayer1.get_textfont(),
                            bgColor = pl.nPlayer1.get_bgcolor(),
                            fgColor = pl.nPlayer1.get_fgcolor(),
                            currentSongIndex = pl.nPlayer1.get_runorder(),
                            isPlaying = pl.nPlayer1.wavePlayer.PlaybackState == PlaybackState.Playing ? true : false,
                            currentMinute = String.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds),
                            durationtotal = String.Format("{0:00}:{1:00}", pl.nPlayer1.audioFileReader.TotalTime.Minutes, pl.nPlayer1.audioFileReader.TotalTime.Seconds),// เวลาทั้งหมด
                            volume = (int)Math.Round(pl.nPlayer1.wavePlayer.Volume * 100f)
                        });
                    }
                    else
                    {
                        plays.Add(new PlayListMusic
                        {
                            namemusic = "",
                            title = "PlayList1",
                            name = pl.nPlayer1.get_textfont(),
                            bgColor = pl.nPlayer1.get_bgcolor(),
                            fgColor = pl.nPlayer1.get_fgcolor(),
                            currentSongIndex = null,
                            isPlaying = null,
                            currentMinute = "00:00",
                            durationtotal = "00:00",

                            volume = 0
                        });
                    }
                    if (pl.nPlayer2.wavePlayer != null)
                    {
                        if (pl.nPlayer2.audioFileReader == null) { json = JsonConvert.SerializeObject("Wait", Formatting.Indented); return json; }
                        TimeSpan currentTime = pl.nPlayer2.audioFileReader.CurrentTime;
                        var namemusic = pl.nPlayer2.audioFileReader.FileName.Split('\\');
                        plays.Add(new PlayListMusic
                        {
                            namemusic = namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", ""),// ชื่อเพลง
                            title = "PlayList2",
                            name = pl.nPlayer2.get_textfont(),
                            bgColor = pl.nPlayer2.get_bgcolor(),
                            fgColor = pl.nPlayer2.get_fgcolor(),
                            currentSongIndex = pl.nPlayer2.get_runorder(),
                            isPlaying = pl.nPlayer2.wavePlayer.PlaybackState == PlaybackState.Playing ? true : false,
                            currentMinute = String.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds),
                            durationtotal = String.Format("{0:00}:{1:00}", pl.nPlayer2.audioFileReader.TotalTime.Minutes, pl.nPlayer2.audioFileReader.TotalTime.Seconds),// เวลาทั้งหมด
                            volume = (int)Math.Round(pl.nPlayer2.wavePlayer.Volume * 100f)
                        });
                    }
                    else
                    {


                        plays.Add(new PlayListMusic
                        {
                            namemusic = "",
                            title = "PlayList2",
                            name = pl.nPlayer2.get_textfont(),
                            bgColor = pl.nPlayer2.get_bgcolor(),
                            fgColor = pl.nPlayer2.get_fgcolor(),
                            currentSongIndex = null,
                            isPlaying = null,
                            currentMinute = "00:00",
                            durationtotal = "00:00",
                            volume = 0
                        });
                    }
                    if (pl.nPlayer3.wavePlayer != null)
                    {
                        if (pl.nPlayer3.audioFileReader == null) { json = JsonConvert.SerializeObject("Wait", Formatting.Indented); return json; }

                        TimeSpan currentTime = pl.nPlayer3.audioFileReader.CurrentTime;
                        var namemusic = pl.nPlayer3.audioFileReader.FileName.Split('\\');
                        plays.Add(new PlayListMusic
                        {
                            namemusic = namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", ""),// ชื่อเพลง
                            title = "PlayList3",
                            name = pl.nPlayer3.get_textfont(),
                            bgColor = pl.nPlayer3.get_bgcolor(),
                            fgColor = pl.nPlayer3.get_fgcolor(),
                            currentSongIndex = pl.nPlayer3.get_runorder(),
                            isPlaying = pl.nPlayer3.wavePlayer.PlaybackState == PlaybackState.Playing ? true : false,
                            currentMinute = String.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds),
                            durationtotal = String.Format("{0:00}:{1:00}", pl.nPlayer3.audioFileReader.TotalTime.Minutes, pl.nPlayer3.audioFileReader.TotalTime.Seconds),// เวลาทั้งหมด
                            volume = (int)Math.Round(pl.nPlayer3.wavePlayer.Volume * 100f)
                        });
                    }
                    else
                    {

                        plays.Add(new PlayListMusic
                        {
                            namemusic = "",
                            title = "PlayList3",
                            name = pl.nPlayer3.get_textfont(),
                            bgColor = pl.nPlayer3.get_bgcolor(),
                            fgColor = pl.nPlayer3.get_fgcolor(),
                            currentSongIndex = null,
                            isPlaying = null,
                            currentMinute = "00:00",
                            durationtotal = "00:00",
                            volume = 0
                        });
                    }
                    if (pl.nPlayer4.wavePlayer != null)
                    {
                        if (pl.nPlayer4.audioFileReader == null) { json = JsonConvert.SerializeObject("Wait", Formatting.Indented); return json; }

                        TimeSpan currentTime = pl.nPlayer4.audioFileReader.CurrentTime;
                        var namemusic = pl.nPlayer4.audioFileReader.FileName.Split('\\');
                        plays.Add(new PlayListMusic
                        {
                            namemusic = namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", ""),// ชื่อเพลง
                            title = "PlayList4",
                            name = pl.nPlayer4.get_textfont(),
                            bgColor = pl.nPlayer4.get_bgcolor(),
                            fgColor = pl.nPlayer4.get_fgcolor(),
                            currentSongIndex = pl.nPlayer4.get_runorder(),
                            isPlaying = pl.nPlayer4.wavePlayer.PlaybackState == PlaybackState.Playing ? true : false,
                            currentMinute = String.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds),
                            durationtotal = String.Format("{0:00}:{1:00}", pl.nPlayer4.audioFileReader.TotalTime.Minutes, pl.nPlayer4.audioFileReader.TotalTime.Seconds),// เวลาทั้งหมด
                            volume = (int)Math.Round(pl.nPlayer4.wavePlayer.Volume * 100f)
                        });
                    }
                    else
                    {
                        plays.Add(new PlayListMusic
                        {
                            namemusic = "",
                            title = "PlayList4",
                            name = pl.nPlayer4.get_textfont(),
                            bgColor = pl.nPlayer4.get_bgcolor(),
                            fgColor = pl.nPlayer4.get_fgcolor(),
                            currentSongIndex = null,
                            isPlaying = null,
                            currentMinute = "00:00",
                            durationtotal = "00:00",
                            volume = 0
                        });
                    }
                    if (pl.nPlayer5.wavePlayer != null)
                    {
                        if (pl.nPlayer5.audioFileReader == null) { json = JsonConvert.SerializeObject("Wait", Formatting.Indented); return json; }
                        TimeSpan currentTime = pl.nPlayer4.audioFileReader.CurrentTime;
                        var namemusic = pl.nPlayer5.audioFileReader.FileName.Split('\\');
                        plays.Add(new PlayListMusic
                        {
                            namemusic = namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", ""),// ชื่อเพลง
                            title = "PlayList5",
                            name = pl.nPlayer5.get_textfont(),
                            bgColor = pl.nPlayer5.get_bgcolor(),
                            fgColor = pl.nPlayer5.get_fgcolor(),
                            currentSongIndex = pl.nPlayer5.get_runorder(),
                            isPlaying = pl.nPlayer5.wavePlayer.PlaybackState == PlaybackState.Playing ? true : false,
                            currentMinute = String.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds),
                            durationtotal = String.Format("{0:00}:{1:00}", pl.nPlayer5.audioFileReader.TotalTime.Minutes, pl.nPlayer5.audioFileReader.TotalTime.Seconds),// เวลาทั้งหมด
                            volume = (int)Math.Round(pl.nPlayer5.wavePlayer.Volume * 100f)
                        });
                    }
                    else
                    {


                        plays.Add(new PlayListMusic
                        {
                            namemusic = "",
                            title = "PlayList5",
                            name = pl.nPlayer5.get_textfont(),
                            bgColor = pl.nPlayer5.get_bgcolor(),
                            fgColor = pl.nPlayer5.get_fgcolor(),
                            currentSongIndex = null,
                            isPlaying = null,
                            currentMinute = "00:00",
                            durationtotal = "00:00",
                            volume = 0
                        });
                    }
                    if (pl.nPlayer6.wavePlayer != null)
                    {
                        if (pl.nPlayer6.audioFileReader == null) { json = JsonConvert.SerializeObject("Wait", Formatting.Indented); return json; }
                        TimeSpan currentTime = pl.nPlayer6.audioFileReader.CurrentTime;
                        var namemusic = pl.nPlayer6.audioFileReader.FileName.Split('\\');
                        plays.Add(new PlayListMusic
                        {
                            namemusic = namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", ""),// ชื่อเพลง
                            title = "PlayList6",
                            name = pl.nPlayer6.get_textfont(),
                            bgColor = pl.nPlayer6.get_bgcolor(),
                            fgColor = pl.nPlayer6.get_fgcolor(),
                            currentSongIndex = pl.nPlayer6.get_runorder(),
                            isPlaying = pl.nPlayer6.wavePlayer.PlaybackState == PlaybackState.Playing ? true : false,
                            currentMinute = String.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds),
                            durationtotal = String.Format("{0:00}:{1:00}", pl.nPlayer6.audioFileReader.TotalTime.Minutes, pl.nPlayer6.audioFileReader.TotalTime.Seconds),// เวลาทั้งหมด
                            volume = (int)Math.Round(pl.nPlayer6.wavePlayer.Volume * 100f)
                        });
                    }
                    else
                    {


                        plays.Add(new PlayListMusic
                        {
                            namemusic = "",
                            title = "PlayList6",
                            name = pl.nPlayer6.get_textfont(),
                            bgColor = pl.nPlayer6.get_bgcolor(),
                            fgColor = pl.nPlayer6.get_fgcolor(),
                            currentSongIndex = null,
                            isPlaying = null,
                            currentMinute = "00:00",
                            durationtotal = "00:00",
                            volume = 0
                        });
                    }
                    if (pl.nPlayer7.wavePlayer != null)
                    {
                        if (pl.nPlayer7.audioFileReader == null) { json = JsonConvert.SerializeObject("Wait", Formatting.Indented); return json; }
                        TimeSpan currentTime = pl.nPlayer7.audioFileReader.CurrentTime;
                        var namemusic = pl.nPlayer7.audioFileReader.FileName.Split('\\');
                        plays.Add(new PlayListMusic
                        {
                            namemusic = namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", ""),// ชื่อเพลง
                            title = "PlayList7",
                            name = pl.nPlayer7.get_textfont(),
                            bgColor = pl.nPlayer7.get_bgcolor(),
                            fgColor = pl.nPlayer7.get_fgcolor(),
                            currentSongIndex = pl.nPlayer7.get_runorder(),
                            isPlaying = pl.nPlayer7.wavePlayer.PlaybackState == PlaybackState.Playing ? true : false,
                            currentMinute = String.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds),
                            durationtotal = String.Format("{0:00}:{1:00}", pl.nPlayer7.audioFileReader.TotalTime.Minutes, pl.nPlayer7.audioFileReader.TotalTime.Seconds),// เวลาทั้งหมด
                            volume = (int)Math.Round(pl.nPlayer7.wavePlayer.Volume * 100f)
                        });
                    }
                    else
                    {


                        plays.Add(new PlayListMusic
                        {
                            namemusic = "",
                            title = "PlayList7",
                            name = pl.nPlayer7.get_textfont(),
                            bgColor = pl.nPlayer7.get_bgcolor(),
                            fgColor = pl.nPlayer7.get_fgcolor(),
                            currentSongIndex = null,
                            isPlaying = null,
                            currentMinute = "00:00",
                            durationtotal = "00:00",
                            volume = 0
                        });
                    }
                    if (pl.nPlayer8.wavePlayer != null)
                    {
                        if (pl.nPlayer8.audioFileReader == null) { json = JsonConvert.SerializeObject("Wait", Formatting.Indented); return json; }
                        TimeSpan currentTime = pl.nPlayer8.audioFileReader.CurrentTime;
                        var namemusic = pl.nPlayer8.audioFileReader.FileName.Split('\\');
                        plays.Add(new PlayListMusic
                        {
                            namemusic = namemusic[namemusic.Length - 1].Replace(".wav", "").Replace("mp3", ""),// ชื่อเพลง
                            title = "PlayList8",
                            name = pl.nPlayer8.get_textfont(),
                            bgColor = pl.nPlayer8.get_bgcolor(),
                            fgColor = pl.nPlayer8.get_fgcolor(),
                            currentSongIndex = pl.nPlayer8.get_runorder(),
                            isPlaying = pl.nPlayer8.wavePlayer.PlaybackState == PlaybackState.Playing ? true : false,
                            currentMinute = String.Format("{0:00}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds),
                            durationtotal = String.Format("{0:00}:{1:00}", pl.nPlayer8.audioFileReader.TotalTime.Minutes, pl.nPlayer8.audioFileReader.TotalTime.Seconds),// เวลาทั้งหมด
                            volume = (int)Math.Round(pl.nPlayer8.wavePlayer.Volume * 100f)
                        });
                    }
                    else
                    {


                        plays.Add(new PlayListMusic
                        {
                            namemusic = "",
                            title = "PlayList8",
                            name = pl.nPlayer8.get_textfont(),
                            bgColor = pl.nPlayer8.get_bgcolor(),
                            fgColor = pl.nPlayer8.get_fgcolor(),
                            currentSongIndex = null,
                            isPlaying = null,
                            currentMinute = "00:00",
                            durationtotal = "00:00",
                            volume = 0
                        });
                    }

                    if (PlayerID == "1")
                    {
                        plsc = pl.nPlayer1;
                        id = 1;
                    }
                    else if (PlayerID == "2")
                    {
                        plsc = pl.nPlayer2;
                        id = 2;
                    }
                    else if (PlayerID == "3")
                    {
                        plsc = pl.nPlayer3;
                        id = 3;
                    }
                    else if (PlayerID == "4")
                    {
                        plsc = pl.nPlayer4;
                        id = 4;
                    }
                    else if (PlayerID == "5")
                    {
                        plsc = pl.nPlayer5;
                        id = 5;
                    }
                    else if (PlayerID == "6")
                    {
                        plsc = pl.nPlayer6;
                        id = 6;
                    }
                    else if (PlayerID == "7")
                    {
                        plsc = pl.nPlayer7;
                        id = 7;
                    }
                    else if (PlayerID == "8")
                    {
                        plsc = pl.nPlayer8;
                        id = 8;
                    }
                    List<songs> songs = new List<songs>();
                    string fileName3 = String.Format("{0}\\{1}-List.txt", System.Environment.CurrentDirectory, Convert.ToInt32(PlayerID));
                    if (System.IO.File.Exists(fileName3))
                    {
                        List<String> data = System.IO.File.ReadAllLines(fileName3).ToList();
                        foreach (string _item in data)
                        {
                            string[] items = _item.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                            //myListView.Items.Add(new System.Windows.Forms.ListViewItem(item));

                            //System.Windows.Forms.ListViewItem item = new System.Windows.Forms.ListViewItem(items[0]);
                            //item.SubItems.Add(items[1]);
                            //item.SubItems.Add(items[2]);
                            //item.SubItems.Add(items[3]);
                            //if (items[3] == filename1)
                            //{
                            //    return true;
                            //}
                            //item.SubItems.Add(items[4]);
                            //myListView.Items.Add(item);
                            var status = plsc.pl.checkfile_gg1(items[3]);

                            songs.Add(new jsonWebAPI.songs
                            {
                                index = Int16.Parse(items[0]),
                                title = items[1],
                                duration = items[2],
                                path = items[3],
                                status_file = status
                            });
                        }
                    }
                    listc.Add(new playlist
                    {
                        playlists = plays,
                        currentPlaylist = Convert.ToInt32(PlayerID),
                        songs = songs,
                        isLoop = plsc.get_isLoop(id),
                        isRandom = plsc.get_isRamdom(id)
                    });
                }
                json = JsonConvert.SerializeObject(listc, Formatting.Indented);
            }
            else if (controlType.ToLower().Trim() == "export_file")
            {
                string base64file = pl.export_file(PlayerID);
                json = JsonConvert.SerializeObject(base64file, Formatting.Indented);
            }
            else if (controlType.ToLower().Trim() == "import_file")
            {
                bool is_save = pl.import_file(PlayerID, base64file_import);
                json = JsonConvert.SerializeObject(is_save, Formatting.Indented);
            }
            else if (controlType.ToLower().Trim() == "reorder_playlist")
            {
                pl.reorder_playlist(PlayerID, playlistData);
            }
            #endregion

            return json;
        }
    }

    public class PRC
    {
        public int PID { get; set; }
        public int Port { get; set; }
        public string Protocol { get; set; }
    }
    public class ProcManager
    {
        public void KillByPort(int port)
        {
            var processes = GetAllProcesses();
            if (processes.Any(p => p.Port == port))
                try
                {
                    Process.GetProcessById(processes.First(p => p.Port == port).PID).Kill();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            else
            {
                Console.WriteLine("No process to kill!");
            }
        }

        public List<PRC> GetAllProcesses()
        {
            var pStartInfo = new ProcessStartInfo();
            pStartInfo.FileName = "netstat.exe";
            pStartInfo.Arguments = "-a -n -o";
            pStartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            pStartInfo.UseShellExecute = false;
            pStartInfo.RedirectStandardInput = true;
            pStartInfo.RedirectStandardOutput = true;
            pStartInfo.RedirectStandardError = true;

            var process = new Process()
            {
                StartInfo = pStartInfo
            };
            process.Start();

            var soStream = process.StandardOutput;

            var output = soStream.ReadToEnd();
            if (process.ExitCode != 0)
                throw new Exception("somethign broke");

            var result = new List<PRC>();

            var lines = Regex.Split(output, "\r\n");
            foreach (var line in lines)
            {
                if (line.Trim().StartsWith("Proto"))
                    continue;

                var parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                var len = parts.Length;
                if (len > 2)
                    result.Add(new PRC
                    {
                        Protocol = parts[0],
                        Port = int.Parse(parts[1].Split(':').Last()),
                        PID = int.Parse(parts[len - 1])
                    });


            }
            return result;
        }
    }
}
