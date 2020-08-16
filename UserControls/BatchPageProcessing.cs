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

namespace DocumentScanner.UserControls
{
    public partial class BatchPageProcessing : UserControl
    {
        #region Private Fields

        private readonly Dictionary<int, RowData> _rows = new Dictionary<int, RowData>();
        private readonly DocumentMetadata _docData;
        private readonly IDocumentSaver _saver;
        private readonly Image _baseImage;
        private readonly PreviewImageCreator _imageCreator;
        private bool _isPopulating = false;
        private Timer _statusTimer;
        private CancellationTokenSource _imagePopCanceler;

        #endregion Private Fields

        public BatchPageProcessing()
        {
            InitializeComponent();
        }

        public BatchPageProcessing(
            DocumentMetadata docData,
            IDocumentSaver saver,
            PreviewImageCreator imageCreator = null) : this()
        {
            _docData = docData;
            _saver = saver;
            _baseImage = _docData.CreateImage();
            _imageCreator = imageCreator ?? new PreviewImageCreator();
            _docData.DateFormatter.FormatChanged += UpdateAllDateLabels;

            this.numSkipInterval.Value = _docData.PageSkipInterval;
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
                _docData.DateFormatter.FormatChanged -= UpdateAllDateLabels;
                _baseImage?.Dispose();
            }

            base.Dispose(disposing);
        }

        private async Task RefreshTableAsync()
        {
            _imagePopCanceler?.Cancel();

            await PersistPageSkipAsync();
            _rows.Clear();
            _isPopulating = true;

            this.tableContainer.Invoke((Action)(() =>
            {
                this.pnlOptions.Enabled = false;
                this.tableContainer.Controls.Clear();
                this.tableContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                this.tableContainer.SuspendLayout();
            }));

            FlashStatusText("Loading page data", Color.Black, Color.Yellow);

            foreach (var pageDate in GetPageDates())
            {
                var row = new RowData(pageDate.Key, pageDate.Value, _docData.DateFormatter);
                _rows[row.Index] = row;

                this.tableContainer.Invoke((Action)(() =>
                {
                    AddRowControls(
                        row.Index,
                        CreatePageLabel(row),
                        CreatePicture(row),
                        CreateControlPanel(row));
                    Application.DoEvents();
                }));
            }

            _imagePopCanceler = new CancellationTokenSource();
            ThreadPool.QueueUserWorkItem(PopulateImages, _imagePopCanceler.Token);

            _isPopulating = false;
            FlashStatusText("Page data successfully loaded", Color.Black, Color.LightGreen, 2000);

            this.tableContainer.Invoke((Action)(() =>
            {
                this.tableContainer.ResumeLayout();
                this.pnlOptions.Enabled = true;
            }));

            // LOCAL HELPER FUNCTIONS

            IEnumerable<KeyValuePair<int, PageDateStatus>> GetPageDates()
            {
                return _docData.PageDates
                .Take(_baseImage.GetFrameCount(FrameDimension.Page))
                .Where((x, ii) => ii % _docData.PageSkipInterval == 0)
                .ToList();
            }

            #region UI Creation

            void PopulateImages(object state)
            {
                var token = (CancellationToken)state;
                _rows.Values
                    .Where(_ => !token.IsCancellationRequested)
                    .ForEach(PopulateImage);
            }

            void PopulateImage(RowData row)
            {
                Image scaledImg = null;
                try
                {
                    scaledImg = _imageCreator.Zoom(_baseImage, row.Index)
                        ?? throw new Exception();
                }
                catch (Exception ex)
                {
                    Debug.Write("Error generating preview image: " + ex.Message);
                    scaledImg = CreateErrorImage();
                }
                finally
                {
                    row.Picture.Invoke((Action)(() => row.Picture.Image = scaledImg));
                }
            }

            Bitmap CreateErrorImage()
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

            void AddRowControls(int row, params Control[] controls)
            {
                void AddRow(Control control, int col)
                {
                    this.tableContainer.Controls.Add(control, col, row);
                    Application.DoEvents();
                }

                this.tableContainer.Invoke((Action)(() => controls.ForEach(AddRow)));
            }

            Label CreatePageLabel(RowData row) => new Label { Text = $"{row.Index}" };

            DateTimePicker CreatePageDatePicker(RowData row)
            {
                var picker = new DateTimePicker()
                {
                    Width = 500,
                    ShowCheckBox = true,
                    Tag = row,
                };
                picker.ValueChanged += DatePicker_ValueChanged;
                picker.KeyDown += Picker_KeyDown;
                return picker;
            }

            PictureBox CreatePicture(RowData row)
            {
                row.Picture = new PictureBox()
                {
                    Padding = new Padding(50),
                    SizeMode = PictureBoxSizeMode.AutoSize,
                    Tag = row,
                };
                row.Picture.DoubleClick += PicDoc_DoubleClick;
                return row.Picture;
            }

            Panel CreateControlPanel(RowData row)
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
                    Text = "◀ Decrement",
                    AutoSize = true,
                };

