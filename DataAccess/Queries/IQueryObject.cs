using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DanielCook.Core.DataAccess
{
    public interface IQueryObject
    {
        IQueryObject AddParameter<T>(string parameterName, T value);
        IQueryObject AddInputOutputParameter<T>(string parameterName, T value);
        IQueryObject AddOutputParameter<T>(string parameterName);
        IQueryObject AddOutputParameter<T>(string parameterName, int size);
        IQueryObject AddTableParameter(string parameterName, DataTable table);

        T GetParameterValue<T>(string parameterName);

        IDataReader ExecuteReader();

        IEnumerable<T> ExecuteList<T>(IObjectMapper<T> mapper);

        IEnumerable<IDictionary<string, object>> ExecuteUntypedList();

        IEnumerable<T> ExecuteList<T>() where T : new();

        Task<IEnumerable<T>> ExecuteListAsync<T>(IObjectMapper<T> mapper);

        Task<IEnumerable<T>> ExecuteListAsync<T>() where T : new();

        T ExecuteObject<T>(IObjectMapper<T> mapper);

        T ExecuteObject<T>() where T : new();

        Task<T> ExecuteObjectAsync<T>(IObjectMapper<T> mapper);

        Task<T> ExecuteObjectAsync<T>() where T : new();

        PagedResult<T> ExecutePagedResult<T>() where T : new();

        PagedResult<T> ExecutePagedResult<T>(IObjectMapper<T> mapper);

        int Execute();

        Task<int> ExecuteAsync();

        T ExecuteScalar<T>();

        Task<T> ExecuteScalarAsync<T>();
    }
}
