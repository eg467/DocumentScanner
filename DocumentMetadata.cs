﻿using iText.Kernel.Pdf.Canvas.Parser.Util;
using iText.Layout;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using Image = System.Drawing.Image;

namespace DocumentScanner
{
    public class DocumentMetadata
    {
        public string ImagePath { get; set; }

        public Image CreateImage() => Image.FromFile(ImagePath);

        public RangedList<DateTime?> PageDates = new RangedList<DateTime?>(DateTime.Now);
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

            var data = File.ReadAllText(projectPath);
            var docData = JsonConvert.DeserializeObject<DocumentMetadata>(data);

            if (!File.Exists(docData.ImagePath))
            {
                // If the image and project were both moved
                // and the old image doesn't exist anymore, use the new image.
                var implicitImage = ProjectToImagePath(projectPath);
                if (!File.Exists(implicitImage))
                {
                    // Neither the image listed in the serialized data
                    // nor any with the corresponding name and dir exist
                    throw new FileNotFoundException("Image file not found.", docData.ImagePath);
                }

                // Save the project with the updated image path.
                docData.ImagePath = implicitImage;
                Save(docData, projectPath);
            }

            return new SerializedMetadata(projectPath, docData, this);
        }
    }
}