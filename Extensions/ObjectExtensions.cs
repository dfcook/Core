using System;

namespace DanielCook.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static T If<T>(this T source, Func<T, bool> predicate) where T : class
        {
            if (predicate == null)
                throw new ArgumentException(nameof(predicate));

            return (source != default(T) && predicate(source)) ? source : default(T);
        }


        public static U With<T, U>(this T source, Func<T, U> mapper) where T : class
        {
            if (mapper == null)
                throw new ArgumentException(nameof(mapper));

            return source == default(T) ? default(U) : mapper(source);
        }

        public static T Else<T>(this T source, Func<T> mapper) where T : class
        {
            if (mapper == null)
                throw new ArgumentException(nameof(mapper));

            return source == default(T) ? mapper() : source;
        }

        public static T Do<T>(this T source, Action<T> action) where T : class
        {
            if (action == null)
                throw new ArgumentException(nameof(action));

            if (source != default(T))
                action(source);

            return source;
        }

        public static bool CanConvertTo(this object value, Type targetType)
        {
            try
            {
                Convert.ChangeType(value, targetType);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

