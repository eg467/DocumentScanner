namespace DocumentScanner.UserControls
{
    partial class PerPageProcessing
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
            this.pnlPageControls = new System.Windows.Forms.FlowLayoutPanel();
            this.btnFirstPage = new System.Windows.Forms.Button();
            this.btnPreviousPage = new System.Windows.Forms.Button();
            this.lblCurrentPage = new System.Windows.Forms.Label();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.btnAdvanceStmtDate = new System.Windows.Forms.Button();
            this.btnLastPage = new System.Windows.Forms.Button();
            this.lblSkipInterval = new System.Windows.Forms.Label();
            this.numSkipInterval = new System.Windows.Forms.NumericUpDown();
            this.pnlControls = new System.Windows.Forms.GroupBox();
            this.lblZoomTrackbar = new System.Windows.Forms.Label();
            this.trackZoom = new System.Windows.Forms.TrackBar();
            this.picPagePreview = new System.Windows.Forms.PictureBox();
            this.lblPreviewDate = new System.Windows.Forms.Label();
            this.picZoom = new System.Windows.Forms.PictureBox();
            this.calendarStatementDate = new System.Windows.Forms.MonthCalendar();
            this.cbHasDate = new System.Windows.Forms.CheckBox();
            this.pnlPageControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSkipInterval)).BeginInit();
            this.pnlControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPagePreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picZoom)).BeginInit();
            this.SuspendLayout();
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
            this.pnlPageControls.Location = new System.Drawing.Point(6, 28);
            this.pnlPageControls.Name = "pnlPageControls";
            this.pnlPageControls.Size = new System.Drawing.Size(1433, 86);
            this.pnlPageControls.TabIndex = 5;
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
            // pnlControls
            // 
            this.pnlControls.Controls.Add(this.lblZoomTrackbar);
            this.pnlControls.Controls.Add(this.trackZoom);
            this.pnlControls.Controls.Add(this.pnlPageControls);
            this.pnlControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlControls.Location = new System.Drawing.Point(0, 0);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(1693, 177);
            this.pnlControls.TabIndex = 6;
            this.pnlControls.TabStop = false;
            this.pnlControls.Text = "Controls";
            // 
            // lblZoomTrackbar
            // 
            this.lblZoomTrackbar.AutoSize = true;
            this.lblZoomTrackbar.Location = new System.Drawing.Point(6, 117);
            this.lblZoomTrackbar.Name = "lblZoomTrackbar";
            this.lblZoomTrackbar.Size = new System.Drawing.Size(120, 25);
            this.lblZoomTrackbar.TabIndex = 17;
            this.lblZoomTrackbar.Text = "Zoom Level:";
            // 
            // trackZoom
            // 
            this.trackZoom.LargeChange = 50;
            this.trackZoom.Location = new System.Drawing.Point(132, 117);
            this.trackZoom.Maximum = 500;
            this.trackZoom.Minimum = 100;
            this.trackZoom.Name = "trackZoom";
            this.trackZoom.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.trackZoom.Size = new System.Drawing.Size(436, 80);
            this.trackZoom.SmallChange = 25;
            this.trackZoom.TabIndex = 16;
            this.trackZoom.TickFrequency = 50;
            this.trackZoom.Value = 100;
            // 
            // picPagePreview
            // 
            this.picPagePreview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.picPagePreview.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picPagePreview.Location = new System.Drawing.Point(0, 183);
            this.picPagePreview.Name = "picPagePreview";
            this.picPagePreview.Size = new System.Drawing.Size(1005, 1226);
            this.picPagePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPagePreview.TabIndex = 7;
            this.picPagePreview.TabStop = false;
            // 
            // lblPreviewDate
            // 
            this.lblPreviewDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPreviewDate.Location = new System.Drawing.Point(1011, 183);
            this.lblPreviewDate.Name = "lblPreviewDate";
            this.lblPreviewDate.Size = new System.Drawing.Size(713, 76);
            this.lblPreviewDate.TabIndex = 13;
            this.lblPreviewDate.Tag = "";
            this.lblPreviewDate.Text = "Date";
            this.lblPreviewDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picZoom
            // 
            this.picZoom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.picZoom.Location = new System.Drawing.Point(1022, 327);
            this.picZoom.Name = "picZoom";
            this.picZoom.Size = new System.Drawing.Size(711, 383);
            this.picZoom.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picZoom.TabIndex = 15;
            this.picZoom.TabStop = false;
            // 
            // calendarStatementDate
            // 
            this.calendarStatementDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.calendarStatementDate.Location = new System.Drawing.Point(1017, 795);
            this.calendarStatementDate.MaxSelectionCount = 1;
            this.calendarStatementDate.Name = "calendarStatementDate";
            this.calendarStatementDate.TabIndex = 14;
            this.calendarStatementDate.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.calendarStatementDate_DateSelected);
            // 
            // cbHasDate
            // 
            this.cbHasDate.AutoSize = true;
            this.cbHasDate.Location = new System.Drawing.Point(1017, 737);
            this.cbHasDate.Name = "cbHasDate";
            this.cbHasDate.Size = new System.Drawing.Size(213, 29);
            this.cbHasDate.TabIndex = 16;
            this.cbHasDate.Text = "Document Has Date";
            this.cbHasDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbHasDate.UseVisualStyleBackColor = true;
            this.cbHasDate.CheckedChanged += new System.EventHandler(this.cbHasDate_CheckedChanged);
            // 
            // PerPageProcessing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbHasDate);
            this.Controls.Add(this.picZoom);
            this.Controls.Add(this.calendarStatementDate);
            this.Controls.Add(this.lblPreviewDate);
            this.Controls.Add(this.picPagePreview);
            this.Controls.Add(this.pnlControls);
            this.Name = "PerPageProcessing";
            this.Size = new System.Drawing.Size(1693, 1064);
            this.Load += new System.EventHandler(this.PerPageProcessing_Load);
            this.pnlPageControls.ResumeLayout(false);
            this.pnlPageControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSkipInterval)).EndInit();
            this.pnlControls.ResumeLayout(false);
            this.pnlControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPagePreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picZoom)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel pnlPageControls;
        private System.Windows.Forms.Button btnFirstPage;
        private System.Windows.Forms.Button btnPreviousPage;
        private System.Windows.Forms.Label lblCurrentPage;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.Button btnAdvanceStmtDate;
        private System.Windows.Forms.Button btnLastPage;
        private System.Windows.Forms.Label lblSkipInterval;
        private System.Windows.Forms.NumericUpDown numSkipInterval;
        private System.Windows.Forms.GroupBox pnlControls;
        private System.Windows.Forms.Label lblZoomTrackbar;
        private System.Windows.Forms.TrackBar trackZoom;
        private System.Windows.Forms.PictureBox picPagePreview;
        private System.Windows.Forms.Label lblPreviewDate;
        private System.Windows.Forms.PictureBox picZoom;
        private System.Windows.Forms.MonthCalendar calendarStatementDate;
        private System.Windows.Forms.CheckBox cbHasDate;
    }
}
