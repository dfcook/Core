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

        public static T Head<T>(this IEnumerable<T> source) =>
            source == null ? default(T) : source.FirstOrDefault();

        public static IEnumerable<T> Tail<T>(this IEnumerable<T> source) =>
            source == null ? new T[0] : source.Skip(1);

        public static T Reduce<T>(this IEnumerable<T> source, T start, Func<T, T, T> reducer)
        {
            if (reducer == null)
                throw new ArgumentNullException(nameof(reducer));

            return (source != null && source.Any()) ?
                source.Tail().
                    Reduce(reducer(start, source.Head()), reducer) :
                start;
        }

        public static bool DeepEquals<T>(this IEnumerable<T> source, IEnumerable<T> comparison)
        {
            return ((source.Count() == comparison.Count()) &&
                    (source.All(x => comparison.Any(y => y.Equals(x)))));
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

        public static IEnumerable<T> Merge<T>(this IEnumerable<T> source,
            params IEnumerable<T>[] toMerge)
        {
            source = source ?? new T[0];
            toMerge = toMerge ?? new[] { new T[0] };

            foreach (var s in source)
                yield return s;

            foreach (var m in toMerge.Filter(_ => _ != null))
                foreach (var s in m)
                    yield return s;
        }

        public static T Find<T>(this IEnumerable<T> source,
            Func<T, bool> predicate)
        {
            predicate.ThrowIfNull(nameof(predicate));

            return source == null ?
                default(T) :
                source.SingleOrDefault(predicate);
        }


        public static IEnumerable<T> Filter<T>(this IEnumerable<T> source,
            Func<T, bool> predicate)
        {
            predicate.ThrowIfNull(nameof(predicate));

            return source == null ?
                new T[0] :
                source.Where(predicate);
        }

        public static IEnumerable<T> Reject<T>(this IEnumerable<T> source,
            Func<T, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source == null ?
                new T[0] :
                source.Where(x => !predicate(x));
        }

        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> source)
        {
            source = source ?? new[] { new T[0] };

            foreach (var inner in source)
            {
                var lst = inner ?? new T[0];

                foreach (var t in lst)
                    yield return t;
            }
        }

        public static IEnumerable<U> FlatMap<T, U>(this IEnumerable<IEnumerable<T>> source,
            Func<T, U> projection)
        {
            projection.ThrowIfNull(nameof(projection));

            return source.Flatten().Map(projection);
        }
    }
}
