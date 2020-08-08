using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;

namespace DocumentScanner
{
    internal class ImageZoomer
    {
        public int MinZoomLevel => 50;
        public int MaxZoomLevel => 500;
        private int _level = 100;
        public Size DestSize { get; set; } = new Size(100, 100);

        public ImageZoomer(Image sourceImage)
        {
            this.SourceImage = sourceImage;
        }

        public int Level
        {
            get => _level;
            set
            {
                if (value < MinZoomLevel || value > MaxZoomLevel)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(Level),
                        value,
                        $"Zoom level must be between {MinZoomLevel} and {MaxZoomLevel}.");
                }
                _level = value;
            }
        }

        public Point Location { get; set; } = Point.Empty;
        public Image SourceImage { get; set; }

        public ImageZoomer Clone(Image newImage = null)
        {
            return new ImageZoomer(newImage ?? SourceImage)
            {
                Location = Location,
                Level = Level,
                DestSize = DestSize,
            };
        }

        /// <summary>
        /// http://csharphelper.com/blog/2015/06/zoom-and-crop-a-picture-in-c/
        /// </summary>
        public Image Zoom(int? page = null)
        {
            if (page.HasValue)
            {
                SourceImage.SelectActiveFrame(FrameDimension.Page, page.Value);
            }

            var zoomFactor = Level / 100.0;
            var aspectRatio = (double)DestSize.Width / DestSize.Height;
            var origSize = SourceImage.Size;
            var srcWidth = (int)((1 / zoomFactor) * DestSize.Width);
            var srcSize = new Size(srcWidth, (int)(srcWidth / aspectRatio));

            var zoomCenter = new Point(
                (int)(origSize.Width * Location.X / 100.0),
                (int)(origSize.Height * Location.Y / 100.0));

            var srcArea = new Rectangle(
                new Point(
                    Math.Max(0, (int)(zoomCenter.X - (srcSize.Width / 2.0))),
                    Math.Max(0, (int)(zoomCenter.Y - (srcSize.Height / 2.0)))),
                srcSize);

            if (srcArea.Right > origSize.Width)
            {
                srcArea.Location = new Point(origSize.Width - srcArea.Width, srcArea.Y);
            }

            if (srcArea.Bottom > origSize.Height)
            {
                srcArea.Location = new Point(srcArea.X, origSize.Height - srcArea.Height);
            }

            var scaledImg = new Bitmap(DestSize.Width, DestSize.Height);
            using (Graphics gr = Graphics.FromImage(scaledImg))
            {
                var destArea = new Rectangle(Point.Empty, DestSize);
                gr.PixelOffsetMode = PixelOffsetMode.Half;
                gr.InterpolationMode = InterpolationMode.NearestNeighbor;
                gr.DrawImage(
                    SourceImage,
                    destArea,
                    srcArea,
                    GraphicsUnit.Pixel);
            }
            return scaledImg;
        }
    }
}