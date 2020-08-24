namespace DocumentScanner.UserControls
{
    partial class MergeScansControl
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
            this.txtInputFiles = new System.Windows.Forms.TextBox();
            this.btnMerge = new System.Windows.Forms.Button();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.txtOutputFile = new System.Windows.Forms.TextBox();
            this.btnBrowseOutputPath = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.txtGlob = new System.Windows.Forms.TextBox();
            this.btnLoadFilesByPattern = new System.Windows.Forms.Button();
            this.lblInputFiles = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtInputFiles
            // 
            this.txtInputFiles.Location = new System.Drawing.Point(21, 401);
            this.txtInputFiles.Multiline = true;
            this.txtInputFiles.Name = "txtInputFiles";
            this.txtInputFiles.Size = new System.Drawing.Size(1045, 295);
            this.txtInputFiles.TabIndex = 0;
            // 
            // btnMerge
            // 
            this.btnMerge.Location = new System.Drawing.Point(21, 777);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(1045, 74);
            this.btnMerge.TabIndex = 1;
            this.btnMerge.Text = "Merge Files";
            this.btnMerge.UseVisualStyleBackColor = true;
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // lblInstructions
            // 
            this.lblInstructions.AutoSize = true;
            this.lblInstructions.Location = new System.Drawing.Point(16, 373);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(717, 25);
            this.lblInstructions.TabIndex = 2;
            this.lblInstructions.Text = "Enter lines of input file(s), one per line, in the form \'file_path|start_page:end" +
    "_page\'.";
            // 
            // txtOutputFile
            // 
            this.txtOutputFile.Location = new System.Drawing.Point(21, 730);
            this.txtOutputFile.Name = "txtOutputFile";
            this.txtOutputFile.Size = new System.Drawing.Size(771, 29);
            this.txtOutputFile.TabIndex = 3;
            // 
            // btnBrowseOutputPath
            // 
            this.btnBrowseOutputPath.Location = new System.Drawing.Point(811, 730);
            this.btnBrowseOutputPath.Name = "btnBrowseOutputPath";
            this.btnBrowseOutputPath.Size = new System.Drawing.Size(205, 29);
            this.btnBrowseOutputPath.TabIndex = 4;
            this.btnBrowseOutputPath.Text = "Browse Output Path...";
            this.btnBrowseOutputPath.UseVisualStyleBackColor = true;
            this.btnBrowseOutputPath.Click += new System.EventHandler(this.btnBrowseOutputPath_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "PDF File|*.pdf|TIFF File|*.tiff|TIf File|*.tif";
            // 
            // txtGlob
            // 
            this.txtGlob.Location = new System.Drawing.Point(21, 317);
            this.txtGlob.Name = "txtGlob";
            this.txtGlob.Size = new System.Drawing.Size(553, 29);
            this.txtGlob.TabIndex = 5;
            // 
            // btnLoadFilesByPattern
            // 
            this.btnLoadFilesByPattern.Location = new System.Drawing.Point(601, 312);
            this.btnLoadFilesByPattern.Name = "btnLoadFilesByPattern";
            this.btnLoadFilesByPattern.Size = new System.Drawing.Size(415, 40);
            this.btnLoadFilesByPattern.TabIndex = 6;
            this.btnLoadFilesByPattern.Text = "Load Files by Glob Pattern (*, ?)";
            this.btnLoadFilesByPattern.UseVisualStyleBackColor = true;
            this.btnLoadFilesByPattern.Click += new System.EventHandler(this.btnLoadFilesByPattern_Click);
            // 
            // lblInputFiles
            // 
            this.lblInputFiles.AutoSize = true;
            this.lblInputFiles.Location = new System.Drawing.Point(16, 289);
            this.lblInputFiles.Name = "lblInputFiles";
            this.lblInputFiles.Size = new System.Drawing.Size(582, 25);
            this.lblInputFiles.TabIndex = 7;
            this.lblInputFiles.Text = "Insert filenames by glob pattern (* for 0 or more characters, ? for 1)";
            // 
            // MergeScansControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblInputFiles);
            this.Controls.Add(this.btnLoadFilesByPattern);
            this.Controls.Add(this.txtGlob);
            this.Controls.Add(this.btnBrowseOutputPath);
            this.Controls.Add(this.txtOutputFile);
            this.Controls.Add(this.lblInstructions);
            this.Controls.Add(this.btnMerge);
            this.Controls.Add(this.txtInputFiles);
            this.Name = "MergeScansControl";
            this.Size = new System.Drawing.Size(1603, 927);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtInputFiles;
        private System.Windows.Forms.Button btnMerge;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.TextBox txtOutputFile;
        private System.Windows.Forms.Button btnBrowseOutputPath;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TextBox txtGlob;
        private System.Windows.Forms.Button btnLoadFilesByPattern;
        private System.Windows.Forms.Label lblInputFiles;
    }
}
