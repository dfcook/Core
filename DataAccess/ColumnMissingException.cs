using System;

namespace DanielCook.Core.DataAccess
{
    public class ColumnMissingException : Exception
    {
        public string ColumnName { get; private set; }

        public ColumnMissingException(string columnName) : 
            base(string.Format("Unable to find column {0} in the resultset", columnName))
        {
            ColumnName = columnName;
        }
    }
}
