using NAudio.Wave;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Controls;

namespace TOAMediaPlayer
{
    public class CoreLibrary
    {
        public interface IOutputDevicePlugin
        {
            IWavePlayer CreateDevice(int latency);
            UserControl CreateSettingsPanel();
            string Name { get; }
            bool IsAvailable { get; }
            int Priority { get; }
        }

        public static string PrettyPrintBytes(long numBytes)
        {
            if (numBytes < 1024)
                return $"{numBytes} B";

            if (numBytes < 1048576)
                return $"{numBytes / 1024d:0.#0} KB";

            if (numBytes < 1073741824)
                return $"{numBytes / 1048576d:0.#0} MB";

            if (numBytes < 1099511627776)
                return $"{numBytes / 1073741824d:0.#0} GB";

            if (numBytes < 1125899906842624)
                return $"{numBytes / 1099511627776d:0.#0} TB";

            if (numBytes < 1152921504606846976)
                return $"{numBytes / 1125899906842624d:0.#0} PB";

            return $"{numBytes / 1152921504606846976d:0.#0} EB";
        }

        public static bool check_audio_file(String fileName)
        {
            try
            {
                if (fileName.EndsWith(".mp3"))
                {
                    WaveStream pcm = WaveFormatConversionStream.CreatePcmStream(new Mp3FileReader(fileName));
                    BlockAlignReductionStream stream = new BlockAlignReductionStream(pcm);
                    return true;

                }
                else if (fileName.EndsWith(".wav"))
                {
                    WaveFileReader wave = new WaveFileReader(fileName);
                    return true;

                }
                return false;
            }catch(Exception ex)
            {
                return false;
            }
        }

        public static TimeSpan GetNAudoSongLength(String fileName)
        {
            
            TimeSpan _timeSpan = new TimeSpan(0, 0, 0, 0);
            if (fileName.EndsWith(".mp3"))
            {
                WaveStream pcm = WaveFormatConversionStream.CreatePcmStream(new Mp3FileReader(fileName));
                BlockAlignReductionStream stream = new BlockAlignReductionStream(pcm);
                _timeSpan = stream.TotalTime;
            }
            else if (fileName.EndsWith(".wav"))
            {
                WaveFileReader wave = new WaveFileReader(fileName);
                _timeSpan = wave.TotalTime;
            }

            return _timeSpan;
            
        }
        public static byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        // Convert a byte array to an Object
        public static Object ByteArrayToObject(byte[] arrBytes)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return obj;
            }
        }
    }
}
