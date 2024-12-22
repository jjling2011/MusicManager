using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicManager.Views
{
    public partial class FormMain : Form
    {
        readonly int MAX_END_SILENCE = 200; // ms
        readonly int MAX_HEAD_SILENCE = 1000; // ms

        CancellationTokenSource cts;
        readonly Comps.Logger logger = null;

        public FormMain()
        {
            InitializeComponent();

            this.Text = "Music manager v0.2.5";

            tboxSrcFolder.Text = Properties.Settings.Default.srcFolder;
            tboxDupFolder.Text = Properties.Settings.Default.dupFolder;
            logger = new Comps.Logger(rtboxLog);
        }

        #region UI handler
        void dbg_getter_exts(string folder)
        {
            var cache = new Dictionary<string, bool>();

            foreach (
                string file in Directory.EnumerateFiles(folder, "*.*", SearchOption.AllDirectories)
            )
            {
                var ext = Path.GetExtension(file);
                if (!cache.ContainsKey(ext))
                {
                    cache.Add(ext, true);
                    logger.Log($"{ext}: {file}");
                }
            }
        }

        private void fixLyricsTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var msg = @"Fix <UNSYNCED LYRICS> in <id3v2.3> tags?";
            var src = tboxSrcFolder.Text;
            void job(CancellationToken token)
            {
                FixLyricsTag(src, token);
            }
            DoJob(job, msg);
        }

        private void removeSilenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var msg = @"Remove head and tail silences of all musice?";
            var src = tboxSrcFolder.Text;
            void job(CancellationToken token)
            {
                RemoveSilence(src, token);
            }
            DoJob(job, msg);
        }

        private void detectSilentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var src = tboxSrcFolder.Text;
            void job(CancellationToken token)
            {
                DetectSilence(src, token);
            }
            DoJob(job, "");
        }

        private void btnMore_Click(object sender, EventArgs e)
        {
            Button btnSender = (Button)sender;
            Point ptLowerLeft = new Point(0, btnSender.Height);
            ptLowerLeft = btnSender.PointToScreen(ptLowerLeft);
            ctxMenuStripMore.Show(ptLowerLeft);
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            logger.Clear();
            // var folder = tboxSrcFolder.Text;
            // dbg_getter_exts(folder);
        }

        private void tboxSrcFolder_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.srcFolder = tboxSrcFolder.Text;
            Properties.Settings.Default.Save();
        }

        private void tboxDupFolder_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.dupFolder = tboxDupFolder.Text;
            Properties.Settings.Default.Save();
        }

        private void btnBrowseSrcFolder_Click(object sender, EventArgs e)
        {
            var src = Properties.Settings.Default.srcFolder;
            var folders = Utils.Tools.SplitFolders(src);

            var folder = Utils.UI.ShowBrowseFolderDialog(folders.LastOrDefault());
            if (string.IsNullOrEmpty(folder))
            {
                return;
            }
            if (!folders.Contains(folder))
            {
                folders.Add(folder);
            }

            src = Utils.Tools.JoinFolders(folders);
            tboxSrcFolder.Text = src;
            Properties.Settings.Default.srcFolder = src;
            Properties.Settings.Default.Save();
        }

        private void btnBrowseDupFolder_Click(object sender, EventArgs e)
        {
            var path = Properties.Settings.Default.dupFolder;
            var dup = Utils.UI.ShowBrowseFolderDialog(path);
            if (!string.IsNullOrEmpty(dup))
            {
                tboxDupFolder.Text = dup;
                Properties.Settings.Default.dupFolder = dup;
                Properties.Settings.Default.Save();
            }
        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            var src = tboxSrcFolder.Text;
            void job(CancellationToken token)
            {
                RenameFolders(src, token);
            }
            DoJob(job, "");
        }

        private void btnDedup_Click(object sender, EventArgs e)
        {
            var src = tboxSrcFolder.Text;
            var dup = tboxDupFolder.Text;

            void job(CancellationToken token)
            {
                DedupFolder(src, dup, token);
            }
            DoJob(job, "");
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            logger.Log("Send stop signal");
            cts?.Cancel();
        }

        FormJanitor janitor = null;
        FormTagsEditor tagsEditor = null;
        readonly object formLock = new object();

        private void tagsEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lock (formLock)
            {
                if (tagsEditor == null)
                {
                    tagsEditor = new FormTagsEditor();
                    tagsEditor.FormClosed += (s, evt) =>
                    {
                        tagsEditor = null;
                    };
                }
            }
            tagsEditor.Show();
            tagsEditor.Activate();
        }

        private void janitorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lock (formLock)
            {
                if (janitor == null)
                {
                    janitor = new FormJanitor();
                    janitor.FormClosed += (s, evt) =>
                    {
                        janitor = null;
                    };
                }
            }
            janitor.Show();
            janitor.Activate();
        }
        #endregion

        #region UI helper
        void ToggleBtnState(bool isEnable)
        {
            btnDedup.Invoke(
                (MethodInvoker)
                    delegate
                    {
                        btnDedup.Enabled = isEnable;
                        btnRename.Enabled = isEnable;
                        removeSilenceToolStripMenuItem.Enabled = isEnable;
                        detectSilentToolStripMenuItem.Enabled = isEnable;
                    }
            );
        }

        void DoJob(Action<CancellationToken> action, string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                if (!Utils.UI.Confirm(msg))
                {
                    return;
                }
            }

            ToggleBtnState(false);
            cts?.Cancel();
            cts = new CancellationTokenSource();

            Task.Run(() =>
            {
                try
                {
                    action(cts.Token);
                }
                catch (Exception ex)
                {
                    logger.Log($"error: {ex}");
                }
                ToggleBtnState(true);
            });
        }

        #endregion

        #region private


        Dictionary<string, string> musics = new Dictionary<string, string>();

        string SearchForDupFile(string file)
        {
            TagLib.File mf = TagLib.File.Create(file);
            var key = mf.Tag.Title + string.Join(",", mf.Tag.Performers);

            if (string.IsNullOrWhiteSpace(key))
            {
                logger.Log($"[empty] {file}");
                return string.Empty;
            }

            if (!musics.ContainsKey(key))
            {
                logger.Log($"[new] {file}");
                musics.Add(key, file);
                return string.Empty;
            }

            // keep old music file by search order
            logger.Log($"[dup] {file} of {musics[key]}");
            return file;
        }

        string invalidFilenameChars =
            new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

        string RemoveIllegalFilenameChars(string filename)
        {
            foreach (char c in invalidFilenameChars)
            {
                filename = filename.Replace(c.ToString(), "");
            }
            return filename;
        }

        string TrimString(string str, int len)
        {
            if (str.Length > len - 3)
            {
                return str.Substring(0, len - 3) + "...";
            }
            return str;
        }

        string MergePerformers(string[] performers)
        {
            var ps = performers
                .SelectMany(p => p.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                .ToList();
            string r = "";
            foreach (var p in ps)
            {
                if (r.Length + p.Length > 45)
                {
                    break;
                }
                r = r + p + ",";
            }
            if (r.Length > 1)
            {
                r = r.Substring(0, r.Length - 1);
            }
            return r;
        }

        bool RenameFile(string src)
        {
            var mf = TagLib.File.Create(src);
            var folder = Path.GetDirectoryName(src);
            var ext = Path.GetExtension(src);
            var artists = MergePerformers(mf.Tag.Performers);

            var title = mf.Tag.Title;
            title = TrimString(title, 70);

            if (
                string.IsNullOrWhiteSpace(ext)
                || string.IsNullOrWhiteSpace(artists)
                || string.IsNullOrWhiteSpace(title)
            )
            {
                logger.Log($"[empt-tag] {src}");
                return false;
            }

            var tf = $"{title} - {artists}{ext}";
            var f = RemoveIllegalFilenameChars(tf);
            var dest = Path.Combine(folder, f);

            if (src == dest)
            {
                logger.Log($"[skip] {src}");
                return false;
            }

            if (File.Exists(dest))
            {
                logger.Log($"[rm] {src}");
                File.Delete(src);
                return false;
            }

            logger.Log($"[mv] {src} -> {dest}");
            File.Move(src, dest);
            return true;
        }

        bool TryGetOffset(string s, out int ms)
        {
            ms = 0;
            if (!s.Contains("[offset:"))
            {
                return false;
            }
            var pat = @"\[offset:([+\-]?\d+)\]";
            try
            {
                var match = Regex.Match(s, pat);
                if (match.Success)
                {
                    var value = match.Groups[1].ToString();
                    if (int.TryParse(value, out ms))
                    {
                        if (ms > 9 || ms < -9)
                        {
                            return true;
                        }
                    }
                }
            }
            catch { }
            return false;
        }

        bool TryFixOneLineOfLyric(string line, int offset, out string newLine)
        {
            newLine = null;
            if (offset < 10 && offset > -10)
            {
                return false;
            }

            var pattern = @"^\[(\d+):(\d\d)\.(\d\d)\](.*)$";
            try
            {
                var match = Regex.Match(line, pattern);
                if (!match.Success)
                {
                    return false;
                }
                var remain = match.Groups[4].ToString();
                if (!int.TryParse(match.Groups[1].ToString(), out int minutes))
                {
                    return false;
                }
                if (!int.TryParse(match.Groups[2].ToString(), out int seconds))
                {
                    return false;
                }
                if (!int.TryParse(match.Groups[3].ToString(), out int hs))
                {
                    return false;
                }
                var ms = (minutes * 60 + seconds) * 1000 + hs * 10 - offset;
                ms = ms < 0 ? 0 : ms;
                hs = (ms / 10) % 100;
                seconds = (ms / 1000) % 60;
                minutes = (ms / 1000 / 60);
                newLine = $"[{minutes:D2}:{seconds:D2}.{hs:D2}]{remain}";
            }
            catch
            {
                return false;
            }
            return true;
        }

        readonly HashSet<string> lineFilters = new HashSet<string>()
        {
            "﻿[id:",
            "[hash:",
            "[length:",
            "[total:",
            "[language:",
        };

        bool IsFilteredLine(string line)
        {
            foreach (var item in lineFilters)
            {
                if (line.StartsWith(item))
                {
                    return true;
                }
            }
            return false;
        }

        string FixLyrics(string lyric)
        {
            var sb = new StringBuilder();
            var offset = 0;

            var lines = lyric.Replace("\r", "").Split('\n');
            foreach (var line in lines)
            {
                if (line == null)
                {
                    continue;
                }
                var lower = line.ToLower();
                if (IsFilteredLine(lower))
                {
                    continue;
                }
                if (TryGetOffset(lower, out int ms))
                {
                    offset = ms;
                    sb.AppendLine("[offset:0]");
                    continue;
                }
                if (TryFixOneLineOfLyric(line, offset, out string newLine))
                {
                    sb.AppendLine(newLine);
                    continue;
                }
                sb.AppendLine(line);
            }
            var s = sb.ToString().TrimEnd('\r', '\n');
            return s;
        }

        void FixLyricsTag(string sources, CancellationToken token)
        {
            var cFixed = 0;
            var cSkip = 0;
            var cNonMusic = 0;
            bool job(int idx, string file, CancellationToken tk)
            {
                if (!Utils.Tools.IsMusicFile(file))
                {
                    cNonMusic++;
                    return true;
                }

                var music = TagLib.File.Create(file);
                var lyrics = music.Tag.Lyrics;
                if (string.IsNullOrEmpty(lyrics))
                {
                    // logger.Log($"<debug> <empty lyric> [{idx}] {file}");
                    cSkip++;
                    return true;
                }

                var newLyrics = FixLyrics(lyrics);
                if (lyrics != newLyrics)
                {
                    logger.Log($"[fix] {file}");

                    // lyric will store in tag "UNSYNCED LYRICS"
                    music.Tag.Lyrics = ""; // clear old lyrics
                    music.Tag.Lyrics = newLyrics; // append lyrics

                    music.Save();
                    cFixed++;
                }
                else
                {
                    cSkip++;
                }
                return true;
            }

            logger.Log("fixing lyrics tag");
            Utils.Tools.ProcessFolders(logger, sources, job, token);
            logger.Log(
                $"total: {cFixed + cSkip + cNonMusic} modify: {cFixed} skip: {cSkip} non-music: {cNonMusic}"
            );
        }

        bool NeedToCutMusic(int head, int end)
        {
            if (end <= MAX_END_SILENCE + 300 && head <= MAX_HEAD_SILENCE + 200)
            {
                return false;
            }
            return true;
        }

        void RemoveSilence(string sources, CancellationToken token)
        {
            if (!Utils.Tools.IsFfmpegExists())
            {
                logger.Log($"\"{Utils.Tools.ffmpeg_exe}\" not found!");
                logger.Log($"please download ffmpeg.exe to \"tools\" folder");
                return;
            }

            Utils.Tools.CreateTempDir();

            var cOk = 0;
            var cFail = 0;
            var cTotal = 0;

            var folders = Utils.Tools.SplitFolders(sources);
            foreach (var folder in folders)
            {
                if (!Directory.Exists(folder))
                {
                    logger.Log($"Dir not exists: {folder}");
                    continue;
                }

                logger.Log($"Processing folder: {folder}");
                var musics = new List<string>();
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
                        goto labelEscape;
                    }

                    if (!Utils.Tools.IsMusicFile(file))
                    {
                        continue;
                    }

                    cTotal++;
                    var info = Comps.SilenceDetector.GetSilenceInfos(file);
                    if (info == null)
                    {
                        logger.Log($"[error] #{cTotal} read music file info fail");
                        continue;
                    }
                    var total = info.GetTotalSilenceMs();
                    var head = info.GetStartSilenceMs();
                    var end = info.GetEndSilenceMs();

                    if (!NeedToCutMusic(head, end))
                    {
                        logger.Log($"[skip] #{cTotal} silence: {total}ms file: {file}");
                        continue;
                    }

                    logger.Log($"[cut ] #{cTotal} silence: {total}ms file: {file}");
                    var ss = 1.0 * head / 1000;
                    var t = 1.0 * info.GetVolumeDurationMs() / 1000;
                    var tail = 1.0 * end / 1000;

                    var cut_ss = 1.0 * Math.Max(head - MAX_HEAD_SILENCE, 0) / 1000;
                    var cut_tail = 1.0 * Math.Min(end, MAX_END_SILENCE) / 1000;

                    logger.Log(
                        $"before: {ss}s tail: {tail}s duration: {t}s total: {ss + t + tail}s"
                    );
                    logger.Log(
                        $"after : {cut_ss}s tail: {cut_tail}s duration: {t}s total: {cut_ss + t + cut_tail}s"
                    );

                    if (Utils.Tools.RemoveSilence(logger, file, cut_ss, cut_tail + t))
                    {
                        logger.Log($"result: success");
                        cOk++;
                    }
                    else
                    {
                        logger.Log($"result: fail");
                        cFail++;
                    }
                }
            }

            labelEscape:
            logger.Log(
                $"Total: {cTotal} Success: {cOk} Fail: {cFail} Skip: {cTotal - cOk - cFail}"
            );
        }

        void DetectSilence(string sources, CancellationToken token)
        {
            var cTotal = 0;
            var cSilence = 0;

            var folders = Utils.Tools.SplitFolders(sources);
            foreach (var folder in folders)
            {
                if (!Directory.Exists(folder))
                {
                    logger.Log($"Dir not exists: {folder}");
                    continue;
                }

                logger.Log($"Detecting folder: {folder}");
                var musics = new List<string>();
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
                        logger.Log($"Total: {cTotal}, Detected: {cSilence}");
                        return;
                    }
                    if (!Utils.Tools.IsMusicFile(file))
                    {
                        continue;
                    }
                    cTotal++;
                    var info = Comps.SilenceDetector.GetSilenceInfos(file);
                    if (
                        info != null
                        && NeedToCutMusic(info.GetStartSilenceMs(), info.GetEndSilenceMs())
                    )
                    {
                        cSilence++;
                        var head = 1.0 * info.GetStartSilenceMs() / 1000;
                        var tail = 1.0 * info.GetEndSilenceMs() / 1000;
                        logger.Log(
                            string.Format(
                                "[detect] #{0} ({1} + {2} = {3}s) file: {4}",
                                cTotal,
                                head,
                                tail,
                                head + tail,
                                file
                            )
                        );
                    }
                }
            }
            logger.Log($"Total: {cTotal}, Detected: {cSilence}");
        }

        void RenameFolders(string src, CancellationToken token)
        {
            var cMv = 0;
            var cSkip = 0;
            var cTotal = 0;

            var folders = Utils.Tools.SplitFolders(src);
            foreach (var folder in folders)
            {
                if (!Directory.Exists(folder))
                {
                    logger.Log($"Dir not exists: {folder}");
                    continue;
                }

                logger.Log($"Rename folder: {folder}");
                var musics = new List<string>();
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
                        logger.Log($"Total: {cTotal}, Move: {cMv}, Skip: {cSkip}");
                        return;
                    }
                    if (Utils.Tools.IsMusicFile(file))
                    {
                        musics.Add(file);
                    }
                }

                foreach (string file in musics)
                {
                    if (token.IsCancellationRequested)
                    {
                        logger.Log("Stop by user");
                        logger.Log($"Total: {cTotal}, Move: {cMv}, Skip: {cSkip}");
                        return;
                    }

                    cTotal++;
                    if (RenameFile(file))
                    {
                        cMv++;
                    }
                    else
                    {
                        cSkip++;
                    }
                }
            }
            logger.Log($"Total: {cTotal}, Move: {cMv}, Skip: {cSkip}");
        }

        void DedupFolder(string src, string dup, CancellationToken token)
        {
            logger.Log("Remove duplicate music files.");
            logger.Log($"Cache folder: {dup}");

            if (!Directory.Exists(dup))
            {
                var msg = $"Folder {dup} not exists!";
                logger.Log(msg);
                MessageBox.Show(msg);
                logger.Log("Done.");
                return;
            }

            var cNew = 0;
            var cDup = 0;
            musics.Clear();

            var folders = Utils.Tools.SplitFolders(src);
            foreach (var folder in folders)
            {
                if (!Directory.Exists(folder))
                {
                    logger.Log($"Dir not exists: {folder}");
                    continue;
                }

                logger.Log($"Dedup folder: {folder}");
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
                        logger.Log($"Total: {cDup + cNew}, New: {cNew}, Dup: {cDup}");
                        return;
                    }
                    if (!Utils.Tools.IsMusicFile(file))
                    {
                        continue;
                    }
                    var dupFile = SearchForDupFile(file);
                    if (string.IsNullOrWhiteSpace(dupFile))
                    {
                        cNew++;
                        continue;
                    }
                    Utils.Tools.MoveFileToFolder(logger, dupFile, dup);
                    cDup++;
                }
            }
            logger.Log($"Total: {cDup + cNew}, New: {cNew}, Dup: {cDup}");
        }

        #endregion
    }
}
