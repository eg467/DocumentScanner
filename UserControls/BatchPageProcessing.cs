using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Threading;
using Timer = System.Windows.Forms.Timer;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DocumentScanner.Properties;
using System.Globalization;

namespace DocumentScanner.UserControls
{
    public partial class BatchPageProcessing : UserControl
    {
        #region Private Fields

        private IDictionary<int, RowData> _rows = new SortedDictionary<int, RowData>();
        private readonly DocumentMetadata _docData;
        private readonly IDocumentSaver _saver;
        private readonly Image _baseImage;
        private readonly PreviewImageCreator _imageCreator;

        /// <summary>
        /// True to avoid event handlers from responding to programmatic changes in UI values.
        /// </summary>
        private bool _isPopulating;

        private Timer _statusTimer;

        #endregion Private Fields

        public BatchPageProcessing()
        {
            InitializeComponent();
        }

        private Size _pagingControlSize;

        public BatchPageProcessing(
            DocumentMetadata docData,
            IDocumentSaver saver,
            PreviewImageCreator imageCreator = null)
        : this()
        {
            _docData = docData;
            _saver = saver;
            _baseImage = _docData.CreateImage();
            _imageCreator = imageCreator ?? new PreviewImageCreator();
            _docData.DateFormatter.FormatChanged += UpdateAllDateLabels;

            this.numSkipInterval.Value = _docData.PageSkipInterval;
            _pagingControlSize = this.pnlPagedRowContainer.Size;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                _rows?.ForEach(r => r.Value.Dispose());
                _docData.DateFormatter.FormatChanged -= UpdateAllDateLabels;
                _baseImage?.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Paging

        // Paging of pages because the max control height is limited by int16

        private class Paging
        {
            /// <summary>
            /// The maximum number of elements per page
            /// </summary>
            ///
            private int _currentPage;

            public int CurrentPage
            {
                get => _currentPage;
                set
                {
                    value = Math.Max(0, value);
                    value = Math.Min(PageCount - 1, value);
                    _currentPage = value;
                }
            }

            public bool IsFirst => CurrentPage == 0;
            public bool IsLast => CurrentPage == PageCount - 1;

            public int PageCount => (int)Math.Ceiling((double)NumItems / PageSize);

            public int NumItems { get; }

            public int PageSize { get; set; }

            public Range CurrentItemRange =>
               new Range()
               {
                   Min = Math.Max(0, CurrentPage * PageSize),
                   Max = Math.Min(NumItems - 1, CurrentPage * PageSize + PageSize - 1)
               };

            public Paging(int numItems, int pageSize)
            {
                NumItems = numItems;
                PageSize = pageSize;
            }
        }

        private Paging _paging;

        private int NumDocPages =>
            (int)Math.Ceiling(
                (double)(_baseImage?.GetFrameCount(FrameDimension.Page) ?? 0)
                / _docData.PageSkipInterval);

        private async Task UpdatePagingAwait()
        {
            await PersistPageSkipAsync();
            var rowHeight = CalculateRowHeight();
            var maxControlHeight = short.MaxValue - _pagingControlSize.Height;
            // The maximum number of rows you can show before hitting the control size limit (short.MaxValue).
            var maxRowsThatWillFit = maxControlHeight / rowHeight;

            var pageSize = Math.Min(maxRowsThatWillFit, _maxRowsPerPage);

            _paging = new Paging(NumDocPages, pageSize);
            Debug.WriteLine($"Updated Paging to NumItems={_paging.NumItems}, PageSize={_paging.PageSize}, PageCount={_paging.PageCount}");
        }

        /// <summary>
        /// The number of rows to ideally show per page, if they will fit.
        /// </summary>
        private const int _maxRowsPerPage = 50;

        private async Task SetPageAsync(int pageNum)
        {
            if (_paging == null)
            {
                await UpdatePagingAwait();
            }
            _paging.CurrentPage = pageNum;

            this.btnPreviousPage.Visible = !_paging.IsFirst;
            this.btnPreviousPage.Text = $"Previous Page ({_paging.CurrentPage}/{_paging.PageCount})";

            this.btnNextPage.Visible = !_paging.IsLast;
            this.btnNextPage.Text = $"Next Page ({_paging.CurrentPage + 2}/{_paging.PageCount})";

            RefreshTable();
        }

        private int CalculateRowHeight()
        {
            var canonicalSizeCtl = _docData.PageDates.FirstOrDefault();
            if (canonicalSizeCtl.Value == null) return 0;

            var row = RowDataFromPageData(canonicalSizeCtl.Key, canonicalSizeCtl.Value);
            using (var rowCtl = CreateRow(row))
            {
                var preferredSize = rowCtl.GetPreferredSize(Size.Empty);
                return preferredSize.Height + rowCtl.Margin.Top + rowCtl.Margin.Bottom;
            }
        }

        #endregion Paging

        private void RefreshTable()
        {
            _isPopulating = true;

            this.pnlRowContainer.Invoke((Action)(() =>
            {
                this.pnlOptions.Enabled = false;
                this.pnlRowContainer.Controls.Clear();
                this.pnlRowContainer.SuspendLayout();
            }));

            FlashStatusText("Loading page data", Color.Black, Color.Yellow);
            var rowRange = _paging.CurrentItemRange;

            _rows?.ForEach(r => r.Value.DisposeUi());
            _rows = _docData.PageDates
                .Where((_, i) => i % _docData.PageSkipInterval == 0)
                .Skip(rowRange.Min)
                .Take(rowRange.Count)
                .ToDictionary(
                    x => x.Key,
                    x => RowDataFromPageData(x.Key, x.Value));

            Debug.WriteLine($"Reading rows starting {rowRange.Min}, taking {rowRange.Count} items.");
            var keys = _docData.PageDates
                .Where((_, i) => i % _docData.PageSkipInterval == 0)
                .Select(x => x.Key)
                .Skip(rowRange.Min)
                .Take(rowRange.Count)
                .ToList();
            Debug.WriteLine(string.Join(", ", keys));

            Application.DoEvents();

            this.pnlRowContainer.Invoke((Action)(() =>
            {
                var rowCtls = _rows.Values.Select(CreateRow).ToArray();
                this.pnlRowContainer.Controls.AddRange(rowCtls);
                this.pnlRowContainer.ResumeLayout();
                this.pnlOptions.Enabled = true;

                Application.DoEvents();
                //this.pnlPagedRowContainer.Location = Point.Empty;
            }));

            _isPopulating = false;
            FlashStatusText("Page data successfully loaded", Color.Black, Color.LightGreen, 2000);
        }

        private RowData RowDataFromPageData(int index, PageDateStatus status) =>
            new RowData(index, status, _docData.DateFormatter);

        #region UI Creation

        private Image GetRowImage(RowData row)
        {
            Image scaledImg;
            try
            {
                scaledImg = _imageCreator.Zoom(_baseImage, row.Index) ?? throw new Exception();
            }
            catch (Exception ex)
            {
                Debug.Write("Error generating preview image: " + ex.Message);
                scaledImg = CreateErrorImage();
            }
            return scaledImg;
        }

        private static Bitmap CreateErrorImage()
        {
            var errorImg = new Bitmap(200, 200, PixelFormat.Format24bppRgb);
            using (var g = Graphics.FromImage(errorImg))
            using (var brush = new SolidBrush(Color.Black))
            {
                g.Clear(Color.Red);
                g.DrawString("Error", Form.DefaultFont, brush, PointF.Empty);
            }
            return errorImg;
        }

        private Panel CreateRow(RowData row)
        {
            var pnlRow = new FlowLayoutPanel()
            {
                AutoSize = true,
                Margin = new Padding(20),
                WrapContents = false,
                FlowDirection = FlowDirection.LeftToRight,
                Tag = row,
            };

            pnlRow.Controls.AddRange(new Control[] {
                CreatePageLabel(row),
                CreatePicture(row),
                CreateControlPanel(row)
            });

            return pnlRow;
        }

        private static Label CreatePageLabel(RowData row) => new Label { Text = $"{row.Index}" };

        private DateTimePicker CreatePageDatePicker(RowData row)
        {
            var picker = new DateTimePicker()
            {
                Width = 500,
                ShowCheckBox = true,
                ShowUpDown = true,
                Tag = row,
            };
            picker.ValueChanged += DatePicker_ValueChanged;
            picker.KeyDown += Picker_KeyDown;
            return picker;
        }

        private PictureBox CreatePicture(RowData row)
        {
            row.Picture = new PictureBox()
            {
                Padding = new Padding(50),
                SizeMode = PictureBoxSizeMode.AutoSize,
                Tag = row,
                Image = GetRowImage(row),
            };
            row.Picture.DoubleClick += PicDoc_DoubleClick;
            return row.Picture;
        }

        private Panel CreateControlPanel(RowData row)
        {
            var pnlControls = new FlowLayoutPanel()
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Dock = DockStyle.Fill,
                BackColor = Color.White,
            };

            var btnDecrement = new Button()
            {
                Text = Strings.DecrementButtonLabel,
                AutoSize = true,
            };

            btnDecrement.Click += async (s, e) => await IncrementAndSave(row, this.incselManual.Decrement);

            var btnIncrement = new Button()
            {
                Text = Strings.IncrementButtonLabel,
                AutoSize = true,
            };
            btnIncrement.Click += async (s, e) => await IncrementAndSave(row, this.incselManual.Increment);

            var pnlIncrement = new FlowLayoutPanel()
            {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
            };

            pnlIncrement.Controls.Add(btnDecrement);
            pnlIncrement.Controls.Add(btnIncrement);

            pnlControls.Controls.Add(pnlIncrement);

            row.TrashToggle = new CheckBox()
            {
                AutoSize = true,
                Text = Strings.DocPageTrashButtonLabel
            };
            row.TrashToggle.CheckedChanged += async (s, e) => await SetTrash(row);
            pnlControls.Controls.Add(row.TrashToggle);

            row.DatePicker = CreatePageDatePicker(row);
            pnlControls.Controls.Add(row.DatePicker);

            var btnSetSubsequent = new Button
            {
                Text = Strings.AutofillDateButtonLabel,
                AutoSize = true,
            };
            btnSetSubsequent.Click += (s, o) =>
            {
                this.numAutoIncrementPageStart.Value = row.Index;
                if (row.Status.HasDate)
                {
                    this.dateAutoIncrementStart.Value = row.Date.Value;
                }
                this.dateAutoIncrementStart.Checked = row.Status.HasDate;

                FlashStatusText("Updated Auto-increment fields", 2000);
            };
            pnlControls.Controls.Add(btnSetSubsequent);

            ProgrammaticallyRefreshRow(row);

            return pnlControls;
        }

