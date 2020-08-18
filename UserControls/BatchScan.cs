using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using DocumentScanner.NapsOptions.Keys;
using iText.Layout.Element;
using System.Drawing.Imaging;
using System.Security.Cryptography.X509Certificates;
using System.Drawing.Text;
using DocumentScanner.NapsOptions;
using System.Runtime.InteropServices;

namespace DocumentScanner.UserControls
{
    public partial class BatchScan : UserControl
    {
        private string OutputExtension => ".pdf";

        private string _outputDir;

        public string OutputDir
        {
            get => _outputDir;
            set
            {
                if (!Directory.Exists(value))
                {
                    throw new DirectoryNotFoundException($"'{value}' is not a directory.");
                }

                _outputDir = value;
                this.btnShowOutputDir.Text = _outputDir;
                this.btnShowOutputDir.Enabled = true;
                PopulateExistingBaseNameComboOptions();

                void PopulateExistingBaseNameComboOptions()
                {
                    var existingFileBaseNames =
                        Directory.GetFiles(OutputDir, $"*{OutputExtension}")
                         .Select(f =>
                         {
                             TryParseDateFromPath(f, out var fileInfo);
                             return fileInfo.baseName;
                         })
                         .Where(x => x != null)
                         .Distinct();

                    var basenameOptions = (new string[] { "Existing names" })
                        .Concat(existingFileBaseNames)
                        .ToArray();
                    this.comboExistingBaseNames.Items.Clear();
                    this.comboExistingBaseNames.Items.AddRange(basenameOptions);
                    this.comboExistingBaseNames.SelectedIndex = 0;
                }
            }
        }

        private ScanFactory _scanFactory;
        private readonly DateFormatter _dateFormatter;
        private readonly IFileMerger _pdfMerger = new ItextPdfMerger();

        public BatchScan(
            ScanFactory scanFactory,
            DateFormatter dateFormatter = null,
            IFileMerger pdfMerger = null) : this()
        {
            _scanFactory = scanFactory;
            _dateFormatter = dateFormatter ?? new DateFormatter();
            _dateFormatter.FormatChanged += dateFormater_FormatChanged;
            _pdfMerger = pdfMerger ?? _pdfMerger;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dateFormatter.FormatChanged -= dateFormater_FormatChanged;
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void dateFormater_FormatChanged(object sender, EventArgs e)
        {
            CurrentDate = CurrentDate;
        }

        public BatchScan()
        {
            InitializeComponent();
        }

        private void btnChooseOutputDir_Click(object sender, EventArgs e)
        {
            if (!this.folderOutput.TryGetPath(out string dir)) return;
            OutputDir = dir;
        }

        private void btnShowOutputDir_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(OutputDir);
        }

        public DateTime? CurrentDate
        {
            get => this.dateCurrentDocumentDate.GetDate();
            set
            {
                value = value.ValueOp(d => d.Date);
                this.dateCurrentDocumentDate.Format = DateTimePickerFormat.Custom;
                this.dateCurrentDocumentDate.CustomFormat = _dateFormatter.CurrentFormat;
                this.dateCurrentDocumentDate.SetDate(value);

                if (value.HasValue)
                {
                    this.calNextMonth.MinDate = new DateTime(1800, 1, 1);
                    this.calNextMonth.MaxDate = DateTime.MaxValue;

                    var minDate = new DateTime(value.Value.Year, value.Value.Month, 1);
                    this.calNextMonth.MaxDate = minDate.AddMonths(2).AddDays(-1);
                    this.calNextMonth.MinDate = minDate;
                    this.calNextMonth.SetSelectionRange(value.Value, value.Value);
                }

                this.calNextMonth.Visible = value.HasValue;

                SetButton(this.btnScanCurrentDate);
                SetButton(this.btnScanNextMonth, months: 1);
                SetButton(this.btnScanSkippingTwoMonths, months: 2);
                SetButton(this.btnScanOneMonthLessOneDay, TimeSpan.FromDays(-1), 1);
                SetButton(this.btnScanOneMonthPlusOneDay, TimeSpan.FromDays(1), 1);

                void SetButton(Button btn, TimeSpan? amount = null, int months = 0)
                {
                    var newDate = AdvanceDate(CurrentDate, amount, months);
                    var label = _dateFormatter.Format(newDate);
                    btn.Text = $"{label}";
                    btn.Tag = newDate;
                }
            }
        }

        private DateTime? AdvanceDate(DateTime? date, TimeSpan? amount = null, int months = 0)
        {
            if (!date.HasValue) return null;
            return date.Value.Add(amount ?? TimeSpan.Zero).AddMonths(months);
        }

        private string UndatedLabel = "undated";

