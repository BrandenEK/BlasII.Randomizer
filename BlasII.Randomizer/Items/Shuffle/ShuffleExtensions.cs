using System;
using System.Collections.Generic;
using System.Linq;

namespace BlasII.Randomizer.Items.Shuffle
{
    internal static class ShuffleExtensions
    {
        public static (IEnumerable<T> success, IEnumerable<T> fail) Split<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            List<T> left = new(), right = new();

            foreach (var item in source)
            {
                if (predicate(item))
                {
                    left.Add(item);
                }
                else
                {
                    right.Add(item);
                }
            }

            return (left, right);
        }

        public static IEnumerable<T> AddVarious<T>(this IEnumerable<T> source, Func<T, int> func)
        {
            var result = new List<T>();

            foreach (var item in source)
            {
                result.AddRange(Enumerable.Repeat(item, func(item)));
            }

            return result;
        }
    }
}
