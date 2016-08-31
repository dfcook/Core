using System.Data;
using System.Data.SqlClient;

namespace DanielCook.Core.DataAccess
{
    public sealed class TransactionBoundSqlServerQueryObject : TransactionBoundAdoNetQueryObject
    {

        public TransactionBoundSqlServerQueryObject(IDbTransaction transaction, string commandText, int timeout, CommandType commandType) :
            base(transaction, commandText, commandType, timeout)
        {
        }

        public TransactionBoundSqlServerQueryObject(IDbTransaction transaction, string commandText, CommandType commandType)
            : base(transaction, commandText, commandType)
        {
        }

        public override IQueryObject AddTableParameter(string parameterName, DataTable table)
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

        protected override IDbDataParameter CreateParameter() =>
            new SqlParameter();
    }
}


