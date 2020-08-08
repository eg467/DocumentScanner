using DocumentScanner.NapsOptions;
using DocumentScanner.PdfProcessing;
using DocumentScanner.Properties;
using iText.IO.Source;
using iText.Kernel.Geom;
using iText.Layout.Element;
using iText.Signatures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Image = System.Drawing.Image;
using Path = System.IO.Path;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;

namespace DocumentScanner
{
    public partial class Form1 : Form
    {
        private readonly ScanFactory _scanCreator =
            new ScanFactory("Naps2App\\App\\NAPS2.Console.exe");

        private readonly DateFormatter _dateFormatter = new DateFormatter();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _dateFormatter.FormatChanged += (s, _) => UpdateDateLabels();
        }

        #region Zoom

        private void picPagePreview_MouseDown(object sender, MouseEventArgs e)
        {
            if (!CurrentPage.HasValue)
                return;
            var imgSize = this.picPagePreview.Size;
            // Save coordinates as percentages of wifth and height.
            _zoomer.Location = new Point(
                    (int)Math.Round(100.0 * e.X / imgSize.Width, 0),
                    (int)Math.Round(100.0 * e.Y / imgSize.Height, 0));
            ZoomChanged();
        }

        private void trackZoom_Scroll(object sender, EventArgs e)
        {
            _zoomer.Level = this.trackZoom.Value;
            ZoomChanged();
        }

        /// <summary>
        /// http://csharphelper.com/blog/2015/06/zoom-and-crop-a-picture-in-c/
        /// </summary>
        private void ZoomChanged()
        {
            this.lblZoomTrackbar.Text = $"Zoom Level: {_zoomer.Level}%";
            this.picZoom.Image?.Dispose();
            this.picZoom.Image = _zoomer.Zoom();
            this.picZoom.Invalidate();
        }

        #endregion Zoom

