using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicManager.Views
{
    public partial class FormJanitor : Form
    {
        public FormJanitor()
        {
            InitializeComponent();
        }

        private void FormJanitor_Load(object sender, EventArgs e)
        {
            tboxLrcFolders.Text = Properties.Settings.Default.jntrLrcFolder;
            tboxMusicFolders.Text = Properties.Settings.Default.jntrMusicFolder;
        }

        #region UI handler
        private void btnBrowseLrcFolders_Click(object sender, EventArgs e)
        {
            var src = Properties.Settings.Default.jntrLrcFolder;
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
            tboxLrcFolders.Text = src;
            Properties.Settings.Default.jntrLrcFolder = src;
            Properties.Settings.Default.Save();
        }

        private void btnBrowseMusicFolders_Click(object sender, EventArgs e)
        {
            var src = Properties.Settings.Default.jntrMusicFolder;
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
            tboxMusicFolders.Text = src;
            Properties.Settings.Default.jntrMusicFolder = src;
            Properties.Settings.Default.Save();
        }

        private void btnNonMusic_Click(object sender, EventArgs e)
        {
            btnNonMusic.Enabled = false;
            var musicFolders = tboxMusicFolders.Text;
            Task.Run(() =>
            {
                var files = SearchFiles(musicFolders, false, false);
                btnNonMusic.Invoke(
                    (MethodInvoker)
                        delegate
                        {
                            mrichFilePaths.Text = string.Join(Environment.NewLine, files);
                            btnNonMusic.Enabled = true;
                            MessageBox.Show($"Total: {files.Count} files");
                        }
                );
            });
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            btnSearch.Enabled = false;
            var lrcFolders = tboxLrcFolders.Text;
            var musicFolders = tboxMusicFolders.Text;
            Task.Run(() =>
            {
                var files = SearchForInvalidLrcs(lrcFolders, musicFolders);
                btnSearch.Invoke(
                    (MethodInvoker)
                        delegate
                        {
                            mrichFilePaths.Text = string.Join(Environment.NewLine, files);
                            btnSearch.Enabled = true;
                            MessageBox.Show($"Total: {files.Count} files");
                        }
                );
            });
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            var files = mrichFilePaths.Text?.Split(
                new char[] { '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries
            );
            if (files == null || files.Length < 1)
            {
                MessageBox.Show("No file to remove.");
                return;
            }
            var ok = Utils.UI.Confirm($"Are you sure want to remove {files.Length} files?");
            if (!ok)
            {
                return;
            }
            btnRemove.Enabled = false;
            Task.Run(() =>
            {
                foreach (var file in files)
                {
                    if (!File.Exists(file))
                    {
                        continue;
                    }

                    try
                    {
                        File.Delete(file);
                    }
                    catch { }
                }
                btnRemove.Invoke(
                    (MethodInvoker)
                        delegate
                        {
                            MessageBox.Show("Done!");
                            mrichFilePaths.Text = "";
                            btnRemove.Enabled = true;
                        }
                );
            });
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tboxMusicFolders_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            e.SuppressKeyPress = true;
            Properties.Settings.Default.jntrMusicFolder = tboxMusicFolders.Text;
            Properties.Settings.Default.Save();
        }

        private void tboxLrcFolder_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            e.SuppressKeyPress = true;
            Properties.Settings.Default.jntrLrcFolder = tboxLrcFolders.Text;
            Properties.Settings.Default.Save();
        }

        #endregion

        #region private
        HashSet<string> SearchFiles(string srcs, bool isMusic, bool filenameOnly)
        {
            var musics = new HashSet<string>();
            var folders = Utils.Tools.SplitFolders(srcs);
            foreach (var folder in folders)
            {
                if (!Directory.Exists(folder))
                {
                    continue;
                }
                var files = Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    if (!(isMusic ^ Utils.Tools.IsMusicFile(file)))
                    {
                        if (filenameOnly)
                        {
                            var fn = Path.GetFileNameWithoutExtension(file);
                            musics.Add(fn);
                        }
                        else
                        {
                            musics.Add(file);
                        }
                    }
                }
            }
            return musics;
        }

        List<string> SearchForInvalidLrcs(string lrcSrc, string musicSrc)
        {
            var result = new List<string>();
            var musics = SearchFiles(musicSrc, true, true);
            var folders = Utils.Tools.SplitFolders(lrcSrc);
            foreach (var folder in folders)
            {
                if (!Directory.Exists(folder))
                {
                    continue;
                }
                var files = Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    if (Utils.Tools.IsLrcFile(file))
                    {
                        var fn = Path.GetFileNameWithoutExtension(file);
                        if (!musics.Contains(fn))
                        {
                            result.Add(file);
                        }
                    }
                }
            }
            return result;
        }
        #endregion
    }
}
