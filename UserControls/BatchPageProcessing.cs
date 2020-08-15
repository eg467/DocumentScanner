using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Threading;
using Timer = System.Windows.Forms.Timer;
using System.Diagnostics;
using System.Reflection;
using DocumentScanner.NapsOptions.Keys;

namespace DocumentScanner.UserControls
{
    public partial class BatchPageProcessing : UserControl
    {
        private readonly DocumentMetadata _docData;
        private readonly IDocumentSaver _saver;
        private readonly Image _baseImage;
        private readonly PreviewImageCreator _imageCreator;
        private readonly IPdfMerger _pdfMerger;

        public BatchPageProcessing()
        {
            InitializeComponent();
        }

        public BatchPageProcessing(
            DocumentMetadata docData,
            IDocumentSaver saver,
            PreviewImageCreator imageCreator = null,
            IPdfMerger pdfMerger = null) : this()
        {
            _docData = docData;
            _saver = saver;
            _baseImage = _docData.CreateImage();
            _imageCreator = imageCreator ?? new PreviewImageCreator();
            _docData.DateFormatter.FormatChanged += UpdateAllDateLabels;
            _pdfMerger = pdfMerger ?? new ItextPdfMerger();

            this.numSkipInterval.Value = _docData.PageSkipInterval;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            _docData.DateFormatter.FormatChanged -= UpdateAllDateLabels;
            _baseImage?.Dispose();
            base.Dispose(disposing);
        }

        //private readonly List<RowData> Rows = new List<RowData>();
        private readonly Dictionary<int, RowData> Rows = new Dictionary<int, RowData>();

        private void UpdateAllDateLabels(object sender, EventArgs e)
        {
            foreach (var r in Rows.Values)
            {
                r.DatePicker.CustomFormat = _docData.DateFormatter.CurrentFormat;
                //r.Date = _docData.PageDates[r.Index];
                r.Refresh();
            }
        }

        private IEnumerable<KeyValuePair<int, DateTime?>> GetPageDates()
        {
            return _docData.PageDates
            .Take(_baseImage.GetFrameCount(FrameDimension.Page))
            .Where((x, ii) => ii % _docData.PageSkipInterval == 0);
        }

        private bool _isPopulating = false;

        private CancellationTokenSource _imagePopCanceler;

