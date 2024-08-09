
namespace MusicManager.Views
{
    partial class FormJanitor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormJanitor));
            this.label1 = new System.Windows.Forms.Label();
            this.tboxLrcFolders = new System.Windows.Forms.TextBox();
            this.btnBrowseLrcFolder = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnBrowseMusicFolders = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tboxMusicFolders = new System.Windows.Forms.TextBox();
            this.btnNonMusic = new System.Windows.Forms.Button();
            this.mrichFilePaths = new MusicManager.Comps.MyRichTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Lyric";
            this.toolTip1.SetToolTip(this.label1, "Lyric folders.");
            // 
            // tboxLrcFolders
            // 
            this.tboxLrcFolders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tboxLrcFolders.Location = new System.Drawing.Point(59, 12);
            this.tboxLrcFolders.Name = "tboxLrcFolders";
            this.tboxLrcFolders.Size = new System.Drawing.Size(526, 21);
            this.tboxLrcFolders.TabIndex = 1;
            this.tboxLrcFolders.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tboxLrcFolder_KeyDown);
            // 
            // btnBrowseLrcFolder
            // 
            this.btnBrowseLrcFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseLrcFolder.Location = new System.Drawing.Point(591, 11);
            this.btnBrowseLrcFolder.Name = "btnBrowseLrcFolder";
            this.btnBrowseLrcFolder.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseLrcFolder.TabIndex = 3;
            this.btnBrowseLrcFolder.Text = "Browse";
            this.toolTip1.SetToolTip(this.btnBrowseLrcFolder, "Browse lyric folders.");
            this.btnBrowseLrcFolder.UseVisualStyleBackColor = true;
            this.btnBrowseLrcFolder.Click += new System.EventHandler(this.btnBrowseLrcFolders_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSearch.Location = new System.Drawing.Point(180, 378);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "Search";
            this.toolTip1.SetToolTip(this.btnSearch, "Search for files which are not associate with music files.");
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnRemove.Location = new System.Drawing.Point(342, 378);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 5;
            this.btnRemove.Text = "Remove";
            this.toolTip1.SetToolTip(this.btnRemove, "Remove files listed above.");
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.Location = new System.Drawing.Point(423, 378);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Close";
            this.toolTip1.SetToolTip(this.btnCancel, "Exit");
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnBrowseMusicFolders
            // 
            this.btnBrowseMusicFolders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseMusicFolders.Location = new System.Drawing.Point(591, 38);
            this.btnBrowseMusicFolders.Name = "btnBrowseMusicFolders";
            this.btnBrowseMusicFolders.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseMusicFolders.TabIndex = 3;
            this.btnBrowseMusicFolders.Text = "Browse";
            this.toolTip1.SetToolTip(this.btnBrowseMusicFolders, "Browse music folders.");
            this.btnBrowseMusicFolders.UseVisualStyleBackColor = true;
            this.btnBrowseMusicFolders.Click += new System.EventHandler(this.btnBrowseMusicFolders_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "Music";
            // 
            // tboxMusicFolders
            // 
            this.tboxMusicFolders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tboxMusicFolders.Location = new System.Drawing.Point(59, 39);
            this.tboxMusicFolders.Name = "tboxMusicFolders";
            this.tboxMusicFolders.Size = new System.Drawing.Size(526, 21);
            this.tboxMusicFolders.TabIndex = 1;
            this.tboxMusicFolders.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tboxMusicFolders_KeyDown);
            // 
            // btnNonMusic
            // 
            this.btnNonMusic.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnNonMusic.Location = new System.Drawing.Point(261, 378);
            this.btnNonMusic.Name = "btnNonMusic";
            this.btnNonMusic.Size = new System.Drawing.Size(75, 23);
            this.btnNonMusic.TabIndex = 4;
            this.btnNonMusic.Text = "NonMusic";
            this.toolTip1.SetToolTip(this.btnNonMusic, "Search for non-music files.");
            this.btnNonMusic.UseVisualStyleBackColor = true;
            this.btnNonMusic.Click += new System.EventHandler(this.btnNonMusic_Click);
            // 
            // mrichFilePaths
            // 
            this.mrichFilePaths.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mrichFilePaths.Location = new System.Drawing.Point(14, 66);
            this.mrichFilePaths.Name = "mrichFilePaths";
            this.mrichFilePaths.Size = new System.Drawing.Size(652, 306);
            this.mrichFilePaths.TabIndex = 2;
            this.mrichFilePaths.Text = "";
            // 
            // FormJanitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(678, 413);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnNonMusic);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnBrowseMusicFolders);
            this.Controls.Add(this.btnBrowseLrcFolder);
            this.Controls.Add(this.mrichFilePaths);
            this.Controls.Add(this.tboxMusicFolders);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tboxLrcFolders);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormJanitor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Janitor";
            this.Load += new System.EventHandler(this.FormJanitor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tboxLrcFolders;
        private Comps.MyRichTextBox mrichFilePaths;
        private System.Windows.Forms.Button btnBrowseLrcFolder;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tboxMusicFolders;
        private System.Windows.Forms.Button btnBrowseMusicFolders;
        private System.Windows.Forms.Button btnNonMusic;
    }
}