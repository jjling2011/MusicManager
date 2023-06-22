using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicManager.Comps
{
    public class Logger
    {
        private readonly RichTextBox richTextBox;
        private readonly List<string> cache = new List<string>();

        public Logger(RichTextBox richTextBox)
        {
            this.richTextBox = richTextBox;
        }

        #region public 
        public void Clear()
        {
            lock (cache)
            {
                cache.Clear();
            }
            Refresh();
        }

        public void Log(string content)
        {
            TrimDown();
            lock (cache)
            {
                cache.Add(content);
            }
            Refresh();
        }
        #endregion

        #region private
        bool needRefresh = false;
        bool isRefreshing = false;
        readonly object refreshLock = new object();

        void Refresh()
        {
            lock (refreshLock)
            {
                if (isRefreshing)
                {
                    needRefresh = true;
                    return;
                }
                isRefreshing = true;
            }

            var content = string.Empty;
            lock (cache)
            {
                content = string.Join(Environment.NewLine, cache);
            }
            richTextBox.Invoke((MethodInvoker)delegate
            {
                richTextBox.Text = content;
                richTextBox.SelectionStart = richTextBox.Text.Length;
                richTextBox.ScrollToCaret();
            });

            Task.Run(() =>
            {
                Task.Delay(1000).Wait();
                lock (refreshLock)
                {
                    isRefreshing = false;
                }
                if (needRefresh)
                {
                    needRefresh = false;
                    Refresh();
                }
            });
        }

        void TrimDown()
        {
            if (cache.Count < 2048)
            {
                return;
            }
            lock (cache)
            {
                while (cache.Count > 1024)
                {
                    cache.RemoveAt(0);
                }
            }
        }
        #endregion


    }
}