        #endregion UI Creation

        #region UI Handlers

        private async Task IncrementAndSave(RowData row, Func<DateTime, DateTime> incrementer)
        {
            PageDateStatus StatusIncrementer(PageDateStatus oldStatus)
            {
                if (oldStatus.IsTrash) return PageDateStatus.Trash;
                if (!oldStatus.HasDate) return PageDateStatus.Undated;
                return incrementer(oldStatus.Date.Value);
            }

            if (this.rbIncrementImplicitlySetPages.Checked)
            {
                ModifyRelatedPages(row.Index, StatusIncrementer);
            }
            else
            {
                ModifyAllSubsequentPages(row, StatusIncrementer);
            }

            await SaveAsync();
        }

        private async Task SetTrash(RowData row)
        {
            _docData.PageDates[row.Index] =
                row.TrashToggle.Checked
                    ? PageDateStatus.Trash
                    : PageDateStatus.Undated;
            RefreshRelatedRowUi(row.Index);
            await SaveAsync();
        }

        private async void DatePicker_ValueChanged(object sender, EventArgs e)
        {
            // Avoid triggering handler when manually updating value
            if (_isPopulating) return;

            var picker = (DateTimePicker)sender;
            var row = (RowData)picker.Tag;
            SetImplicitlyConnectedPages(row, row.DatePicker.GetDate());
            await SaveAsync();
        }

