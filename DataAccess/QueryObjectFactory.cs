using System;
using System.Data;

namespace DanielCook.Core.DataAccess
{
    public class QueryObjectFactory : IQueryObjectFactory
    {
        private string ConnectionString { get; set; }
        private QueryType QueryType { get; set; }
        private DatabaseType DatabaseType { get; set; }

        public QueryObjectFactory(string connectionString, QueryType queryType, DatabaseType databaseType)
        {
            ConnectionString = connectionString;
            DatabaseType = databaseType;
            QueryType = queryType;
        }

        public IQueryObject GetQuery(string commandText)
        {
            var commandType = QueryType == QueryType.Adhoc ? CommandType.Text : CommandType.StoredProcedure;

            switch (DatabaseType)
            {
                case DatabaseType.Oracle:
                    return new OracleQueryObject(ConnectionString, commandText, commandType);

                case DatabaseType.SqlServer:
                    return new SqlServerQueryObject(ConnectionString, commandText, commandType);

                case DatabaseType.MySql:
                    return new MySqlQueryObject(ConnectionString, commandText, commandType);

                default:
                    throw new ArgumentException($"Unknown DatabaseType: {DatabaseType}");
            }
        }
    }
}
