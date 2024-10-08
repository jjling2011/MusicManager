﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    public partial class FormTagsEditor : Form
    {
        readonly Comps.Logger logger = null;

        public FormTagsEditor()
        {
            InitializeComponent();
            logger = new Comps.Logger(rtboxLog);
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
        private void btnPickRandomMusic_Click(object sender, EventArgs e)
        {
            var idx = cboxSource.SelectedIndex;
            UpdateMatchingText(idx);
            UpdatePreviews();
        }

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
            var idx = (sender as ComboBox).SelectedIndex;
            Properties.Settings.Default.tagsEditorIndex = idx;
            Properties.Settings.Default.Save();

            UpdateMatchingText(idx);
            UpdatePreviews();
        }

        private void tboxArtist_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.tagsEditorArtistRegex = (sender as TextBox).Text;
            Properties.Settings.Default.Save();

            UpdatePreviews();
        }

        private void tboxTitle_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.tagsEditorTitleRegex = (sender as TextBox).Text;
            Properties.Settings.Default.Save();

            UpdatePreviews();
        }

        private void tboxAlbum_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.tagsEditorAlbumRegex = (sender as TextBox).Text;
            Properties.Settings.Default.Save();

            UpdatePreviews();
        }

        private void tboxRegex_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.tagsEditorSrcRegex = (sender as TextBox).Text;
            Properties.Settings.Default.Save();

            UpdatePreviews();
        }

        private void tboxFolder_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.tagsEditorSrcFolder = (sender as TextBox).Text;
            Properties.Settings.Default.Save();
        }

        CancellationTokenSource cts = null;

        private void btnModify_Click(object sender, EventArgs e)
        {
            ProcessFolder(false);
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            ProcessFolder(true);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            logger.Clear();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            cts?.Cancel();
        }

        #endregion

        #region private
        private void ProcessFolder(bool isDryRun)
        {
            var src = tboxFolder.Text;
            if (!Directory.Exists(src))
            {
                logger.Log($"Error: source folder do not exists!");
                return;
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
                ModifyMusicFiles(isDryRun, src, idx, r, artist, title, album, cts.Token);
                logger.Log("Done!");
            });
        }

        void TryUpdatePreview(Label label, TextBox tbox)
        {
            var tag = lbMatching.Text;
            var matchRegex = tboxRegex.Text;
            var repalceRegex = tbox.Text;
            var text = "";
            try
            {
                text = Regex.Replace(tag, matchRegex, repalceRegex);
            }
            catch { }
            label.Text = $"\"{text}\"";
            toolTip1.SetToolTip(label, label.Text);
        }

        void UpdatePreviews()
        {
            TryUpdatePreview(lbArtist, tboxArtist);
            TryUpdatePreview(lbAlbum, tboxAlbum);
            TryUpdatePreview(lbTitle, tboxTitle);
        }

        string PickOneRandomMusic()
        {
            var src = tboxFolder.Text;

            var r = new Random(Guid.NewGuid().GetHashCode());
            var c = 1;
            var pick = "";

            foreach (var file in Directory.EnumerateFiles(src, "*.*", SearchOption.AllDirectories))
            {
                if (!Utils.Tools.IsMusicFile(file))
                {
                    continue;
                }

                // https://stackoverflow.com/questions/32328620/read-random-line-from-a-large-text-file
                if (r.Next(c) == 0)
                {
                    pick = file;
                }
                c++;
            }
            if (!string.IsNullOrEmpty(pick))
            {
                return pick;
            }
            throw new Exception("find no music file");
        }

        void ModifyMusicFiles(
            bool isDryRun,
            string src,
            int index,
            string regex,
            string artist,
            string title,
            string album,
            CancellationToken token
        )
        {
            foreach (
                string file in Directory.EnumerateFiles(src, "*.*", SearchOption.AllDirectories)
            )
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
                    ModifyTags(isDryRun, index, file, regex, artist, title, album);
                }
                catch (Exception ex)
                {
                    logger.Log(ex.ToString());
                }
            }
        }

        void ModifyTags(
            bool isDryRun,
            int index,
            string file,
            string regex,
            string artist,
            string title,
            string album
        )
        {
            logger.Log($"File: {file}");
            var tag = GetTagFromFile(index, file);
            var music = TagLib.File.Create(file);

            logger.Log($"Text: {tag}");
            if (artist != null)
            {
                var o = Regex.Replace(tag, regex, artist);
                logger.Log($"Artis: {o}");
                music.Tag.Performers = new string[1] { o };
            }

            if (title != null)
            {
                var o = Regex.Replace(tag, regex, title);
                logger.Log($"Title: {o}");
                music.Tag.Title = o;
            }

            if (album != null)
            {
                var o = Regex.Replace(tag, regex, album);
                logger.Log($"Album: {o}");
                music.Tag.Album = o;
            }

            if (isDryRun)
            {
                return;
            }

            if (artist != null || title != null || album != null)
            {
                music.Save();
            }
        }

        void UpdateMatchingText(int idx)
        {
            var tag = "";
            try
            {
                var file = PickOneRandomMusic();
                tag = GetTagFromFile(idx, file);
            }
            catch { }

            lbMatching.Text = tag;
            toolTip1.SetToolTip(lbMatching, tag);
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
                case 3:
                    return Path.GetFileName(file);
                default:
                    return string.Join(",", tags.Performers);
            }
        }
        #endregion
    }
}
