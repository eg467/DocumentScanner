namespace DocumentScanner
{
    partial class frmConfigureZoom
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;



        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.picSource = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblPage = new System.Windows.Forms.Label();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.btnPrevPage = new System.Windows.Forms.Button();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.lblSizeScale = new System.Windows.Forms.Label();
            this.trackScalePercentage = new System.Windows.Forms.TrackBar();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlOverlay = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picSource)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackScalePercentage)).BeginInit();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // picSource
            // 
            this.picSource.BackColor = System.Drawing.Color.White;
            this.picSource.Location = new System.Drawing.Point(0, 0);
            this.picSource.Name = "picSource";
            this.picSource.Size = new System.Drawing.Size(528, 348);
            this.picSource.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSource.TabIndex = 0;
            this.picSource.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lblPage);
            this.groupBox1.Controls.Add(this.btnNextPage);
            this.groupBox1.Controls.Add(this.btnPrevPage);
            this.groupBox1.Controls.Add(this.btnSubmit);
            this.groupBox1.Controls.Add(this.btnReset);
            this.groupBox1.Controls.Add(this.lblSizeScale);
            this.groupBox1.Controls.Add(this.trackScalePercentage);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(3113, 144);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Controls";
            // 
            // lblPage
            // 
            this.lblPage.AutoSize = true;
            this.lblPage.Location = new System.Drawing.Point(86, 42);
            this.lblPage.Name = "lblPage";
            this.lblPage.Size = new System.Drawing.Size(58, 25);
            this.lblPage.TabIndex = 7;
            this.lblPage.Text = "Page";
            // 
            // btnNextPage
            // 
            this.btnNextPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.85714F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNextPage.Location = new System.Drawing.Point(46, 29);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(34, 53);
            this.btnNextPage.TabIndex = 6;
            this.btnNextPage.Text = "⏵︎";
            this.btnNextPage.UseVisualStyleBackColor = true;
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // btnPrevPage
            // 
            this.btnPrevPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.85714F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrevPage.Location = new System.Drawing.Point(9, 29);
            this.btnPrevPage.Margin = new System.Windows.Forms.Padding(0);
            this.btnPrevPage.Name = "btnPrevPage";
            this.btnPrevPage.Size = new System.Drawing.Size(34, 53);
            this.btnPrevPage.TabIndex = 4;
            this.btnPrevPage.Text = "⏴︎";
            this.btnPrevPage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPrevPage.UseVisualStyleBackColor = true;
            this.btnPrevPage.Click += new System.EventHandler(this.btnPrevPage_Click);
            // 
            // btnSubmit
            // 
            this.btnSubmit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSubmit.Location = new System.Drawing.Point(812, 22);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(121, 58);
            this.btnSubmit.TabIndex = 3;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(705, 22);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(101, 56);
            this.btnReset.TabIndex = 2;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // lblSizeScale
            // 
            this.lblSizeScale.Location = new System.Drawing.Point(261, 57);
            this.lblSizeScale.Name = "lblSizeScale";
            this.lblSizeScale.Size = new System.Drawing.Size(425, 37);
            this.lblSizeScale.TabIndex = 1;
            this.lblSizeScale.Text = "Scale";
            this.lblSizeScale.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackScalePercentage
            // 
            this.trackScalePercentage.Location = new System.Drawing.Point(261, 14);
            this.trackScalePercentage.Maximum = 500;
            this.trackScalePercentage.Minimum = 25;
            this.trackScalePercentage.Name = "trackScalePercentage";
            this.trackScalePercentage.Size = new System.Drawing.Size(425, 80);
            this.trackScalePercentage.TabIndex = 0;
            this.trackScalePercentage.Value = 100;
            this.trackScalePercentage.Scroll += new System.EventHandler(this.trackScalePercentage_Scroll);
            // 
            // pnlMain
            // 
            this.pnlMain.AutoScroll = true;
            this.pnlMain.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pnlMain.Controls.Add(this.pnlOverlay);
            this.pnlMain.Controls.Add(this.picSource);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 144);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(3113, 1357);
            this.pnlMain.TabIndex = 2;
            // 
            // pnlOverlay
            // 
            this.pnlOverlay.BackColor = System.Drawing.Color.Transparent;
            this.pnlOverlay.Location = new System.Drawing.Point(577, 25);
            this.pnlOverlay.Name = "pnlOverlay";
            this.pnlOverlay.Size = new System.Drawing.Size(439, 331);
            this.pnlOverlay.TabIndex = 1;
            this.pnlOverlay.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlOverlay_Paint);
            this.pnlOverlay.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel2_MouseDown);
            this.pnlOverlay.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel2_MouseMove);
            this.pnlOverlay.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel2_MouseUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 97);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1043, 32);
            this.label1.TabIndex = 8;
            this.label1.Text = "Select a preview area by dragging your cursor across a region of the image below." +
    "";
            // 
            // frmConfigureZoom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(3113, 1501);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmConfigureZoom";
            this.Text = "Choose Your Image Preview Area";
            this.Load += new System.EventHandler(this.frmConfigureZoom_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picSource)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackScalePercentage)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picSource;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlOverlay;
        private System.Windows.Forms.TrackBar trackScalePercentage;
        private System.Windows.Forms.Label lblSizeScale;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.Button btnPrevPage;
        private System.Windows.Forms.Label lblPage;
        private System.Windows.Forms.Label label1;
    }
}