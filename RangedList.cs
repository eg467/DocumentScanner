using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace DocumentScanner
{
    /// <summary>
    /// Stores objects with integer indexes. Retrieves the highest keyed value below a specified index key.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [JsonObject(MemberSerialization.OptIn)]
    public class RangedList<T> : IEnumerable<KeyValuePair<int, T>>
    {
        /// <summary>
        /// The value to use for each index in [0, first set index).
        /// </summary>
        [JsonProperty()]
        private readonly T _defaultValue;

        [JsonProperty()]
        private readonly SortedList<int, T> _values = new SortedList<int, T>();

        /// <summary>
        /// Avoid scenario where [index=0]->1, [2]->2, [1]->1.
        /// The final [1]->1 is ignored since its value is already implicitly 1.
        /// </summary>
        [JsonProperty()]
        private readonly bool _avoidRedundancy;

        /// <summary>
        ///
        /// </summary>
        /// <param name="defaultValue">The value to use for each index in [0, first set index).</param>
        /// <param name="avoidRedundancy">Ignore entries where the preceeding index has the same value.</param>
        public RangedList(T defaultValue, bool avoidRedundancy = true)
        {
            _defaultValue = defaultValue;
            _avoidRedundancy = avoidRedundancy;
        }

        public void Clear() => _values.Clear();

        public void Remove(int index) => _values.Remove(index);

        /// <summary>
        /// Note, these values must be contiguous (e.g. you can't have [0]=1, [1]=2, [2]=1).
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public IDictionary<T, List<(int min, int? max)>> GetRanges(int start, int end)
        {
            var i = start;
            var dic = new Dictionary<T, List<(int, int?)>>();
            void AddRange(T val, (int, int?) range)
            {
                if (!dic.TryGetValue(val, out var list))
                {
                    list = new List<(int, int?)>();
                    dic[val] = list;
                }
                list.Add(range);
            }

            while (i <= end)
            {
                (int min, int? max) = BinaryRangeSearch(i);
                var val = _values[min];
                var realStart = Math.Max(start, min);

                if (!max.HasValue || max.Value >= end)
                {
                    AddRange(val, (realStart, end));
                    break;
                }
                AddRange(val, (realStart, max.Value - 1));
                i = max.Value;
            }
            return dic;
        }

        public T this[int index]
        {
            get
            {
                (int start, int? _) = BinaryRangeSearch(index);
                return start >= 0 ? _values[start] : _defaultValue;
            }
            set
            {
                if (_avoidRedundancy)
                {
                    var comparer = Comparer<T>.Default;
                    if (comparer.Compare(value, this[index]) == 0)
                        return;
                }
                _values[index] = value;
            }
        }

        public IEnumerable<KeyValuePair<int, T>> ExplicitlySetValues => _values;

        /// <summary>
        /// Gets the indexes of the range that contains the current index and its value.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public (int, int?) BinaryRangeSearch(int index)
        {
            var keys = _values.Keys;
            var start = 0;
            var stop = keys.Count - 1;
            while (start <= stop)
            {
                var m = (stop + start) / 2;
                var currentKey = keys[m];
                if (index < currentKey)
                {
                    // To left
                    stop = m - 1;
                }
                else if (m == keys.Count - 1)
                {
                    // Implicit match past end
                    return (currentKey, null);
                }
                else if (index < keys[m + 1])
                {
                    // Match
                    return (currentKey, keys[m + 1]);
                }
                else
                {
                    // To right
                    start = m + 1;
                }
            }
            return (-1, -1);
        }

        public IEnumerator<KeyValuePair<int, T>> GetEnumerator()
        {
            var valEnum = _values.GetEnumerator();
            var currentVal = _defaultValue;
            var start = 0;
            while (valEnum.MoveNext())
            {
                for (var i = start; i < valEnum.Current.Key; i++)
                    yield return new KeyValuePair<int, T>(i, currentVal);
                start = valEnum.Current.Key;
                currentVal = valEnum.Current.Value;
            }
            while (true)
            {
                yield return new KeyValuePair<int, T>(start++, currentVal);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class Range
    {
        public int Min { get; set; }
        public int Max { get; set; }
    }
}