using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace DocumentScanner.UserControls
{
    public partial class BatchScan : UserControl
    {
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
            }
        }

        private ScanFactory _scanFactory;
        private readonly DateFormatter _dateFormatter;

        public BatchScan(ScanFactory scanFactory, DateFormatter dateFormatter = null) : this()
        {
            _scanFactory = scanFactory;
            _dateFormatter = dateFormatter ?? new DateFormatter();
            _dateFormatter.FormatChanged += dateFormater_FormatChanged;
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
                this.dateCurrentDocumentDate.Format = DateTimePickerFormat.Custom;
                this.dateCurrentDocumentDate.CustomFormat = _dateFormatter.CurrentFormat;
                this.dateCurrentDocumentDate.SetDate(value);

                var nextDate = AdvanceDate(CurrentDate);
                var nextDateLabel = _dateFormatter.Format(nextDate);
                this.btnScanIncrementedDate.Text = $"Scan for {nextDateLabel}";
                this.btnScanIncrementedDate.Tag = nextDate;

                nextDate = AdvanceDate(nextDate);
                nextDateLabel = _dateFormatter.Format(nextDate);
                this.btnScanDoubleIncrementedDate.Text = $"Scan for {nextDateLabel}";
                this.btnScanDoubleIncrementedDate.Tag = nextDate;
            }
        }

        private DateTime? AdvanceDate(DateTime? date)
        {
            if (!date.HasValue) return null;
            return date.Value.AddMonths(1);
        }

        private string OutputExtension => ".pdf";
        private string UndatedLabel = "undated";

        private bool TryParseDateFromPath(string path, out (string baseName, DateTime? date) result)
        {
            var escapedUndated = Regex.Escape(UndatedLabel);
            var escapedExt = Regex.Escape(OutputExtension);

            var pattern =
                @"(.*?)-(" + escapedUndated + @"|\d{4}-\d{2}-\d{2})" +
                @"-\d{3,}" + escapedExt;

            Match m = Regex.Match(path, pattern);
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
            System.IO.Path.Combine(OutputDir, relativePath);

        private string CombinedFilename(DateTime? date, string baseName = null) =>
            System.IO.Path.Combine("combined", $"{BaseFilename(date, baseName)}{OutputExtension}");

        private void ScanForDate(DateTime? date)
        {
            if (!Directory.Exists(OutputDir))
            {
                MessageBox.Show(
                    "The output directory must be set first.",
                    "No output directory",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            var filepath = GetOutputPathForDate(date);
            var result = _scanFactory.Create()
                .OutputPath(filepath)
                .Verbose()
                .Execute();

            Log($"Scan result for {date}", $"{result.Output}\r\n{result.Error}");

            _lastFilesCreated = result.OutputFiles.ToArray();

            if (!_lastFilesCreated.Any()) return;

            CombineFilesForDate(date);

            foreach (string file in _lastFilesCreated)
            {
                this.listRecentFiles.Items.Insert(0, file);
            }
        }

        /// <summary>
        /// The length of the number, right-padded with zeros, for duplicate filenames.
        /// </summary>
        private const int _counterLen = 4;

        private void CombineFilesForDate(DateTime? date, string baseName = null)
        {
            baseName = baseName ?? CurrentSanitizedBaseName;
            var counter = new string('?', _counterLen);
            string filenamePattern =
                $"{BaseFilename(date, CurrentSanitizedBaseName)}-{counter}{OutputExtension}";
            var filesForDate = Directory.GetFiles(
                OutputDir,
                filenamePattern);

            string combinedFilename = CombinedFilename(date, baseName);
            string outputPath = AbsolutePath(combinedFilename);
            var result = _scanFactory.Create()
                .AddInputFiles(filesForDate)
                .OutputPath(outputPath)
                .NumScans(0)
                .ForceOverwrite()
                .Execute();

            Log($"Merge file result for {baseName} @ {date}", $"{result.Output}\r\n{result.Error}");

            if (result.ExitCode != 0 || !string.IsNullOrEmpty(result.Error))
            {
                MessageBox.Show(
                    result.Output + Environment.NewLine + result.Error,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void Log(string title, string message)
        {
            var logEntry = $@"
=======================================
{title}
{DateTime.Now}
---------------------------------------
{message}
";

            // TODO inject logging dependency
            // Log to debug
            Debug.Write(logEntry);

            // Log to file
            try
            {
                var logPath = AbsolutePath(LogFilename);
                var x = System.IO.Path.GetTempFileName();

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

            var failures = new List<Exception>();
            foreach (string file in _lastFilesCreated)
            {
                if (File.Exists(file))
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception ex)
                    {
                        failures.Add(ex);
                    }
                }
            }

            if (failures.Any())
            {
                throw new AggregateException("Failed to delete some files.", failures);
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
            var selectedFile = this.listRecentFiles.SelectedItems.Cast<string>().FirstOrDefault();
            if (selectedFile != null)
            {
                Process.Start(selectedFile);
            }
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
    }
}