        private void Picker_KeyDown(object sender, KeyEventArgs e)
        {
            var picker = (DateTimePicker)sender;
            var row = (RowData)picker.Tag;

            RowData IncrementRow(int index, int amount) =>
                _rows.TryGetValue(
                    index + amount * _docData.PageSkipInterval,
                    out var nextRow)
                ? nextRow
                : null;

            switch (e.KeyCode)
            {
                case Keys.End:
                case Keys.Enter:
                    var nextRow = IncrementRow(row.Index, 1);
                    if (nextRow == null) return;

                    var downScrollTarget = nextRow.Picture.Parent;
                    this.pnlMain.ScrollControlIntoView(downScrollTarget);
                    nextRow.DatePicker.Select();
                    e.SuppressKeyPress = true;
                    break;

                case Keys.Home:
                    var prevRow = IncrementRow(row.Index, -1);
                    if (prevRow == null) return;
                    var upScrollTarget = prevRow.Picture.Parent;
                    this.pnlMain.ScrollControlIntoView(upScrollTarget);
                    prevRow.DatePicker.Select();
                    e.SuppressKeyPress = true;
                    break;
            }
        }

        private void PicDoc_DoubleClick(object sender, EventArgs e)
        {
            var pic = sender as PictureBox;
            var row = pic.Tag as RowData;

            var lbl = new Label()
            {
                Text = Strings.ViewingFullSizeDocButtonLabel,
                ForeColor = Color.DarkRed,
                Font = new Font(
                    FontFamily.GenericSansSerif,
                    14f,
                    FontStyle.Bold,
                    GraphicsUnit.Point),
                AutoSize = true,
            };
            row.DatePicker.Parent.Controls.Add(lbl);

            var frm = new frmImageViewer(_docData.ImagePath, row.Index);

            async void FormClosed(object s, EventArgs args)
            {
                await Task.Delay(3000);
                lbl?.Parent?.Controls.Remove(lbl);
                lbl?.Dispose();
                frm.FormClosed -= FormClosed;
                frm.Dispose();
                _viewerReferences.Remove(frm);
            }

            frm.FormClosed += FormClosed;
            frm.Show();
            _viewerReferences.Add(frm);
        }

