using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicManager
{
    public partial class FormMain : Form
    {
        readonly string supportedMusicFormats = @"aa, aax, aac, aiff, ape, dsf, flac, m4a, m4b, m4p, mp3, mpc, mpp, ogg, oga, wav, wma, wv, webm";

        const char pathSpliter = '|';

        CancellationTokenSource cts;

        Dictionary<string, bool> musicExtensions = new Dictionary<string, bool>();

        public FormMain()
        {
            InitializeComponent();

            this.Text = "Music manager v0.1.1";

            tboxSrcFolder.Text = Properties.Settings.Default.srcFolder;
            tboxDupFolder.Text = Properties.Settings.Default.dupFolder;

            var formats = supportedMusicFormats.Replace(" ", "").Split(',');
            foreach (var format in formats)
            {
                musicExtensions.Add($".{format.ToLower()}", true);
            }

        }

        #region UI handler
        void dbg_getter_exts(string folder)
        {
            var cache = new Dictionary<string, bool>();

            foreach (string file in Directory.EnumerateFiles(folder, "*.*", SearchOption.AllDirectories))
            {
                var ext = Path.GetExtension(file);
                if (!cache.ContainsKey(ext))
                {
                    cache.Add(ext, true);
                    Log($"{ext}: {file}");
                }
            }
        }
        private void btnClearLog_Click(object sender, EventArgs e)
        {
            rtboxLog.Text = string.Empty;
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
            var folders = SrcToFolders(src);

            var folder = ShowBrowseFolderDialog(folders.LastOrDefault());
            if (string.IsNullOrEmpty(folder))
            {
                return;
            }
            if (!folders.Contains(folder))
            {
                folders.Add(folder);
            }

            src = FoldersToSrc(folders);
            tboxSrcFolder.Text = src;
            Properties.Settings.Default.srcFolder = src;
            Properties.Settings.Default.Save();
        }

        private void btnBrowseDupFolder_Click(object sender, EventArgs e)
        {
            var path = Properties.Settings.Default.dupFolder;
            var dup = ShowBrowseFolderDialog(path);
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
            Log("Send stop signal");
            cts?.Cancel();
        }
        #endregion

        #region UI helper
        void ToggleBtnState(bool isEnable)
        {
            btnDedup.Invoke((MethodInvoker)delegate
            {
                btnDedup.Enabled = isEnable;
                btnRename.Enabled = isEnable;
            });
        }

        private static string ShowBrowseFolderDialog(string initPath)
        {
            string folderPath = "";
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = initPath;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                folderPath = dialog.SelectedPath;
            }

            return folderPath;
        }
        #endregion

        #region private
        string FoldersToSrc(IEnumerable<string> folders)
        {
            var r = folders.Select(e => e.Trim()).Where(e => !string.IsNullOrWhiteSpace(e));
            return string.Join($" {pathSpliter} ", r);
        }

        List<string> SrcToFolders(string src)
        {
            return src.Split(pathSpliter)
                .Select(e => e.Trim())
                .Where(e => !string.IsNullOrWhiteSpace(e))
                .ToList();
        }

        void Log(string content)
        {
            rtboxLog.Invoke((MethodInvoker)delegate
            {
                rtboxLog.Text = rtboxLog.Text + content + "\n";
                rtboxLog.SelectionStart = rtboxLog.Text.Length;
                rtboxLog.ScrollToCaret();
            });
        }

        Dictionary<string, string> musics = new Dictionary<string, string>();

        string SearchForDupFile(string file)
        {
            TagLib.File mf = TagLib.File.Create(file);
            var key = mf.Tag.Title + string.Join(",", mf.Tag.Performers);

            if (string.IsNullOrWhiteSpace(key))
            {
                Log($"[empty] {file}");
                return string.Empty;
            }

            if (!musics.ContainsKey(key))
            {
                Log($"[new] {file}");
                musics.Add(key, file);
                return string.Empty;
            }

            var dbFileTimestamp = new FileInfo(musics[key]).CreationTime;
            var curFileTimestamp = new FileInfo(file).CreationTime;
            if (dbFileTimestamp.CompareTo(curFileTimestamp) > 0)
            {
                var r = musics[key];
                musics[key] = file;
                Log($"[dup] {r} of {file}");
                return r;
            }

            Log($"[dup] {file} of {musics[key]}");
            return file;
        }

        void MoveFileToFolder(string srcFile, string destFolder)
        {
            var filename = Path.GetFileName(srcFile);
            var destFile = Path.Combine(destFolder, filename);
            if (File.Exists(destFile))
            {
                File.Delete(destFile);
            }
            Log($"[mv] {srcFile} to {destFile}");
            File.Move(srcFile, destFile);
        }

        string invalidFilenameChars = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
        string RemoveIllegalFilenameChars(string filename)
        {
            foreach (char c in invalidFilenameChars)
            {
                filename = filename.Replace(c.ToString(), "");
            }
            return filename;
        }

        bool RenameFile(string src)
        {
            var mf = TagLib.File.Create(src);
            var folder = Path.GetDirectoryName(src);
            var ext = Path.GetExtension(src);
            var artists = string.Join(",", mf.Tag.Performers);
            var title = mf.Tag.Title;

            if (string.IsNullOrWhiteSpace(ext) || string.IsNullOrWhiteSpace(artists) || string.IsNullOrWhiteSpace(title))
            {
                Log($"[empt-tag] {src}");
                return false;
            }

            var tf = $"{title} - {artists}{ext}";
            var f = RemoveIllegalFilenameChars(tf);
            var dest = Path.Combine(folder, f);

            if (src == dest)
            {
                Log($"[skip] {src}");
                return false;
            }

            if (File.Exists(dest))
            {
                Log($"[rm] {src}");
                File.Delete(src);
                return false;
            }

            Log($"[mv] {src} -> {dest}");
            File.Move(src, dest);
            return true;
        }

        bool IsMusicFile(string file)
        {
            var ext = Path.GetExtension(file).ToLower();
            return musicExtensions.ContainsKey(ext);
        }

        void RenameFolders(string src, CancellationToken token)
        {
            var cMv = 0;
            var cSkip = 0;
            var cTotal = 0;

            var folders = SrcToFolders(src);
            foreach (var folder in folders)
            {
                Log($"Rename folder: {folder}");
                foreach (string file in Directory.EnumerateFiles(folder, "*.*", SearchOption.AllDirectories))
                {
                    if (token.IsCancellationRequested)
                    {
                        Log("Stop by user");
                        Log($"Total: {cTotal}, Move: {cMv}, Skip: {cSkip}");
                        return;
                    }
                    if (IsMusicFile(file))
                    {
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
            }
            Log($"Total: {cTotal}, Move: {cMv}, Skip: {cSkip}");
        }

        void DedupFolder(string src, string dup, CancellationToken token)
        {
            Log("Remove duplicate music files.");
            Log($"Cache folder: {dup}");

            if (!Directory.Exists(dup))
            {
                var msg = $"Folder {dup} not exists!";
                Log(msg);
                MessageBox.Show(msg);
                Log("Done.");
                return;
            }

            var cNew = 0;
            var cDup = 0;
            musics.Clear();

            var folders = SrcToFolders(src);
            foreach (var folder in folders)
            {
                Log($"Dedup folder: {folder}");
                foreach (string file in Directory.EnumerateFiles(folder, "*.*", SearchOption.AllDirectories))
                {
                    if (token.IsCancellationRequested)
                    {
                        Log("Stop by user");
                        Log($"Total: {cDup + cNew}, New: {cNew}, Dup: {cDup}");
                        return;
                    }
                    if (!IsMusicFile(file))
                    {
                        continue;
                    }
                    var dupFile = SearchForDupFile(file);
                    if (string.IsNullOrWhiteSpace(dupFile))
                    {
                        cNew++;
                        continue;
                    }
                    MoveFileToFolder(dupFile, dup);
                    cDup++;
                }
            }
            Log($"Total: {cDup + cNew}, New: {cNew}, Dup: {cDup}");
        }



        #endregion


    }
}