        private async Task RefreshTableAsync()
        {
            _imagePopCanceler?.Cancel();

            await PersistPageSkipAsync();
            Rows.Clear();
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
                var row = new RowData(
                    pageDate.Key,
                    pageDate.Value,
                    _docData.DateFormatter);

                Rows[row.Index] = row;

                this.tableContainer.Invoke((Action)(() =>
                {
                    AddRowControls(
                        row.Index,
                        CreatePageLabel(row),
                        CreatePic(row),
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

            void PopulateImages(object obj)
            {
                var token = (CancellationToken)obj;
                foreach (RowData r in Rows.Values)
                {
                    if (token.IsCancellationRequested) break;
                    var pic = r.Picture;
                    Image scaledImg = null;
                    try
                    {
                        scaledImg = _imageCreator.Zoom(_baseImage, r.Index);
                        if (scaledImg == null) throw new Exception();
                    }
                    catch (Exception ex)
                    {
                        Debug.Write("Error generating preview image: " + ex.Message);

                        var errorImg = new Bitmap(200, 200, PixelFormat.Format24bppRgb);
                        using (var g = Graphics.FromImage(errorImg))
                        using (var brush = new SolidBrush(Color.Black))
                        {
                            g.Clear(Color.Red);
                            g.DrawString("Error", Form.DefaultFont, brush, PointF.Empty);
                        }
                    }
                    finally
                    {
                        pic.Invoke((Action)(() =>
                        {
                            pic.Image = scaledImg;
                        }));
                    }
                }
                return;
            }

            async Task PersistPageSkipAsync()
            {
                _docData.PageSkipInterval = (int)this.numSkipInterval.Value;
                await SaveAsync();
            }

            void AddRowControls(int row, params Control[] controls)
            {
                this.tableContainer.Invoke((Action)(() =>
                {
                    for (var i = 0; i < controls.Length; i++)
                    {
                        this.tableContainer.Controls.Add(controls[i], i, row);
                    }
                    Application.DoEvents();
                }));
            }

            Label CreatePageLabel(RowData row) =>
                new Label { Text = row.Index.ToString() };

            DateTimePicker CreatePageDatePicker(RowData row)
            {
                var picker = new DateTimePicker()
                {
                    Width = 500,
                    ShowCheckBox = true,
                    Tag = row,
                };
                picker.ValueChanged += DatePicker_ValueChanged;
                return picker;
            }

            PictureBox CreatePic(RowData row)
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

            async Task Increment(RowData row, Func<DateTime?, DateTime?> incrementer)
            {
                IncrementRows(row, incrementer);
                await SaveAsync();
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
                btnDecrement.Click += async (s, e) => await Increment(row, this.incselManual.Decrement);

                var btnIncrement = new Button()
                {
                    Text = "Increment ▶",
                    AutoSize = true,
                };
                btnIncrement.Click += async (s, e) => await Increment(row, this.incselManual.Increment);

                var pnlIncrement = new FlowLayoutPanel()
                {
                    FlowDirection = FlowDirection.LeftToRight,
                    AutoSize = true,
                };
                pnlIncrement.Controls.Add(btnDecrement);
                pnlIncrement.Controls.Add(btnIncrement);
                pnlControls.Controls.Add(pnlIncrement);

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
                    if (row.Date.HasValue)
                    {
                        this.dateAutoIncrementStart.Value = row.Date.Value;
                    }
                    this.dateAutoIncrementStart.Checked = row.Date.HasValue;

                    FlashStatusText("Updated Auto-increment fields", 2000);
                };
                pnlControls.Controls.Add(btnSetSubsequent);

                ProgrammaticallyRefreshRow(row);

                return pnlControls;
            }
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

        private void IncrementRows(RowData row, Func<DateTime?, DateTime?> incrementer)
        {
            if (this.rbIncrementImplicitlySetPages.Checked)
            {
                IncrementImplicitlyConnected(row, incrementer);
            }
            else
            {
                IncrementAllSubsequentPages(row, incrementer);
            }
        }

        private void IncrementAllSubsequentPages(RowData row, Func<DateTime?, DateTime?> incrementer)
        {
            //incrementer(row.Date)

            // Ensure the row/page date being set is explicitly set first
            _docData.PageDates[row.Index] = row.Date;
            var rows = _docData.PageDates.ExplicitlySetValues
                .Where(x => x.Key >= row.Index)
                .Select(x => Rows[x.Key])
                .ToList();

            rows.ForEach(r => IncrementImplicitlyConnected(r, incrementer));
        }

        /// <summary>
        /// Increments the date of only the selected row/page, as well as implicitly
        /// updating all the contiguous set of untouched rows/pages following it.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="increment">True to increment, false to decrement.</param>
        private void IncrementImplicitlyConnected(
            RowData row,
            Func<DateTime?, DateTime?> incrementer)
        {
            row.Date = row.Date.HasValue ? incrementer(row.Date.Value) : null;
            _docData.PageDates[row.Index] = row.Date;
            (int start, int? end) = _docData.PageDates.BinaryRangeSearch(row.Index);
            RefreshRowUi(start, end ?? Rows.Keys.Max());
        }

        private async Task SaveAsync()
        {
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

        private Timer _statusTimer;

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

        private void SetImplicitlyConnectedPages(RowData row, DateTime? date)
        {
            IncrementImplicitlyConnected(row, _ => date);
        }

        private void RefreshRowUi(int start, int end)
        {
            Rows.Values.Where(r => r.Index >= start && r.Index <= end)
                .ToList()
                .ForEach(ProgrammaticallyRefreshRow);
        }

        private void PicDoc_DoubleClick(object sender, EventArgs e)
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

        internal class RowData
        {
            public int Index { get; }
            public PictureBox Picture { get; set; }
            public DateTime? Date { get; set; }
            public DateFormatter Formatter { get; }
            public CheckBox cbHasDate { get; set; }
            public DateTimePicker DatePicker { get; set; }

            public RowData(int index, DateTime? date, DateFormatter formatter)
            {
                Index = index;
                this.Date = date;
                this.Formatter = formatter;
            }

            public void Refresh()
            {
                DatePicker.Format = DateTimePickerFormat.Custom;
                DatePicker.CustomFormat = Formatter.CurrentFormat;
                //cbHasDate.Checked = Date.HasValue;
                DatePicker.Checked = Date.HasValue;
                DatePicker.Value = Date.HasValue ? Date.Value : DateTime.Now;
            }
        }

        private void ProgrammaticallyRefreshRow(RowData row)
        {
            _isPopulating = true;
            row.Date = _docData.PageDates[row.Index];
            row.Refresh();
            _isPopulating = false;
        }

        private void BatchPageProcessing_Load(object sender, EventArgs e)
        {
        }

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

            var affectedRows = Rows.Values.Where(x => x.Index >= firstPage);
            foreach (RowData r in affectedRows)
            {
                _docData.PageDates[r.Index] = currentDate;
                r.Date = currentDate;
                r.Refresh();
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

        private void lblStatus_Click(object sender, EventArgs e)
        {
            var x = _docData.PageDates;
        }
    }
}