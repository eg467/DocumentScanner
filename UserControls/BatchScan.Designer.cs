namespace DocumentScanner.UserControls
{
    partial class BatchScan
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;



        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dateCurrentDocumentDate = new System.Windows.Forms.DateTimePicker();
            this.btnScanIncrementedDate = new System.Windows.Forms.Button();
            this.btnChooseOutputDir = new System.Windows.Forms.Button();
            this.folderOutput = new System.Windows.Forms.FolderBrowserDialog();
            this.btnScanCurrentDate = new System.Windows.Forms.Button();
            this.btnShowOutputDir = new System.Windows.Forms.LinkLabel();
            this.txtBaseFilename = new System.Windows.Forms.TextBox();
            this.lblBaseFilename = new System.Windows.Forms.Label();
            this.listRecentFiles = new System.Windows.Forms.ListView();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.btnDeleteLastScan = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnClearLogFile = new System.Windows.Forms.Button();
            this.btnScanDoubleIncrementedDate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dateCurrentDocumentDate
            // 
            this.dateCurrentDocumentDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateCurrentDocumentDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateCurrentDocumentDate.Location = new System.Drawing.Point(31, 240);
            this.dateCurrentDocumentDate.Name = "dateCurrentDocumentDate";
            this.dateCurrentDocumentDate.ShowCheckBox = true;
            this.dateCurrentDocumentDate.Size = new System.Drawing.Size(795, 92);
            this.dateCurrentDocumentDate.TabIndex = 0;
            this.dateCurrentDocumentDate.ValueChanged += new System.EventHandler(this.dateCurrentDocumentDate_ValueChanged);
            // 
            // btnScanIncrementedDate
            // 
            this.btnScanIncrementedDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScanIncrementedDate.Location = new System.Drawing.Point(31, 346);
            this.btnScanIncrementedDate.Name = "btnScanIncrementedDate";
            this.btnScanIncrementedDate.Size = new System.Drawing.Size(1378, 138);
            this.btnScanIncrementedDate.TabIndex = 1;
            this.btnScanIncrementedDate.Text = "Scan to Next Month";
            this.btnScanIncrementedDate.UseVisualStyleBackColor = true;
            this.btnScanIncrementedDate.Click += new System.EventHandler(this.btnScanIncrementedDate_Click);
            // 
            // btnChooseOutputDir
            // 
            this.btnChooseOutputDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChooseOutputDir.Location = new System.Drawing.Point(31, 82);
            this.btnChooseOutputDir.Name = "btnChooseOutputDir";
            this.btnChooseOutputDir.Size = new System.Drawing.Size(795, 49);
            this.btnChooseOutputDir.TabIndex = 2;
            this.btnChooseOutputDir.Text = "Choose Output Directory";
            this.btnChooseOutputDir.UseVisualStyleBackColor = true;
            this.btnChooseOutputDir.Click += new System.EventHandler(this.btnChooseOutputDir_Click);
            // 
            // btnScanCurrentDate
            // 
            this.btnScanCurrentDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnScanCurrentDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScanCurrentDate.Location = new System.Drawing.Point(832, 82);
            this.btnScanCurrentDate.Name = "btnScanCurrentDate";
            this.btnScanCurrentDate.Size = new System.Drawing.Size(577, 250);
            this.btnScanCurrentDate.TabIndex = 3;
            this.btnScanCurrentDate.Text = "Scan to Here";
            this.btnScanCurrentDate.UseVisualStyleBackColor = false;
            this.btnScanCurrentDate.Click += new System.EventHandler(this.btnScanCurrentDate_Click);
            // 
            // btnShowOutputDir
            // 
            this.btnShowOutputDir.AutoSize = true;
            this.btnShowOutputDir.Enabled = false;
            this.btnShowOutputDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowOutputDir.Location = new System.Drawing.Point(31, 134);
            this.btnShowOutputDir.Name = "btnShowOutputDir";
            this.btnShowOutputDir.Size = new System.Drawing.Size(344, 32);
            this.btnShowOutputDir.TabIndex = 4;
            this.btnShowOutputDir.TabStop = true;
            this.btnShowOutputDir.Text = "Choose a Directory Above";
            this.btnShowOutputDir.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnShowOutputDir_LinkClicked);
            // 
            // txtBaseFilename
            // 
            this.txtBaseFilename.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBaseFilename.Location = new System.Drawing.Point(253, 186);
            this.txtBaseFilename.Name = "txtBaseFilename";
            this.txtBaseFilename.Size = new System.Drawing.Size(573, 39);
            this.txtBaseFilename.TabIndex = 5;
            // 
            // lblBaseFilename
            // 
            this.lblBaseFilename.AutoSize = true;
            this.lblBaseFilename.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBaseFilename.Location = new System.Drawing.Point(34, 189);
            this.lblBaseFilename.Name = "lblBaseFilename";
            this.lblBaseFilename.Size = new System.Drawing.Size(213, 32);
            this.lblBaseFilename.TabIndex = 6;
            this.lblBaseFilename.Text = "Base Filename:";
            // 
            // listRecentFiles
            // 
            this.listRecentFiles.HideSelection = false;
            this.listRecentFiles.Location = new System.Drawing.Point(31, 561);
            this.listRecentFiles.Name = "listRecentFiles";
            this.listRecentFiles.Size = new System.Drawing.Size(1378, 332);
            this.listRecentFiles.TabIndex = 7;
            this.listRecentFiles.UseCompatibleStateImageBehavior = false;
            this.listRecentFiles.View = System.Windows.Forms.View.List;
            this.listRecentFiles.SelectedIndexChanged += new System.EventHandler(this.listRecentFiles_SelectedIndexChanged);
            this.listRecentFiles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listRecentFiles_MouseDoubleClick);
            // 
            // lblInstructions
            // 
            this.lblInstructions.AutoSize = true;
            this.lblInstructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInstructions.Location = new System.Drawing.Point(31, 37);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(1193, 32);
            this.lblInstructions.TabIndex = 8;
            this.lblInstructions.Text = "Use this when documents are already separated by date and/or contain many pages p" +
    "er scan.\r\n";
            // 
            // btnDeleteLastScan
            // 
            this.btnDeleteLastScan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnDeleteLastScan.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteLastScan.Location = new System.Drawing.Point(31, 899);
            this.btnDeleteLastScan.Name = "btnDeleteLastScan";
            this.btnDeleteLastScan.Size = new System.Drawing.Size(444, 48);
            this.btnDeleteLastScan.TabIndex = 9;
            this.btnDeleteLastScan.Text = "Delete the Last Scan";
            this.btnDeleteLastScan.UseVisualStyleBackColor = false;
            this.btnDeleteLastScan.Click += new System.EventHandler(this.btnDeleteLastScan_Click);
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(31, 991);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(1378, 500);
            this.txtLog.TabIndex = 10;
            // 
            // btnClearLogFile
            // 
            this.btnClearLogFile.Location = new System.Drawing.Point(1238, 943);
            this.btnClearLogFile.Name = "btnClearLogFile";
            this.btnClearLogFile.Size = new System.Drawing.Size(171, 42);
            this.btnClearLogFile.TabIndex = 11;
            this.btnClearLogFile.Text = "Clear Log File";
            this.btnClearLogFile.UseVisualStyleBackColor = true;
            this.btnClearLogFile.Click += new System.EventHandler(this.btnClearLogFile_Click);
            // 
            // btnScanDoubleIncrementedDate
            // 
            this.btnScanDoubleIncrementedDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.14286F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScanDoubleIncrementedDate.Location = new System.Drawing.Point(31, 490);
            this.btnScanDoubleIncrementedDate.Name = "btnScanDoubleIncrementedDate";
            this.btnScanDoubleIncrementedDate.Size = new System.Drawing.Size(1378, 65);
            this.btnScanDoubleIncrementedDate.TabIndex = 12;
            this.btnScanDoubleIncrementedDate.Text = "Scan to Next 2 Months";
            this.btnScanDoubleIncrementedDate.UseVisualStyleBackColor = true;
            this.btnScanDoubleIncrementedDate.Click += new System.EventHandler(this.btnScanIncrementedDate_Click);
            // 
            // BatchScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnScanDoubleIncrementedDate);
            this.Controls.Add(this.btnClearLogFile);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnDeleteLastScan);
            this.Controls.Add(this.lblInstructions);
            this.Controls.Add(this.listRecentFiles);
            this.Controls.Add(this.lblBaseFilename);
            this.Controls.Add(this.txtBaseFilename);
            this.Controls.Add(this.btnShowOutputDir);
            this.Controls.Add(this.btnScanCurrentDate);
            this.Controls.Add(this.btnChooseOutputDir);
            this.Controls.Add(this.btnScanIncrementedDate);
            this.Controls.Add(this.dateCurrentDocumentDate);
            this.Name = "BatchScan";
            this.Size = new System.Drawing.Size(1815, 1657);
            this.Load += new System.EventHandler(this.BatchScan_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateCurrentDocumentDate;
        private System.Windows.Forms.Button btnScanIncrementedDate;
        private System.Windows.Forms.Button btnChooseOutputDir;
        private System.Windows.Forms.FolderBrowserDialog folderOutput;
        private System.Windows.Forms.Button btnScanCurrentDate;
        private System.Windows.Forms.LinkLabel btnShowOutputDir;
        private System.Windows.Forms.TextBox txtBaseFilename;
        private System.Windows.Forms.Label lblBaseFilename;
        private System.Windows.Forms.ListView listRecentFiles;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.Button btnDeleteLastScan;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnClearLogFile;
        private System.Windows.Forms.Button btnScanDoubleIncrementedDate;
    }
}
