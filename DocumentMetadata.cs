using iText.Kernel.Pdf.Canvas.Parser.Util;
using iText.Layout;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Image = System.Drawing.Image;

namespace DocumentScanner
{
    public class DocumentMetadata
    {
        public string ImagePath { get; set; }

        public Image CreateImage() => Image.FromFile(ImagePath);

        public RangedList<PageDateStatus> PageDates = new RangedList<PageDateStatus>(PageDateStatus.Undated);
        public DateFormatter DateFormatter { get; set; } = new DateFormatter();
        public int PageSkipInterval { get; set; } = 1;

        public DocumentMetadata(string imagePath)
        {
            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException("File does not exist.", ImagePath);
            }

            ImagePath = imagePath;
        }
    }

    public class PageDateStatus : IEquatable<PageDateStatus>, IComparer<PageDateStatus>, IComparable<PageDateStatus>
    {
        public DateTime? Date { get; set; }
        private bool _trash = false;

        public bool IsTrash
        {
            get => _trash;
            set
            {
                _trash = value;
                if (value)
                {
                    Date = null;
                }
            }
        }

        public bool HasDate => Date.HasValue;

        public bool Equals(PageDateStatus other) =>
            this.IsTrash == other.IsTrash && this.Date == other.Date;

        public override int GetHashCode()
        {
            int hashCode = -342212554;
            hashCode = hashCode * -1521134295 + this.Date.GetHashCode();
            hashCode = hashCode * -1521134295 + this.IsTrash.GetHashCode();
            return hashCode;
        }

        public PageDateStatus()
        {
        }

        public PageDateStatus(DateTime? date, bool isTrash = false)
        {
            Date = date;
            IsTrash = isTrash;
        }

        public static PageDateStatus FromDate(DateTime? date) => new PageDateStatus(date);

        public static implicit operator PageDateStatus(DateTime? value) =>
            new PageDateStatus(value);

        public static implicit operator PageDateStatus(DateTime value) =>
            new PageDateStatus(value);

        public static PageDateStatus Trash => new PageDateStatus(null, true);

        public static PageDateStatus Undated => new PageDateStatus(null, false);

        public override string ToString() =>
            IsTrash
                ? "Unused"
                : Date.HasValue
                    ? Date.Value.ToString("d")
                    : "Undated";

        public int Compare(PageDateStatus x, PageDateStatus y)
        {
            if (x == null)
            {
                return y == null ? 0 : -1;
            }
            else if (y == null)
            {
                return 1;
            }

            // Trash appears after useful pages
            if (x.IsTrash && !y.IsTrash) return 1;
            if (!x.IsTrash && y.IsTrash) return -1;

            // Undated appears before dated
            int? dateComparison = x.Date?.CompareTo(y.Date);
            if (dateComparison.HasValue) return dateComparison.Value;
            // x.Date == null
            return y.Date.HasValue ? -1 : 0;
        }

        public int CompareTo(PageDateStatus other) => Compare(this, other);
    }

    public interface IDocumentSaver
    {
        Task SaveAsync();
    }

    public class SerializedMetadata : IDocumentSaver
    {
        public string ProjectPath { get; }
        public DocumentMetadata Data { get; }
        public DocumentMetadataSerializer Serializer { get; }

        public SerializedMetadata(string projectPath, DocumentMetadata data, DocumentMetadataSerializer serializer)
        {
            ProjectPath = projectPath;
            Data = data;
            Serializer = serializer;
        }

        public async Task SaveAsync()
        {
            await Task.FromResult(true);
            Serializer.Save(Data, ProjectPath);
        }
    }

    public class DocumentMetadataSerializer
    {
        public const string Extension = ".pagedata";
        public const string SourceExtension = ".tiff";

        public string ImageToProjectPath(string imagePath)
        {
            if (!imagePath.EndsWith(SourceExtension, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"The selected file must be a {SourceExtension} file.");
            }

            return $"{imagePath}{Extension}";
        }

        public string ProjectToImagePath(string projectPath)
        {
            if (!projectPath.EndsWith(Extension, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"The selected file must be a {Extension} file.");
            }
            return projectPath.Substring(0, projectPath.Length - Extension.Length);
        }

        public SerializedMetadata Create(string imgPath, string projectPath = null, bool overwriteFile = false)
        {
            if (!File.Exists(imgPath))
            {
                throw new FileNotFoundException("Image file doesn't exist.", imgPath);
            }

            var docData = new DocumentMetadata(imgPath);
            projectPath = string.IsNullOrEmpty(projectPath) ? ImageToProjectPath(docData.ImagePath) : projectPath;

            if (!overwriteFile && File.Exists(projectPath))
            {
                throw new InvalidOperationException("You cannot overwrite the project file.");
            }

            Save(docData, projectPath);
            return new SerializedMetadata(projectPath, docData, this);
        }

        public SerializedMetadata LoadOrCreate(string projectOrImagePath)
        {
            if (string.IsNullOrWhiteSpace(projectOrImagePath))
            {
                throw new ArgumentNullException(nameof(projectOrImagePath));
            }

            string projectPath =
                projectOrImagePath.EndsWith(SourceExtension, StringComparison.OrdinalIgnoreCase)
                ? ImageToProjectPath(projectOrImagePath)
                : projectOrImagePath;

            if (!projectPath.EndsWith(Extension, StringComparison.OrdinalIgnoreCase))
            {
                throw new FileNotFoundException($"The project file must have a '{Extension}' extension.");
            }

            return File.Exists(projectPath)
                ? Load(projectPath)
                : Create(ProjectToImagePath(projectPath), projectPath, true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="docData"></param>
        /// <param name="projectPath">
        ///     The path to save the serialized project to.
        ///     If none is specified, choose one based on the image path.
        /// </param>
        public SerializedMetadata Save(DocumentMetadata docData, string projectPath = null)
        {
            projectPath = string.IsNullOrEmpty(projectPath) ? ImageToProjectPath(docData.ImagePath) : projectPath;
            var serializedData = JsonConvert.SerializeObject(docData);
            File.WriteAllText(projectPath, serializedData);
            return new SerializedMetadata(projectPath, docData, this);
        }

        /// <summary>
        /// </summary>
        /// <param name="projectOrImagePath">The serialized project path or an image path with a corresponding project.</param>
        /// <returns></returns>
        public SerializedMetadata Load(string projectOrImagePath)
        {
            string projectPath = projectOrImagePath;
            // Try to load the project associated with a provided image file.
            if (projectOrImagePath.EndsWith(SourceExtension, StringComparison.OrdinalIgnoreCase))
            {
                projectPath = ImageToProjectPath(projectOrImagePath);
            }

            if (!File.Exists(projectPath))
            {
                throw new FileNotFoundException("Project file not found", projectPath);
            }

            var serializedData = File.ReadAllText(projectPath);
            var metadata = JsonConvert.DeserializeObject<DocumentMetadata>(serializedData);

            // The source image listed in the metadata file doesn't exist
            if (!File.Exists(metadata.ImagePath))
            {
                // If the image and project were both moved
                // and the old image doesn't exist anymore, use the new image.
                var implicitImage = ProjectToImagePath(projectPath);
                if (!File.Exists(implicitImage))
                {
                    // Neither the image listed in the serialized data
                    // nor any with the corresponding name and dir exist
                    throw new FileNotFoundException("Image file not found.", metadata.ImagePath);
                }

                // Save the project with the updated image path.
                metadata.ImagePath = implicitImage;
                Save(metadata, projectPath);
            }

            return new SerializedMetadata(projectPath, metadata, this);
        }
    }
}