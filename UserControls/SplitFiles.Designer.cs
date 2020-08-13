namespace DocumentScanner.UserControls
{
    partial class SplitFilesControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSplitFiles = new System.Windows.Forms.Button();
            this.lblBaseName = new System.Windows.Forms.Label();
            this.txtBaseFileName = new System.Windows.Forms.TextBox();
            this.folderOutputPath = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // btnSplitFiles
            // 
            this.btnSplitFiles.Location = new System.Drawing.Point(518, 211);
            this.btnSplitFiles.Name = "btnSplitFiles";
            this.btnSplitFiles.Size = new System.Drawing.Size(289, 178);
            this.btnSplitFiles.TabIndex = 14;
            this.btnSplitFiles.Text = "Split Files by Date";
            this.btnSplitFiles.UseVisualStyleBackColor = true;
            this.btnSplitFiles.Click += new System.EventHandler(this.btnSplitFiles_Click);
            // 
            // lblBaseName
            // 
            this.lblBaseName.AutoSize = true;
            this.lblBaseName.Location = new System.Drawing.Point(513, 148);
            this.lblBaseName.Name = "lblBaseName";
            this.lblBaseName.Size = new System.Drawing.Size(114, 25);
            this.lblBaseName.TabIndex = 13;
            this.lblBaseName.Text = "Base Name";
            // 
            // txtBaseFileName
            // 
            this.txtBaseFileName.Location = new System.Drawing.Point(518, 176);
            this.txtBaseFileName.Name = "txtBaseFileName";
            this.txtBaseFileName.Size = new System.Drawing.Size(289, 29);
            this.txtBaseFileName.TabIndex = 12;
            // 
            // SplitFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnSplitFiles);
            this.Controls.Add(this.lblBaseName);
            this.Controls.Add(this.txtBaseFileName);
            this.Name = "SplitFiles";
            this.Size = new System.Drawing.Size(1320, 536);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSplitFiles;
        private System.Windows.Forms.Label lblBaseName;
        private System.Windows.Forms.TextBox txtBaseFileName;
        private System.Windows.Forms.FolderBrowserDialog folderOutputPath;
    }
}