        private void SelectFileInExplorer(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }
            string argument = "/select, \"" + filePath + "\"";
            Process.Start("explorer.exe", argument);
        }

        private void btnPerformScan(object sender, EventArgs e)
        {
            if (!SetSaveFilePath(this.fileScan, out var outputPath))
                return;

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
            var matches = Regex.Matches(
                output,
                filePattern,
                RegexOptions.IgnoreCase | RegexOptions.Multiline);
            foreach (Match m in matches)
            {
                var file = m.Groups[1].Value.Trim('\r', '\n');
                output += $"{Environment.NewLine}----------{Environment.NewLine}{file}{Environment.NewLine}";
            }

            MessageBox.Show(output);
            SelectFileInExplorer(Settings.Default.LastAccessedDirectory);
        }

        // return true to continue with the scanning operation, false to abort
        private bool SetSaveFilePath(SaveFileDialog dialog, out string path)
        {
            this.fileScan.InitialDirectory = Settings.Default.LastAccessedDirectory;
            path = DialogResult.OK == dialog.ShowDialog()
                    ? dialog.FileName
                    : null;

            if (path == null)
                return false;

            Settings.Default.LastAccessedDirectory = Path.GetDirectoryName(path);
            Settings.Default.Save();

            return !File.Exists(path) || ConfirmOverwrite(dialog, ref path);
        }

        // return true to continue with the scanning operation, false to abort
        private bool ConfirmOverwrite(SaveFileDialog dialog, ref string existingFile)
        {
            var response = MessageBox.Show(
                this,
                "Select Abort to cancel this operation, Retry to choose another path, or Ignore to overwrite the file.",
                "The selected file already exists",
                MessageBoxButtons.AbortRetryIgnore);

            if (response == DialogResult.Abort)
                return false;

            if (response == DialogResult.Retry)
                return SetSaveFilePath(dialog, out existingFile);

            try
            {
                File.Delete(existingFile);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not delete the file: " + ex.Message, "Error");
                return false;
            }
        }

        private readonly RangedList<DateTime> _statementDates =
            new RangedList<DateTime>(DateTime.Today);

        private int? _currentPage;

        /// <summary>
        /// Key=page index (0-indexed), Value=statement date
        /// </summary>
        private readonly List<KeyValuePair<int, DateTime>> StatementDates =
            new List<KeyValuePair<int, DateTime>>();

        private System.Drawing.Image PreviewImage
        {
            get => this.picPagePreview.Image;
            set
            {
                StatementDates.Clear();
                this.picPagePreview.Image = value;
                _zoomer = new ImageZoomer(value)
                {
                    DestSize = this.picZoom.ClientSize,
                    Level = this.trackZoom.Value,
                };
                CurrentPage = 0;
            }
        }

        /// <summary>
        /// The image page currently being displayed
        /// </summary>
        /// <param name="page">0-indexed page index.</param>
        private int? CurrentPage
        {
            get => _currentPage;
            set
            {
                var pageCount = PageCount;

                if (
                    value.HasValue
                    && (this.picPagePreview.Image == null
                        || value.Value < 0
                        || value.Value >= pageCount
                    )
                )
                {
                    throw new ArgumentOutOfRangeException(nameof(CurrentPage));
                }

                // Valid page

                _currentPage = value;
                this.pnlPageControls.Enabled = value.HasValue;
                this.picPagePreview.Visible = value.HasValue;
                this.calendarStatementDate.Visible = value.HasValue;
                if (!_currentPage.HasValue)
                {
                    // Page deselected
                    this.lblCurrentPage.Text = "";
                    return;
                }

                // Page Selected

                int page = value.Value;
                this.picPagePreview.Image.SelectActiveFrame(FrameDimension.Page, page);
                this.picPagePreview.Invalidate();

                this.btnFirstPage.Enabled = value > 0;
                this.btnPreviousPage.Enabled = value - this.numSkipInterval.Value >= 0;
                this.btnNextPage.Enabled = value + this.numSkipInterval.Value < pageCount;
                this.btnLastPage.Enabled = value < pageCount - 1;
                this.lblCurrentPage.Text = $"Page {_currentPage + 1} of {pageCount}";
                var date = _statementDates[page];

                // TODO: standardize skip duration with click handler code.
                // hard coded to one month ahead for now.

                SetCurrentDate(date);
                ZoomChanged();
            }
        }

        private int PageCount => this.picPagePreview?.Image?.GetFrameCount(FrameDimension.Page) ?? 0;

        private void btnCurrentTiffPath_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_previewImagePath == null)
            {
                SelectImage();
            }
            else
            {
                SelectFileInExplorer(_previewImagePath);
            }
        }

        private void SelectImage()
        {
            SetOpenFilePath(this.fileOpenImage, out var path);
            DisplayImage(path);
        }

        private string _previewImagePath;
        private ImageZoomer _zoomer;

        private void DisplayImage(string path)
        {
            _previewImagePath = path;
            PreviewImage = Image.FromFile(path);
            CurrentPage = 0;
        }

        private void btnLoadTiff_Click(object sender, EventArgs e)
        {
            SelectImage();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            CurrentPage = 0;
        }

        private void btnPreviousPage_Click(object sender, EventArgs e)
        {
            CurrentPage -= (int)this.numSkipInterval.Value;
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            CurrentPage += (int)this.numSkipInterval.Value;
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            CurrentPage = PageCount - 1;
        }

        private void btnAdvanceStmtDate_Click(object sender, EventArgs e)
        {
            var start = this.calendarStatementDate.SelectionStart;
            var nextMonth = start.AddMonths(1);
            SetCurrentDate(nextMonth);
            this.btnNextPage.PerformClick();
        }

        /// <summary>
        /// Change selected date in calendar without triggering selection change event.
        /// </summary>
        private void SetCurrentDate(DateTime date)
        {
            _statementDates[CurrentPage.Value] = date;
            UpdateDateLabels();
            this.calendarStatementDate.SetSelectionRange(date, date);
        }

        private void UpdateDateLabels()
        {
            if (!CurrentPage.HasValue)
            {
                return;
            }

            var currentDate = _statementDates[CurrentPage.Value];
            this.lblPreviewDate.Text = _dateFormatter.Format(currentDate);
            this.btnAdvanceStmtDate.Text = $"Set to {_dateFormatter.Format(currentDate.AddMonths(1))}";
        }

        private void btnSplitFiles_Click(object sender, EventArgs e)
        {
            if (DialogResult.Cancel == this.folderMonthlyPath.ShowDialog())
            {
                return;
            }

            var dir = this.folderMonthlyPath.SelectedPath;

            var sanitizedFilename = this.txtBaseFileName.Text;
            Path.GetInvalidFileNameChars()
                .ToList()
                .ForEach(c => sanitizedFilename = sanitizedFilename.Replace(c, '-'));

            var operationStart = DateTime.Now;
            string CreateOutputPath(DateTime stmtDate) =>
                Path.Combine(
                    dir,
                    $@"{sanitizedFilename}-{stmtDate:yyyy-MM-dd}-$(nnn).pdf");

            InputFile GetFileSlice(Range r) =>
                new InputFile(_previewImagePath, r.Min, r.Max);

            _statementDates
                .GetRanges(0, PageCount - 1)
                .ToList()
                .ForEach(r =>
                {
                    _scanCreator
                      .Create()
                      .AddInputFiles(r.Value.Select(GetFileSlice))
                      .OutputPath(CreateOutputPath(r.Key))
                          .PdfSettings(new PdfSettings
                          {
                              Author = Environment.UserName,
                              Title = this.txtBaseFileName.Text,
                              Subject = $"Document Dated: {r.Key.Date}"
                          })
                          .NumScans(0)
                          .ForceOverwrite()
                          .Execute();
                });

            MessageBox.Show("The documents have been successfully split by date.");
            SelectFileInExplorer(dir);
        }

        private void lblPreviewDate_Click(object sender, EventArgs e)
        {
            _dateFormatter.Toggle();
        }

        private void calendarStatementDate_DateSelected(object sender, DateRangeEventArgs e)
        {
            SetCurrentDate(e.Start);
        }

        private void btnVerifyDates_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK != MessageBox.Show(
                "Don't forget to set the zoom level and location as well as the page skip interval.",
                "Reminder",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1,
                0))
            {
                return;
            }

            using (Image imgCopy = (Image)PreviewImage.Clone())
            {
                var frm = new frmVerification()
                {
                    PageSkip = (int)this.numSkipInterval.Value,
                    DateFormatter = _dateFormatter,
                    DocDates = _statementDates,
                    Zoomer = _zoomer.Clone(imgCopy)
                };
                frm.ShowDialog(this);
            }

            // Refresh
            CurrentPage = CurrentPage;
        }

        private bool SetOpenFilePath(OpenFileDialog dialog, out string path)
        {
            dialog.InitialDirectory = Settings.Default.LastAccessedDirectory;
            if (DialogResult.OK != dialog.ShowDialog())
            {
                path = null;
                return false;
            }

            path = dialog.FileName;

            if (!File.Exists(path))
            {
                MessageBox.Show(
                    "File does not exist.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                path = null;
                return false;
            }

            Settings.Default.LastAccessedDirectory = Path.GetDirectoryName(path);
            Settings.Default.Save();

            return true;
        }

        private void btnConvertPdfToTiff_Click(object sender, EventArgs e)
        {
            if (!SetOpenFilePath(this.fileOpenPdf, out var inputPdf))
                return;

            if (!SetSaveFilePath(this.fileScan, out var outputPath))
                return;

            _scanCreator.Create()
                .AddInputFiles(new InputFile(inputPdf))
                .TiffCompression(TiffCompressionType.Auto)
                .ForceOverwrite()
                .NumScans(0)
                .OutputPath(outputPath)
                .Execute();

            DisplayImage(outputPath);
        }
    }

    internal sealed class DateFormatter
    {
        public event EventHandler FormatChanged;

        private readonly string[] _formats;

        public DateFormatter() : this(new string[]
            {
                "MMM dd yyyy",
                "MM/dd/yyyy",
                "MMMM dd, yyyy"
            })
        {
        }

        public DateFormatter(params string[] formats)
        {
            if (formats == null || formats.Length == 0)
            {
                throw new ArgumentException(
                    "You must provide some date format strings.",
                    nameof(formats));
            }

            _formats = formats;
        }

        private int _index = 0;

        public void Toggle()
        {
            _index = (_index + 1) % _formats.Length;
            FormatChanged?.Invoke(this, EventArgs.Empty);
        }

        public string CurrentFormat => _formats[_index];

        public string Format(DateTime date) => date.ToString(CurrentFormat);
    }

    internal class Range
    {
        public int Min { get; set; }
        public int Max { get; set; }
    }

    /// <summary>
    /// Stores objects with integer indexes. Retrieves the highest keyed value below a specified index key.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class RangedList<T> : IEnumerable<KeyValuePair<int, T>>
    {
        /// <summary>
        /// The value to use for each index in [0, first set index).
        /// </summary>
        private readonly T _defaultValue;

        private readonly SortedList<int, T> _values = new SortedList<int, T>();

        /// <summary>
        /// Avoid scenario where [index=0]->1, [2]->2, [1]->1.
        /// The final [1]->1 is ignored since its value is already implicitly 1.
        /// </summary>
        private readonly bool _avoidRedundancy;

        /// <summary>
        ///
        /// </summary>
        /// <param name="defaultValue">The value to use for each index in [0, first set index).</param>
        /// <param name="avoidRedundancy">Ignore entries where the preceeding index has the same value.</param>
        public RangedList(T defaultValue, bool avoidRedundancy = true)
        {
            _defaultValue = defaultValue;
            _avoidRedundancy = avoidRedundancy;
        }

        public void Clear() => _values.Clear();

        public void Remove(int index) => _values.Remove(index);

        /// <summary>
        /// Note, these values must be contiguous (e.g. you can't have [0]=1, [1]=2, [2]=1).
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public IDictionary<T, List<Range>> GetRanges(int start, int end)
        {
            var rangesByValue = new SortedDictionary<T, List<Range>>();

            var currentValue = _defaultValue;

            for (var i = start; i <= end; i++)
            {
                if (_values.TryGetValue(i, out var newVal))
                {
                    currentValue = newVal;
                }

                if (!rangesByValue.TryGetValue(currentValue, out var ranges))
                {
                    ranges = new List<Range>();
                    rangesByValue[currentValue] = ranges;
                }

                if (ranges.LastOrDefault()?.Max == i - 1)
                {
                    ranges.Last().Max = i;
                }
                else
                {
                    var newRange = new Range() { Min = i, Max = i };
                    ranges.Add(newRange);
                }
            }
            return rangesByValue;
        }

        public T this[int index]
        {
            get
            {
                int? key = BinaryRangeKeySearch(index);
                return key.HasValue ? _values[key.Value] : _defaultValue;
            }
            set
            {
                if (_avoidRedundancy)
                {
                    var comparer = Comparer<T>.Default;
                    if (comparer.Compare(value, this[index]) == 0)
                        return;
                }
                _values[index] = value;
                Debug();
            }
        }

        private int? BinaryRangeKeySearch(int index)
        {
            var keys = _values.Keys;
            var start = 0;
            var stop = keys.Count - 1;
            while (start <= stop)
            {
                var m = (stop + start) / 2;
                var currentKey = keys[m];
                if (index < currentKey)
                {
                    // To left
                    stop = m - 1;
                }
                else if (m == keys.Count - 1 || index < keys[m + 1])
                {
                    return currentKey;
                }
                else
                {
                    // To right
                    start = m + 1;
                }
            }
            return null;
        }

        private void Debug()
        {
            var msg = string.Join(", ", _values.Select(x => $"[{x.Key}]={x.Value}"));
            System.Diagnostics.Debug.WriteLine(msg);
        }

        public IEnumerator<KeyValuePair<int, T>> GetEnumerator()
        {
            var valEnum = _values.GetEnumerator();
            var currentVal = _defaultValue;
            var start = 0;
            while (valEnum.MoveNext())
            {
                for (var i = start; i < valEnum.Current.Key; i++)
                    yield return new KeyValuePair<int, T>(i, currentVal);
                start = valEnum.Current.Key;
                currentVal = valEnum.Current.Value;
            }
            while (true)
            {
                yield return new KeyValuePair<int, T>(start++, currentVal);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}