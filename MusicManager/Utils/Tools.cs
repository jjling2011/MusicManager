using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicManager.Utils
{

    public static class Tools
    {
        readonly static string supportedMusicFormats = @"aa, aax, aac, aiff, ape, dsf, flac, m4a, m4b, m4p, mp3, mpc, mpp, ogg, oga, wav, wma, wv, webm";
        static Dictionary<string, bool> musicExtensions = new Dictionary<string, bool>();

        static Tools()
        {
            var formats = supportedMusicFormats.Replace(" ", "").Split(',');
            foreach (var format in formats)
            {
                musicExtensions.Add($".{format.ToLower()}", true);
            }
        }

        public static string GetFirstMusicFile(string folder)
        {
            if (!Directory.Exists(folder))
            {
                return null;
            }

            foreach (string file in Directory.EnumerateFiles(folder, "*.*", SearchOption.AllDirectories))
            {

                if (IsMusicFile(file))
                {
                    return file;
                }
            }
            return null;
        }

        public static bool IsMusicFile(string file)
        {
            var ext = Path.GetExtension(file).ToLower();
            return musicExtensions.ContainsKey(ext);
        }
    }
}
