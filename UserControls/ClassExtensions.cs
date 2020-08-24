using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DocumentScanner.UserControls
{
    public static class ClassExtensions
    {
        public static DateTime? GetDate(this DateTimePicker picker) =>
            picker?.Checked == true ? picker.Value : (DateTime?)null;

        public static void SetDate(this DateTimePicker picker, DateTime? date)
        {
            if (picker is null)
            {
                throw new ArgumentNullException(nameof(picker));
            }
            if (date.HasValue)
            {
                picker.Value = date.Value;
                picker.Checked = true;
            }
            else
            {
                picker.Checked = false;
            }
        }

        public static DateTime? Increment(
            this DateIncrementSelector selector,
            DateTime? start) =>
            start.HasValue ? (selector?.Increment(start.Value) ?? null) : (DateTime?)null;

        public static DateTime? Decrement(
            this DateIncrementSelector selector,
            DateTime? start) =>
            start.HasValue ? (selector?.Decrement(start.Value) ?? null) : (DateTime?)null;

        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            if (items is null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            foreach (T item in items)
            {
                action?.Invoke(item);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> items, Action<T, int> action)
        {
            if (items is null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            if (action is null) return;
            int i = 0;
            foreach (T item in items)
            {
                action(item, i++);
            }
        }
    }
}