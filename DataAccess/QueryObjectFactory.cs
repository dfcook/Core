using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return DatabaseType == DatabaseType.Oracle ?
                (IQueryObject)new OracleQueryObject(ConnectionString, commandText, commandType) :
                (IQueryObject)new SqlServerQueryObject(ConnectionString, commandText, commandType);            
        }        
    }
}
