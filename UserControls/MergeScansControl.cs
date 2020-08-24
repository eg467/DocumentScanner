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
using System.Collections;
using System.Globalization;

namespace DocumentScanner.UserControls
{
    public partial class MergeScansControl : UserControl
    {
        private readonly IFileMerger _merger;

        /// <summary>
        /// A character invalid in paths used to separate path from page range in text input.
        /// </summary>
        private const char _pathRangeSeparator = '|';

        public MergeScansControl()
        {
            InitializeComponent();
        }

        public MergeScansControl(IFileMerger pdfMerger) : this()
        {
            _merger = pdfMerger ?? new ItextPdfMerger();
        }

        private InputFile ParseInputFilePathLine(string line)
        {
            var separator = Regex.Escape(_pathRangeSeparator.ToString());

            int? ParseMatchInt(Match m, string name) =>
                m.Groups[name].Success ? int.Parse(m.Groups[name].Value, CultureInfo.InvariantCulture) : (int?)null;

            var match = Regex.Match(
                line,
                $@"^(?<path>[^\r\n{separator}]+)({separator}(?<start>\d+):(?<end>\d+)?)?",
                RegexOptions.Multiline);

            return match.Success
                ? new InputFile(
                    match.Groups["path"].Value,
                    ParseMatchInt(match, "start"),
                    ParseMatchInt(match, "end"))
                : null;
        }

        private IEnumerable<InputFile> ParseInputLines() =>
            GetInputLines()
                .Select(ParseInputFilePathLine)
                .Where(x => x != null);

        private string[] GetInputLines() => this.txtInputFiles.Text.Split(
                    new[] { '\r', '\n' },
                    StringSplitOptions.RemoveEmptyEntries);

        public void Parse()
        {
            var inputFiles = ParseInputLines();
            if (!CheckForMissingInputFiles(inputFiles)) return;

            var outputFile = this.txtOutputFile.Text;
            if (!CheckForExistingOutputFile(outputFile)) return;

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
            bool CheckForMissingInputFiles(IEnumerable<InputFile> files)
            {
                var dneInputs = files.Where(f => !File.Exists(f.Path));
                var prompt = "Proceed even though the following files don't exist? \r\n";
                var dneInputLabel = string.Join("\r\n", dneInputs);

                return !dneInputs.Any()
                        || FileExtensions.ConfirmAction($"{prompt}{dneInputLabel}");
            }

            // Return true to continue
            bool CheckForExistingOutputFile(string outputPath)
            {
                var fi = new FileInfo(outputPath);
                if (fi.Exists)
                {
                    var confirmMsg = $"{outputPath} already exists, do you want to overwrite it?";
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

        private void btnLoadFilesByPattern_Click(object sender, EventArgs e)
        {
            var pattern = this.txtGlob.Text;
            var dir = System.IO.Path.GetDirectoryName(pattern);
            var filenamePattern = System.IO.Path.GetFileName(pattern);

            var files = Directory.GetFiles(
                dir,
                filenamePattern,
                SearchOption.TopDirectoryOnly).ToList();

            var existingInputPaths = ParseInputLines().Select(i => i.Path);

            files
                .Except(existingInputPaths, StringComparer.OrdinalIgnoreCase)
                .Select(f => new InputFile(f))
                .ForEach(AddLine);
        }

        private void AddLine(InputFile file)
        {
            this.txtInputFiles.Text += $"\r\n{file.Path}";
            if (file.StartPage.HasValue || file.EndPage.HasValue)
            {
                this.txtInputFiles.Text += $"{_pathRangeSeparator}{file.StartPage}:{file.EndPage}";
            }
        }
    }
}