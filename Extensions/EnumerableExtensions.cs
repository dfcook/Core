using System;
using System.Collections.Generic;
using System.Linq;

namespace DanielCook.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<U> Map<T, U>(this IEnumerable<T> source, Func<T, U> mapper)
        {
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));

            source = source ?? new T[0];

            foreach (var t in source)
                yield return mapper(t);
        }

        public static IEnumerable<T> Each<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if (source != null)
            {
                foreach (var t in source)
                    action(t);
            }

            return source;
        }

        public static IEnumerable<T> Merge<T>(this IEnumerable<T> source, IEnumerable<T> toMerge)
        {
            if (source == null || toMerge == null || !toMerge.Any())
                return source;

            var list = new List<T>(source);
            list.AddRange(toMerge);
            return list;
        }

        public static T Find<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            return source == null ?
                default(T) :
                source.Single(predicate);
        }

        public static IEnumerable<T> Filter<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            return source == null ?
                new T[0] :
                source.Where(predicate);
        }

        public static IEnumerable<T> Reject<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source == null ?
                new T[0] :
                source.Where(x => !predicate(x));
        }

        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> source)
        {
            IEnumerable<T> list = new List<T>();
            source.Each(x => list = list.Merge(x));
            return list;
        }

        public static IEnumerable<U> FlatMap<T, U>(this IEnumerable<IEnumerable<T>> source, Func<T, U> projection)
        {
            return source.Flatten().Map(projection);
        }
    }
}
