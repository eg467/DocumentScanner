using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;

namespace DocumentScanner
{
    internal partial class frmVerification : Form
    {
        public DateFormatter DateFormatter { get; set; }
        public RangedList<DateTime> DocDates { get; set; }
        public int PageSkip { get; set; } = 1;
        public Image PagedScan => Zoomer.SourceImage;
        private int PageCount => PagedScan?.GetFrameCount(FrameDimension.Page) ?? 0;
        public ImageZoomer Zoomer { get; set; }

        public frmVerification()
        {
            InitializeComponent();
        }

        private void frmVerification_Load(object sender, EventArgs e)
        {
            DateFormatter.FormatChanged += UpdateAllDateLabels;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            DateFormatter.FormatChanged -= UpdateAllDateLabels;
        }

        private readonly List<RowData> Rows = new List<RowData>();
        private readonly Dictionary<int, RowData> RowsByPageIndex = new Dictionary<int, RowData>();

        private void UpdateAllDateLabels(object sender, EventArgs e)
        {
            Rows.ForEach(r => r.DatePicker.CustomFormat = DateFormatter.CurrentFormat);
        }

        private IEnumerable<KeyValuePair<int, DateTime>> GetPageDates() =>
            DocDates
            .Take(PageCount)
            .Where((x, ii) => ii % PageSkip == 0);

        private void RefreshTable()
        {
            this.lblLoading.Visible = true;
            this.tableContainer.Visible = false;

            Rows.Clear();
            this.tableContainer.Controls.Clear();
            this.tableContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            var currentRow = 0;
            //this.tableContainer.RowCount = RowCount;

            foreach (var pageDate in GetPageDates())
            {
                var idx = pageDate.Key;
                var row = new RowData(idx, pageDate.Value, DateFormatter);
                Rows.Add(row);
                RowsByPageIndex[row.Index] = row;

                void SetDate(DateTime date)
                {
                    DocDates[row.Index] = date;

                    DateTime prevDate = DateTime.MinValue;
                    foreach (var kv in GetPageDates())
                    {
                        var r = RowsByPageIndex[kv.Key];
                        r.Date = kv.Value;
                        r.Refresh();
                    }
                }

                var picDoc = new PictureBox()
                {
                    Padding = new Padding(50),
                    Image = Zoomer.Zoom(idx),
                    Size = Zoomer.DestSize,
                    SizeMode = PictureBoxSizeMode.CenterImage,
                };
                // Add these last for responsiveness
                this.tableContainer.Controls.Add(picDoc, 0, currentRow);

                var pnlControl = new FlowLayoutPanel()
                {
                    FlowDirection = FlowDirection.TopDown,
                    AutoSize = true,
                    Dock = DockStyle.Fill,
                };

                var btnIncrement = new Button()
                {
                    Text = "Increment",
                    AutoSize = true,
                };
                btnIncrement.Click += (s, e) => SetDate(row.Date.AddMonths(1));
                pnlControl.Controls.Add(btnIncrement);
                this.tableContainer.Controls.Add(pnlControl, 1, currentRow);

                row.DatePicker = new DateTimePicker()
                {
                    Value = row.Date,
                    Width = 500,
                    Format = DateTimePickerFormat.Custom,
                    CustomFormat = DateFormatter.CurrentFormat,
                    Font = new Font(FontFamily.GenericSansSerif, 15f, FontStyle.Bold, GraphicsUnit.Point)
                };
                row.DatePicker.ValueChanged += (s, _) => SetDate(row.DatePicker.Value);
                row.DatePicker.MouseDown += (s, e) =>
                {
                    if (e.Button == MouseButtons.Right)
                        DateFormatter.Toggle();
                };
                this.tableContainer.Controls.Add(row.DatePicker, 2, currentRow);

                row.Refresh();
                currentRow++;
                Application.DoEvents();
            }
            this.lblLoading.Visible = false;
            this.tableContainer.Visible = true;
        }

        private class RowData
        {
            public int Index { get; }
            public DateTime Date { get; set; }
            public DateFormatter Formatter { get; }
            public DateTimePicker DatePicker { get; set; }

            public RowData(int index, DateTime date, DateFormatter formatter)
            {
                Index = index;
                this.Date = date;
                this.Formatter = formatter;
            }

            public void Refresh()
            {
                DatePicker.Format = DateTimePickerFormat.Custom;
                DatePicker.CustomFormat = Formatter.CurrentFormat;
                DatePicker.Value = Date;
            }
        }

        private void frmVerification_Shown(object sender, EventArgs e)
        {
            if (DateFormatter == null || DocDates == null || Zoomer == null)
            {
                throw new InvalidOperationException("Please provide all required dependcies to the verification form.");
            }
            RefreshTable();
        }

        private void frmVerification_Scroll(object sender, ScrollEventArgs e)
        {
        }
    }
}