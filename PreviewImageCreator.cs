using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;

namespace DocumentScanner
{
    /// <summary>
    /// Scales and crops to a region of the image to make
    /// certain info (e.g. statement dates) easier to find.
    /// </summary>
    public class PreviewImageCreator
    {
        public float ScaleFactor { get; set; } = 1f;

        public Rectangle SourceRectangle { get; set; }

        public PreviewImageCreator Clone()
        {
            return new PreviewImageCreator()
            {
                ScaleFactor = ScaleFactor,
                SourceRectangle = SourceRectangle,
            };
        }

        /// <summary>
        /// http://csharphelper.com/blog/2015/06/zoom-and-crop-a-picture-in-c/
        /// </summary>
        public Image Zoom(Image source, int? page = null)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (page.HasValue)
            {
                try
                {
                    var maxPage = source.GetFrameCount(FrameDimension.Page) - 1;
                    Debug.WriteLine($"Creating preview for page idx={page.Value}={maxPage}");
                    source.SelectActiveFrame(FrameDimension.Page, page.Value);
                }
                catch (ArgumentException ex)
                {
                    // Image was probably disposed during closing
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }

            // Use the full source image if no source rectangle is selected.
            if (SourceRectangle.Size == Size.Empty)
            {
                var boundingBox = new Rectangle(Point.Empty, source.Size);

                SourceRectangle = new Rectangle(
                    SourceRectangle.Location,
                    source.Size.Scale(ScaleFactor))
                .BoundBy(boundingBox);
            }

            var finalSize = SourceRectangle.Size.Scale(ScaleFactor);
            var scaledImg = new Bitmap(finalSize.Width, finalSize.Height);
            using (Graphics gr = Graphics.FromImage(scaledImg))
            {
                var destArea = new Rectangle(Point.Empty, finalSize);
                gr.PixelOffsetMode = PixelOffsetMode.Half;
                gr.InterpolationMode = InterpolationMode.NearestNeighbor;
                gr.DrawImage(
                    source,
                    destArea,
                    SourceRectangle,
                    GraphicsUnit.Pixel);
            }
            return scaledImg;
        }
    }

    public static class CoordinateExtensions
    {
        public static Point Scale(this Point pt, float factor) =>
            new Point(pt.X.Scale(factor), pt.Y.Scale(factor));

        public static Size Scale(this Size size, float factor) =>
            new Size(size.Width.Scale(factor), size.Height.Scale(factor));

        public static int Scale(this int value, float factor) =>
            (int)Math.Round(factor * value, 0);

        public static Rectangle Scale(this Rectangle rect, float factor) =>
            new Rectangle(rect.Location.Scale(factor), rect.Size.Scale(factor));

        public static Rectangle BoundBy(this Rectangle rect, Rectangle bounds)
        {
            var l = Math.Max(rect.Left, bounds.Left);
            var r = Math.Min(rect.Right, bounds.Right);
            var t = Math.Max(rect.Top, bounds.Top);
            var b = Math.Min(rect.Bottom, bounds.Bottom);
            return new Rectangle(l, t, r - l, b - t);
        }
    }
}