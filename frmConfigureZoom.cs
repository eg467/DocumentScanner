using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocumentScanner
{
    public partial class frmConfigureZoom : Form
    {
        private Image _baseImage;
        private string _imagePath { get; set; }

        public PreviewImageCreator PreviewImageCreator { get; }

        public frmConfigureZoom(string imagePath) : this()
        {
            _imagePath = imagePath;
            _baseImage = Image.FromFile(_imagePath);

            PreviewImageCreator = new PreviewImageCreator();
        }

        public frmConfigureZoom()
        {
            InitializeComponent();

            PreventCroppingRectFlicker();
        }

        private void PreventCroppingRectFlicker()
        {
            this.pnlOverlay.GetType()
                .GetProperty(
                    "DoubleBuffered",
                    BindingFlags.Instance
                        | BindingFlags.NonPublic)
                .SetValue(this.pnlOverlay, true, null);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                _baseImage?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmConfigureZoom_Load(object sender, EventArgs e)
        {
            this.picSource.Image = _baseImage;
            this.picSource.Size = _baseImage.Size;

            this.pnlOverlay.Parent = this.picSource;
            RefreshZoomLevelUi();
            UpdatePageUi();
        }

        private Point? _zoomDragStart;
        private Point? _zoomDragEnd;

        private Rectangle FormRectangle(Point p1, Point p2)
        {
            var l = Math.Min(p1.X, p2.X);
            var r = Math.Max(p1.X, p2.X);
            var t = Math.Min(p1.Y, p2.Y);
            var b = Math.Max(p1.Y, p2.Y);
            return new Rectangle(l, t, r - l, b - t);
        }

        private void RefreshCroppingRectangle()
        {
            this.pnlOverlay.Invalidate();
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_zoomDragStart.HasValue) return;
            _zoomDragEnd = e.Location.Scale(1 / PreviewImageCreator.ScaleFactor);
            var boundingRect = new Rectangle(Point.Empty, this.picSource.Size);
            PreviewImageCreator.SourceRectangle =
                FormRectangle(_zoomDragStart.Value, _zoomDragEnd.Value)
                .BoundBy(boundingRect); ;

            RefreshCroppingRectangle();
        }

        private void panel2_MouseUp(object sender, MouseEventArgs e)
        {
            _zoomDragStart = null;
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            _zoomDragStart = e.Location.Scale(1 / PreviewImageCreator.ScaleFactor);
        }

        private void trackScalePercentage_Scroll(object sender, EventArgs e)
        {
            PreviewImageCreator.ScaleFactor = this.trackScalePercentage.Value / 100f;
            RefreshZoomLevelUi();
        }

        private void RefreshZoomLevelUi()
        {
            this.picSource.ClientSize =
                this.picSource.Image.Size.Scale(PreviewImageCreator.ScaleFactor);

            this.pnlOverlay.Location = this.picSource.ClientRectangle.Location;
            this.pnlOverlay.Size = this.picSource.ClientSize;

            this.lblSizeScale.Text = $"{Math.Round(PreviewImageCreator.ScaleFactor * 100, 0)}% Size";
            RefreshCroppingRectangle();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            PreviewImageCreator.ScaleFactor = 1f;
            PreviewImageCreator.SourceRectangle =
                new Rectangle(Point.Empty, this.picSource.Image.Size);

            RefreshZoomLevelUi();
        }

        private void pnlOverlay_Paint(object sender, PaintEventArgs e)
        {
            using (var brush = new SolidBrush(Color.Red))
            using (var pen = new Pen(brush, 3f))
            {
                e.Graphics.DrawRectangle(
                    pen,
                    PreviewImageCreator.SourceRectangle.Scale(
                        PreviewImageCreator.ScaleFactor));
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private int _pageIndex = 0;

        private int PageIndex
        {
            get => _pageIndex;
            set
            {
                _pageIndex = value;
                UpdatePageUi();
            }
        }

        private void UpdatePageUi()
        {
            this.lblPage.Text = $"Page: {_pageIndex}";
            this.picSource.Image.SelectActiveFrame(FrameDimension.Page, PageIndex);
            this.picSource.Invalidate();
            this.btnPrevPage.Enabled = _pageIndex > 0;
            this.btnNextPage.Enabled = _pageIndex < this.picSource.Image.GetFrameCount(FrameDimension.Page) - 1;
        }

        private void btnPrevPage_Click(object sender, EventArgs e)
        {
            PageIndex -= 1;
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            PageIndex += 1;
        }
    }
}