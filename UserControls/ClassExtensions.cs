using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DocumentScanner.UserControls
{
    public static class ClassExtensions
    {
        public static DateTime? GetDate(this DateTimePicker picker) =>
            picker.Checked ? picker.Value : (DateTime?)null;

        public static void SetDate(this DateTimePicker picker, DateTime? date)
        {
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
            start.HasValue ? selector.Increment(start.Value) : (DateTime?)null;

        public static DateTime? Decrement(
            this DateIncrementSelector selector,
            DateTime? start) =>
            start.HasValue ? selector.Decrement(start.Value) : (DateTime?)null;

        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (T item in items)
            {
                action(item);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> items, Action<T, int> action)
        {
            int i = 0;
            foreach (T item in items)
            {
                action(item, i++);
            }
        }
    }
}