using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

namespace DanielCook.Core.DataAccess
{
    public class MySqlQueryObject : AdoNetQueryObject
    {
        public MySqlQueryObject(string connectionString, string commandText, int timeout, CommandType commandType) : 
            base(connectionString, commandText, commandType, timeout)
        {            
        }

        public MySqlQueryObject(string connectionString, string commandText, CommandType commandType)
            : base(connectionString, commandText, commandType)
        {            
        }

        protected override IDbConnection GetConnection()
        {
            return new MySqlConnection();
        }

        protected override IDbDataParameter CreateParameter()
        {
            return new MySqlParameter();
        }

        public override IQueryObject AddTableParameter<T>(string parameterName, System.Data.DataTable table)
        {
            throw new NotImplementedException();
        }
    }
}
