using DocumentScanner.NapsOptions.Keys;
using iText.Kernel.XMP.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// See https://www.naps2.com/doc-command-line.html
/// </summary>
namespace DocumentScanner.NapsOptions
{
    public interface INapsOption
    {
        string Key { get; }

        string ToString();
    }

    public class StringOption : INapsOption
    {
        private static string Quoted(string arg) => $"\"{arg.Replace("\"", "\\\"")}\"";

        public string Key { get; }
        public bool IsQuoted { get; }
        public HashSet<string> AllowedValues { get; }
        public string Value { get; set; }

        public StringOption(
            string key,
            string value = "",
            bool isQuoted = true)
        {
            this.Key = key;
            this.Value = value;
            this.IsQuoted = isQuoted;
        }

        public override string ToString() =>
            $"{Key} {(IsQuoted ? Quoted(Value) : Value)}";
    }

    public class BooleanOption : INapsOption
    {
        public string Key { get; }

        public BooleanOption(string key)
        {
            Key = key;
        }

        public override string ToString() => Key;
    }

    public class ImportOption : INapsOption
    {
        public string Key => Keys.Main.Import;
        public IEnumerable<InputFile> Files => _files;
        private List<InputFile> _files = new List<InputFile>();

        public ImportOption(IEnumerable<InputFile> files = null)
        {
            if (files != null)
            {
                AddFiles(files);
            }
        }

        public void AddFiles(IEnumerable<InputFile> files)
        {
            _files = _files.Concat(files).ToList();
        }

        public void RemoveDuplicates()
        {
            _files = _files.Distinct().ToList();
        }

        public override string ToString()
        {
            var filePaths = _files.Select(f => f.ToString());
            var value = string.Join(";", filePaths);
            return new StringOption(Key, value).ToString();
        }
    }

    public class InputFile : IEquatable<InputFile>
    {
        public string Path { get; }
        public int? StartPage { get; }
        public int? EndPage { get; }
        private static readonly string[] SliceableExtensions = new[] { ".tif", ".tiff", ".pdf" };
        private bool IsSliced => StartPage.HasValue || EndPage.HasValue;

        public InputFile(string path, int? startPage = null, int? endPage = null)
        {
            Path = path;
            StartPage = startPage;
            EndPage = endPage;

            var extension = System.IO.Path.GetExtension(Path);
            var canSlice = SliceableExtensions.Contains(extension);
            if ((this.StartPage.HasValue || this.EndPage.HasValue) && !canSlice)
            {
                throw new ArgumentException($"You can only slice the following file types: {string.Join(", ", SliceableExtensions)}.");
            }
        }

        public static explicit operator InputFile(string filePath) => FromString(filePath);

        public static InputFile FromString(string filePath) =>
            new InputFile(filePath, null, null);

        public override bool Equals(object obj) => Equals(obj as InputFile);

        public override string ToString() =>

            IsSliced
            ? $"{Path}[{StartPage}:{EndPage + 1}]"
            : Path;

        public bool Equals(InputFile other)
        {
            return other != null &&
                   this.Path == other.Path &&
                   this.StartPage == other.StartPage &&
                   this.EndPage == other.EndPage;
        }

        public override int GetHashCode()
        {
            int hashCode = 6337355;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.Path);
            hashCode = hashCode * -1521134295 + this.StartPage.GetHashCode();
            hashCode = hashCode * -1521134295 + this.EndPage.GetHashCode();
            return hashCode;
        }
    }

    public class EmailSettings
    {
        public string To { get; set; } = "";
        public string Cc { get; set; } = "";
        public string Bcc { get; set; } = "";
        public string Subject { get; set; } = "";
        public string Body { get; set; } = "";
        public bool AutoSend { get; set; }
        public bool SilentSend { get; set; }
    }

    public class PdfSettings
    {
        public string Title { get; set; } = "";
        public string Author { get; set; } = "";

        public string Subject { get; set; } = "";

        /// <summary>
        /// Comma-delimited list of keywords
        /// </summary>
        public string Keywords { get; set; } = "";

        /// <summary>
        ///  Specifies that the metadata configured in the GUI should be used for generated PDF.
        /// </summary>
        public bool UsesSavedMetadata { get; set; }

        /// <summary>
        ///  Specifies that the encryption configured in the GUI should be used for the generated PDF.
        /// </summary>
        public bool UseSavedEncryptConfig { get; set; }

        /// <summary>
        ///  Specifies the name and path of an XML file to configure encryption for the generated PDF.
        ///
        /// </summary>
        public string EncryptConfig { get; set; }
    }

    public class NapsOptionCollection
    {
        private readonly List<INapsOption> _options = new List<INapsOption>();

        private int FindKeyIndex(string key) => _options.FindIndex(m => m.Key == key);

        public void Validate()
        {
            var requiredFields = new[]
            {
                Main.Output, Main.Email, Main.Autosave
            };

            if (!requiredFields.Any(this.Contains))
            {
                throw new InvalidOperationException(
                    $"At least one of the following options must be specified:  {string.Join(",", requiredFields)}");
            }
        }

        public void Clear()
        {
            _options.Clear();
        }

        public void Toggle(INapsOption option, bool value)
        {
            Remove(option);
            if (value)
            {
                Add(option);
            }
        }

        public bool Contains(string key) => FindKeyIndex(key) >= 0;

        public INapsOption this[string key]
        {
            get
            {
                var idx = FindKeyIndex(key);
                return idx >= 0 ? _options[idx] : null;
            }
            set
            {
                var existingIdx = FindKeyIndex(key);
                if (existingIdx >= 0)
                {
                    _options[existingIdx] = value;
                }
                else
                {
                    _options.Add(value);
                }
            }
        }

        public bool TryGetValue(string key, out INapsOption option)
        {
            var idx = FindKeyIndex(key);
            if (idx == -1)
            {
                option = null;
                return false;
            }
            option = _options[idx];
            return true;
        }

        public void Add(INapsOption option)
        {
            if (option is null)
            {
                throw new ArgumentNullException(nameof(option));
            }
            this[option.Key] = option;
        }

        public bool Remove(string key)
        {
            var idx = FindKeyIndex(key);
            if (idx == -1)
            {
                return false;
            }
            _options.RemoveAt(idx);
            return true;
        }

        public bool Remove(INapsOption option)
        {
            if (option is null)
            {
                throw new ArgumentNullException(nameof(option));
            }

            return Remove(option.Key);
        }

        public override string ToString() =>
            string.Join(" ", _options.Select(o => o.ToString()));
    }
}