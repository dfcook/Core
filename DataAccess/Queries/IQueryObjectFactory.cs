using System;
using System.Data;

namespace DanielCook.Core.DataAccess
{
    public enum QueryType
    {
        StoredProcedure,
        Adhoc
    }

    public enum DatabaseType
    {
        SqlServer,
        Oracle,
        MySql
    }

    public interface IQueryObjectFactory
    {
        IQueryObject GetQuery(string commandText);
        void InTransaction(Action action);
        IDbTransaction BeginTransaction();
    }
}
