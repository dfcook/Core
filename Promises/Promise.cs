using System;
using System.Threading.Tasks;

namespace Promises
{
    /// <summary>
    /// Implementation of a Promise, common in the Javascript world.
    ///
    /// Allows asynchronous functions to be chained together using Then, or something as simple as
    /// setting a datasource property once db excecution has completed.
    /// </summary>
    /// <typeparam name="T">The type returned by the function being executed</typeparam>
    public class Promise<T> : IPromise
    {
        private TaskCompletionSource<T> Source { get; set; }
        private Task<T> InnerTask { get; set; }
        private IPromise Parent { get; set; }

        public Exception Exception { get; set; }

        public Promise(Func<T> func)
        {
            InnerTask = RunTaskAsync(func);
        }

        private Promise(IPromise parent, Task<T> task)
        {
            Parent = parent;
            InnerTask = RunTaskAsync(task);
        }

        private Task<T> RunTaskAsync(Func<T> func)
        {
            return RunTaskAsync(Task.Factory.StartNew(func));
        }

        private Task<T> RunTaskAsync(Task<T> task)
        {
            Source = new TaskCompletionSource<T>();

            task.ContinueWith(x =>
            {
                if (x.IsFaulted)
                {
                    Exception = x.Exception.GetBaseException();
                    Source.TrySetException(Exception);
                }
                else if (x.IsCanceled)
                {
                    if (Parent != null && Parent.Exception != null)
                    {
                        Exception = Parent.Exception;
                        Source.TrySetException(Exception);
                    }
                    else
                    {
                        Source.TrySetCanceled();
                    }
                }
                else
                {
                    Source.TrySetResult(x.Result);
                }
            }, TaskContinuationOptions.ExecuteSynchronously);

            return Source.Task;
        }

        private T GetResult()
        {
            try
            {
                InnerTask.Wait();
                return InnerTask.Result;
            }
            catch (Exception e)
            {
                throw e.GetBaseException();
            }
        }

        public Promise<T> Then(Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            InnerTask.
                ContinueWith(x => action(x.Result),
                TaskContinuationOptions.OnlyOnRanToCompletion);

            return this;
        }

        public Promise<U> Then<U>(Func<T, U> mapper)
        {
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));

            var task = InnerTask.
                ContinueWith(x => mapper(x.Result),
                TaskContinuationOptions.OnlyOnRanToCompletion);

            return new Promise<U>(this, task);
        }

        public static implicit operator T(Promise<T> promise)
        {
            return promise.GetResult();
        }
    }
}
