using System.Collections.Generic;
using DanielCook.Core.Functional;

namespace DanielCook.Core.DataAccess
{
    public interface IPersistenceManager<T> where T : class, new()
    {
        IEnumerable<T> All();

        bool Any(object dynamicParameters);

        IEnumerable<T> Filter(object dynamicParameters);

        Maybe<T> Find(object dynamicParameters);

        Maybe<T> Get<V>(V key);

        void Delete(T entity);
        void Insert(T entity);
        void Update(T entity);
    }
}