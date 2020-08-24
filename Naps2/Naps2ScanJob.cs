using DocumentScanner.NapsOptions;
using DocumentScanner.NapsOptions.Keys;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

using Keys = DocumentScanner.NapsOptions.Keys;

namespace DocumentScanner
{
    public class ScanResults
    {
        public string Output { get; }
        public string Error { get; }
        public int ExitCode { get; }
        public bool Success => ExitCode == 0;

        public IEnumerable<string> OutputFiles
        {
            get
            {
                var filePattern = @"^(?:Finished saving images to ([^$]+)|Successfully saved \w+ file to ([^$]+))$";
                var matches = Regex.Matches(
                    Output ?? "",
                    filePattern,
                    RegexOptions.IgnoreCase | RegexOptions.Multiline);

                return matches
                    .Cast<Match>()
                    .Select(m =>
                    {
                        var savedPath = m.Groups[1].Success ? m.Groups[1].Value : m.Groups[2].Value;
                        return savedPath.Trim('\r', '\n');
                    });
            }
        }

        public ScanResults(Process process, bool redirected)
        {
            if (process is null)
            {
                throw new ArgumentNullException(nameof(process));
            }

            Output = redirected ? process.StandardOutput.ReadToEnd() : "";
            Error = redirected ? process.StandardError.ReadToEnd() : "";
            ExitCode = process.ExitCode;
        }

        public override string ToString() => Output;
    }

    public class ScanFactory
    {
        public string Path { get; }

        public ScanFactory(string path)
        {
            this.Path = path;
        }

        public Naps2ScanJob Create()
        {
            return new Naps2ScanJob(Path);
        }
    }

    public enum TiffCompressionType
    {
        Auto,
        Lzw,
        Ccitt4,
        None
    }

    public class Naps2ScanJob
    {
        public string NapsConsolePath { get; set; }

        public NapsOptionCollection Args { get; } = new NapsOptionCollection();

        private bool _redirectOutput = true;

        public Naps2ScanJob(string path)
        {
            NapsConsolePath = path;
        }

        public ScanResults Execute()
        {
            Args.Validate();
            var CliArgs = Args.ToString();
            Debug.WriteLine($"Running NAPS2 with process arguments: {CliArgs}");
            var startInfo = new ProcessStartInfo(NapsConsolePath, CliArgs)
            {
                WindowStyle = _redirectOutput ? ProcessWindowStyle.Hidden : ProcessWindowStyle.Normal,
                CreateNoWindow = _redirectOutput,
                UseShellExecute = !_redirectOutput,
                RedirectStandardError = _redirectOutput,
                RedirectStandardOutput = _redirectOutput
            };

            var process = Process.Start(startInfo);
            process.WaitForExit();
            var results = new ScanResults(process, _redirectOutput);
            return results;
        }

        private static bool IsPathValid(string path)
        {
            if (string.IsNullOrEmpty(path)) return false;
            // https://stackoverflow.com/questions/422090/in-c-sharp-check-that-filename-is-possibly-valid-not-that-it-exists
            FileInfo fi = null;
            try
            {
                fi = new FileInfo(path);
            }
            catch (ArgumentException) { }
            catch (PathTooLongException) { }
            catch (NotSupportedException) { }
            return !ReferenceEquals(fi, null);
        }

        public Naps2ScanJob ShowOutput()
        {
            _redirectOutput = false;
            return this;
        }

        public Naps2ScanJob OutputPath(string path)
        {
            if (!IsPathValid(path))
            {
                throw new ArgumentException("The specified path is not valid.", nameof(path));
            }
            return AddStringOption(Keys.Main.Output, path);
        }

        public Naps2ScanJob AddInputFiles(IEnumerable<string> paths)
        {
            return AddInputFiles(paths.Select(p => new InputFile(p)));
        }

        public Naps2ScanJob AddInputFiles(params InputFile[] paths)
        {
            return AddInputFiles(paths as IEnumerable<InputFile>);
        }

        public Naps2ScanJob AddInputFiles(IEnumerable<InputFile> paths)
        {
            if (Args.TryGetValue(Keys.Main.Import, out var option))
            {
                (option as ImportOption).AddFiles(paths);
            }
            else
            {
                option = new ImportOption(paths);
                Args.Add(option);
            }
            return this;
        }

        public Naps2ScanJob SetEmailSettings(EmailSettings settings)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var options = new List<INapsOption>
            {
                new StringOption(EmailFlags.To, settings.To),
                new StringOption(EmailFlags.Cc, settings.Cc),
                new StringOption(EmailFlags.Bcc, settings.Bcc),
                new StringOption(EmailFlags.Subject, settings.Subject)
            };