        private HashSet<frmImageViewer> _viewerReferences = new HashSet<frmImageViewer>();

        #endregion UI Handlers

        private async Task PersistPageSkipAsync()
        {
            _docData.PageSkipInterval = (int)this.numSkipInterval.Value;
            await SaveAsync();
        }

        #region Row/Date Modification

        private void ProgrammaticallyRefreshRow(RowData row)
        {
            _isPopulating = true;
            row.Status = _docData.PageDates[row.Index];
            row.RefreshUi();
            _isPopulating = false;
        }

        /// <summary>
        /// Modifies all explicitly set dates and statuses beginning with <see cref="row"/>
        /// </summary>
        /// <param name="row">The row with the current <see cref="RowData.Status"/> to use.</param>
        /// <param name="modifier"></param>
        private void ModifyAllSubsequentPages(
            RowData row,
            Func<PageDateStatus, PageDateStatus> modifier)
        {
            // Ensure the row/page date being set is explicitly set first
            _docData.PageDates.Set(row.Index, row.Status);

            var subsequentRowIndexes = _docData.PageDates.ExplicitlySetValues
                .Select(x => x.Key)
                .Where(i => i >= row.Index)
                .ToList();

            foreach (int idx in subsequentRowIndexes)
            {
                ModifyRelatedPages(idx, modifier);
            }
        }

        /// <summary>
        /// Increments the date of only the selected row/page, as well as implicitly
        /// updating all the contiguous set of untouched rows/pages following it.
        /// </summary>
        /// <param name="row">The row with the current <see cref="RowData.Status"/> to use.</param>
        /// <param name="increment">True to increment, false to decrement.</param>
        private void ModifyRelatedPages(
            int rowIndex,
            Func<PageDateStatus, PageDateStatus> modifier)
        {
            var oldStatus = _docData.PageDates[rowIndex];
            var newStatus = modifier(oldStatus);
            _docData.PageDates[rowIndex] = newStatus;
            if (_rows.ContainsKey(rowIndex))
            {
                var row = _rows[rowIndex];
                row.Status = newStatus;
                RefreshRelatedRowUi(row.Index);
            }
        }

        private void RefreshRelatedRowUi(int startIndex)
        {
            (int start, int? end) = _docData.PageDates.BinaryRangeSearch(startIndex);

            _rows.Values.Where(r =>
                    r.Index >= start
                    && r.Index <= (end ?? _rows.Keys.Max()))
                .ToList()
                .ForEach(ProgrammaticallyRefreshRow);
        }

        private void SetImplicitlyConnectedPages(RowData row, DateTime? date)
        {
            ModifyRelatedPages(row.Index, _ => date);
        }

        private void UpdateAllDateLabels(object sender, EventArgs e)
        {
            _rows.Values.ForEach(r => r.RefreshUi());
        }

        private async Task SaveAsync()
        {
            Debug.Write("Saving data");
            FlashStatusText("Saving Dates", Color.DarkOrange, Color.White);
            try
            {
                await _saver.SaveAsync();
            }
            catch (Exception)
            {
                FlashStatusText("Error saving dates", Color.DarkRed, Color.White, 4000);
                return;
            }
            FlashStatusText("Save Successful", Color.LightGreen, Color.Black, 1000);
        }

        #endregion Row/Date Modification

        private void FlashStatusText(string message, int? duration = null)
        {
            FlashStatusText(message, Color.Black, Color.White, duration);
        }

        private void FlashStatusText(string message, Color fgcolor, Color bgcolor, int? duration = null)
        {
            this.lblStatus.Invoke((Action)(() =>
            {
                _statusTimer?.Stop();
                _statusTimer = null;
                this.lblStatus.Text = message;
                this.lblStatus.ForeColor = fgcolor;
                this.lblStatus.BackColor = bgcolor;
                this.lblStatus.Visible = true;

                if (!duration.HasValue) return;

                _statusTimer = new Timer()
                {
                    Enabled = false,
                    Interval = duration.Value
                };
                _statusTimer.Tick += (s, o) =>
                {
                    this.lblStatus.Visible = false;
                };
                _statusTimer.Start();
            }));
        }

