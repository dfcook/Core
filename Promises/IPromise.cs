using System;

namespace Promises
{
    public interface IPromise
    {
        Exception Exception { get; set; }
    }
}
