using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace DocumentScanner
{
    public class PageDateStatus : IEquatable<PageDateStatus>, IComparer<PageDateStatus>, IComparable<PageDateStatus>
    {
        public DateTime? Date { get; set; }
        private bool _trash;

        public bool IsTrash
        {
            get => _trash;
            set
            {
                _trash = value;
                if (value)
                {
                    Date = null;
                }
            }
        }

        public bool HasDate => Date.HasValue;

        public bool Equals(PageDateStatus other) => CompareTo(other) == 0;

        public override int GetHashCode()
        {
            int hashCode = -342212554;
            hashCode = hashCode * -1521134295 + this.Date.GetHashCode();
            hashCode = hashCode * -1521134295 + this.IsTrash.GetHashCode();
            return hashCode;
        }

        public PageDateStatus()
        {
        }

        public PageDateStatus(DateTime? date, bool isTrash = false)
        {
            Date = date;
            IsTrash = isTrash;
        }

        public static PageDateStatus FromDate(DateTime? date) => new PageDateStatus(date);

        public static implicit operator PageDateStatus(DateTime? value) => FromNullable(value);

        public static PageDateStatus FromNullable(DateTime? date) => new PageDateStatus(date);

        public static implicit operator PageDateStatus(DateTime value) => FromDateTime(value);

        public static PageDateStatus FromDateTime(DateTime date) => FromNullable(date);

        public static PageDateStatus Trash => new PageDateStatus(null, true);

        public static PageDateStatus Undated => new PageDateStatus(null, false);

        public override string ToString() =>
            IsTrash
                ? "Unused"
                : Date.HasValue
                    ? Date.Value.ToString("d", CultureInfo.InvariantCulture.DateTimeFormat)
                    : "Undated";

        public int Compare(PageDateStatus x, PageDateStatus y)
        {
            if (x == null)
            {
                return y == null ? 0 : -1;
            }
            else if (y == null)
            {
                return 1;
            }

            // Trash appears after useful pages
            if (x.IsTrash && !y.IsTrash) return 1;
            if (!x.IsTrash && y.IsTrash) return -1;

            // Undated appears before dated
            int? dateComparison = x.Date?.CompareTo(y.Date);
            if (dateComparison.HasValue) return dateComparison.Value;
            // x.Date == null
            return y.Date.HasValue ? -1 : 0;
        }

        public int CompareTo(PageDateStatus other) => Compare(this, other);

        public override bool Equals(object obj)
        {
            return Equals(obj as PageDateStatus);
        }

        public static bool operator ==(PageDateStatus left, PageDateStatus right) =>
            left is null ? right is null : left.Equals(right);

        public static bool operator !=(PageDateStatus left, PageDateStatus right) => !(left == right);

        public static bool operator <(PageDateStatus left, PageDateStatus right) =>
            left is null ? right is object : left.CompareTo(right) < 0;

        public static bool operator <=(PageDateStatus left, PageDateStatus right) =>
            left is null || left.CompareTo(right) <= 0;

        public static bool operator >(PageDateStatus left, PageDateStatus right) =>
            left is object && left.CompareTo(right) > 0;

        public static bool operator >=(PageDateStatus left, PageDateStatus right) =>
            left is null ? right is null : left.CompareTo(right) >= 0;
    }
}