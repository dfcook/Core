using System;

namespace DanielCook.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static U With<T, U>(this T source, Func<T, U> mapper) where T : class
        {
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));

            return source == default(T) ? default(U) : mapper(source);
        }

        public static T Do<T, U>(this T source, Action<T> action) where T : class
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if (source != default(T))
                action(source);

            return source;
        }
    }
}
