using DocumentScanner.NapsOptions;
using DocumentScanner.NapsOptions.Keys;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

        public ScanResults(Process process)
        {
            Output = process.StandardOutput.ReadToEnd();
            Error = process.StandardError.ReadToEnd();
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

        public readonly NapsOptionCollection Args = new NapsOptionCollection();

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
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = false,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            var process = Process.Start(startInfo);
            var results = new ScanResults(process);
            return results;
        }

        private static bool IsPathValid(string path)
        {
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

        public Naps2ScanJob OutputPath(string path)
        {
            if (!IsPathValid(path))
            {
                throw new ArgumentException(
                    nameof(path),
                    "The specified path is not valid.");
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
            var options = new List<INapsOption>
            {
                new StringOption(Email.To, settings.To),
                new StringOption(Email.Cc, settings.Cc),
                new StringOption(Email.Bcc, settings.Bcc),
                new StringOption(Email.Subject, settings.Subject)
            };

            if (settings.AutoSend)
            {
                options.Add(new BooleanOption(Email.AutoSend));
            }
            if (settings.SilentSend)
            {
                options.Add(new BooleanOption(Email.SilentSend));
            }

            options.ForEach(Args.Add);
            return this;
        }

        public Naps2ScanJob PdfSettings(PdfSettings settings)
        {
            // Metadata
            var customMeta = settings.UsesSavedMetadata;
            AddBooleanOption(Pdf.UseSavedMetadata, customMeta);
            AddStringOption(Pdf.Title, settings.Title, !customMeta);
            AddStringOption(Pdf.Author, settings.Author, !customMeta);
            AddStringOption(Pdf.Subject, settings.Subject, !customMeta);
            AddStringOption(Pdf.Keywords, settings.Keywords, !customMeta);

            // Encryption
            AddBooleanOption(Pdf.UseSavedEncryptConfig, settings.UseSavedEncryptConfig);
            AddStringOption(
                Pdf.EncryptConfig,
                settings.EncryptConfig,
                enabled: !string.IsNullOrWhiteSpace(settings.EncryptConfig));

            return this;
        }

        public Naps2ScanJob InstallOcr(string language = "ocr-eng")
        {
            if (!language.StartsWith("ocr-"))
            {
                throw new ArgumentException(nameof(language), "Language module must be of the form 'ocr-eng'.");
            }
            return AddStringOption(Main.Install, language);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <param name="language">Ensure language is installed manually with NAPS GUI or via <see cref="InstallOcr(string)"/></param>
        /// <returns></returns>
        public Naps2ScanJob ToggleOcr(bool value, string language = "eng")
        {
            var disable = new BooleanOption(Ocr.OcrDisable);
            // Implies enabled
            var lang = new StringOption(Ocr.OcrLang, language);

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
            return AddStringOption(Quality.JpegQuality, quality.ToString(), false);
        }

        public Naps2ScanJob TiffCompression(TiffCompressionType method)
        {
            var methodArg = method.ToString().ToLower();
            return AddStringOption(Quality.TiffCompression, methodArg, false);
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

            return AddStringOption(Main.NumScans, n.ToString(), false);
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