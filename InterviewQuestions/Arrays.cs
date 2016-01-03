using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewQuestions
{
    public enum SortOrder
    {
        Ascending,
        Descending
    }

    public static class EnumerableQuestions
    {
        public static void Main(string[] args)
        {

        }

        public static T MostFrequent<T>(this IEnumerable<T> source)
        {
            if (source == null || !source.Any())
                return default(T);

            return source.
                GroupBy(x => x).
                Select(x => new
                    {
                        Value = x.Key,
                        Count = x.Count()
                    }).
                OrderByDescending(x => x.Count).
                First().
                Value;
        }

        public static IEnumerable<T> BubbleSort<T>(this IEnumerable<T> source, 
            SortOrder order = SortOrder.Ascending)
        {
            var sorting = true;
            var sorted = new List<T>(source);
            var comparer = Comparer<T>.Default;

            while (sorting)
            {
                sorting = false;

                for (var i = 0; i < sorted.Count - 1; i++)
                {
                    var cmp = comparer.Compare(sorted[i], sorted[i + 1]);
                    var swap = order == SortOrder.Ascending ? cmp > 0 : cmp < 0;   

                    if (swap)
                    {
                        sorted.Swap(i, i + 1);
                        sorting = true;
                    }
                }
            }

            return sorted;
        }
        
        public static void Swap<T>(this IList<T> source, int from, int to)
        {
            T tmp = source[from];
            source[from] = source[to];
            source[to] = tmp;
        }

        private static int BinarySearch<T>(this IList<T> source, T toFind, int start, 
            int end, IComparer<T> comparer)
        {            
            var idx = (int)Math.Ceiling(((double)end - (double)start) / 2) + start;
            var cmp = comparer.Compare(source[idx], toFind);

            if (cmp == 0)
                return idx;
            else if (cmp > 0)
                return source.BinarySearch(toFind, start, idx, comparer);
            else
                return source.BinarySearch(toFind, idx, end, comparer);                
        }

        public static int BinarySearch<T>(this IList<T> source, T toFind)
        {
            var comparer = Comparer<T>.Default;
            return source.BinarySearch(toFind, 0, source.Count - 1, comparer);
        }        
    }
}
