using System;

namespace DanielCook.Core.DataAccess
{
    public class ColumnMissingException : Exception
    {
        public string ColumnName { get; }

        public ColumnMissingException(string columnName) :
            base($"Unable to find column {columnName} in the resultset")
        {
            ColumnName = columnName;
        }
    }
}
