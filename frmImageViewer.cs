using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocumentScanner
{
    public partial class frmImageViewer : Form
    {
        public string ImagePath { get; set; }
        public int? TiffPage { get; set; }

        public frmImageViewer()
        {
            InitializeComponent();
        }

        public frmImageViewer(string imagePath, int? tiffPage) : this()
        {
            this.ImagePath = imagePath;
            this.TiffPage = tiffPage;
        }

        private void frmImageViewer_Load(object sender, EventArgs e)
        {
            if (!File.Exists(this.ImagePath))
                return;

            this.picImage.Image = Image.FromFile(this.ImagePath);
            var numPages = this.picImage.Image.GetFrameCount(FrameDimension.Page);
            if (TiffPage.HasValue && TiffPage.Value < numPages)
                this.picImage.Image.SelectActiveFrame(FrameDimension.Page, TiffPage.Value);
            this.picImage.Size = this.picImage.Image.Size;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            this.picImage.Image?.Dispose();
        }
    }
}