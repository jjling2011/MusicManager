
namespace MusicManager.Views
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.label1 = new System.Windows.Forms.Label();
            this.tboxSrcFolder = new System.Windows.Forms.TextBox();
            this.rtboxLog = new MusicManager.Comps.MyRichTextBox();
            this.btnBrowseSrcFolder = new System.Windows.Forms.Button();
            this.btnDedup = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tboxDupFolder = new System.Windows.Forms.TextBox();
            this.btnBrowseDupFolder = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnRename = new System.Windows.Forms.Button();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnMore = new System.Windows.Forms.Button();
            this.ctxMenuStripMore = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tagsEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.janitorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.detectSilentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeSilenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fixLyricsTagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxMenuStripMore.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sources";
            this.toolTip1.SetToolTip(this.label1, "Music file folders. Seperated by \'|\'.");
            // 
            // tboxSrcFolder
            // 
            this.tboxSrcFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tboxSrcFolder.Location = new System.Drawing.Point(75, 12);
            this.tboxSrcFolder.Name = "tboxSrcFolder";
            this.tboxSrcFolder.Size = new System.Drawing.Size(418, 21);
            this.tboxSrcFolder.TabIndex = 1;
            this.tboxSrcFolder.TextChanged += new System.EventHandler(this.tboxSrcFolder_TextChanged);
            // 
            // rtboxLog
            // 
            this.rtboxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtboxLog.Location = new System.Drawing.Point(12, 95);
            this.rtboxLog.Name = "rtboxLog";
            this.rtboxLog.ReadOnly = true;
            this.rtboxLog.Size = new System.Drawing.Size(562, 220);
            this.rtboxLog.TabIndex = 2;
            this.rtboxLog.Text = "";
            // 
            // btnBrowseSrcFolder
            // 
            this.btnBrowseSrcFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseSrcFolder.Location = new System.Drawing.Point(499, 10);
            this.btnBrowseSrcFolder.Name = "btnBrowseSrcFolder";
            this.btnBrowseSrcFolder.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseSrcFolder.TabIndex = 3;
            this.btnBrowseSrcFolder.Text = "Browse";
            this.toolTip1.SetToolTip(this.btnBrowseSrcFolder, "Open folder browser.");
            this.btnBrowseSrcFolder.UseVisualStyleBackColor = true;
            this.btnBrowseSrcFolder.Click += new System.EventHandler(this.btnBrowseSrcFolder_Click);
            // 
            // btnDedup
            // 
            this.btnDedup.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnDedup.Location = new System.Drawing.Point(94, 66);
            this.btnDedup.Name = "btnDedup";
            this.btnDedup.Size = new System.Drawing.Size(75, 23);
            this.btnDedup.TabIndex = 3;
            this.btnDedup.Text = "Dedup";
            this.toolTip1.SetToolTip(this.btnDedup, "Move duplicate music files to duplicate folder base on title and artists from tag" +
        "s.");
            this.btnDedup.UseVisualStyleBackColor = true;
            this.btnDedup.Click += new System.EventHandler(this.btnDedup_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "Duplicate";
            this.toolTip1.SetToolTip(this.label2, "Move duplicated music files to this folder.");
            // 
            // tboxDupFolder
            // 
            this.tboxDupFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tboxDupFolder.Location = new System.Drawing.Point(75, 39);
            this.tboxDupFolder.Name = "tboxDupFolder";
            this.tboxDupFolder.Size = new System.Drawing.Size(418, 21);
            this.tboxDupFolder.TabIndex = 1;
            this.tboxDupFolder.TextChanged += new System.EventHandler(this.tboxDupFolder_TextChanged);
            // 
            // btnBrowseDupFolder
            // 
            this.btnBrowseDupFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseDupFolder.Location = new System.Drawing.Point(499, 37);
            this.btnBrowseDupFolder.Name = "btnBrowseDupFolder";
            this.btnBrowseDupFolder.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseDupFolder.TabIndex = 3;
            this.btnBrowseDupFolder.Text = "Browse";
            this.toolTip1.SetToolTip(this.btnBrowseDupFolder, "Open folder browser.");
            this.btnBrowseDupFolder.UseVisualStyleBackColor = true;
            this.btnBrowseDupFolder.Click += new System.EventHandler(this.btnBrowseDupFolder_Click);
            // 
            // btnStop
            // 
            this.btnStop.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnStop.Location = new System.Drawing.Point(256, 66);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnRename
            // 
            this.btnRename.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnRename.Location = new System.Drawing.Point(175, 66);
            this.btnRename.Name = "btnRename";
            this.btnRename.Size = new System.Drawing.Size(75, 23);
            this.btnRename.TabIndex = 3;
            this.btnRename.Text = "Rename";
            this.toolTip1.SetToolTip(this.btnRename, "Rename music file to {title} - {artists}.mp3");
            this.btnRename.UseVisualStyleBackColor = true;
            this.btnRename.Click += new System.EventHandler(this.btnRename_Click);
            // 
            // btnClearLog
            // 
            this.btnClearLog.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnClearLog.Location = new System.Drawing.Point(337, 66);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(75, 23);
            this.btnClearLog.TabIndex = 3;
            this.btnClearLog.Text = "Clear";
            this.toolTip1.SetToolTip(this.btnClearLog, "Clear log.");
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // btnMore
            // 
            this.btnMore.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnMore.Location = new System.Drawing.Point(418, 66);
            this.btnMore.Name = "btnMore";
            this.btnMore.Size = new System.Drawing.Size(75, 23);
            this.btnMore.TabIndex = 3;
            this.btnMore.Text = "More";
            this.btnMore.UseVisualStyleBackColor = true;
            this.btnMore.Click += new System.EventHandler(this.btnMore_Click);
            // 
            // ctxMenuStripMore
            // 
            this.ctxMenuStripMore.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tagsEditorToolStripMenuItem,
            this.janitorToolStripMenuItem,
            this.detectSilentToolStripMenuItem,
            this.removeSilenceToolStripMenuItem,
            this.fixLyricsTagToolStripMenuItem});
            this.ctxMenuStripMore.Name = "ctxMenuStripMore";
            this.ctxMenuStripMore.Size = new System.Drawing.Size(181, 136);
            // 
            // tagsEditorToolStripMenuItem
            // 
            this.tagsEditorToolStripMenuItem.Name = "tagsEditorToolStripMenuItem";
            this.tagsEditorToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.tagsEditorToolStripMenuItem.Text = "tags editor";
            this.tagsEditorToolStripMenuItem.Click += new System.EventHandler(this.tagsEditorToolStripMenuItem_Click);
            // 
            // janitorToolStripMenuItem
            // 
            this.janitorToolStripMenuItem.Name = "janitorToolStripMenuItem";
            this.janitorToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.janitorToolStripMenuItem.Text = "janitor";
            this.janitorToolStripMenuItem.Click += new System.EventHandler(this.janitorToolStripMenuItem_Click);
            // 
            // detectSilentToolStripMenuItem
            // 
            this.detectSilentToolStripMenuItem.Name = "detectSilentToolStripMenuItem";
            this.detectSilentToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.detectSilentToolStripMenuItem.Text = "detect silence";
            this.detectSilentToolStripMenuItem.ToolTipText = "Detect front + tail silence.";
            this.detectSilentToolStripMenuItem.Click += new System.EventHandler(this.detectSilentToolStripMenuItem_Click);
            // 
            // removeSilenceToolStripMenuItem
            // 
            this.removeSilenceToolStripMenuItem.Name = "removeSilenceToolStripMenuItem";
            this.removeSilenceToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.removeSilenceToolStripMenuItem.Text = "remove silence";
            this.removeSilenceToolStripMenuItem.Click += new System.EventHandler(this.removeSilenceToolStripMenuItem_Click);
            // 
            // fixLyricsTagToolStripMenuItem
            // 
            this.fixLyricsTagToolStripMenuItem.Name = "fixLyricsTagToolStripMenuItem";
            this.fixLyricsTagToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.fixLyricsTagToolStripMenuItem.Text = "fix lyrics tag";
            this.fixLyricsTagToolStripMenuItem.ToolTipText = "Remove language tag and merge offset.";
            this.fixLyricsTagToolStripMenuItem.Click += new System.EventHandler(this.fixLyricsTagToolStripMenuItem_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 326);
            this.Controls.Add(this.btnMore);
            this.Controls.Add(this.btnClearLog);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnRename);
            this.Controls.Add(this.btnDedup);
            this.Controls.Add(this.btnBrowseDupFolder);
            this.Controls.Add(this.btnBrowseSrcFolder);
            this.Controls.Add(this.rtboxLog);
            this.Controls.Add(this.tboxDupFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tboxSrcFolder);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Music manager";
            this.ctxMenuStripMore.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tboxSrcFolder;
        private Comps.MyRichTextBox rtboxLog;
        private System.Windows.Forms.Button btnBrowseSrcFolder;
        private System.Windows.Forms.Button btnDedup;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tboxDupFolder;
        private System.Windows.Forms.Button btnBrowseDupFolder;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnRename;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnMore;
        private System.Windows.Forms.ContextMenuStrip ctxMenuStripMore;
        private System.Windows.Forms.ToolStripMenuItem tagsEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem janitorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem detectSilentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeSilenceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fixLyricsTagToolStripMenuItem;
    }
}

