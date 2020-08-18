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
            this.SuspendLayout();
            // 
            // txtInputFiles
            // 
            this.txtInputFiles.Location = new System.Drawing.Point(23, 199);
            this.txtInputFiles.Multiline = true;
            this.txtInputFiles.Name = "txtInputFiles";
            this.txtInputFiles.Size = new System.Drawing.Size(1045, 295);
            this.txtInputFiles.TabIndex = 0;
            // 
            // btnMerge
            // 
            this.btnMerge.Location = new System.Drawing.Point(23, 575);
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
            this.lblInstructions.Location = new System.Drawing.Point(113, 28);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(717, 25);
            this.lblInstructions.TabIndex = 2;
            this.lblInstructions.Text = "Enter lines of input file(s), one per line, in the form \'file_path|start_page:end" +
    "_page\'.";
            // 
            // txtOutputFile
            // 
            this.txtOutputFile.Location = new System.Drawing.Point(23, 528);
            this.txtOutputFile.Name = "txtOutputFile";
            this.txtOutputFile.Size = new System.Drawing.Size(771, 29);
            this.txtOutputFile.TabIndex = 3;
            // 
            // btnBrowseOutputPath
            // 
            this.btnBrowseOutputPath.Location = new System.Drawing.Point(813, 528);
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
            // MergePdfsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnBrowseOutputPath);
            this.Controls.Add(this.txtOutputFile);
            this.Controls.Add(this.lblInstructions);
            this.Controls.Add(this.btnMerge);
            this.Controls.Add(this.txtInputFiles);
            this.Name = "MergePdfsControl";
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
    }
}
