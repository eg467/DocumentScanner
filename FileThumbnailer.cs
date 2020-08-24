using DocumentScanner.UserControls;
using ImageMagick;
using iText.IO;
using iText.Kernel.Pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;

namespace DocumentScanner
{
    internal abstract class FileThumbnailer : IDisposable
    {
        public string Path { get; }
        protected bool WasDisposed;
        public Size Size { get; set; } = new Size(200, 260);

        private IReadOnlyList<Image> _images = Array.Empty<Image>();

        public IReadOnlyList<Image> Images
        {
            get => _images;
            protected set
            {
                value = value ?? Array.Empty<Image>();
                if (_images == value) return;
                DisposeImages();
                _images = value;
            }
        }

        public static FileThumbnailer FromFile(string path, Size size)
        {
            switch (System.IO.Path.GetExtension(path).ToUpper(CultureInfo.InvariantCulture))
            {
                case ".PDF":
                    return new PdfThumbnailer(path, size);

                case ".TIFF":
                    return new TiffThumbnailer(path, size);

                default:
                    throw new ArgumentException("The specified path is not supported.");
            }
        }

        protected FileThumbnailer(string path, Size size)
        {
            Path = path;
            Size = size;
        }

        public abstract void LoadImages();

        private void DisposeImages()
        {
            _images.ForEach(i => i.Dispose());
            _images = Array.Empty<Image>();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!WasDisposed)
            {
                if (disposing)
                {
                    DisposeImages();
                }
                WasDisposed = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    internal class TiffThumbnailer : FileThumbnailer
    {
        private readonly Image _image;

        public TiffThumbnailer(string path, Size size) : base(path, size)
        {
            using (var fullsize = Image.FromFile(path))
            {
                size = size.IsEmpty ? _image.Size : size;
                _image = fullsize.GetThumbnailImage(size.Width, size.Height, null, IntPtr.Zero);
            }
        }

        public override void LoadImages()
        {
            Images = Enumerable.Range(0, Images.Count)
                .Select(p => (Image)_image.Clone())
                .ToArray();
            Images.ForEach((img, p) => img.SelectActiveFrame(FrameDimension.Page, p));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !WasDisposed)
            {
                _image.Dispose();
            }

            base.Dispose(disposing);
        }
    }

    internal class PdfThumbnailer : FileThumbnailer
    {
        static PdfThumbnailer()
        {
            MagickNET.SetGhostscriptDirectory(Environment.CurrentDirectory);
        }

        public PdfThumbnailer(string path, Size size) : base(path, size)
        {
            var outputDir = System.IO.Path.GetDirectoryName(Path);
            var tmpPath = System.IO.Path.Combine(outputDir, "magick-tmp");
            Directory.CreateDirectory(tmpPath);
            MagickNET.SetTempDirectory(tmpPath);
        }

        /// <summary>
        /// Required because changes to Ghostscript prevent reading number of pages from GhostScript.NET
        /// <a href="https://stackoverflow.com/questions/57709390/ghostscriptrasterizer-pagecount-always-returns-zero">SO Link</a>
        /// </summary>
        /// <returns>The number of pages in the PDF</returns>
        private int GetPageCount()
        {
            using (var reader = new PdfReader(Path))
            {
                var doc = new PdfDocument(reader);
                int numPages = doc.GetNumberOfPages();
                doc.Close();
                return numPages;
            }
        }

        public override void LoadImages()
        {
            var images = new List<Image>();

            var settings = new MagickReadSettings
            {
                Density = new Density(96, 96)
            };

            using (var mImages = new MagickImageCollection())
            {
                mImages.Read(Path, settings);

                var page = 1;
                foreach (MagickImage image in mImages)
                {
                    image.Thumbnail(Size.Width, Size.Height);
                    using (var stream = new MemoryStream())
                    {
                        image.Write(stream, MagickFormat.Jpg);
                        var thumb = Image.FromStream(stream);
                        images.Add(thumb);
                    }
                    page++;
                }
            }

            Images = images;
        }
    }
}