        private bool TryParseDateFromPath(string path, out (string baseName, DateTime? date) result)
        {
            var escapedUndated = Regex.Escape(UndatedLabel);
            var escapedExt = Regex.Escape(OutputExtension);
            var filename = Path.GetFileName(path);

            var pattern =
                @"(.*?)-(" + escapedUndated + @"|\d{4}-\d{2}-\d{2})" +
                @"-\d{3,}" + escapedExt;

            Match m = Regex.Match(filename, pattern);
            if (!m.Success)
            {
                result = (null, null);
                return false;
            }

            string fileBaseName = m.Groups[1].Value;

            var fileDate = m.Groups[2].Value;
            var date = DateTime.TryParse(fileDate, out var parsedDate)
                ? parsedDate : (DateTime?)null;

            result = (fileBaseName, date);
            return true;
        }

        private string BaseFilename(DateTime? date, string baseName = null)
        {
            var baseNameLabel = baseName != null
                ? baseName.SanitizeFilename()
                : CurrentSanitizedBaseName;

            var dateLabel = date.HasValue
                ? date.Value.ToString("yyyy-MM-dd")
                : UndatedLabel;
            return $"{baseNameLabel}-{dateLabel}";
        }

        private string ScanFilePath(DateTime? date, string baseName = null) =>
            $"{BaseFilename(date)}-$({new string('n', _counterLen)}){OutputExtension}";

        private string LogFilename => "docscanner.log";

        private string GetOutputPathForDate(DateTime? date) =>
            AbsolutePath(ScanFilePath(CurrentDate));

        private string AbsolutePath(string relativePath) =>
            Path.Combine(OutputDir, relativePath);

        private string CombinedFilename(DateTime? date, string baseName = null) =>
            Path.Combine("combined", $"{BaseFilename(date, baseName)}{OutputExtension}");

        public bool ViewMergedOutputOnCreation { get; set; } = false;

        private void ScanForDate(DateTime? date)
        {
            if (!EnsureOutputDir()) return;

            var filepath = GetOutputPathForDate(date);
            var result = _scanFactory.Create()
                .OutputPath(filepath)
                .TiffCompression(TiffCompressionType.Auto)
                .Verbose()
                .Execute();

            _lastFilesCreated = result.OutputFiles.ToArray();

            var totalPages = 0;
            foreach (string file in _lastFilesCreated)
            {
                var scanPages = GetFilePageCount(file);
                totalPages += scanPages;
                RecordFileSave(file, scanPages);
            }

            Log($"Scan result for {date} (**{totalPages} pages SS / {totalPages / 2} pages DS**)",
                $"{result.Output}\r\n{result.Error}");

            if (!_lastFilesCreated.Any()) return;

            var outputPath = CombineFilesForDate(date);
            if (outputPath != null && ViewMergedOutputOnCreation)
            {
                Process.Start(outputPath);
            }
        }

        private void RecordFileSave(string file, int numPages)
        {
            var pageLabel = $"{numPages} SS / {numPages / 2} DS";
            this.listRecentFiles.Items
                .Insert(0, file)
                .SubItems.Add(pageLabel);
        }

        private int GetFilePageCount(string path)
        {
            switch (Path.GetExtension(path).ToUpper())
            {
                case ".PDF":
                    using (var documentReader = new iText.Kernel.Pdf.PdfReader(path))
                    {
                        var doc = new iText.Kernel.Pdf.PdfDocument(documentReader);
                        return doc.GetNumberOfPages();
                    }
                case ".TIFF":
                case ".TIF":
                    using (System.Drawing.Image img = System.Drawing.Image.FromFile(path))
                    {
                        return img.GetFrameCount(FrameDimension.Page);
                    }
                default:
                    return 0;
            }
        }

        /// <summary>
        /// The length of the number, right-padded with zeros, for duplicate filenames.
        /// </summary>
        private const int _counterLen = 3;

        private bool EnsureOutputDir()
        {
            if (!Directory.Exists(OutputDir))
            {
                MessageBox.Show("Please set an output directory first.");
                return false;
            }
            return true;
        }

        private string CombineFilesForDate(DateTime? date, string baseName = null)
        {
            if (!EnsureOutputDir()) return null;
            baseName = baseName ?? CurrentSanitizedBaseName;
            var counterPattern = new string('?', _counterLen);
            string filenamePattern = $"{BaseFilename(date, baseName)}-{counterPattern}{OutputExtension}";
            var filesForDate = Directory
                .GetFiles(OutputDir, filenamePattern)
                .Select(f => (InputFile)f)
                .ToList();

            string combinedFilename = CombinedFilename(date, baseName);
            string outputPath = AbsolutePath(combinedFilename);

            _pdfMerger.Merge(filesForDate, outputPath);

            var numPages = GetFilePageCount(outputPath);
            RecordFileSave(outputPath, numPages);

            return outputPath;
        }