                btnDecrement.Click += async (s, e) => await IncrementAndSave(row, this.incselManual.Decrement);

                var btnIncrement = new Button()
                {
                    Text = "Increment ▶",
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
                    Text = "Trash"
                };
                row.TrashToggle.CheckedChanged += async (s, e) => await SetTrash(row);
                pnlControls.Controls.Add(row.TrashToggle);

                row.DatePicker = CreatePageDatePicker(row);
                pnlControls.Controls.Add(row.DatePicker);

                var btnSetSubsequent = new Button
                {
                    Text = "Autofill AutoIncrement fields above",
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

            async Task IncrementAndSave(RowData row, Func<DateTime, DateTime> incrementer)
            {
                PageDateStatus StatusIncrementer(PageDateStatus oldStatus)
                {
                    if (row.Status.IsTrash) return PageDateStatus.Trash;
                    if (!row.Status.HasDate) return PageDateStatus.Undated;
                    return incrementer(row.Date.Value);
                }

                if (this.rbIncrementImplicitlySetPages.Checked)
                {
                    ModifyRelatedPages(row, StatusIncrementer);
                }
                else
                {
                    ModifyAllSubsequentPages(row, StatusIncrementer);
                }

                await SaveAsync();
            }

            async Task SetTrash(RowData row)
            {
                _docData.PageDates[row.Index] =
                    row.TrashToggle.Checked
                        ? PageDateStatus.Trash
                        : PageDateStatus.Undated;
                RefreshRelatedRowUi(row.Index);
                await SaveAsync();
            }

            async void DatePicker_ValueChanged(object sender, EventArgs e)
            {
                // Avoid triggering handler when manually updating value
                if (_isPopulating) return;

                var picker = (DateTimePicker)sender;
                var row = (RowData)picker.Tag;
                SetImplicitlyConnectedPages(row, row.DatePicker.GetDate());
                await SaveAsync();
            }

            void Picker_KeyDown(object sender, KeyEventArgs e)
            {
                var picker = (DateTimePicker)sender;
                var row = (RowData)picker.Tag;

                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        if (!_rows.TryGetValue(row.Index + 1, out RowData nextRow)) return;
                        nextRow.DatePicker.Select();
                        break;
                }
            }

            void PicDoc_DoubleClick(object sender, EventArgs e)
            {
                var pic = sender as PictureBox;
                var row = pic.Tag as RowData;

                var lbl = new Label()
                {
                    Text = "Currently Viewing",
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

                async void RemoveIndicator(object s, EventArgs args)
                {
                    await Task.Delay(3000);
                    lbl.Parent.Controls.Remove(lbl);
                    lbl.Dispose();
                    frm.FormClosed -= RemoveIndicator;
                }

                frm.FormClosed += RemoveIndicator;

                frm.Show();
            }

            #endregion UI Handlers
        }

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
            _docData.PageDates[row.Index] = row.Status;
            var rows = _docData.PageDates.ExplicitlySetValues
                .Where(x => x.Key >= row.Index)
                .Select(x => _rows[x.Key])
                .ToList();

            rows.ForEach(r => ModifyRelatedPages(r, modifier));
        }

        /// <summary>
        /// Increments the date of only the selected row/page, as well as implicitly
        /// updating all the contiguous set of untouched rows/pages following it.
        /// </summary>
        /// <param name="row">The row with the current <see cref="RowData.Status"/> to use.</param>
        /// <param name="increment">True to increment, false to decrement.</param>
        private void ModifyRelatedPages(
            RowData row,
            Func<PageDateStatus, PageDateStatus> modifier)
        {
            row.Status = modifier(row.Status);
            _docData.PageDates[row.Index] = row.Status;
            RefreshRelatedRowUi(row.Index);
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
            ModifyRelatedPages(row, _ => date);
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

        internal class RowData
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
            public CheckBox cbHasDate { get; set; }
            public DateTimePicker DatePicker { get; set; }

            public RowData(int index, PageDateStatus status, DateFormatter formatter)
            {
                Index = index;
                this.Status = status;
                this.Formatter = formatter;
            }

            private readonly Font _trashFont =
                new Font(FontFamily.GenericSansSerif, 16f, FontStyle.Bold);

            private readonly Font _activeFont =
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

            string confirmationMsg = $"Auto increment all pages starting with {(currentDate.HasValue ? currentDate.Value.ToString("d") : "<Undated>")} at page {firstPage}";
            if (!FileExtensions.ConfirmAction(confirmationMsg)) return;

            var affectedRows = _rows.Values.Where(x => x.Index >= firstPage);
            foreach (RowData r in affectedRows)
            {
                r.Status = _docData.PageDates[r.Index];
                r.RefreshUi();
                currentDate = this.incselAutoIncrement.Increment(currentDate);
            }
            this.rbIncrementAllSubsequentPages.Checked = true;
            await SaveAsync();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await Task.Run(RefreshTableAsync);
        }

        private async void btnResetDates_Click(object sender, EventArgs e)
        {
            _docData.PageDates.Clear();
            await Task.Run(RefreshTableAsync);
        }

        #endregion UI Handlers
    }
}