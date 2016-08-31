using System;

namespace DanielCook.Core.Functional
{
    public static class ResultExtensions
    {
        public static Result<T> ToResult<T>(this Maybe<T> maybe, string errorMessage) where T : class =>
            maybe.Match((val) => Result.Ok(val), () => Result.Fail<T>(errorMessage));

        public static Result OnSuccess(this Result result, Action action)
        {
            if (result.IsFailure)
                return result;

            action?.Invoke();

            return Result.Ok();
        }

        public static Result OnSuccess(this Result result, Func<Result> func) =>
            result.IsFailure ? result : func?.Invoke();

        public static Result OnFailure(this Result result, Action action)
        {
            if (result.IsFailure)
            {
                action?.Invoke();
            }

            return result;
        }

        public static Result OnBoth(this Result result, Action<Result> action)
        {
            action?.Invoke(result);

            return result;
        }

        public static T OnBoth<T>(this Result result, Func<Result, T> func) where T : class =>
            func?.Invoke(result) ?? default(T);
    }
}
