using System.Collections.Generic;

namespace DanielCook.Core.DataAccess
{
    public class PagedResult<T>
    {
        public IEnumerable<T> PagedData { get; }
        public int TotalCount { get; }

        public PagedResult(IEnumerable<T> pagedData, int totalCount)
        {
            PagedData = pagedData;
            TotalCount = totalCount;
        }
    }
}
