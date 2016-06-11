using System.Collections;
using System.Collections.Generic;


namespace DanielCook.Core.Options
{
    public class Option<T> : IEnumerable<T>
    {
        private readonly T[] _data;

        private Option(T element) : this(new T[] { element })
        {
        }

        private Option() : this(new T[0])
        {
        }

        private Option(T[] data)
        {
            _data = data;
        }

        public static Option<T> Some(T data)
        {
            return new Option<T>(data);
        }

        public static Option<T> None()
        {
            return new Option<T>();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_data).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
