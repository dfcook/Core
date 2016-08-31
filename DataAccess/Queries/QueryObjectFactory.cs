using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DanielCook.Core.Extensions;

namespace DanielCook.Core.DataAccess
{
    public class QueryObjectFactory : IQueryObjectFactory, IDisposable
    {
        private string ConnectionString { get; set; }
        private QueryType QueryType { get; set; }
        private DatabaseType DatabaseType { get; set; }

        private Stack<Transaction> Transactions { get; set; } = new Stack<Transaction>();

        private Transaction CurrentTransaction => Transactions.Any() ? Transactions.Peek() : null;

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
                case DatabaseType.SqlServer:
                    return CurrentTransaction != null ?
                        (IQueryObject)new TransactionBoundSqlServerQueryObject(CurrentTransaction.InnerTransaction,
                        commandText, commandType) :
                        new SqlServerQueryObject(ConnectionString, commandText, commandType);

                default:
                    throw new ArgumentException($"Unknown DatabaseType: {DatabaseType}");
            }
        }

        private IDbConnection GetConnection()
        {
            switch (DatabaseType)
            {
                case DatabaseType.SqlServer:
                    return new SqlConnection(ConnectionString);

                default:
                    throw new ArgumentException($"Unknown DatabaseType: {DatabaseType}");
            }
        }

        public IDbTransaction BeginTransaction()
        {
            var connection = GetConnection();
            connection.Open();
            var transaction = new Transaction(connection.BeginTransaction());
            transaction.TransactionStatusChanged += (s, e) => Transactions.Pop();
            Transactions.Push(transaction);

            return transaction;
        }

        public void InTransaction(Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            Disposable.Using(() => BeginTransaction(),
                tx =>
                {
                    try
                    {
                        action();
                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                });
        }

        #region IDisposable Support
        private bool disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (CurrentTransaction != null)
                    {
                        CurrentTransaction.Dispose();
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
