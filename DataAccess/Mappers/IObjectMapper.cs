using System.Collections.Generic;
using System.Data;

namespace DanielCook.Core.DataAccess
{
    public interface IObjectMapper
    {
    }

    public interface IObjectMapper<T> : IObjectMapper
    {
        T Map(IDataReader reader);
        ICollection<T> MapList(IDataReader reader);
    }
}
