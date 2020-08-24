using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;

namespace DocumentScanner.UserControls
{
    public partial class PerPageProcessing : UserControl
    {
        public PerPageProcessing()
        {
            InitializeComponent();
        }

        public PerPageProcessing(DocumentMetadata docData) : this()
        {
            this.DocData = docData;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.picPagePreview.Image.Dispose();
                components?.Dispose();
            }

            base.Dispose(disposing);
        }

        private Image PreviewImage => this.picPagePreview.Image;
        private int PageCount => PreviewImage?.GetFrameCount(FrameDimension.Page) ?? 0;

        private int _currentPage;

        /// <summary>
        /// The image page currently being displayed
        /// </summary>
        /// <param name="page">0-indexed page index.</param>
        private int CurrentPage
        {
            get => _currentPage;
            set
            {
                var pageCount = PageCount;
                if (this.picPagePreview.Image == null)
                    return;

                if (value < 0 || value >= pageCount)
                    throw new ArgumentOutOfRangeException(nameof(CurrentPage));

                _currentPage = value;
                this.picPagePreview.Image.SelectActiveFrame(FrameDimension.Page, _currentPage);
                this.picPagePreview.Invalidate();

                this.btnFirstPage.Enabled = value > 0;
                this.btnPreviousPage.Enabled = value - this.numSkipInterval.Value >= 0;
                this.btnNextPage.Enabled = value + this.numSkipInterval.Value < pageCount;
                this.btnLastPage.Enabled = value < pageCount - 1;
                this.lblCurrentPage.Text = $"Page {_currentPage + 1} of {pageCount}";

                // Refresh UI with initial date
                CurrentDate = DocData.PageDates[_currentPage].Date;
            }
        }

        private DateTime? CurrentDate
        {
            get => DocData.PageDates[CurrentPage].Date;
            set
            {
                DocData.PageDates[CurrentPage] = value;
                UpdateDateLabels();

                bool isDated = value.HasValue;
                this.cbHasDate.Checked = isDated;
                this.calendarStatementDate.Visible = isDated;
                if (isDated)
                    this.calendarStatementDate.SetSelectionRange(value.Value, value.Value);
            }
        }

        public DocumentMetadata DocData { get; }

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
            CurrentDate = start.AddMonths(1);
            this.btnNextPage.PerformClick();
        }

        private void UpdateDateLabels()
        {
            var currentDate = CurrentDate;

            if (currentDate.HasValue)
            {
                this.lblPreviewDate.Text = DocData.DateFormatter.Format(currentDate.Value);
                var nextMonth = currentDate.Value.AddMonths(1);
                this.btnAdvanceStmtDate.Enabled = true;
                this.btnAdvanceStmtDate.Text = $"Set to {DocData.DateFormatter.Format(nextMonth)}";
            }
            else
            {
                this.lblPreviewDate.Text = Strings.UndatedPreviewText;
                this.btnAdvanceStmtDate.Enabled = false;
                this.btnAdvanceStmtDate.Text = Strings.IncrementUndatedButtonText;
            }
        }

        private void calendarStatementDate_DateSelected(object sender, DateRangeEventArgs e)
        {
            CurrentDate = e.Start;
        }

        private void PerPageProcessing_Load(object sender, EventArgs e)
        {
            this.picPagePreview.Image = DocData.CreateImage();
            CurrentPage = 0;
        }

        private void cbHasDate_CheckedChanged(object sender, EventArgs e)
        {
            CurrentDate = this.cbHasDate.Checked ? DateTime.Now.Date : (DateTime?)null;
        }
    }
}