using System;
using System.Data;

namespace DanielCook.Core.DataAccess
{
    public sealed class Transaction : IDbTransaction
    {
        public event EventHandler TransactionStatusChanged;

        private bool _active = true;

        public IDbTransaction InnerTransaction { get; }

        public Transaction(IDbTransaction inner)
        {
            InnerTransaction = inner;
        }

        public IDbConnection Connection => InnerTransaction.Connection;

        public IsolationLevel IsolationLevel => InnerTransaction.IsolationLevel;

        public void Commit()
        {
            InnerTransaction.Commit();
            _active = false;
            TransactionStatusChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Rollback()
        {
            InnerTransaction.Rollback();
            _active = false;
            TransactionStatusChanged?.Invoke(this, EventArgs.Empty);
        }

        #region IDisposable Support
        private bool disposedValue; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_active) Commit();

                    InnerTransaction.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
