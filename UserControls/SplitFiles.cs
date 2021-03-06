﻿using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using DocumentScanner.NapsOptions;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Globalization;

namespace DocumentScanner.UserControls
{
    public partial class SplitFilesControl : UserControl
    {
        public SplitFilesControl()
        {
            InitializeComponent();
        }

        private readonly ScanFactory _scanFactory;
        private readonly DocumentMetadata _docData;

        public SplitFilesControl(DocumentMetadata docData, ScanFactory scanFactory) : this()
        {
            _docData = docData;
            _scanFactory = scanFactory;
        }

        private void btnSplitFiles_Click(object sender, EventArgs e)
        {
            if (!this.folderOutputPath.TryGetPath(out var dir)) return;
            var sanitizedFilename = this.txtBaseFileName.Text.SanitizeFilename();
            var dateRanges = _docData.PageDates
                .GetRanges(0, GetPageCount() - 1)
                .Where(s => !s.Key.IsTrash)
                .ToList();

            foreach (KeyValuePair<PageDateStatus, List<(int min, int? max)>> r in dateRanges)
            {
                var inputSlices = r.Value.Select(GetFileSlice);
                var outputPath = CreateOutputPath(r.Key);

                _scanFactory
                    .Create()
                    .AddInputFiles(inputSlices)
                    .OutputPath(outputPath)
                    .PdfSettings(new PdfSettings
                    {
                        Author = Environment.UserName,
                        Title = this.txtBaseFileName.Text,
                        Subject = $"Document Dated: {r.Key?.Date.ToString() ?? "undated"}"
                    })
                    .NumScans(0)
                    .ShowOutput()
                    .ForceOverwrite()
                    .Execute();
            };

            MessageBox.Show("The documents have been successfully split by date.");
            new FileInfo(dir).ShowInExplorer();

            // Helper Functions

            int GetPageCount()
            {
                using (var img = _docData.CreateImage())
                {
                    return img.GetFrameCount(FrameDimension.Page);
                }
            }

            InputFile GetFileSlice((int min, int? max) r) =>
                new InputFile(_docData.ImagePath, r.min, r.max);

            string CreateOutputPath(PageDateStatus stmtDate, bool includeCounter = true)
            {
                var dateLabel = (stmtDate.HasDate)
                    ? stmtDate.Date.Value.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo)
                    : stmtDate.ToString();
                var counter = includeCounter ? "-$(nn)" : "";
                return Path.Combine(dir, $@"{sanitizedFilename}-{dateLabel}{counter}.pdf");
            }
        }
    }
}