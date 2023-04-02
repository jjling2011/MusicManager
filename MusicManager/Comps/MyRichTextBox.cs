using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicManager.Comps
{
    internal class MyRichTextBox : RichTextBox
    {
        // https://stackoverflow.com/questions/53518678/how-to-display-the-symbols-in-richtextbox
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr LoadLibrary(string lpFileName);

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cparams = base.CreateParams;
                if (LoadLibrary("msftedit.dll") != IntPtr.Zero)
                {
                    cparams.ClassName = "RICHEDIT50W";
                }
                return cparams;
            }
        }
    }
}
