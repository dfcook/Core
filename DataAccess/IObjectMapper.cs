using System.Collections.Generic;
using System.Data;

namespace DanielCook.Core.DataAccess
{
    public interface IObjectMapper<T>
    {
        T Map(IDataRecord record);
        ICollection<T> MapList(IDataReader reader);
    }
}