            if (settings.AutoSend)
            {
                options.Add(new BooleanOption(EmailFlags.AutoSend));
            }
            if (settings.SilentSend)
            {
                options.Add(new BooleanOption(EmailFlags.SilentSend));
            }

            options.ForEach(Args.Add);
            return this;
        }

        public Naps2ScanJob PdfSettings(PdfSettings settings)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            // Metadata
            var customMeta = settings.UsesSavedMetadata;
            AddBooleanOption(PdfFlags.UseSavedMetadata, customMeta);
            AddStringOption(PdfFlags.Title, settings.Title, !customMeta);
            AddStringOption(PdfFlags.Author, settings.Author, !customMeta);
            AddStringOption(PdfFlags.Subject, settings.Subject, !customMeta);
            AddStringOption(PdfFlags.Keywords, settings.Keywords, !customMeta);

            // Encryption
            AddBooleanOption(PdfFlags.UseSavedEncryptConfig, settings.UseSavedEncryptConfig);
            AddStringOption(
                PdfFlags.EncryptConfig,
                settings.EncryptConfig,
                enabled: !string.IsNullOrWhiteSpace(settings.EncryptConfig));

            return this;
        }

        //public Naps2ScanJob InstallOcr(string language = "ocr-eng")
        //{
        //    if (!language.StartsWith("ocr-"))
        //    {
        //        throw new ArgumentException(nameof(language), "Language module must be of the form 'ocr-eng'.");
        //    }
        //    return AddStringOption(Main.Install, language);
        //}

        public Naps2ScanJob Install(string module)
        {
            return AddStringOption(Main.Install, module);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <param name="language">Ensure language is installed manually with NAPS GUI or via <see cref="InstallOcr(string)"/></param>
        /// <returns></returns>
        public Naps2ScanJob ToggleOcr(bool value, string language = "eng")
        {
            var disable = new BooleanOption(OcrFlags.OcrDisable);
            // Implies enabled
            var lang = new StringOption(OcrFlags.OcrLang, language);

            Args.Toggle(lang, value);
            Args.Toggle(disable, !value);
            return this;
        }

        public Naps2ScanJob JpegQuality(int quality = 75)
        {
            const int min = 0;
            const int max = 100;
            if (quality < min || quality > max)
            {
                throw new ArgumentOutOfRangeException(nameof(quality), $"Value must be from {min}-{max}, but was {quality}");
            }
            return AddStringOption(
                Quality.JpegQuality,
                quality.ToString(CultureInfo.InvariantCulture),
                false);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "<Pending>")]
        public Naps2ScanJob TiffCompression(TiffCompressionType method)
        {
            var methodArg = method.ToString().ToLowerInvariant();
            return AddStringOption(Quality.TiffCompression, methodArg, false);
        }

        public Naps2ScanJob WaitForUserInput()
        {
            return AddBooleanOption(Main.Wait);
        }

        public Naps2ScanJob Interleave()
        {
            return AddBooleanOption(Order.Interleave);
        }

        public Naps2ScanJob Reverse()
        {
            return AddBooleanOption(Order.Reverse);
        }

        /// <summary>
        /// The profile name as set in the NAPS2 GUI, if unset, uses the last used profile in the GUI.
        /// </summary>
        /// <param name="profileName"></param>
        /// <returns></returns>
        public Naps2ScanJob Profile(string profileName)
        {
            return AddStringOption(
                Main.Profile,
                profileName,
                enabled: !string.IsNullOrEmpty(profileName));
        }

        public Naps2ScanJob NumScans(int n)
        {
            if (n < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(n), "Value must be non-negative.");
            }

            return AddStringOption(Main.NumScans, n.ToString(CultureInfo.InvariantCulture), false);
        }

        public Naps2ScanJob Verbose()
        {
            return AddBooleanOption(Main.Verbose);
        }

        public Naps2ScanJob Autosave()
        {
            return AddBooleanOption(Main.Autosave);
        }

        public Naps2ScanJob ForceOverwrite()
        {
            return AddBooleanOption(Main.ForceOverwrite);
        }

        private Naps2ScanJob AddBooleanOption(string key, bool enabled = true)
        {
            var option = new BooleanOption(key);
            return AddOption(option, enabled);
        }

        private Naps2ScanJob AddStringOption(
            string key,
            string value,
            bool quotes = true,
            bool enabled = true)
        {
            var option = new StringOption(key, value, quotes);
            return AddOption(option, enabled);
        }

        private Naps2ScanJob AddOption(INapsOption option, bool enabled = true)
        {
            Args.Toggle(option, enabled);
            return this;
        }
    }
}