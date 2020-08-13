namespace DocumentScanner.UserControls
{
    partial class DateIncrementSelector
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
            this.numIntervalAmount = new System.Windows.Forms.NumericUpDown();
            this.comboIntervalType = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.numIntervalAmount)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // numIntervalAmount
            // 
            this.numIntervalAmount.Location = new System.Drawing.Point(3, 3);
            this.numIntervalAmount.Name = "numIntervalAmount";
            this.numIntervalAmount.Size = new System.Drawing.Size(98, 29);
            this.numIntervalAmount.TabIndex = 0;
            this.numIntervalAmount.ValueChanged += new System.EventHandler(this.numIntervalAmount_ValueChanged);
            // 
            // comboIntervalType
            // 
            this.comboIntervalType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboIntervalType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboIntervalType.FormattingEnabled = true;
            this.comboIntervalType.Location = new System.Drawing.Point(107, 3);
            this.comboIntervalType.Name = "comboIntervalType";
            this.comboIntervalType.Size = new System.Drawing.Size(190, 32);
            this.comboIntervalType.TabIndex = 1;
            this.comboIntervalType.SelectedIndexChanged += new System.EventHandler(this.comboIntervalType_SelectedIndexChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.numIntervalAmount);
            this.flowLayoutPanel1.Controls.Add(this.comboIntervalType);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(300, 38);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // DateIncrementSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "DateIncrementSelector";
            this.Size = new System.Drawing.Size(303, 41);
            ((System.ComponentModel.ISupportInitialize)(this.numIntervalAmount)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numIntervalAmount;
        private System.Windows.Forms.ComboBox comboIntervalType;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}
