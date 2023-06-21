
namespace MusicManager.Views
{
    partial class FormTagsEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTagsEditor));
            this.label1 = new System.Windows.Forms.Label();
            this.tboxFolder = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkAlbum = new System.Windows.Forms.CheckBox();
            this.checkArtist = new System.Windows.Forms.CheckBox();
            this.checkTitle = new System.Windows.Forms.CheckBox();
            this.tboxAlbum = new System.Windows.Forms.TextBox();
            this.tboxTitle = new System.Windows.Forms.TextBox();
            this.tboxArtist = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboxSource = new System.Windows.Forms.ComboBox();
            this.tboxRegex = new System.Windows.Forms.TextBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rtboxLog = new MusicManager.Comps.MyRichTextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnModify = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Folder";
            // 
            // tboxFolder
            // 
            this.tboxFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tboxFolder.Location = new System.Drawing.Point(68, 12);
            this.tboxFolder.Name = "tboxFolder";
            this.tboxFolder.Size = new System.Drawing.Size(574, 21);
            this.tboxFolder.TabIndex = 1;
            this.tboxFolder.TextChanged += new System.EventHandler(this.tboxFolder_TextChanged);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(648, 11);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.checkAlbum);
            this.groupBox1.Controls.Add(this.checkArtist);
            this.groupBox1.Controls.Add(this.checkTitle);
            this.groupBox1.Controls.Add(this.tboxAlbum);
            this.groupBox1.Controls.Add(this.tboxTitle);
            this.groupBox1.Controls.Add(this.tboxArtist);
            this.groupBox1.Location = new System.Drawing.Point(14, 65);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(260, 321);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Modify";
            // 
            // checkAlbum
            // 
            this.checkAlbum.AutoSize = true;
            this.checkAlbum.Location = new System.Drawing.Point(6, 74);
            this.checkAlbum.Name = "checkAlbum";
            this.checkAlbum.Size = new System.Drawing.Size(54, 16);
            this.checkAlbum.TabIndex = 0;
            this.checkAlbum.Text = "Album";
            this.checkAlbum.UseVisualStyleBackColor = true;
            // 
            // checkArtist
            // 
            this.checkArtist.AutoSize = true;
            this.checkArtist.Location = new System.Drawing.Point(6, 20);
            this.checkArtist.Name = "checkArtist";
            this.checkArtist.Size = new System.Drawing.Size(60, 16);
            this.checkArtist.TabIndex = 0;
            this.checkArtist.Text = "Artist";
            this.checkArtist.UseVisualStyleBackColor = true;
            // 
            // checkTitle
            // 
            this.checkTitle.AutoSize = true;
            this.checkTitle.Location = new System.Drawing.Point(6, 47);
            this.checkTitle.Name = "checkTitle";
            this.checkTitle.Size = new System.Drawing.Size(54, 16);
            this.checkTitle.TabIndex = 0;
            this.checkTitle.Text = "Title";
            this.checkTitle.UseVisualStyleBackColor = true;
            // 
            // tboxAlbum
            // 
            this.tboxAlbum.Location = new System.Drawing.Point(72, 72);
            this.tboxAlbum.Name = "tboxAlbum";
            this.tboxAlbum.Size = new System.Drawing.Size(182, 21);
            this.tboxAlbum.TabIndex = 1;
            this.tboxAlbum.TextChanged += new System.EventHandler(this.tboxAlbum_TextChanged);
            // 
            // tboxTitle
            // 
            this.tboxTitle.Location = new System.Drawing.Point(72, 45);
            this.tboxTitle.Name = "tboxTitle";
            this.tboxTitle.Size = new System.Drawing.Size(182, 21);
            this.tboxTitle.TabIndex = 1;
            this.tboxTitle.TextChanged += new System.EventHandler(this.tboxTitle_TextChanged);
            // 
            // tboxArtist
            // 
            this.tboxArtist.Location = new System.Drawing.Point(72, 18);
            this.tboxArtist.Name = "tboxArtist";
            this.tboxArtist.Size = new System.Drawing.Size(182, 21);
            this.tboxArtist.TabIndex = 1;
            this.tboxArtist.TextChanged += new System.EventHandler(this.tboxArtist_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(278, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "Regex";
            // 
            // cboxSource
            // 
            this.cboxSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxSource.FormattingEnabled = true;
            this.cboxSource.Items.AddRange(new object[] {
            "Artist",
            "Title",
            "Album",
            "Filename"});
            this.cboxSource.Location = new System.Drawing.Point(68, 39);
            this.cboxSource.Name = "cboxSource";
            this.cboxSource.Size = new System.Drawing.Size(204, 20);
            this.cboxSource.TabIndex = 4;
            this.cboxSource.SelectedIndexChanged += new System.EventHandler(this.cboxSource_SelectedIndexChanged);
            // 
            // tboxRegex
            // 
            this.tboxRegex.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tboxRegex.Location = new System.Drawing.Point(342, 39);
            this.tboxRegex.Name = "tboxRegex";
            this.tboxRegex.Size = new System.Drawing.Size(381, 21);
            this.tboxRegex.TabIndex = 1;
            this.tboxRegex.TextChanged += new System.EventHandler(this.tboxRegex_TextChanged);
            // 
            // btnTest
            // 
            this.btnTest.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnTest.Location = new System.Drawing.Point(208, 392);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 2;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "Source";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.rtboxLog);
            this.groupBox2.Location = new System.Drawing.Point(280, 65);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(443, 321);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Log";
            // 
            // rtboxLog
            // 
            this.rtboxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtboxLog.Location = new System.Drawing.Point(6, 20);
            this.rtboxLog.Name = "rtboxLog";
            this.rtboxLog.ReadOnly = true;
            this.rtboxLog.Size = new System.Drawing.Size(431, 295);
            this.rtboxLog.TabIndex = 0;
            this.rtboxLog.Text = "";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnClose.Location = new System.Drawing.Point(532, 392);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnModify
            // 
            this.btnModify.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnModify.Location = new System.Drawing.Point(289, 392);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(75, 23);
            this.btnModify.TabIndex = 2;
            this.btnModify.Text = "Modify";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnClear.Location = new System.Drawing.Point(451, 392);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnStop
            // 
            this.btnStop.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnStop.Location = new System.Drawing.Point(370, 392);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // FormTagsEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 427);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.cboxSource);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnModify);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.tboxRegex);
            this.Controls.Add(this.tboxFolder);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormTagsEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tags editor";
            this.Load += new System.EventHandler(this.FormTagsEditor_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tboxFolder;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkAlbum;
        private System.Windows.Forms.CheckBox checkArtist;
        private System.Windows.Forms.CheckBox checkTitle;
        private System.Windows.Forms.TextBox tboxAlbum;
        private System.Windows.Forms.TextBox tboxTitle;
        private System.Windows.Forms.TextBox tboxArtist;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboxSource;
        private System.Windows.Forms.TextBox tboxRegex;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private Comps.MyRichTextBox rtboxLog;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnStop;
    }
}