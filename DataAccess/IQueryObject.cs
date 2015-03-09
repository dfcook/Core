using System;
using System.Collections.Generic;
using System.Data;

namespace DanielCook.Core.DataAccess
{
    public interface IQueryObject
    {
        IQueryObject AddParameter<T>(string parameterName, T value);
        IQueryObject AddInputOutputParameter<T>(string parameterName, T value);
        IQueryObject AddOutputParameter<T>(string parameterName);
        IQueryObject AddTableParameter<T>(string parameterName, DataTable table);

        T GetParameterValue<T>(string parameterName);

        ICollection<T> ExecuteList<T>(IObjectMapper<T> mapper);
        ICollection<T> ExecuteList<T>() where T : new();

        T ExecuteObject<T>(IObjectMapper<T> mapper);
        T ExecuteObject<T>() where T : new();

        int Execute();

        T ExecuteScalar<T>();
    }
}
