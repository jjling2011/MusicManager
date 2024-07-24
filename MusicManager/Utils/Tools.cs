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
        static readonly string supportedMusicFormats =
            @"aa, aax, aac, aiff, ape, dsf, flac, m4a, m4b, m4p, mp3, mpc, mpp, ogg, oga, wav, wma, wv, webm";
        static HashSet<string> musicExtensions = new HashSet<string>();

        static HashSet<string> lrcExtensions = new HashSet<string>() { ".lrc", };

        static Tools()
        {
            var formats = supportedMusicFormats.Replace(" ", "").Split(',');
            foreach (var format in formats)
            {
                musicExtensions.Add($".{format.ToLower()}");
            }
        }

        static readonly char pathSpliter = '|';

        public static string JoinFolders(IEnumerable<string> folders)
        {
            var r = folders.Select(e => e.Trim()).Where(e => !string.IsNullOrWhiteSpace(e));
            return string.Join($" {pathSpliter} ", r);
        }

        public static List<string> SplitFolders(string src)
        {
            return src.Split(pathSpliter)
                .Select(e => e.Trim())
                .Where(e => !string.IsNullOrWhiteSpace(e))
                .ToList();
        }

        public static string GetFirstMusicFile(string folder)
        {
            if (!Directory.Exists(folder))
            {
                return null;
            }

            foreach (
                string file in Directory.EnumerateFiles(folder, "*.*", SearchOption.AllDirectories)
            )
            {
                if (IsMusicFile(file))
                {
                    return file;
                }
            }
            return null;
        }

        public static bool IsLrcFile(string file)
        {
            var ext = Path.GetExtension(file).ToLower();
            return lrcExtensions.Contains(ext);
        }

        public static bool IsMusicFile(string file)
        {
            var ext = Path.GetExtension(file).ToLower();
            return musicExtensions.Contains(ext);
        }
    }
}