        internal class RowData : IDisposable
        {
            public int Index { get; }
            public PictureBox Picture { get; set; }
            public PageDateStatus Status { get; set; }

            public DateTime? Date
            {
                get => Status.Date;
                set => Status.Date = value;
            }

            public CheckBox TrashToggle { get; set; }
            public DateFormatter Formatter { get; }
            public DateTimePicker DatePicker { get; set; }

            public RowData(int index, PageDateStatus status, DateFormatter formatter)
            {
                Index = index;
                this.Status = status;
                this.Formatter = formatter;
            }

            private readonly static Font _trashFont =
                new Font(FontFamily.GenericSansSerif, 16f, FontStyle.Bold);

            private readonly static Font _activeFont =
                new Font(FontFamily.GenericSansSerif, 12f, FontStyle.Regular);

            public void RefreshUi()
            {
                DatePicker.Format = DateTimePickerFormat.Custom;
                DatePicker.CustomFormat = Formatter.CurrentFormat;

                if (DatePicker.GetDate() != Status.Date)
                {
                    DatePicker.Checked = Status.HasDate;
                    DatePicker.Value = Status.Date ?? DateTime.Now.Date;
                }

                var isTrash = Status.IsTrash;
                if (isTrash != TrashToggle.Tag as bool?)
                {
                    // Track changes with Tag and not Checked, since Checked might have changed
                    // due to user input and not programmatically with the rest of
                    // the styling.
                    TrashToggle.Tag = (bool?)isTrash;
                    TrashToggle.Checked = isTrash;
                    TrashToggle.BackColor = isTrash ? Color.Pink : Color.White;
                    TrashToggle.ForeColor = isTrash ? Color.DarkRed : Color.Black;
                    TrashToggle.Font = isTrash ? _trashFont : _activeFont;
                }
            }

            public void DisposeUi()
            {
                if (this.Picture == null) return;

                var img = this.Picture?.Image;
                this.Picture?.Dispose();
                this.Picture = null;
                img?.Dispose();

                this.TrashToggle?.Dispose();
                this.TrashToggle = null;

                this.DatePicker?.Dispose();
                this.DatePicker = null;
            }

            public void Dispose()
            {
                DisposeUi();
                //_trashFont.Dispose();
                //_activeFont.Dispose();
            }
        }

        #region UI Handlers

        private async void btnAutoIncrement_Click(object sender, EventArgs e)
        {
            DateTime? currentDate = this.dateAutoIncrementStart.GetDate();
            var firstPage = (int)this.numAutoIncrementPageStart.Value;

            if (firstPage % _docData.PageSkipInterval != 0)
            {
                MessageBox.Show("The first page must be one visible according to the number of pages you skip.");
                return;
            }

            string confirmationMsg = $"Auto increment all pages starting with {(currentDate.HasValue ? currentDate.Value.ToString("d", CultureInfo.InvariantCulture) : "<Undated>")} at page {firstPage}";
            if (!FileExtensions.ConfirmAction(confirmationMsg)) return;

            var affectedRows = _rows.Values.Where(x => x.Index >= firstPage);
            foreach (RowData r in affectedRows)
            {
                PageDateStatus status = currentDate;
                _docData.PageDates[r.Index] = status;
                r.Status = status;
                r.RefreshUi();
                currentDate = this.incselAutoIncrement.Increment(currentDate);
            }
            this.rbIncrementAllSubsequentPages.Checked = true;
            await SaveAsync();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await ResetPaging();
        }

        private async void btnResetDates_Click(object sender, EventArgs e)
        {
            _docData.PageDates.Clear();
            await ResetPaging();
        }

        /// <summary>
        /// Updates paging parameters and displays the first page
        /// </summary>
        /// <returns></returns>
        private async Task ResetPaging()
        {
            await UpdatePagingAwait();
            await SetPageAsync(0);
        }

        #endregion UI Handlers

        private async void btnPreviousPage_Click(object sender, EventArgs e)
        {
            await SetPageAsync(_paging.CurrentPage - 1);
        }

        private async void btnNextPage_Click(object sender, EventArgs e)
        {
            await SetPageAsync(_paging.CurrentPage + 1);
        }

        private void numSkipInterval_ValueChanged(object sender, EventArgs e)
        {
        }
    }
}