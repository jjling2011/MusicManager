using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicManager.Utils
{
    public static class UI
    {
        public static bool Confirm(string text)
        {
            var ok = MessageBox.Show(
                text,
                "Confirm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2
            );
            return ok == DialogResult.Yes;
        }

        public static string ShowBrowseFolderDialog(string initPath)
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
    }
}
