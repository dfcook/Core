using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DanielCook.Core.Functional
{
    public static class Maybe
    {
        public static Maybe<T> ToMaybe<T>(this T value) where T : class
        {
            if (value == null)
                return None<T>();
            return Some(value);
        }

        public static Maybe<T> ToMaybe<T>(this T? value) where T : struct
        {
            if (!value.HasValue)
                return None<T>();
            return Some(value.Value);
        }

        private static Maybe<T> Some<T>(T value) => new Maybe<T>(value);
        public static Maybe<T> None<T>() => new Maybe<T>();
    }

    public struct Maybe<T>
    {
        public static readonly Maybe<T> None = new Maybe<T>();

        [JsonProperty("HasValue")]
        private readonly bool _hasValue;
        [JsonProperty("Value")]
        private readonly T _value;

        internal Maybe(T value)
        {
            _value = value;
            _hasValue = true;
        }

        public U Match<U>(Func<T, U> some = null, Func<U> none = null)
        {
            if (_hasValue)
                if (some != null)
                    return some(_value);

            if (none != null)
                return none();

            return default(U);
        }

        public void Match(Action<T> some = null, Action none = null)
        {
            if (_hasValue)
                some?.Invoke(_value);

            none?.Invoke();
        }

        public Task<U> MatchAsync<U>(Func<T, Task<U>> some = null, Func<Task<U>> none = null)
        {
            if (_hasValue)
                if (some != null)
                    return some(_value);

            if (none != null)
                return none();

            return Task.FromResult(default(U));
        }
    }
}
