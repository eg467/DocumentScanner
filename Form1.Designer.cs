namespace DocumentScanner
{
    partial class Form1
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
            this.btnScan = new System.Windows.Forms.Button();
            this.picPagePreview = new System.Windows.Forms.PictureBox();
            this.pnlPageControls = new System.Windows.Forms.FlowLayoutPanel();
            this.btnFirstPage = new System.Windows.Forms.Button();
            this.btnPreviousPage = new System.Windows.Forms.Button();
            this.lblCurrentPage = new System.Windows.Forms.Label();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.btnAdvanceStmtDate = new System.Windows.Forms.Button();
            this.btnLastPage = new System.Windows.Forms.Button();
            this.lblSkipInterval = new System.Windows.Forms.Label();
            this.numSkipInterval = new System.Windows.Forms.NumericUpDown();
            this.btnLoadTiff = new System.Windows.Forms.Button();
            this.btnViewFileInExplorer = new System.Windows.Forms.LinkLabel();
            this.fileOpenImage = new System.Windows.Forms.OpenFileDialog();
            this.txtBaseFileName = new System.Windows.Forms.TextBox();
            this.lblBaseName = new System.Windows.Forms.Label();
            this.calendarStatementDate = new System.Windows.Forms.MonthCalendar();
            this.btnSplitFiles = new System.Windows.Forms.Button();
            this.lblPreviewDate = new System.Windows.Forms.Label();
            this.picZoom = new System.Windows.Forms.PictureBox();
            this.trackZoom = new System.Windows.Forms.TrackBar();
            this.lblZoomTrackbar = new System.Windows.Forms.Label();
            this.fileScan = new System.Windows.Forms.SaveFileDialog();
            this.folderMonthlyPath = new System.Windows.Forms.FolderBrowserDialog();
            this.btnVerifyDates = new System.Windows.Forms.Button();
            this.btnConvertPdfToTiff = new System.Windows.Forms.Button();
            this.fileOpenPdf = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.picPagePreview)).BeginInit();
            this.pnlPageControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSkipInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackZoom)).BeginInit();
            this.SuspendLayout();
            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(15, 12);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(406, 58);
            this.btnScan.TabIndex = 0;
            this.btnScan.Text = "Step 1a) Scan Documents into TIFF";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnPerformScan);
            // 
            // picPagePreview
            // 
            this.picPagePreview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.picPagePreview.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picPagePreview.Location = new System.Drawing.Point(21, 315);
            this.picPagePreview.Name = "picPagePreview";
            this.picPagePreview.Size = new System.Drawing.Size(1005, 1226);
            this.picPagePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPagePreview.TabIndex = 3;
            this.picPagePreview.TabStop = false;
            this.picPagePreview.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picPagePreview_MouseDown);
            // 
            // pnlPageControls
            // 
            this.pnlPageControls.AutoSize = true;
            this.pnlPageControls.Controls.Add(this.btnFirstPage);
            this.pnlPageControls.Controls.Add(this.btnPreviousPage);
            this.pnlPageControls.Controls.Add(this.lblCurrentPage);
            this.pnlPageControls.Controls.Add(this.btnNextPage);
            this.pnlPageControls.Controls.Add(this.btnAdvanceStmtDate);
            this.pnlPageControls.Controls.Add(this.btnLastPage);
            this.pnlPageControls.Controls.Add(this.lblSkipInterval);
            this.pnlPageControls.Controls.Add(this.numSkipInterval);
            this.pnlPageControls.Location = new System.Drawing.Point(12, 188);
            this.pnlPageControls.Name = "pnlPageControls";
            this.pnlPageControls.Size = new System.Drawing.Size(1433, 100);
            this.pnlPageControls.TabIndex = 4;
            // 
            // btnFirstPage
            // 
            this.btnFirstPage.Location = new System.Drawing.Point(3, 3);
            this.btnFirstPage.Name = "btnFirstPage";
            this.btnFirstPage.Size = new System.Drawing.Size(166, 71);
            this.btnFirstPage.TabIndex = 3;
            this.btnFirstPage.Text = "First";
            this.btnFirstPage.UseVisualStyleBackColor = true;
            this.btnFirstPage.Click += new System.EventHandler(this.btnFirstPage_Click);
            // 
            // btnPreviousPage
            // 
            this.btnPreviousPage.Location = new System.Drawing.Point(175, 3);
            this.btnPreviousPage.Name = "btnPreviousPage";
            this.btnPreviousPage.Size = new System.Drawing.Size(166, 71);
            this.btnPreviousPage.TabIndex = 0;
            this.btnPreviousPage.Text = "Previous";
            this.btnPreviousPage.UseVisualStyleBackColor = true;
            this.btnPreviousPage.Click += new System.EventHandler(this.btnPreviousPage_Click);
            // 
            // lblCurrentPage
            // 
            this.lblCurrentPage.Location = new System.Drawing.Point(347, 0);
            this.lblCurrentPage.Name = "lblCurrentPage";
            this.lblCurrentPage.Size = new System.Drawing.Size(184, 74);
            this.lblCurrentPage.TabIndex = 1;
            this.lblCurrentPage.Text = "Current Page";
            this.lblCurrentPage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnNextPage
            // 
            this.btnNextPage.Location = new System.Drawing.Point(537, 3);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(148, 71);
            this.btnNextPage.TabIndex = 2;
            this.btnNextPage.Text = "Next";
            this.btnNextPage.UseVisualStyleBackColor = true;
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // btnAdvanceStmtDate
            // 
            this.btnAdvanceStmtDate.AutoSize = true;
            this.btnAdvanceStmtDate.BackColor = System.Drawing.Color.Beige;
            this.btnAdvanceStmtDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdvanceStmtDate.Location = new System.Drawing.Point(691, 3);
            this.btnAdvanceStmtDate.Name = "btnAdvanceStmtDate";
            this.btnAdvanceStmtDate.Size = new System.Drawing.Size(363, 59);
            this.btnAdvanceStmtDate.TabIndex = 10;
            this.btnAdvanceStmtDate.Text = "Advance One Month";
            this.btnAdvanceStmtDate.UseVisualStyleBackColor = false;
            this.btnAdvanceStmtDate.Click += new System.EventHandler(this.btnAdvanceStmtDate_Click);
            // 
            // btnLastPage
            // 
            this.btnLastPage.Location = new System.Drawing.Point(1060, 3);
            this.btnLastPage.Name = "btnLastPage";
            this.btnLastPage.Size = new System.Drawing.Size(148, 71);
            this.btnLastPage.TabIndex = 4;
            this.btnLastPage.Text = "Last";
            this.btnLastPage.UseVisualStyleBackColor = true;
            this.btnLastPage.Click += new System.EventHandler(this.btnLastPage_Click);
            // 
            // lblSkipInterval
            // 
            this.lblSkipInterval.AutoSize = true;
            this.lblSkipInterval.Location = new System.Drawing.Point(1214, 0);
            this.lblSkipInterval.Name = "lblSkipInterval";
            this.lblSkipInterval.Size = new System.Drawing.Size(90, 25);
            this.lblSkipInterval.TabIndex = 6;
            this.lblSkipInterval.Text = "Skip By: ";
            // 
            // numSkipInterval
            // 
            this.numSkipInterval.Location = new System.Drawing.Point(1310, 3);
            this.numSkipInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSkipInterval.Name = "numSkipInterval";
            this.numSkipInterval.Size = new System.Drawing.Size(120, 29);
            this.numSkipInterval.TabIndex = 5;
            this.numSkipInterval.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnLoadTiff
            // 
            this.btnLoadTiff.Location = new System.Drawing.Point(223, 103);
            this.btnLoadTiff.Name = "btnLoadTiff";
            this.btnLoadTiff.Size = new System.Drawing.Size(246, 63);
            this.btnLoadTiff.TabIndex = 5;
            this.btnLoadTiff.Text = "Load TIFF Image";
            this.btnLoadTiff.UseVisualStyleBackColor = true;
            this.btnLoadTiff.Click += new System.EventHandler(this.btnLoadTiff_Click);
            // 
            // btnViewFileInExplorer
            // 
            this.btnViewFileInExplorer.AutoSize = true;
            this.btnViewFileInExplorer.Location = new System.Drawing.Point(27, 126);
            this.btnViewFileInExplorer.Name = "btnViewFileInExplorer";
            this.btnViewFileInExplorer.Size = new System.Drawing.Size(188, 25);
            this.btnViewFileInExplorer.TabIndex = 6;
            this.btnViewFileInExplorer.TabStop = true;
            this.btnViewFileInExplorer.Text = "View File in Explorer";
            this.btnViewFileInExplorer.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnCurrentTiffPath_LinkClicked);
            // 
            // fileOpenImage
            // 
            this.fileOpenImage.FileName = "image.tiff";
            this.fileOpenImage.Filter = "Tiff Images|*.tiff|Tiff Images|*.tif";
            // 
            // txtBaseFileName
            // 
            this.txtBaseFileName.Location = new System.Drawing.Point(1457, 874);
            this.txtBaseFileName.Name = "txtBaseFileName";
            this.txtBaseFileName.Size = new System.Drawing.Size(289, 29);
            this.txtBaseFileName.TabIndex = 7;
            // 
            // lblBaseName
            // 
            this.lblBaseName.AutoSize = true;
            this.lblBaseName.Location = new System.Drawing.Point(1452, 846);
            this.lblBaseName.Name = "lblBaseName";
            this.lblBaseName.Size = new System.Drawing.Size(114, 25);
            this.lblBaseName.TabIndex = 8;
            this.lblBaseName.Text = "Base Name";
            // 
            // calendarStatementDate
            // 
            this.calendarStatementDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.calendarStatementDate.Location = new System.Drawing.Point(1038, 789);
            this.calendarStatementDate.MaxSelectionCount = 1;
            this.calendarStatementDate.Name = "calendarStatementDate";
            this.calendarStatementDate.TabIndex = 9;
            this.calendarStatementDate.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.calendarStatementDate_DateSelected);
            // 
            // btnSplitFiles
            // 
            this.btnSplitFiles.Location = new System.Drawing.Point(1457, 909);
            this.btnSplitFiles.Name = "btnSplitFiles";
            this.btnSplitFiles.Size = new System.Drawing.Size(289, 178);
            this.btnSplitFiles.TabIndex = 11;
            this.btnSplitFiles.Text = "Split Files by Date";
            this.btnSplitFiles.UseVisualStyleBackColor = true;
            this.btnSplitFiles.Click += new System.EventHandler(this.btnSplitFiles_Click);
            // 
            // lblPreviewDate
            // 
            this.lblPreviewDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPreviewDate.Location = new System.Drawing.Point(1042, 303);
            this.lblPreviewDate.Name = "lblPreviewDate";
            this.lblPreviewDate.Size = new System.Drawing.Size(713, 76);
            this.lblPreviewDate.TabIndex = 12;
            this.lblPreviewDate.Tag = "";
            this.lblPreviewDate.Text = "Date";
            this.lblPreviewDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblPreviewDate.Click += new System.EventHandler(this.lblPreviewDate_Click);
            // 
            // picZoom
            // 
            this.picZoom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.picZoom.Location = new System.Drawing.Point(1053, 394);
            this.picZoom.Name = "picZoom";
            this.picZoom.Size = new System.Drawing.Size(711, 383);
            this.picZoom.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picZoom.TabIndex = 13;
            this.picZoom.TabStop = false;
            // 
            // trackZoom
            // 
            this.trackZoom.LargeChange = 50;
            this.trackZoom.Location = new System.Drawing.Point(1485, 220);
            this.trackZoom.Maximum = 500;
            this.trackZoom.Minimum = 100;
            this.trackZoom.Name = "trackZoom";
            this.trackZoom.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.trackZoom.Size = new System.Drawing.Size(436, 80);
            this.trackZoom.SmallChange = 25;
            this.trackZoom.TabIndex = 14;
            this.trackZoom.TickFrequency = 50;
            this.trackZoom.Value = 100;
            this.trackZoom.Scroll += new System.EventHandler(this.trackZoom_Scroll);
            // 
            // lblZoomTrackbar
            // 
            this.lblZoomTrackbar.AutoSize = true;
            this.lblZoomTrackbar.Location = new System.Drawing.Point(1552, 188);
            this.lblZoomTrackbar.Name = "lblZoomTrackbar";
            this.lblZoomTrackbar.Size = new System.Drawing.Size(120, 25);
            this.lblZoomTrackbar.TabIndex = 15;
            this.lblZoomTrackbar.Text = "Zoom Level:";
            // 
            // fileScan
            // 
            this.fileScan.Filter = "TIFF File|*.tiff";
            // 
            // btnVerifyDates
            // 
            this.btnVerifyDates.Location = new System.Drawing.Point(1053, 12);
            this.btnVerifyDates.Name = "btnVerifyDates";
            this.btnVerifyDates.Size = new System.Drawing.Size(364, 67);
            this.btnVerifyDates.TabIndex = 16;
            this.btnVerifyDates.Text = "Verify Dates";
            this.btnVerifyDates.UseVisualStyleBackColor = true;
            this.btnVerifyDates.Click += new System.EventHandler(this.btnVerifyDates_Click);
            // 
            // btnConvertPdfToTiff
            // 
            this.btnConvertPdfToTiff.Location = new System.Drawing.Point(427, 12);
            this.btnConvertPdfToTiff.Name = "btnConvertPdfToTiff";
            this.btnConvertPdfToTiff.Size = new System.Drawing.Size(477, 57);
            this.btnConvertPdfToTiff.TabIndex = 17;
            this.btnConvertPdfToTiff.Text = "Step 1b) Convert PDF to Multi-page TIFF";
            this.btnConvertPdfToTiff.UseVisualStyleBackColor = true;
            this.btnConvertPdfToTiff.Click += new System.EventHandler(this.btnConvertPdfToTiff_Click);
            // 
            // fileOpenPdf
            // 
            this.fileOpenPdf.FileName = "openFileDialog1";
            this.fileOpenPdf.Filter = "PDF Files|*.pdf";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2203, 1553);
            this.Controls.Add(this.btnConvertPdfToTiff);
            this.Controls.Add(this.btnVerifyDates);
            this.Controls.Add(this.lblZoomTrackbar);
            this.Controls.Add(this.trackZoom);
            this.Controls.Add(this.picZoom);
            this.Controls.Add(this.lblPreviewDate);
            this.Controls.Add(this.btnSplitFiles);
            this.Controls.Add(this.calendarStatementDate);
            this.Controls.Add(this.lblBaseName);
            this.Controls.Add(this.txtBaseFileName);
            this.Controls.Add(this.btnViewFileInExplorer);
            this.Controls.Add(this.btnLoadTiff);
            this.Controls.Add(this.pnlPageControls);
            this.Controls.Add(this.picPagePreview);
            this.Controls.Add(this.btnScan);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picPagePreview)).EndInit();
            this.pnlPageControls.ResumeLayout(false);
            this.pnlPageControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSkipInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackZoom)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.PictureBox picPagePreview;
        private System.Windows.Forms.FlowLayoutPanel pnlPageControls;
        private System.Windows.Forms.Button btnFirstPage;
        private System.Windows.Forms.Button btnPreviousPage;
        private System.Windows.Forms.Label lblCurrentPage;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.Button btnLastPage;
        private System.Windows.Forms.Button btnLoadTiff;
        private System.Windows.Forms.LinkLabel btnViewFileInExplorer;
        private System.Windows.Forms.OpenFileDialog fileOpenImage;
        private System.Windows.Forms.TextBox txtBaseFileName;
        private System.Windows.Forms.Label lblBaseName;
        private System.Windows.Forms.MonthCalendar calendarStatementDate;
        private System.Windows.Forms.Button btnAdvanceStmtDate;
        private System.Windows.Forms.Button btnSplitFiles;
        private System.Windows.Forms.Label lblPreviewDate;
        private System.Windows.Forms.PictureBox picZoom;
        private System.Windows.Forms.TrackBar trackZoom;
        private System.Windows.Forms.Label lblZoomTrackbar;
        private System.Windows.Forms.SaveFileDialog fileScan;
        private System.Windows.Forms.FolderBrowserDialog folderMonthlyPath;
        private System.Windows.Forms.NumericUpDown numSkipInterval;
        private System.Windows.Forms.Label lblSkipInterval;
        private System.Windows.Forms.Button btnVerifyDates;
        private System.Windows.Forms.Button btnConvertPdfToTiff;
        private System.Windows.Forms.OpenFileDialog fileOpenPdf;
    }
}

