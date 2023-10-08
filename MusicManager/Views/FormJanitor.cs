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
            tboxFolder.Text = Properties.Settings.Default.jntrSrcFolder;
        }

        #region UI handler
        private void btnBrowseFolder_Click(object sender, EventArgs e)
        {
            var folder = Utils.UI.ShowBrowseFolderDialog(tboxFolder.Text);
            if (!string.IsNullOrEmpty(folder))
            {
                tboxFolder.Text = folder;
                Properties.Settings.Default.jntrSrcFolder = folder;
                Properties.Settings.Default.Save();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            btnSearch.Enabled = false;
            var folder = tboxFolder.Text;
            Task.Run(() =>
            {
                var files = Search(folder);
                btnSearch.Invoke(
                    (MethodInvoker)
                        delegate
                        {
                            mrichFilePaths.Text = string.Join(Environment.NewLine, files);
                            btnSearch.Enabled = true;
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
                            btnRemove.Enabled = true;
                            btnSearch.PerformClick();
                        }
                );
            });
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tboxFolder_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            e.SuppressKeyPress = true;
            Properties.Settings.Default.jntrSrcFolder = tboxFolder.Text;
            Properties.Settings.Default.Save();
        }

        #endregion

        #region private
        List<string> Search(string folder)
        {
            var musics = new HashSet<string>();
            var result = new List<string>();
            if (!Directory.Exists(folder))
            {
                return result;
            }

            var files = Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories);
            var oths = new List<string>();
            foreach (var file in files)
            {
                if (Utils.Tools.IsMusicFile(file))
                {
                    musics.Add(Path.ChangeExtension(file, null));
                }
                else
                {
                    oths.Add(file);
                }
            }

            foreach (var oth in oths)
            {
                var o = Path.ChangeExtension(oth, null);
                if (!musics.Contains(o))
                {
                    result.Add(oth);
                }
            }
            return result;
        }
        #endregion
    }
}
