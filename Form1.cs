using DocumentScanner.NapsOptions;
using DocumentScanner.Properties;
using DocumentScanner.UserControls;
using iText.Kernel.Pdf;
using iText.Layout;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Path = System.IO.Path;

namespace DocumentScanner
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Factory based on relative path from build-copied application
        /// </summary>
        private readonly ScanFactory _scanCreator =
            new ScanFactory("Naps2App\\App\\NAPS2.Console.exe");

        private DocumentMetadataSerializer _docSerializer =
            new DocumentMetadataSerializer();

        private SerializedMetadata _docProject;
        private DocumentMetadata _docMetadata => _docProject.Data;

        private DateFormatter _dateFormatter = new DateFormatter();

        public Form1()
        {
            InitializeComponent();
            UpdateMenuButtons();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void UpdateMenuButtons()
        {
            var loaded = _docProject != null;
            var projectDependentControls = new List<ToolStripItem>
            {
                btnStep2ProcessDates,
                btnStep3SplitToPdf,
                btnConfigureZoom,
            };
            projectDependentControls.ForEach(c => c.Enabled = _docProject != null);

            this.comboDateFormats.Items.Clear();
            var formats = _dateFormatter.Formats
                .Select((x, i) => new DateFormatSample(x))
                .ToArray();
            this.comboDateFormats.Items.AddRange(formats);
            this.comboDateFormats.SelectedIndex = _dateFormatter.CurrentIndex;
        }

        private class DateFormatSample
        {
            public string Format { get; }

            public DateFormatSample(string format)
            {
                Format = format;
            }

            public override string ToString() => DateTime.Now.Date.ToString(Format);
        }

        private void LoadProject(SerializedMetadata docProject)
        {
            _docProject = docProject;
            _dateFormatter = docProject.Data.DateFormatter;
            UpdateMenuButtons();
        }

        private void btnScanToTiff_Click(object sender, EventArgs e)
        {
            if (!this.fileScan.TryGetPath(out var outputPath)) return;

            var result = _scanCreator
              .Create()
              .OutputPath(outputPath)
              .TiffCompression(TiffCompressionType.Auto)
              .NumScans(1)
              .ForceOverwrite()
              .Verbose()
              .Execute();

            var output = result.ToString();
            var filePattern = @"^(?:Finished saving images to ([^$]+)|Successfully saved \w+ file to ([^$]+))$";
            var outputFileMatches = Regex.Matches(output, filePattern, RegexOptions.IgnoreCase | RegexOptions.Multiline)
                .Cast<Match>()
                .Select(m => m.Groups[1].Value.Trim('\r', '\n'));

            output += Environment.NewLine + string.Join(Environment.NewLine, outputFileMatches);

            MessageBox.Show(output);
            var dir = Path.GetDirectoryName(outputPath);
            new FileInfo(dir).ShowInExplorer();
        }

        private void btnLoadTiff_Click(object sender, EventArgs e)
        {
            var filter = $"TIFF Image|*.tiff|TIFF Image|*.tif";
            if (!TryChooseOpenFile(out string projectPath, filter)) return;

            var doc = _docSerializer.LoadOrCreate(projectPath);
            LoadProject(doc);
        }

        private void btnBatchDateProcessing_Click(object sender, EventArgs e)
        {
            var frm = new frmConfigureZoom(_docMetadata.ImagePath);
            if (DialogResult.OK != frm.ShowDialog()) return;

            var imageCreator = frm.PreviewImageCreator;
            var ctl = new BatchPageProcessing(_docMetadata, _docProject, imageCreator);
            SetMainControl(ctl);
        }

        private void btnLoadProject_Click(object sender, EventArgs e)
        {
            var extension = DocumentMetadataSerializer.Extension;
            var filter = $"Document Project File|*{extension}";
            if (!TryChooseOpenFile(out string projectPath, filter)) return;

            var doc = _docSerializer.LoadOrCreate(projectPath);
            LoadProject(doc);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">The user-selected path if one was selected</param>
        /// <param name="filter">Filter used by the <see cref="OpenFileDialog"></see> e.g. 'PDF File|*.pdf|TEXT file|*.txt"/></param>
        /// <param name="initialDirectory"></param>
        /// <returns>True if the user selected a valid path</returns>
        private bool TryChooseOpenFile(out string path, string filter = null, string initialDirectory = null)
        {
            using (var openDialog = new OpenFileDialog()
            {
                Filter = filter,
                InitialDirectory = initialDirectory ?? Settings.Default.LastAccessedDirectory,
            })
            {
                return openDialog.TryGetPath(out path);
            }
        }

        private void btnStep3SplitToPdf_Click(object sender, EventArgs e)
        {
            var split = new SplitFilesControl(_docMetadata, _scanCreator);
            SetMainControl(split);
        }

        private void CloseMainControl()
        {
            var container = this.pnlMainContainer;
            if (container.Controls.Count == 0) return;
            Control ctl = container.Controls[0];
            container.Controls.RemoveAt(0);
            ctl.Dispose();
        }

        private void SetMainControl(Control ctl)
        {
            CloseMainControl();
            ctl.Dock = DockStyle.Fill;
            this.pnlMainContainer.Controls.Add(ctl);
        }

        private void btnTestZoom_Click(object sender, EventArgs e)
        {
            using (var frm = new frmConfigureZoom(@"Z:\Personal\Test\test-tiff.tiff"))
            {
                frm.ShowDialog();
            }
        }

        private void btnConvertPdfToTiff_Click(object sender, EventArgs e)
        {
            var filter = "PDF File|*.pdf";
            if (!TryChooseOpenFile(out string inputPdf, filter)) return;

            if (!this.fileScan.TryGetPath(out string outputPath)) return;

            _scanCreator.Create()
                .AddInputFiles(new InputFile(inputPdf))
                .TiffCompression(TiffCompressionType.Auto)
                .ForceOverwrite()
                .NumScans(0)
                .OutputPath(outputPath)
                .Execute();
        }

        private void btnBatchScan_Click(object sender, EventArgs e)
        {
            var ctl = new BatchScan(_scanCreator, _dateFormatter);
            SetMainControl(ctl);
        }

        private void comboDateFormats_SelectedIndexChanged(object sender, EventArgs e)
        {
            var oldIdx = _dateFormatter.CurrentIndex;
            var newIdx = Math.Max(0, this.comboDateFormats.SelectedIndex);
            Debug.WriteLine($"Date Formatter went from {oldIdx} to {newIdx}");
            _dateFormatter.CurrentIndex = newIdx;
        }

        private void btnMergePdfs_Click(object sender, EventArgs e)
        {
            var merger = new OptimalMerger(_scanCreator);
            var ctl = new MergeScansControl(merger);
            SetMainControl(ctl);
        }
    }

    /// <summary>
    /// Merges multiple PDF files into one.
    /// </summary>
    public interface IFileMerger
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="inputFiles">Input files including paths and 1-indexed pages to include</param>
        /// <param name="outputFile"></param>
        void Merge(IEnumerable<InputFile> inputFiles, string outputFile);
    }

    /// <summary>
    /// Uses the optimal merger for the input file types.
    /// </summary>
    public class OptimalMerger : IFileMerger
    {
        private ScanFactory _scanFactory;

        public OptimalMerger(ScanFactory scanFactory)
        {
            _scanFactory = scanFactory;
        }

        public void Merge(IEnumerable<InputFile> inputFiles, string outputFile)
        {
            var inputExtensions = inputFiles
                .Select(file => Path.GetExtension(file.Path).ToLower());
            var outputExtension = Path.GetExtension(outputFile).ToLower();

            // Mergers in best->worst order of speed/features
            var mergers = new (string[] supportedInputs, string[] supportedOutputs, Func<IFileMerger> factory)[]
            {
                (ItextPdfMerger.SupportedInputFiles,
                ItextPdfMerger.SupportedOutputFiles,
                () => new ItextPdfMerger()),
                (Naps2FileMerger.SupportedInputFiles,
                Naps2FileMerger.SupportedOutputFiles,
                () => new Naps2FileMerger(_scanFactory))
            };

            var (supportedInputs, supportedOutputs, factory) = mergers
                .FirstOrDefault(m =>
                    inputExtensions.All(m.supportedInputs.Contains)
                    && m.supportedOutputs.Contains(outputExtension));

            if (factory == null)
            {
                MessageBox.Show("The input file types cannot be merged.");
                return;
            }

            var merger = factory();
            merger.Merge(inputFiles, outputFile);
            MessageBox.Show("Done merging files.");
        }
    }

    /// <summary>
    /// Merges multiple PDF files into one using the iText library.
    /// </summary>
    /// <remarks>Much faster than <see cref="Naps2FileMerger"/>.</remarks>
    public class ItextPdfMerger : IFileMerger
    {
        public static readonly string[] SupportedInputFiles = new[] { ".pdf" };
        public static readonly string[] SupportedOutputFiles = new[] { ".pdf" };

        public void Merge(IEnumerable<InputFile> inputFiles, string outputFile)
        {
            if (!inputFiles.Any()) return;

            var output = new FileInfo(outputFile);
            output.SafeDelete();

            Directory.CreateDirectory(output.Directory.FullName);

            using (var writer = new PdfWriter(outputFile))
            {
                var doc = new PdfDocument(writer);
                var merger = new iText.Kernel.Utils.PdfMerger(doc, true, false);
                merger.SetCloseSourceDocuments(true);
                foreach (InputFile inFile in inputFiles)
                {
                    Debug.WriteLine("itext merging " + inFile);
                    using (var reader = new PdfReader(inFile.Path))
                    {
                        var inDoc = new PdfDocument(reader);
                        merger.Merge(
                            inDoc, inFile.StartPage ?? 1,
                            inFile.EndPage ?? inDoc.GetNumberOfPages());
                    }
                }

                try
                {
                    doc.Close();
                }
                catch (Exception)
                {
                    Debug.Write("Error closing merge document.");
                }
            }
        }
    }

    /// <summary>
    /// Merges multiple PDF files into one using the NAPS2 console app.
    /// Supports PDF and TIFF files.
    /// </summary>
    public class Naps2FileMerger : IFileMerger
    {
        public static readonly string[] SupportedInputFiles = new[] { ".pdf", ".tif", ".tiff" };
        public static readonly string[] SupportedOutputFiles = new[] { ".pdf", ".tif", ".tiff" };

        private readonly ScanFactory _scanCreator;

        public Naps2FileMerger(ScanFactory scanCreator)
        {
            _scanCreator = scanCreator;
        }

        public void Merge(IEnumerable<InputFile> inputFiles, string outputFile)
        {
            if (File.Exists(outputFile))
            {
                File.Delete(outputFile);
            }

            // convert from one-based to zero-based indexes.
            inputFiles = inputFiles.Select(f =>
                new InputFile(
                    f.Path,
                    f.StartPage.ValueOp(x => x - 1),
                    f.EndPage.ValueOp(x => x - 1)));

            _scanCreator.Create()
                .AddInputFiles(inputFiles)
                .OutputPath(outputFile)
                .ForceOverwrite()
                .Verbose()
                .NumScans(0)
                .ShowOutput()
                .Execute();
        }
    }

    public static class UtilExtensions
    {
        /// <summary>
        /// Performs an operation on the Nullable if it has a value or returns null if it already is.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="fn"></param>
        /// <returns>Returns the result of <see cref="fn"/> if <see cref="this"/> has a value, null otherwise.</returns>
        public static Nullable<T> ValueOp<T>(this Nullable<T> item, Func<T, T> fn) where T : struct
        {
            if (!item.HasValue) return null;
            return (T?)(fn(item.Value));
        }
    }
}