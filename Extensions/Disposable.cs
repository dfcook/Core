using System;

namespace DanielCook.Core.Extensions
{
    public class Disposable
    {
        public static void Using<T>(Func<T> creator, Action<T> action) where T : IDisposable
        {
            if (creator == null)
                throw new ArgumentNullException(nameof(creator));

            if (action == null)
                throw new ArgumentNullException(nameof(action));

            using (var obj = creator())
            {
                action(obj);
            }
        }
    }
}
