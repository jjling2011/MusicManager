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
