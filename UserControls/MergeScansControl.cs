using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DocumentScanner.NapsOptions;
using System.Text.RegularExpressions;
using iText.Kernel.Geom;
using System.IO;
using System.Drawing.Text;
using System.Diagnostics;

namespace DocumentScanner.UserControls
{
    public partial class MergeScansControl : UserControl
    {
        private readonly IFileMerger _merger;

        public MergeScansControl()
        {
            InitializeComponent();
        }

        public MergeScansControl(IFileMerger pdfMerger) : this()
        {
            _merger = pdfMerger ?? new ItextPdfMerger();
        }

        public void Parse()
        {
            Match ParseLine(string f) =>
                Regex.Match(f, @"^(?<path>[^\r\n\|]+)(\|(?<start>\d+):(?<end>\d+)?)?", RegexOptions.Multiline);

            int? ParseMatchInt(Match m, string name) =>
                m.Groups[name].Success ? int.Parse(m.Groups[name].Value) : (int?)null;

            var lines = this.txtInputFiles.Text.Split(
                    new[] { '\r', '\n' },
                    StringSplitOptions.RemoveEmptyEntries);

            var inputFiles =
                lines
                    .Select(ParseLine)
                    .Select(m =>
                        new InputFile(
                            m.Groups["path"].Value,
                            ParseMatchInt(m, "start"),
                            ParseMatchInt(m, "end")));

            if (!CheckMissingFiles()) return;

            var outputFile = this.txtOutputFile.Text;
            CheckForExistingOutputFile();

            try
            {
                _merger.Merge(inputFiles, outputFile);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                MessageBox.Show(
                    $"Error: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            // LOCAL HELPER FUNCTIONS

            // Return true to continue
            bool CheckMissingFiles()
            {
                var dneInputs = inputFiles
                    .Where(f => !File.Exists(f.Path))
                    .ToList();
                var prompt = "Proceed even though the following files don't exist:\r\n";
                var dneInputLabel = string.Join("\r\n", dneInputs);

                return !dneInputs.Any()
                        || FileExtensions.ConfirmAction($"{prompt}{dneInputLabel}");
            }

            // Return true to continue
            bool CheckForExistingOutputFile()
            {
                var fi = new FileInfo(outputFile);
                if (fi.Exists)
                {
                    var confirmMsg = $"{outputFile} already exists, do you want to overwrite it?";
                    if (!FileExtensions.ConfirmAction(confirmMsg)) return false;

                    if (!fi.SafeDelete())
                    {
                        MessageBox.Show("Couldn't delete file, cancelling merge.");
                        return false;
                    }
                }
                return true;
            }
        }

        private void btnBrowseOutputPath_Click(object sender, EventArgs e)
        {
            if (this.saveFileDialog1.TryGetPath(out string path, false))
            {
                this.txtOutputFile.Text = path;
            }
        }

        private void btnMerge_Click(object sender, EventArgs e)
        {
            Parse();
        }
    }
}