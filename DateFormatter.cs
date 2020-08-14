using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace DocumentScanner
{
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class DateFormatter
    {
        public event EventHandler FormatChanged;

        [JsonProperty]
        private readonly string[] _formats;

        public IEnumerable<string> Formats => _formats;

        public DateFormatter() : this(new string[]
            {
                "MM/dd/yyyy",
                "MMM dd yyyy",
                "MMMM dd, yyyy",
            })
        {
        }

        public DateFormatter(params string[] formats)
        {
            if (formats == null || formats.Length == 0)
            {
                throw new ArgumentException(
                    "You must provide some date format strings.",
                    nameof(formats));
            }

            _formats = formats;
        }

        [JsonProperty]
        private int _currentIndex = 0;

        public int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                if (value == _currentIndex)
                    return;

                if (value < 0 || value >= _formats.Length)
                    throw new ArgumentOutOfRangeException(
                        nameof(CurrentIndex),
                        value,
                        $"Index must be between 0 and {_formats.Length - 1}");
                _currentIndex = value;
                FormatChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Toggle()
        {
            CurrentIndex = (CurrentIndex + 1) % _formats.Length;
        }

        public string CurrentFormat => _formats[CurrentIndex];

        public string Format(DateTime? date) =>
            date.HasValue
            ? date.Value.ToString(CurrentFormat)
            : "undated";
    }
}