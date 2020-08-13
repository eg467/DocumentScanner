using DocumentScanner.NapsOptions;
using DocumentScanner.Properties;
using DocumentScanner.UserControls;
using iText.Layout;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private readonly ScanFactory _scanCreator =
            new ScanFactory("Naps2App\\App\\NAPS2.Console.exe");

        private DocumentMetadataSerializer _docSerializer =
            new DocumentMetadataSerializer();

        private SerializedMetadata _docProject;
        private DocumentMetadata _docMetadata => _docProject.Data;

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
                btnSetDateFormat,
                btnConfigureZoom,
                comboDateFormats
            };
            projectDependentControls.ForEach(c => c.Enabled = _docProject != null);

            this.comboDateFormats.Items.Clear();
            if (_docProject == null) return;
            var formats = _docMetadata.DateFormatter.Formats
                .Select((x, i) => new DateFormatSample(x))
                .ToArray();
            this.comboDateFormats.Items.AddRange(formats);
            this.comboDateFormats.SelectedIndex = _docMetadata.DateFormatter.CurrentIndex;
        }

        private class DateFormatSample
        {
            public string Format { get; }

            public DateFormatSample(string format)
            {
                Format = format;
            }

            public override string ToString() => DateTime.Now.ToString(Format);
        }

        private void LoadProject(SerializedMetadata docProject)
        {
            _docProject = docProject;
            UpdateMenuButtons();
        }

        //private void btnSplitFiles_Click(object sender, EventArgs e)
        //{
        //    if (DialogResult.Cancel == this.folderMonthlyPath.ShowDialog())
        //    {
        //        return;
        //    }

        //    var dir = this.folderMonthlyPath.SelectedPath;

        //    var sanitizedFilename = this.txtBaseFileName.Text;
        //    Path.GetInvalidFileNameChars()
        //        .ToList()
        //        .ForEach(c => sanitizedFilename = sanitizedFilename.Replace(c, '-'));

        //    var operationStart = DateTime.Now;
        //    string CreateOutputPath(DateTime? stmtDate)
        //    {
        //        var dateLabel = stmtDate?.ToString("yyyy-MM-dd") ?? "-undated";
        //        return Path.Combine(dir, $@"{sanitizedFilename}-{dateLabel}-$(nnn).pdf");
        //    }

        //    using (var img = _docData.CreateImage())
        //    {
        //        InputFile GetFileSlice(Range r) =>
        //            new InputFile(_docData.ImagePath, r.Min, r.Max);

        //        var pageCount = img.GetFrameCount(FrameDimension.Page);

        //        _docData.PageDates
        //            .GetRanges(0, pageCount - 1)
        //            .ToList()
        //            .ForEach(r =>
        //            {
        //                _scanCreator
        //                .Create()
        //                .AddInputFiles(r.Value.Select(GetFileSlice))
        //                .OutputPath(CreateOutputPath(r.Key))
        //                .PdfSettings(new PdfSettings
        //                {
        //                    Author = Environment.UserName,
        //                    Title = this.txtBaseFileName.Text,
        //                    Subject = $"Document Dated: {r.Key?.Date.ToString() ?? "undated"}"
        //                })
        //                .NumScans(0)
        //                .ForceOverwrite()
        //                .Execute();
        //            });
        //    }

        //    MessageBox.Show("The documents have been successfully split by date.");
        //    new FileInfo(dir).ShowInExplorer();
        //}

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
            using (var dialog = CreateOpenFileDialog($"TIFF Image|*.tiff|TIFF Image|*.tif"))
            {
                if (!dialog.TryGetPath(out string projectPath)) return;
                var doc = _docSerializer.LoadOrCreate(projectPath);
                LoadProject(doc);
            }
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
            using (var dialog = CreateOpenFileDialog($"Document Project File|*{extension}"))
            {
                if (!dialog.TryGetPath(out string projectPath)) return;
                var doc = _docSerializer.LoadOrCreate(projectPath);
                LoadProject(doc);
            }
        }

        private void comboDateFormats_Click(object sender, EventArgs e)
        {
            _docMetadata.DateFormatter.CurrentIndex =
                Math.Max(0, this.comboDateFormats.SelectedIndex);
        }

        private OpenFileDialog CreateOpenFileDialog(string filter = null, string initialDirectory = null)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = filter
            };
            return dialog;
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
            string inputPdf;
            string outputPath;

            using (var openDialog = CreateOpenFileDialog("PDF File|*.pdf"))
            {
                if (!openDialog.TryGetPath(out inputPdf)) return;
            }

            if (!this.fileScan.TryGetPath(out outputPath)) return;

            _scanCreator.Create()
                .AddInputFiles(new InputFile(inputPdf))
                .TiffCompression(TiffCompressionType.Auto)
                .ForceOverwrite()
                .NumScans(0)
                .OutputPath(outputPath)
                .Execute();
        }
    }
}