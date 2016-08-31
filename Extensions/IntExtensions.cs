using System;
using System.Collections.Generic;
using System.Linq;

namespace DanielCook.Core.Extensions
{
    public static class IntExtensions
    {
        public static IEnumerable<int> To(this int from, int to) =>
            from > to ?
                Enumerable.Range(to, from - to - 1).Reverse() :
                Enumerable.Range(from, to - from + 1);

        public static void Times(this int source, Action<int> action) =>
            1.To(source).Each(action);

        public static IEnumerable<T> Times<T>(this int source, Func<int, T> mapper)  =>
            1.To(source).Map(mapper);
    }
}
