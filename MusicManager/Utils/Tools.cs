﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MusicManager.Comps;

namespace MusicManager.Utils
{
    public static class Tools
    {
        static readonly string supportedMusicFormats =
            @"aa, aax, aac, aiff, ape, dsf, flac, m4a, m4b, m4p, mp3, mpc, mpp, ogg, oga, wav, wma, wv, webm";
        static HashSet<string> musicExtensions = new HashSet<string>();

        static HashSet<string> lrcExtensions = new HashSet<string>() { ".lrc" };

        static Tools()
        {
            var formats = supportedMusicFormats.Replace(" ", "").Split(',');
            foreach (var format in formats)
            {
                musicExtensions.Add($".{format.ToLower()}");
            }
        }

        public static readonly string ffmpeg_exe = "tools\\ffmpeg.exe";
        public static readonly string temp_dir = Path.Combine(Path.GetTempPath(), "MusicManager");

        public static void CreateTempDir()
        {
            if (!Directory.Exists(temp_dir))
            {
                Directory.CreateDirectory(temp_dir);
            }
        }

        public static bool IsFfmpegExists()
        {
            return File.Exists(ffmpeg_exe);
        }

        public static bool AddSilence(Logger logger, string path, double s)
        {
            var filename = Path.GetFileName(path);
            var tmpdir = Path.Combine(temp_dir, filename);

            // beginning 3s end 1s
            // ffmpeg.exe -i input.mp3 -af apad=pad_dur=1s,areverse,apad=pad_dur=3s,areverse output.mp3
            var args =
                $"-i \"{path}\" -af apad=pad_dur={s}s,areverse,apad=pad_dur={s}s,areverse \"{tmpdir}\"";

            var ps = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = ffmpeg_exe,
                    Arguments = args,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                },
            };

            ps.Start();
            ps.WaitForExit();
            if (ps.ExitCode == 0)
            {
                var dir = Path.GetDirectoryName(path);
                MoveFileToFolder(logger, tmpdir, dir);
                return true;
            }
            return false;
        }

        public static bool RemoveSilence(Logger logger, string path, double ss, double t)
        {
            var filename = Path.GetFileName(path);
            var tmpdir = Path.Combine(temp_dir, filename);
            var args = $"-i \"{path}\" -c copy -ss {ss} -t {t} \"{tmpdir}\"";

            // logger.Log($"cmd: {ffmpeg_exe} {args}");

            var ps = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = ffmpeg_exe,
                    Arguments = args,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                },
            };

            ps.Start();
            ps.WaitForExit();
            if (ps.ExitCode == 0)
            {
                var dir = Path.GetDirectoryName(path);
                MoveFileToFolder(logger, tmpdir, dir);
                return true;
            }
            return false;
        }

        public static void MoveFileToFolder(Logger logger, string srcFile, string destFolder)
        {
            var filename = Path.GetFileName(srcFile);
            var destFile = Path.Combine(destFolder, filename);
            if (File.Exists(destFile))
            {
                File.Delete(destFile);
            }
            logger.Log($"move: {srcFile}");
            logger.Log($"to: {destFile}");
            File.Move(srcFile, destFile);
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

        public static void ProcessFolders(
            Logger logger,
            string sources,
            Func<int, string, CancellationToken, bool> action,
            CancellationToken token
        )
        {
            if (action == null)
            {
                logger.Log("error: empty action!");
                return;
            }

            var c = 0;
            var folders = SplitFolders(sources);
            foreach (var folder in folders)
            {
                if (!Directory.Exists(folder))
                {
                    logger.Log($"Dir not exists: {folder}");
                    continue;
                }
                foreach (
                    string file in Directory.EnumerateFiles(
                        folder,
                        "*.*",
                        SearchOption.AllDirectories
                    )
                )
                {
                    if (token.IsCancellationRequested)
                    {
                        logger.Log("Stop by user");
                        return;
                    }
                    if (!action(c++, file, token))
                    {
                        return;
                    }
                }
            }
        }
    }
}
