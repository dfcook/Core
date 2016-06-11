using System.Data;
using System.Data.SqlClient;

namespace DanielCook.Core.DataAccess
{
    public class SqlServerQueryObject : AdoNetQueryObject
    {
        public SqlServerQueryObject(string connectionString, string commandText, int timeout, CommandType commandType) :
            base(connectionString, commandText, commandType, timeout)
        {
        }

        public SqlServerQueryObject(string connectionString, string commandText, CommandType commandType)
            : base(connectionString, commandText, commandType)
        {
        }

        public override IQueryObject AddTableParameter<T>(string parameterName, DataTable table)
        {
            var param = CreateParameter() as SqlParameter;
            if (param != null)
            {
                param.Direction = ParameterDirection.Input;
                param.ParameterName = parameterName;
                param.SqlDbType = SqlDbType.Structured;
                param.Value = table;

                Parameters.Add(parameterName, param);
            }

            return this;
        }

        protected override IDbDataParameter CreateParameter()
        {
            return new SqlParameter();
        }

        protected override IDbConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
