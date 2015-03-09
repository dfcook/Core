using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanielCook.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static U With<T, U>(this T source, Func<T, U> mapper) where T : class
        {
            return source == default(T) ? default(U) : mapper(source);
        }

        public static T Do<T, U>(this T source, Action<T> action) where T : class
        {
            if (source != default(T))
                action(source);

            return source;
        }
    }
}
