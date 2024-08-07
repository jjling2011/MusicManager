﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicManager.Views
{
    public partial class FormMain : Form
    {
        CancellationTokenSource cts;
        readonly Comps.Logger logger = null;

        public FormMain()
        {
            InitializeComponent();

            this.Text = "Music manager v0.2.2";

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

        private void removeSilenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleBtnState(false);

            var src = tboxSrcFolder.Text;
            cts?.Cancel();
            cts = new CancellationTokenSource();

            Task.Run(() =>
            {
                RemoveSilence(src, cts.Token);
                ToggleBtnState(true);
            });
        }

        private void detectSilentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleBtnState(false);

            var src = tboxSrcFolder.Text;
            cts?.Cancel();
            cts = new CancellationTokenSource();

            Task.Run(() =>
            {
                DetectSilence(src, cts.Token);
                ToggleBtnState(true);
            });
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
            ToggleBtnState(false);

            var src = tboxSrcFolder.Text;
            cts?.Cancel();
            cts = new CancellationTokenSource();

            Task.Run(() =>
            {
                RenameFolders(src, cts.Token);
                ToggleBtnState(true);
            });
        }

        private void btnDedup_Click(object sender, EventArgs e)
        {
            ToggleBtnState(false);

            var src = tboxSrcFolder.Text;
            var dup = tboxDupFolder.Text;

            cts?.Cancel();
            cts = new CancellationTokenSource();

            Task.Run(() =>
            {
                DedupFolder(src, dup, cts.Token);
                ToggleBtnState(true);
            });
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
                    }
            );
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

            /*
            // keep old music file by timestamp
            var dbFileTimestamp = new FileInfo(musics[key]).CreationTime;
            var curFileTimestamp = new FileInfo(file).CreationTime;
            if (dbFileTimestamp.CompareTo(curFileTimestamp) > 0)
            {
                var r = musics[key];
                musics[key] = file;
                logger.Log($"[dup] {r} of {file}");
                return r;
            }
            */

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
                    var ms = Comps.SilenceDetector.GetSilenceStartMS(file);
                    if (ms < 2 * 1000)
                    {
                        continue;
                    }

                    if (Utils.Tools.RemoveSilence(logger, file, ms))
                    {
                        cOk++;
                    }
                    else
                    {
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
                    var ms = Comps.SilenceDetector.GetSilenceStartMS(file);
                    if (ms > 2 * 1000)
                    {
                        cSilence++;
                        logger.Log($"[{cTotal}]({ms}ms) {file}");
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
