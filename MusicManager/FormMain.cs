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

        CancellationTokenSource cts;

        Dictionary<string, bool> musicExtensions = new Dictionary<string, bool>();

        public FormMain()
        {
            InitializeComponent();

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

        private void btnBrowseSrcFolder_Click(object sender, EventArgs e)
        {
            var path = Properties.Settings.Default.srcFolder;
            var src = ShowBrowseFolderDialog(path);
            if (!string.IsNullOrEmpty(src))
            {
                tboxSrcFolder.Text = src;
                Properties.Settings.Default.srcFolder = src;
                Properties.Settings.Default.Save();
            }
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
                RenameFolder(src, cts.Token);
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

        bool IsDupFile(string dup, string file)
        {
            TagLib.File mf = TagLib.File.Create(file);

            var key = mf.Tag.Title + string.Join(",", mf.Tag.Performers);
            // Log($"[dbg] {key}");

            if (string.IsNullOrWhiteSpace(key))
            {
                Log($"[empty] {file}");
                return false;
            }

            if (!musics.ContainsKey(key))
            {
                Log($"[new] {file}");
                musics.Add(key, file);
                return false;
            }

            // dup
            Log($"[dup] {file} of {musics[key]}");
            MoveFileToFolder(file, dup);
            return true;
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

        bool RenameFile(string file)
        {
            var mf = TagLib.File.Create(file);
            var folder = Path.GetDirectoryName(file);
            var ext = Path.GetExtension(file);
            var artists = string.Join(",", mf.Tag.Performers);
            var title = mf.Tag.Title;

            if (string.IsNullOrWhiteSpace(ext) || string.IsNullOrWhiteSpace(artists) || string.IsNullOrWhiteSpace(title))
            {
                return false;
            }

            var tf = $"{title} - {artists}{ext}";
            var f = RemoveIllegalFilenameChars(tf);
            var dest = Path.Combine(folder, f);

            if (file == dest)
            {
                return false;
            }
            Log($"[mv] {file} -> {dest}");
            if (File.Exists(dest))
            {
                Log($"[rm] {file}");
                File.Delete(file);
                return false;
            }
            File.Move(file, dest);
            return true;
        }

        bool IsMusicFile(string file)
        {
            var ext = Path.GetExtension(file).ToLower();
            return musicExtensions.ContainsKey(ext);
        }

        void RenameFolder(string folder, CancellationToken token)
        {
            Log($"Rename folder: {folder}");
            var cMv = 0;
            var cSkip = 0;
            var cTotal = 0;
            foreach (string file in Directory.EnumerateFiles(folder, "*.*", SearchOption.AllDirectories))
            {
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
                if (token.IsCancellationRequested)
                {
                    Log("Stop by user");
                    break;
                }
            }
            Log($"Total: {cTotal}, Move: {cMv}, Skip: {cSkip}");
        }

        void DedupFolder(string src, string dup, CancellationToken token)
        {
            Log("Remove duplicate music files.");
            Log($"Source folder: {src}");
            Log($"Duplicate folder: {dup}");

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
            foreach (string file in Directory.EnumerateFiles(src, "*.*", SearchOption.AllDirectories))
            {
                if (IsMusicFile(file))
                {
                    if (IsDupFile(dup, file))
                    {
                        cDup++;
                    }
                    else
                    {
                        cNew++;
                    }
                }
                if (token.IsCancellationRequested)
                {
                    Log("Stop by user");
                    break;
                }
            }
            Log($"Total: {cDup + cNew}, New: {cNew}, Dup: {cDup}");
        }


        #endregion


    }
}