        private void Log(string title, string message)
        {
            var logEntry = $@"=======================================
{title}
{DateTime.Now}
---------------------------------------
{message}";

            // TODO inject logging dependency
            // Log to debug
            Debug.WriteLine(logEntry);

            // Log to file
            try
            {
                var logPath = AbsolutePath(LogFilename);
                var x = Path.GetTempFileName();

                var tmpLogPath = AbsolutePath($"log-{Guid.NewGuid()}.log");
                var currentLog = File.Exists(logPath) ? File.ReadAllText(logPath) : "";
                var newLog = logEntry + currentLog;
                File.WriteAllText(tmpLogPath, newLog);
                File.Delete(logPath);
                File.Copy(tmpLogPath, logPath);
                File.Delete(tmpLogPath);
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "Failed to write to the log file. "
                    + "See the developer console or debugging textbox.");
            }

            // log to text file
            this.txtLog.Text = logEntry + this.txtLog.Text;
        }

        private void CombineAllFiles()
        {
            if (!EnsureOutputDir()) return;
            var completed = new HashSet<(string baseName, DateTime? date)>();
            foreach (string file in Directory.GetFiles(OutputDir, $"*.{OutputExtension}"))
            {
                var parseSuccess = TryParseDateFromPath(file, out var result);
                if (!parseSuccess || completed.Contains(result)) continue;
                CombineFilesForDate(result.date, result.baseName);
                completed.Add(result);
            }
        }

        private void dateCurrentDocumentDate_ValueChanged(object sender, EventArgs e)
        {
            CurrentDate = this.dateCurrentDocumentDate.GetDate();
        }

        private void ScanCurrentDate() => ScanForDate(CurrentDate);

        private void btnScanCurrentDate_Click(object sender, EventArgs e)
        {
            ScanCurrentDate();
        }

        private void btnScanIncrementedDate_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            CurrentDate = (DateTime?)btn.Tag;
            Application.DoEvents();
            ScanCurrentDate();
        }

        private string[] _lastFilesCreated;

        private void btnDeleteLastScan_Click(object sender, EventArgs e)
        {
            if (_lastFilesCreated?.Any() != true) return;
            var fileDescription = string.Join(Environment.NewLine, _lastFilesCreated);
            string confirmMsg = $"Overwrite these files:\r\n{fileDescription}";
            if (!FileExtensions.ConfirmAction(confirmMsg)) return;

            var failedDeletions = _lastFilesCreated
                .Select(f => new FileInfo(f))
                .Where(fi => !fi.SafeDelete())
                .ToList();

            if (failedDeletions.Any())
            {
                MessageBox.Show(
                    "The following files could not be deleted:\r\n"
                    + string.Join("\r\n", failedDeletions));
            }

            // Recombine files in case a portion of them were deleted.
            _lastFilesCreated
                .Select(file =>
                {
                    TryParseDateFromPath(file, out var result);
                    return result.date;
                })
                .Distinct()
                .ToList()
                .ForEach(date => CombineFilesForDate(date));

            _lastFilesCreated = null;
        }

        private string CurrentSanitizedBaseName =>
            this.txtBaseFilename.Text.SanitizeFilename();

        private void listRecentFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void listRecentFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.listRecentFiles.SelectedItems
                .Cast<ListViewItem>()
                .Select(i => i.Text)
                .ForEach(f => Process.Start(f));
        }

        private void BatchScan_Load(object sender, EventArgs e)
        {
            CurrentDate = DateTime.Now.Date;
        }

        private void btnClearLogFile_Click(object sender, EventArgs e)
        {
            var logPath = AbsolutePath(LogFilename);
            try
            {
                File.Delete(logPath);
                this.txtBaseFilename.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"The log could not be deleted because of the following:\r\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void AlterCurrentDateButton_Click(object sender, EventArgs e)
        {
            var incrementTag = ((Button)sender).Tag as string;
            var incrementAmounts = incrementTag.Split(':').Select(int.Parse).ToArray();
            DateTime? newDate = AdvanceDate(
                CurrentDate,
                TimeSpan.FromDays(incrementAmounts[1]),
                incrementAmounts[0]);

            this.dateCurrentDocumentDate.SetDate(newDate);
        }

        private void btnCombineFilesForCurrentDate_Click(object sender, EventArgs e)
        {
            CombineFilesForDate(CurrentDate);
        }

        private void btnCombineAllFilesByDate_Click(object sender, EventArgs e)
        {
            CombineAllFiles();
        }

        private void comboExistingBaseNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboExistingBaseNames.SelectedIndex <= 0) return;
            this.txtBaseFilename.Text = (string)this.comboExistingBaseNames.SelectedItem;
        }

        private void calNextMonth_DateChanged(object sender, DateRangeEventArgs e)
        {
            CurrentDate = this.calNextMonth.SelectionStart;
        }
    }
}