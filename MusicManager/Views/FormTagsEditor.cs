using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicManager.Views
{
    public partial class FormTagsEditor : Form
    {
        public FormTagsEditor()
        {
            InitializeComponent();
        }

        private void FormTagsEditor_Load(object sender, EventArgs e)
        {
            tboxFolder.Text = Properties.Settings.Default.tagsEditorSrcFolder;
            cboxSource.SelectedIndex = Properties.Settings.Default.tagsEditorIndex;
            tboxRegex.Text = Properties.Settings.Default.tagsEditorSrcRegex;
            tboxArtist.Text = Properties.Settings.Default.tagsEditorArtistRegex;
            tboxTitle.Text = Properties.Settings.Default.tagsEditorTitleRegex;
            tboxAlbum.Text = Properties.Settings.Default.tagsEditorAlbumRegex;
        }

        #region UI handler
        private void btnClose_Click(object sender, EventArgs e)
        {
            cts?.Cancel();
            this.Close();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var src = Properties.Settings.Default.tagsEditorSrcFolder;

            var folder = Utils.UI.ShowBrowseFolderDialog(src);
            if (string.IsNullOrEmpty(folder))
            {
                return;
            }
            tboxFolder.Text = folder;
            Properties.Settings.Default.tagsEditorSrcFolder = folder;
            Properties.Settings.Default.Save();
        }
        private void cboxSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.tagsEditorIndex = (sender as ComboBox).SelectedIndex;
            Properties.Settings.Default.Save();
        }
        private void tboxArtist_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.tagsEditorArtistRegex = (sender as TextBox).Text;
            Properties.Settings.Default.Save();
        }

        private void tboxTitle_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.tagsEditorTitleRegex = (sender as TextBox).Text;
            Properties.Settings.Default.Save();
        }

        private void tboxAlbum_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.tagsEditorAlbumRegex = (sender as TextBox).Text;
            Properties.Settings.Default.Save();
        }

        private void tboxRegex_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.tagsEditorSrcRegex = (sender as TextBox).Text;
            Properties.Settings.Default.Save();
        }
        private void tboxFolder_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.tagsEditorSrcFolder = (sender as TextBox).Text;
            Properties.Settings.Default.Save();
        }

        CancellationTokenSource cts = null;
        private void btnModify_Click(object sender, EventArgs e)
        {
            var src = tboxFolder.Text;
            if (!Directory.Exists(src))
            {
                Log($"Error: source folder do not exists!");
                return;
            }

            var dest = Path.Combine(src, "output");
            if (!Directory.Exists(dest))
            {
                Directory.CreateDirectory(dest);
            }

            var r = tboxRegex.Text;
            var artist = checkArtist.Checked ? tboxArtist.Text : null;
            var title = checkTitle.Checked ? tboxTitle.Text : null;
            var album = checkAlbum.Checked ? tboxAlbum.Text : null;
            var idx = cboxSource.SelectedIndex;

            cts?.Cancel();
            cts = new CancellationTokenSource();


            Task.Run(() =>
            {
                ModifyMusicFiles(src, dest, idx, r, artist, title, album, cts.Token);
                Log("Done!");
            });
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            var folder = tboxFolder.Text;
            var file = Utils.Tools.GetFirstMusicFile(folder);
            if (string.IsNullOrEmpty(file))
            {
                Log($"Error: find no musice file!");
                return;
            }

            Log($"File: {file}");
            string tag = GetTagFromFile(cboxSource.SelectedIndex, file);
            try
            {
                Log($"Tag: {tag}");
                var p = tboxRegex.Text;
                if (checkArtist.Checked)
                {
                    var o = Regex.Replace(tag, p, tboxArtist.Text);
                    Log($"Artis: {o}");
                }
                if (checkTitle.Checked)
                {
                    var o = Regex.Replace(tag, p, tboxTitle.Text);
                    Log($"Title: {o}");
                }
                if (checkAlbum.Checked)
                {
                    var o = Regex.Replace(tag, p, tboxAlbum.Text);
                    Log($"Album: {o}");
                }
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }
            Log("Done!");
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lock (logs)
            {
                logs.Clear();
            }
            RefreshLog();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            cts?.Cancel();
        }

        #endregion

        #region private
        void ModifyMusicFiles(string src, string dest, int index, string regex, string artist, string title, string album,
            CancellationToken token)
        {
            foreach (string file in Directory.EnumerateFiles(src, "*.*", SearchOption.AllDirectories))
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                if (!Utils.Tools.IsMusicFile(file))
                {
                    continue;
                }
                try
                {
                    ModifyTags(index, file, regex, artist, title, album);
                }
                catch (Exception ex)
                {
                    Log(ex.ToString());
                }
            }
        }

        void ModifyTags(int index, string file, string regex, string artist, string title, string album)
        {
            Log($"File: {file}");
            var tag = GetTagFromFile(index, file);
            var music = TagLib.File.Create(file);

            Log($"Tag: {tag}");
            if (artist != null)
            {
                var o = Regex.Replace(tag, regex, artist);
                Log($"Artis: {o}");
                music.Tag.Performers = new string[1] { o };
            }

            if (title != null)
            {
                var o = Regex.Replace(tag, regex, title);
                Log($"Title: {o}");
                music.Tag.Title = o;
            }

            if (album != null)
            {
                var o = Regex.Replace(tag, regex, album);
                Log($"Album: {o}");
                music.Tag.Album = o;
            }

            if (artist != null || title != null || album != null)
            {
                music.Save();
            }
        }

        private string GetTagFromFile(int index, string file)
        {
            var tags = TagLib.File.Create(file).Tag;
            switch (index)
            {
                case 1:
                    return tags.Title;
                case 2:
                    return tags.Album;
                default:
                    return string.Join(",", tags.Performers);
            }
        }

        List<string> logs = new List<string>();

        void Log(string content)
        {
            lock (logs)
            {
                logs.Add(content);
                if (logs.Count > 1024)
                {
                    while (logs.Count > 300)
                    {
                        logs.RemoveAt(0);
                    }
                }
            }
            RefreshLog();
        }

        void RefreshLog()
        {
            string content = null;
            lock (logs)
            {
                content = string.Join(Environment.NewLine, logs) ?? string.Empty;
            }
            rtboxLog.Invoke((MethodInvoker)delegate
            {
                rtboxLog.Text = content;
                rtboxLog.SelectionStart = rtboxLog.Text.Length;
                rtboxLog.ScrollToCaret();
            });
        }



        #endregion


    }
}
