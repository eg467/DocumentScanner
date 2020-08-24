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
            this.btnScanNextMonth = new System.Windows.Forms.Button();
            this.btnChooseOutputDir = new System.Windows.Forms.Button();
            this.folderOutput = new System.Windows.Forms.FolderBrowserDialog();
            this.btnScanCurrentDate = new System.Windows.Forms.Button();
            this.btnShowOutputDir = new System.Windows.Forms.LinkLabel();
            this.txtBaseFilename = new System.Windows.Forms.TextBox();
            this.lblBaseFilename = new System.Windows.Forms.Label();
            this.listRecentFiles = new System.Windows.Forms.ListView();
            this.colPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPages = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblInstructions = new System.Windows.Forms.Label();
            this.btnDeleteLastScan = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnClearLogFile = new System.Windows.Forms.Button();
            this.btnScanSkippingTwoMonths = new System.Windows.Forms.Button();
            this.btnScanOneMonthLessOneDay = new System.Windows.Forms.Button();
            this.btnScanOneMonthPlusOneDay = new System.Windows.Forms.Button();
            this.grpAdvanceScanning = new System.Windows.Forms.GroupBox();
            this.pnlNextMonthCalendarContainer = new System.Windows.Forms.Panel();
            this.calNextMonth = new DocumentScanner.UserControls.CustomSizeCalendar();
            this.grpModifyCurrentDate = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnBackMonth = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.btnCombineFilesForCurrentDate = new System.Windows.Forms.Button();
            this.btnCombineAllFilesByDate = new System.Windows.Forms.Button();
            this.comboExistingBaseNames = new System.Windows.Forms.ComboBox();
            this.grpAdvanceScanning.SuspendLayout();
            this.pnlNextMonthCalendarContainer.SuspendLayout();
            this.grpModifyCurrentDate.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dateCurrentDocumentDate
            // 
            this.dateCurrentDocumentDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateCurrentDocumentDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateCurrentDocumentDate.Location = new System.Drawing.Point(31, 240);
            this.dateCurrentDocumentDate.Name = "dateCurrentDocumentDate";
            this.dateCurrentDocumentDate.ShowCheckBox = true;
            this.dateCurrentDocumentDate.Size = new System.Drawing.Size(1003, 92);
            this.dateCurrentDocumentDate.TabIndex = 0;
            this.dateCurrentDocumentDate.ValueChanged += new System.EventHandler(this.dateCurrentDocumentDate_ValueChanged);
            // 
            // btnScanNextMonth
            // 
            this.btnScanNextMonth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScanNextMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScanNextMonth.Location = new System.Drawing.Point(6, 840);
            this.btnScanNextMonth.Name = "btnScanNextMonth";
            this.btnScanNextMonth.Size = new System.Drawing.Size(1149, 241);
            this.btnScanNextMonth.TabIndex = 1;
            this.btnScanNextMonth.Text = "Next Month";
            this.btnScanNextMonth.UseVisualStyleBackColor = true;
            this.btnScanNextMonth.Click += new System.EventHandler(this.btnScanIncrementedDate_Click);
            // 
            // btnChooseOutputDir
            // 
            this.btnChooseOutputDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChooseOutputDir.Location = new System.Drawing.Point(31, 82);
            this.btnChooseOutputDir.Name = "btnChooseOutputDir";
            this.btnChooseOutputDir.Size = new System.Drawing.Size(1003, 49);
            this.btnChooseOutputDir.TabIndex = 2;
            this.btnChooseOutputDir.Text = "Choose Output Directory";
            this.btnChooseOutputDir.UseVisualStyleBackColor = true;
            this.btnChooseOutputDir.Click += new System.EventHandler(this.btnChooseOutputDir_Click);
            // 
            // btnScanCurrentDate
            // 
            this.btnScanCurrentDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnScanCurrentDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScanCurrentDate.Location = new System.Drawing.Point(6, 45);
            this.btnScanCurrentDate.Name = "btnScanCurrentDate";
            this.btnScanCurrentDate.Size = new System.Drawing.Size(1149, 284);
            this.btnScanCurrentDate.TabIndex = 3;
            this.btnScanCurrentDate.Text = "Scan as Current Date";
            this.btnScanCurrentDate.UseVisualStyleBackColor = false;
            this.btnScanCurrentDate.Click += new System.EventHandler(this.btnScanCurrentDate_Click);
            // 
            // btnShowOutputDir
            // 
            this.btnShowOutputDir.Enabled = false;
            this.btnShowOutputDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowOutputDir.Location = new System.Drawing.Point(31, 134);
            this.btnShowOutputDir.Name = "btnShowOutputDir";
            this.btnShowOutputDir.Size = new System.Drawing.Size(1003, 32);
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
            this.txtBaseFilename.Size = new System.Drawing.Size(449, 39);
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
            this.listRecentFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colPath,
            this.colPages});
            this.listRecentFiles.HideSelection = false;
            this.listRecentFiles.Location = new System.Drawing.Point(31, 521);
            this.listRecentFiles.Name = "listRecentFiles";
            this.listRecentFiles.Size = new System.Drawing.Size(1003, 258);
            this.listRecentFiles.TabIndex = 7;
            this.listRecentFiles.UseCompatibleStateImageBehavior = false;
            this.listRecentFiles.View = System.Windows.Forms.View.Details;
            this.listRecentFiles.SelectedIndexChanged += new System.EventHandler(this.listRecentFiles_SelectedIndexChanged);
            this.listRecentFiles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listRecentFiles_MouseDoubleClick);
            // 
            // colPath
            // 
            this.colPath.Text = "File Path";
            this.colPath.Width = 800;
            // 
            // colPages
            // 
            this.colPages.Text = "# Pages";
            this.colPages.Width = 200;
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
            this.btnDeleteLastScan.Location = new System.Drawing.Point(31, 785);
            this.btnDeleteLastScan.Name = "btnDeleteLastScan";
            this.btnDeleteLastScan.Size = new System.Drawing.Size(444, 48);
            this.btnDeleteLastScan.TabIndex = 9;
            this.btnDeleteLastScan.Text = "Delete the Last Scan";
            this.btnDeleteLastScan.UseVisualStyleBackColor = false;
            this.btnDeleteLastScan.Click += new System.EventHandler(this.btnDeleteLastScan_Click);
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(31, 839);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(1003, 500);
            this.txtLog.TabIndex = 10;
            // 
            // btnClearLogFile
            // 
            this.btnClearLogFile.Location = new System.Drawing.Point(863, 791);
            this.btnClearLogFile.Name = "btnClearLogFile";
            this.btnClearLogFile.Size = new System.Drawing.Size(171, 42);
            this.btnClearLogFile.TabIndex = 11;
            this.btnClearLogFile.Text = "Clear Log File";
            this.btnClearLogFile.UseVisualStyleBackColor = true;
            this.btnClearLogFile.Click += new System.EventHandler(this.btnClearLogFile_Click);
            // 
            // btnScanSkippingTwoMonths
            // 
            this.btnScanSkippingTwoMonths.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScanSkippingTwoMonths.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.14286F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScanSkippingTwoMonths.Location = new System.Drawing.Point(6, 1186);
            this.btnScanSkippingTwoMonths.Name = "btnScanSkippingTwoMonths";
            this.btnScanSkippingTwoMonths.Size = new System.Drawing.Size(1149, 65);
            this.btnScanSkippingTwoMonths.TabIndex = 12;
            this.btnScanSkippingTwoMonths.Text = "Next 2 Months";
            this.btnScanSkippingTwoMonths.UseVisualStyleBackColor = true;
            this.btnScanSkippingTwoMonths.Click += new System.EventHandler(this.btnScanIncrementedDate_Click);
            // 
            // btnScanOneMonthLessOneDay
            // 
            this.btnScanOneMonthLessOneDay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScanOneMonthLessOneDay.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.14286F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScanOneMonthLessOneDay.Location = new System.Drawing.Point(6, 1092);
            this.btnScanOneMonthLessOneDay.Name = "btnScanOneMonthLessOneDay";
            this.btnScanOneMonthLessOneDay.Size = new System.Drawing.Size(564, 76);
            this.btnScanOneMonthLessOneDay.TabIndex = 13;
            this.btnScanOneMonthLessOneDay.Text = "Next Month - 1 Day";
            this.btnScanOneMonthLessOneDay.UseVisualStyleBackColor = true;
            this.btnScanOneMonthLessOneDay.Click += new System.EventHandler(this.btnScanIncrementedDate_Click);
            // 
            // btnScanOneMonthPlusOneDay
            // 
            this.btnScanOneMonthPlusOneDay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScanOneMonthPlusOneDay.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.14286F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScanOneMonthPlusOneDay.Location = new System.Drawing.Point(614, 1092);
            this.btnScanOneMonthPlusOneDay.Name = "btnScanOneMonthPlusOneDay";
            this.btnScanOneMonthPlusOneDay.Size = new System.Drawing.Size(541, 76);
            this.btnScanOneMonthPlusOneDay.TabIndex = 14;
            this.btnScanOneMonthPlusOneDay.Text = "Next Month + 1 Day";
            this.btnScanOneMonthPlusOneDay.UseVisualStyleBackColor = true;
            this.btnScanOneMonthPlusOneDay.Click += new System.EventHandler(this.btnScanIncrementedDate_Click);
            // 
            // grpAdvanceScanning
            // 
            this.grpAdvanceScanning.Controls.Add(this.pnlNextMonthCalendarContainer);
            this.grpAdvanceScanning.Controls.Add(this.btnScanNextMonth);
            this.grpAdvanceScanning.Controls.Add(this.btnScanOneMonthPlusOneDay);
            this.grpAdvanceScanning.Controls.Add(this.btnScanSkippingTwoMonths);
            this.grpAdvanceScanning.Controls.Add(this.btnScanOneMonthLessOneDay);
            this.grpAdvanceScanning.Controls.Add(this.btnScanCurrentDate);
            this.grpAdvanceScanning.Location = new System.Drawing.Point(1104, 82);
            this.grpAdvanceScanning.Name = "grpAdvanceScanning";
            this.grpAdvanceScanning.Size = new System.Drawing.Size(1161, 1257);
            this.grpAdvanceScanning.TabIndex = 15;
            this.grpAdvanceScanning.TabStop = false;
            this.grpAdvanceScanning.Text = "Scan as the following date...";
            // 
            // pnlNextMonthCalendarContainer
            // 
            this.pnlNextMonthCalendarContainer.Controls.Add(this.calNextMonth);
            this.pnlNextMonthCalendarContainer.Location = new System.Drawing.Point(6, 335);
            this.pnlNextMonthCalendarContainer.Name = "pnlNextMonthCalendarContainer";
            this.pnlNextMonthCalendarContainer.Size = new System.Drawing.Size(1149, 486);
            this.pnlNextMonthCalendarContainer.TabIndex = 15;
            // 
            // calNextMonth
            // 
            this.calNextMonth.CalendarDimensions = new System.Drawing.Size(2, 1);
            this.calNextMonth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.calNextMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.calNextMonth.Location = new System.Drawing.Point(0, 0);
            this.calNextMonth.MaxSelectionCount = 1;
            this.calNextMonth.Name = "calNextMonth";
            this.calNextMonth.TabIndex = 0;
            this.calNextMonth.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.calNextMonth_DateChanged);
            // 
            // grpModifyCurrentDate
            // 
            this.grpModifyCurrentDate.AutoSize = true;
            this.grpModifyCurrentDate.Controls.Add(this.flowLayoutPanel1);
            this.grpModifyCurrentDate.Location = new System.Drawing.Point(31, 338);
            this.grpModifyCurrentDate.Name = "grpModifyCurrentDate";
            this.grpModifyCurrentDate.Size = new System.Drawing.Size(1003, 118);
            this.grpModifyCurrentDate.TabIndex = 16;
            this.grpModifyCurrentDate.TabStop = false;
            this.grpModifyCurrentDate.Text = "Modify the current date";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add(this.btnBackMonth);
            this.flowLayoutPanel1.Controls.Add(this.button1);
            this.flowLayoutPanel1.Controls.Add(this.button2);
            this.flowLayoutPanel1.Controls.Add(this.button3);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 25);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(997, 90);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // btnBackMonth
            // 
            this.btnBackMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBackMonth.Location = new System.Drawing.Point(3, 3);
            this.btnBackMonth.Name = "btnBackMonth";
            this.btnBackMonth.Size = new System.Drawing.Size(241, 79);
            this.btnBackMonth.TabIndex = 0;
            this.btnBackMonth.Tag = "-1:0";
            this.btnBackMonth.Text = "⇐ Month";
            this.btnBackMonth.UseVisualStyleBackColor = true;
            this.btnBackMonth.Click += new System.EventHandler(this.AlterCurrentDateButton_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(250, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(241, 79);
            this.button1.TabIndex = 1;
            this.button1.Tag = "0:-1";
            this.button1.Text = "← Day";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.AlterCurrentDateButton_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(497, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(241, 79);
            this.button2.TabIndex = 2;
            this.button2.Tag = "0:1";
            this.button2.Text = "Day →";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.AlterCurrentDateButton_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(744, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(241, 79);
            this.button3.TabIndex = 3;
            this.button3.Tag = "1:0";
            this.button3.Text = "Month ⇒";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.AlterCurrentDateButton_Click);
            // 
            // btnCombineFilesForCurrentDate
            // 
            this.btnCombineFilesForCurrentDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCombineFilesForCurrentDate.Location = new System.Drawing.Point(31, 463);
            this.btnCombineFilesForCurrentDate.Name = "btnCombineFilesForCurrentDate";
            this.btnCombineFilesForCurrentDate.Size = new System.Drawing.Size(435, 52);
            this.btnCombineFilesForCurrentDate.TabIndex = 17;
            this.btnCombineFilesForCurrentDate.Text = "Combine Files for This Date";
            this.btnCombineFilesForCurrentDate.UseVisualStyleBackColor = true;
            this.btnCombineFilesForCurrentDate.Click += new System.EventHandler(this.btnCombineFilesForCurrentDate_Click);
            // 
            // btnCombineAllFilesByDate
            // 
            this.btnCombineAllFilesByDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCombineAllFilesByDate.Location = new System.Drawing.Point(599, 462);
            this.btnCombineAllFilesByDate.Name = "btnCombineAllFilesByDate";
            this.btnCombineAllFilesByDate.Size = new System.Drawing.Size(435, 52);
            this.btnCombineAllFilesByDate.TabIndex = 18;
            this.btnCombineAllFilesByDate.Text = "Combine All Files by Date";
            this.btnCombineAllFilesByDate.UseVisualStyleBackColor = true;
            this.btnCombineAllFilesByDate.Click += new System.EventHandler(this.btnCombineAllFilesByDate_Click);
            // 
            // comboExistingBaseNames
            // 
            this.comboExistingBaseNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboExistingBaseNames.FormattingEnabled = true;
            this.comboExistingBaseNames.Location = new System.Drawing.Point(718, 193);
            this.comboExistingBaseNames.Name = "comboExistingBaseNames";
            this.comboExistingBaseNames.Size = new System.Drawing.Size(313, 32);
            this.comboExistingBaseNames.TabIndex = 19;
            this.comboExistingBaseNames.SelectedIndexChanged += new System.EventHandler(this.comboExistingBaseNames_SelectedIndexChanged);
            // 
            // BatchScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboExistingBaseNames);
            this.Controls.Add(this.btnCombineAllFilesByDate);
            this.Controls.Add(this.btnCombineFilesForCurrentDate);
            this.Controls.Add(this.grpModifyCurrentDate);
            this.Controls.Add(this.grpAdvanceScanning);
            this.Controls.Add(this.btnClearLogFile);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnDeleteLastScan);
            this.Controls.Add(this.lblInstructions);
            this.Controls.Add(this.listRecentFiles);
            this.Controls.Add(this.lblBaseFilename);
            this.Controls.Add(this.txtBaseFilename);
            this.Controls.Add(this.btnShowOutputDir);
            this.Controls.Add(this.btnChooseOutputDir);
            this.Controls.Add(this.dateCurrentDocumentDate);
            this.Name = "BatchScan";
            this.Size = new System.Drawing.Size(2556, 1363);
            this.Load += new System.EventHandler(this.BatchScan_Load);
            this.grpAdvanceScanning.ResumeLayout(false);
            this.pnlNextMonthCalendarContainer.ResumeLayout(false);
            this.grpModifyCurrentDate.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateCurrentDocumentDate;
        private System.Windows.Forms.Button btnScanNextMonth;
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
        private System.Windows.Forms.Button btnScanSkippingTwoMonths;
        private System.Windows.Forms.Button btnScanOneMonthLessOneDay;
        private System.Windows.Forms.Button btnScanOneMonthPlusOneDay;
        private System.Windows.Forms.GroupBox grpAdvanceScanning;
        private System.Windows.Forms.GroupBox grpModifyCurrentDate;
        private System.Windows.Forms.Button btnBackMonth;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnCombineFilesForCurrentDate;
        private System.Windows.Forms.Button btnCombineAllFilesByDate;
        private System.Windows.Forms.ComboBox comboExistingBaseNames;
        private System.Windows.Forms.ColumnHeader colPath;
        private System.Windows.Forms.ColumnHeader colPages;
        private System.Windows.Forms.Panel pnlNextMonthCalendarContainer;
        private CustomSizeCalendar calNextMonth;
    }
}
