using System;
using System.Collections.Generic;
using System.Linq;

namespace DanielCook.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<U> Map<T,U>(this IEnumerable<T> source, Func<T, U> mapper)
        {
            return source == null ? new List<U>() : from t in source select mapper(t);        
        }
            
        public static void Each<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source != null)
            {
                foreach (var t in source)
                    action(t);
            }
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
                source.FirstOrDefault(predicate);        
        }

        public static IEnumerable<T> Filter<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            return source == null ?
                new List<T>() :
                source.Where(predicate);
        }
            
        public static IEnumerable<T> Reject<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            return source == null ?
                new List<T>() :
                from t in source where !predicate(t) select t;        
        }
            
        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> source)
        {
            IEnumerable<T> list = new List<T>();
            source.Each(x => list = list.Merge(x));
            return list;
        }

        public static IEnumerable<U> FlatMap<T, U>(this IEnumerable<IEnumerable<T>> source, Func<T,U> projection)
        {
            return source.Flatten().Map(projection);
        }            
    }
}
