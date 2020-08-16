using DocumentScanner.NapsOptions;
using DocumentScanner.Properties;
using DocumentScanner.UserControls;
using iText.Kernel.Pdf;
using iText.Layout;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
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
    }

    /// <summary>
    /// Merges multiple PDF files into one.
    /// </summary>
    public interface IPdfMerger
    {
        void Merge(IEnumerable<string> inputFiles, string outputFile);
    }

    /// <summary>
    /// Merges multiple PDF files into one using the iText library.
    /// </summary>
    /// <remarks>Much faster than <see cref="Naps2PdfMerger"/>.</remarks>
    public class ItextPdfMerger : IPdfMerger
    {
        public void Merge(IEnumerable<string> inputFiles, string outputFile)
        {
            if (File.Exists(outputFile))
            {
                File.Delete(outputFile);
            }

            var dir = Path.GetDirectoryName(outputFile);
            Directory.CreateDirectory(dir);

            using (var writer = new PdfWriter(outputFile, new WriterProperties { }))
            {
                var doc = new PdfDocument(writer);
                var merger = new iText.Kernel.Utils.PdfMerger(doc, true, false);
                merger.SetCloseSourceDocuments(true);
                foreach (string inFile in inputFiles)
                {
                    Debug.WriteLine("itext merging " + inFile);
                    using (var reader = new PdfReader(inFile))
                    {
                        var inDoc = new PdfDocument(reader);
                        var numPages = inDoc.GetNumberOfPages();
                        merger.Merge(inDoc, 1, numPages);
                    }
                }
                merger.Close();
                doc.Close();
            }
        }
    }

    /// <summary>
    /// Merges multiple PDF files into one using the NAPS2 console app.
    /// </summary>
    public class Naps2PdfMerger : IPdfMerger
    {
        private readonly ScanFactory _scanCreator;

        public Naps2PdfMerger(ScanFactory scanCreator)
        {
            _scanCreator = scanCreator;
        }

        public void Merge(IEnumerable<string> inputFiles, string outputFile)
        {
            if (File.Exists(outputFile))
            {
                File.Delete(outputFile);
            }

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
}