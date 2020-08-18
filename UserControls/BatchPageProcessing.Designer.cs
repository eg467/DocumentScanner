namespace DocumentScanner.UserControls
{
    partial class BatchPageProcessing
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
            this.tableContainer = new System.Windows.Forms.TableLayoutPanel();
            this.pnlOptions = new System.Windows.Forms.GroupBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblSkipIntervalRight = new System.Windows.Forms.Label();
            this.numSkipInterval = new System.Windows.Forms.NumericUpDown();
            this.lblSkipIntervalLeft = new System.Windows.Forms.Label();
            this.btnResetDates = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.pnlIncrementMode = new System.Windows.Forms.GroupBox();
            this.incselManual = new DocumentScanner.UserControls.DateIncrementSelector();
            this.lblManuallyIncrementBy = new System.Windows.Forms.Label();
            this.rbIncrementImplicitlySetPages = new System.Windows.Forms.RadioButton();
            this.rbIncrementAllSubsequentPages = new System.Windows.Forms.RadioButton();
            this.pnlAutoIncrement = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.incselAutoIncrement = new DocumentScanner.UserControls.DateIncrementSelector();
            this.label2 = new System.Windows.Forms.Label();
            this.dateAutoIncrementStart = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.numAutoIncrementPageStart = new System.Windows.Forms.NumericUpDown();
            this.btnAutoIncrement = new System.Windows.Forms.Button();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.btnRepaint = new System.Windows.Forms.Button();
            this.pnlOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSkipInterval)).BeginInit();
            this.pnlIncrementMode.SuspendLayout();
            this.pnlAutoIncrement.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAutoIncrementPageStart)).BeginInit();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableContainer
            // 
            this.tableContainer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableContainer.AutoSize = true;
            this.tableContainer.BackColor = System.Drawing.Color.Transparent;
            this.tableContainer.ColumnCount = 3;
            this.tableContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableContainer.Location = new System.Drawing.Point(0, 0);
            this.tableContainer.Name = "tableContainer";
            this.tableContainer.RowCount = 1;
            this.tableContainer.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableContainer.Size = new System.Drawing.Size(1604, 43);
            this.tableContainer.TabIndex = 0;
            // 
            // pnlOptions
            // 
            this.pnlOptions.Controls.Add(this.btnRepaint);
            this.pnlOptions.Controls.Add(this.btnRefresh);
            this.pnlOptions.Controls.Add(this.lblSkipIntervalRight);
            this.pnlOptions.Controls.Add(this.numSkipInterval);
            this.pnlOptions.Controls.Add(this.lblSkipIntervalLeft);
            this.pnlOptions.Controls.Add(this.btnResetDates);
            this.pnlOptions.Controls.Add(this.lblStatus);
            this.pnlOptions.Controls.Add(this.pnlIncrementMode);
            this.pnlOptions.Controls.Add(this.pnlAutoIncrement);
            this.pnlOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlOptions.Location = new System.Drawing.Point(0, 0);
            this.pnlOptions.Name = "pnlOptions";
            this.pnlOptions.Size = new System.Drawing.Size(1636, 299);
            this.pnlOptions.TabIndex = 2;
            this.pnlOptions.TabStop = false;
            this.pnlOptions.Text = "Options and Tools";
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.14286F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(897, 148);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(314, 55);
            this.btnRefresh.TabIndex = 15;
            this.btnRefresh.Text = "Refresh Data";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lblSkipIntervalRight
            // 
            this.lblSkipIntervalRight.AutoSize = true;
            this.lblSkipIntervalRight.Location = new System.Drawing.Point(908, 221);
            this.lblSkipIntervalRight.Name = "lblSkipIntervalRight";
            this.lblSkipIntervalRight.Size = new System.Drawing.Size(66, 25);
            this.lblSkipIntervalRight.TabIndex = 14;
            this.lblSkipIntervalRight.Text = "pages";
            // 
            // numSkipInterval
            // 
            this.numSkipInterval.Location = new System.Drawing.Point(825, 222);
            this.numSkipInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSkipInterval.Name = "numSkipInterval";
            this.numSkipInterval.Size = new System.Drawing.Size(77, 29);
            this.numSkipInterval.TabIndex = 13;
            this.numSkipInterval.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblSkipIntervalLeft
            // 
            this.lblSkipIntervalLeft.AutoSize = true;
            this.lblSkipIntervalLeft.Location = new System.Drawing.Point(710, 221);
            this.lblSkipIntervalLeft.Name = "lblSkipIntervalLeft";
            this.lblSkipIntervalLeft.Size = new System.Drawing.Size(108, 25);
            this.lblSkipIntervalLeft.TabIndex = 12;
            this.lblSkipIntervalLeft.Text = "View every";
            // 
            // btnResetDates
            // 
            this.btnResetDates.Location = new System.Drawing.Point(710, 148);
            this.btnResetDates.Name = "btnResetDates";
            this.btnResetDates.Size = new System.Drawing.Size(162, 55);
            this.btnResetDates.TabIndex = 11;
            this.btnResetDates.Text = "Reset Dates";
            this.btnResetDates.UseVisualStyleBackColor = true;
            this.btnResetDates.Click += new System.EventHandler(this.btnResetDates_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.BackColor = System.Drawing.Color.Green;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.85714F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.White;
            this.lblStatus.Location = new System.Drawing.Point(1007, 208);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Padding = new System.Windows.Forms.Padding(10);
            this.lblStatus.Size = new System.Drawing.Size(153, 62);
            this.lblStatus.TabIndex = 10;
            this.lblStatus.Text = "Saving";
            this.lblStatus.Visible = false;
            // 
            // pnlIncrementMode
            // 
            this.pnlIncrementMode.Controls.Add(this.incselManual);
            this.pnlIncrementMode.Controls.Add(this.lblManuallyIncrementBy);
            this.pnlIncrementMode.Controls.Add(this.rbIncrementImplicitlySetPages);
            this.pnlIncrementMode.Controls.Add(this.rbIncrementAllSubsequentPages);
            this.pnlIncrementMode.Location = new System.Drawing.Point(24, 137);
            this.pnlIncrementMode.Name = "pnlIncrementMode";
            this.pnlIncrementMode.Size = new System.Drawing.Size(665, 156);
            this.pnlIncrementMode.TabIndex = 9;
            this.pnlIncrementMode.TabStop = false;
            this.pnlIncrementMode.Text = "Manual Incremeting";
            // 
            // incselManual
            // 
            this.incselManual.AutoSize = true;
            this.incselManual.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.incselManual.Location = new System.Drawing.Point(139, 111);
            this.incselManual.Name = "incselManual";
            this.incselManual.Size = new System.Drawing.Size(303, 41);
            this.incselManual.TabIndex = 9;
            // 
            // lblManuallyIncrementBy
            // 
            this.lblManuallyIncrementBy.AutoSize = true;
            this.lblManuallyIncrementBy.Location = new System.Drawing.Point(1, 111);
            this.lblManuallyIncrementBy.Name = "lblManuallyIncrementBy";
            this.lblManuallyIncrementBy.Size = new System.Drawing.Size(132, 25);
            this.lblManuallyIncrementBy.TabIndex = 8;
            this.lblManuallyIncrementBy.Text = "Increment By:";
            // 
            // rbIncrementImplicitlySetPages
            // 
            this.rbIncrementImplicitlySetPages.AutoSize = true;
            this.rbIncrementImplicitlySetPages.Checked = true;
            this.rbIncrementImplicitlySetPages.Location = new System.Drawing.Point(6, 28);
            this.rbIncrementImplicitlySetPages.Name = "rbIncrementImplicitlySetPages";
            this.rbIncrementImplicitlySetPages.Size = new System.Drawing.Size(601, 29);
            this.rbIncrementImplicitlySetPages.TabIndex = 6;
            this.rbIncrementImplicitlySetPages.TabStop = true;
            this.rbIncrementImplicitlySetPages.Text = "Increment only subsequent pages until the next explicitly set date.";
            this.rbIncrementImplicitlySetPages.UseVisualStyleBackColor = true;
            // 
            // rbIncrementAllSubsequentPages
            // 
            this.rbIncrementAllSubsequentPages.AutoSize = true;
            this.rbIncrementAllSubsequentPages.Location = new System.Drawing.Point(6, 63);
            this.rbIncrementAllSubsequentPages.Name = "rbIncrementAllSubsequentPages";
            this.rbIncrementAllSubsequentPages.Size = new System.Drawing.Size(335, 29);
            this.rbIncrementAllSubsequentPages.TabIndex = 7;
            this.rbIncrementAllSubsequentPages.Text = "Increment ALL subsequent pages.";
            this.rbIncrementAllSubsequentPages.UseVisualStyleBackColor = true;
            // 
            // pnlAutoIncrement
            // 
            this.pnlAutoIncrement.Controls.Add(this.flowLayoutPanel1);
            this.pnlAutoIncrement.Location = new System.Drawing.Point(24, 28);
            this.pnlAutoIncrement.Name = "pnlAutoIncrement";
            this.pnlAutoIncrement.Size = new System.Drawing.Size(1580, 103);
            this.pnlAutoIncrement.TabIndex = 8;
            this.pnlAutoIncrement.TabStop = false;
            this.pnlAutoIncrement.Text = "AutoIncrement Pages";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.incselAutoIncrement);
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Controls.Add(this.dateAutoIncrementStart);
            this.flowLayoutPanel1.Controls.Add(this.label3);
            this.flowLayoutPanel1.Controls.Add(this.numAutoIncrementPageStart);
            this.flowLayoutPanel1.Controls.Add(this.btnAutoIncrement);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(6, 25);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1555, 49);
            this.flowLayoutPanel1.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 37);
            this.label1.TabIndex = 6;
            this.label1.Text = "Increment by";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // incselAutoIncrement
            // 
            this.incselAutoIncrement.AutoSize = true;
            this.incselAutoIncrement.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.incselAutoIncrement.Location = new System.Drawing.Point(133, 3);
            this.incselAutoIncrement.Name = "incselAutoIncrement";
            this.incselAutoIncrement.Size = new System.Drawing.Size(303, 41);
            this.incselAutoIncrement.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(442, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 37);
            this.label2.TabIndex = 7;
            this.label2.Text = "starting at";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dateAutoIncrementStart
            // 
            this.dateAutoIncrementStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateAutoIncrementStart.Location = new System.Drawing.Point(557, 3);
            this.dateAutoIncrementStart.Name = "dateAutoIncrementStart";
            this.dateAutoIncrementStart.ShowCheckBox = true;
            this.dateAutoIncrementStart.Size = new System.Drawing.Size(160, 29);
            this.dateAutoIncrementStart.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(723, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 37);
            this.label3.TabIndex = 8;
            this.label3.Text = "on page";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numAutoIncrementPageStart
            // 
            this.numAutoIncrementPageStart.Location = new System.Drawing.Point(821, 3);
            this.numAutoIncrementPageStart.Maximum = new decimal(new int[] {
            1410065407,
            2,
            0,
            0});
            this.numAutoIncrementPageStart.Name = "numAutoIncrementPageStart";
            this.numAutoIncrementPageStart.Size = new System.Drawing.Size(120, 29);
            this.numAutoIncrementPageStart.TabIndex = 9;
            // 
            // btnAutoIncrement
            // 
            this.btnAutoIncrement.Location = new System.Drawing.Point(947, 3);
            this.btnAutoIncrement.Name = "btnAutoIncrement";
            this.btnAutoIncrement.Size = new System.Drawing.Size(243, 34);
            this.btnAutoIncrement.TabIndex = 4;
            this.btnAutoIncrement.Text = "Auto Increment";
            this.btnAutoIncrement.UseVisualStyleBackColor = true;
            this.btnAutoIncrement.Click += new System.EventHandler(this.btnAutoIncrement_Click);
            // 
            // pnlMain
            // 
            this.pnlMain.AutoScroll = true;
            this.pnlMain.BackColor = System.Drawing.Color.DimGray;
            this.pnlMain.Controls.Add(this.tableContainer);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 299);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1636, 1056);
            this.pnlMain.TabIndex = 3;
            // 
            // btnRepaint
            // 
            this.btnRepaint.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnRepaint.Location = new System.Drawing.Point(1350, 165);
            this.btnRepaint.Name = "btnRepaint";
            this.btnRepaint.Size = new System.Drawing.Size(204, 44);
            this.btnRepaint.TabIndex = 16;
            this.btnRepaint.Text = "Repaint";
            this.btnRepaint.UseVisualStyleBackColor = false;
            // 
            // BatchPageProcessing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlOptions);
            this.Name = "BatchPageProcessing";
            this.Size = new System.Drawing.Size(1636, 1355);
            this.pnlOptions.ResumeLayout(false);
            this.pnlOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSkipInterval)).EndInit();
            this.pnlIncrementMode.ResumeLayout(false);
            this.pnlIncrementMode.PerformLayout();
            this.pnlAutoIncrement.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAutoIncrementPageStart)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.TableLayoutPanel tableContainer;
        #endregion

        private System.Windows.Forms.GroupBox pnlOptions;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.DateTimePicker dateAutoIncrementStart;
        private System.Windows.Forms.Button btnAutoIncrement;
        private System.Windows.Forms.GroupBox pnlIncrementMode;
        private System.Windows.Forms.RadioButton rbIncrementImplicitlySetPages;
        private System.Windows.Forms.RadioButton rbIncrementAllSubsequentPages;
        private System.Windows.Forms.GroupBox pnlAutoIncrement;
        private DateIncrementSelector incselAutoIncrement;
        private DateIncrementSelector incselManual;
        private System.Windows.Forms.Label lblManuallyIncrementBy;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnResetDates;
        private System.Windows.Forms.Label lblSkipIntervalRight;
        private System.Windows.Forms.NumericUpDown numSkipInterval;
        private System.Windows.Forms.Label lblSkipIntervalLeft;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numAutoIncrementPageStart;
        private System.Windows.Forms.Button btnRepaint;
    }
}
