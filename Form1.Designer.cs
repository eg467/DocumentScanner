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
            this.fileOpenImage = new System.Windows.Forms.OpenFileDialog();
            this.fileScan = new System.Windows.Forms.SaveFileDialog();
            this.folderMonthlyPath = new System.Windows.Forms.FolderBrowserDialog();
            this.fileOpenPdf = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.btnStep1ScanImage = new System.Windows.Forms.ToolStripMenuItem();
            this.btnScanToTiff = new System.Windows.Forms.ToolStripMenuItem();
            this.btnConvertPdfToTiff = new System.Windows.Forms.ToolStripMenuItem();
            this.btnLoadTiff = new System.Windows.Forms.ToolStripMenuItem();
            this.btnLoadProject = new System.Windows.Forms.ToolStripMenuItem();
            this.btnStep2ProcessDates = new System.Windows.Forms.ToolStripMenuItem();
            this.btnBatchDateProcessing = new System.Windows.Forms.ToolStripMenuItem();
            this.individualPagedProcessingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnViewImageInExplorer = new System.Windows.Forms.ToolStripMenuItem();
            this.btnStep3SplitToPdf = new System.Windows.Forms.ToolStripMenuItem();
            this.btnTools = new System.Windows.Forms.ToolStripMenuItem();
            this.btnConfigureZoom = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSetDateFormat = new System.Windows.Forms.ToolStripMenuItem();
            this.comboDateFormats = new System.Windows.Forms.ToolStripComboBox();
            this.pnlMainContainer = new System.Windows.Forms.Panel();
            this.btnTestZoom = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileOpenImage
            // 
            this.fileOpenImage.FileName = "image.tiff";
            this.fileOpenImage.Filter = "Tiff Images|*.tiff|Tiff Images|*.tif";
            // 
            // fileScan
            // 
            this.fileScan.Filter = "TIFF File|*.tiff";
            // 
            // fileOpenPdf
            // 
            this.fileOpenPdf.FileName = "openFileDialog1";
            this.fileOpenPdf.Filter = "PDF Files|*.pdf";
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnStep1ScanImage,
            this.btnStep2ProcessDates,
            this.btnStep3SplitToPdf,
            this.btnTools});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(2203, 38);
            this.menuStrip1.TabIndex = 18;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // btnStep1ScanImage
            // 
            this.btnStep1ScanImage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnScanToTiff,
            this.btnConvertPdfToTiff,
            this.btnLoadTiff,
            this.btnLoadProject});
            this.btnStep1ScanImage.Name = "btnStep1ScanImage";
            this.btnStep1ScanImage.Size = new System.Drawing.Size(320, 34);
            this.btnStep1ScanImage.Text = "Step 1) Scan or Convert Images";
            // 
            // btnScanToTiff
            // 
            this.btnScanToTiff.Name = "btnScanToTiff";
            this.btnScanToTiff.Size = new System.Drawing.Size(501, 40);
            this.btnScanToTiff.Text = "Scan to TIFF image using TWAIN drivers";
            this.btnScanToTiff.Click += new System.EventHandler(this.btnScanToTiff_Click);
            // 
            // btnConvertPdfToTiff
            // 
            this.btnConvertPdfToTiff.Name = "btnConvertPdfToTiff";
            this.btnConvertPdfToTiff.Size = new System.Drawing.Size(501, 40);
            this.btnConvertPdfToTiff.Text = "Convert PDF to TIFF image";
            this.btnConvertPdfToTiff.Click += new System.EventHandler(this.btnConvertPdfToTiff_Click);
            // 
            // btnLoadTiff
            // 
            this.btnLoadTiff.Name = "btnLoadTiff";
            this.btnLoadTiff.Size = new System.Drawing.Size(501, 40);
            this.btnLoadTiff.Text = "Load an existing TIFF image file";
            this.btnLoadTiff.Click += new System.EventHandler(this.btnLoadTiff_Click);
            // 
            // btnLoadProject
            // 
            this.btnLoadProject.Name = "btnLoadProject";
            this.btnLoadProject.Size = new System.Drawing.Size(501, 40);
            this.btnLoadProject.Text = "Load an existing project";
            this.btnLoadProject.Click += new System.EventHandler(this.btnLoadProject_Click);
            // 
            // btnStep2ProcessDates
            // 
            this.btnStep2ProcessDates.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnBatchDateProcessing,
            this.individualPagedProcessingToolStripMenuItem,
            this.btnViewImageInExplorer});
            this.btnStep2ProcessDates.Name = "btnStep2ProcessDates";
            this.btnStep2ProcessDates.Size = new System.Drawing.Size(226, 34);
            this.btnStep2ProcessDates.Text = "Step 2) Process dates";
            // 
            // btnBatchDateProcessing
            // 
            this.btnBatchDateProcessing.Name = "btnBatchDateProcessing";
            this.btnBatchDateProcessing.Size = new System.Drawing.Size(490, 40);
            this.btnBatchDateProcessing.Text = "Batch processing";
            this.btnBatchDateProcessing.Click += new System.EventHandler(this.btnBatchDateProcessing_Click);
            // 
            // individualPagedProcessingToolStripMenuItem
            // 
            this.individualPagedProcessingToolStripMenuItem.Name = "individualPagedProcessingToolStripMenuItem";
            this.individualPagedProcessingToolStripMenuItem.Size = new System.Drawing.Size(490, 40);
            this.individualPagedProcessingToolStripMenuItem.Text = "Individual paged processing";
            // 
            // btnViewImageInExplorer
            // 
            this.btnViewImageInExplorer.Name = "btnViewImageInExplorer";
            this.btnViewImageInExplorer.Size = new System.Drawing.Size(490, 40);
            this.btnViewImageInExplorer.Text = "View the loaded TIFF image in Explorer";
            // 
            // btnStep3SplitToPdf
            // 
            this.btnStep3SplitToPdf.Name = "btnStep3SplitToPdf";
            this.btnStep3SplitToPdf.Size = new System.Drawing.Size(349, 34);
            this.btnStep3SplitToPdf.Text = "Step 3) Save to individual PDF files";
            this.btnStep3SplitToPdf.Click += new System.EventHandler(this.btnStep3SplitToPdf_Click);
            // 
            // btnTools
            // 
            this.btnTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnConfigureZoom,
            this.btnSetDateFormat,
            this.btnTestZoom});
            this.btnTools.Name = "btnTools";
            this.btnTools.Size = new System.Drawing.Size(78, 34);
            this.btnTools.Text = "Tools";
            // 
            // btnConfigureZoom
            // 
            this.btnConfigureZoom.Name = "btnConfigureZoom";
            this.btnConfigureZoom.Size = new System.Drawing.Size(315, 40);
            this.btnConfigureZoom.Text = "Configure Zoom";
            // 
            // btnSetDateFormat
            // 
            this.btnSetDateFormat.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.comboDateFormats});
            this.btnSetDateFormat.Name = "btnSetDateFormat";
            this.btnSetDateFormat.Size = new System.Drawing.Size(315, 40);
            this.btnSetDateFormat.Text = "Date Format";
            // 
            // comboDateFormats
            // 
            this.comboDateFormats.Name = "comboDateFormats";
            this.comboDateFormats.Size = new System.Drawing.Size(121, 38);
            this.comboDateFormats.Click += new System.EventHandler(this.comboDateFormats_Click);
            // 
            // pnlMainContainer
            // 
            this.pnlMainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMainContainer.Location = new System.Drawing.Point(0, 38);
            this.pnlMainContainer.Name = "pnlMainContainer";
            this.pnlMainContainer.Size = new System.Drawing.Size(2203, 1515);
            this.pnlMainContainer.TabIndex = 19;
            // 
            // btnTestZoom
            // 
            this.btnTestZoom.Name = "btnTestZoom";
            this.btnTestZoom.Size = new System.Drawing.Size(315, 40);
            this.btnTestZoom.Text = "Test Zoom";
            this.btnTestZoom.Click += new System.EventHandler(this.btnTestZoom_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2203, 1553);
            this.Controls.Add(this.pnlMainContainer);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog fileOpenImage;
        private System.Windows.Forms.SaveFileDialog fileScan;
        private System.Windows.Forms.FolderBrowserDialog folderMonthlyPath;
        private System.Windows.Forms.OpenFileDialog fileOpenPdf;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem btnStep1ScanImage;
        private System.Windows.Forms.ToolStripMenuItem btnScanToTiff;
        private System.Windows.Forms.ToolStripMenuItem btnConvertPdfToTiff;
        private System.Windows.Forms.ToolStripMenuItem btnStep2ProcessDates;
        private System.Windows.Forms.ToolStripMenuItem btnBatchDateProcessing;
        private System.Windows.Forms.ToolStripMenuItem individualPagedProcessingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem btnStep3SplitToPdf;
        private System.Windows.Forms.ToolStripMenuItem btnLoadTiff;
        private System.Windows.Forms.ToolStripMenuItem btnViewImageInExplorer;
        private System.Windows.Forms.ToolStripMenuItem btnLoadProject;
        private System.Windows.Forms.ToolStripMenuItem btnTools;
        private System.Windows.Forms.ToolStripMenuItem btnConfigureZoom;
        private System.Windows.Forms.ToolStripMenuItem btnSetDateFormat;
        private System.Windows.Forms.ToolStripComboBox comboDateFormats;
        private System.Windows.Forms.Panel pnlMainContainer;
        private System.Windows.Forms.ToolStripMenuItem btnTestZoom;
    }
}

