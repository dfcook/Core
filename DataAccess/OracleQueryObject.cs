using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanielCook.Core.DataAccess
{
    public class OracleQueryObject : AdoNetQueryObject
    {
        public OracleQueryObject(string connectionString, string commandText, int timeout, CommandType commandType) : 
            base(connectionString, commandText, commandType, timeout)
        {            
        }

        public OracleQueryObject(string connectionString, string commandText, CommandType commandType)
            : base(connectionString, commandText, commandType)
        {            
        }

        protected override IDbConnection GetConnection()
        {
            return new OracleConnection();
        }

        protected override IDbDataParameter CreateParameter()
        {
            return new OracleParameter();
        }

        public override IQueryObject AddTableParameter<T>(string parameterName, System.Data.DataTable table)
        {
            throw new NotImplementedException();
        }
    }
}
