using System.Collections.Generic;

namespace DanielCook.Core.DataAccess
{
    public interface IPersistenceManager<T> where T : new()
    {
        IEnumerable<T> All();

        bool Any(object dynamicParameters);

        IEnumerable<T> Filter(object dynamicParameters);

        T Find(object dynamicParameters);

        T Get<V>(V key);

        void Delete(T entity);
        void Insert(T entity);
        void Update(T entity);
    }
}