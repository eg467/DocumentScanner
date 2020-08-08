namespace DocumentScanner
{
    partial class frmVerification
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
            this.tableContainer = new System.Windows.Forms.TableLayoutPanel();
            this.lblLoading = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tableContainer
            // 
            this.tableContainer.AutoScroll = true;
            this.tableContainer.AutoSize = true;
            this.tableContainer.ColumnCount = 3;
            this.tableContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableContainer.Location = new System.Drawing.Point(0, 0);
            this.tableContainer.Name = "tableContainer";
            this.tableContainer.RowCount = 1;
            this.tableContainer.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableContainer.Size = new System.Drawing.Size(1128, 38);
            this.tableContainer.TabIndex = 0;
            // 
            // lblLoading
            // 
            this.lblLoading.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLoading.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.85714F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoading.Location = new System.Drawing.Point(0, 0);
            this.lblLoading.Name = "lblLoading";
            this.lblLoading.Size = new System.Drawing.Size(1636, 111);
            this.lblLoading.TabIndex = 1;
            this.lblLoading.Text = "LOADING THUMBNAILS AND DATA.";
            this.lblLoading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmVerification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1636, 1355);
            this.Controls.Add(this.lblLoading);
            this.Controls.Add(this.tableContainer);
            this.Name = "frmVerification";
            this.Text = "Date Verification";
            this.Load += new System.EventHandler(this.frmVerification_Load);
            this.Shown += new System.EventHandler(this.frmVerification_Shown);
            this.Scroll += new System.Windows.Forms.ScrollEventHandler(this.frmVerification_Scroll);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableContainer;
        private System.Windows.Forms.Label lblLoading;
    }
}