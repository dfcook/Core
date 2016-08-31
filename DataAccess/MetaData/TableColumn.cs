using System;

namespace DanielCook.Core.DataAccess.MetaData
{
    internal class TableColumn
    {
        public string ColumnName { get; }
        public Type DataType { get; }

        public TableColumn(string columnName, Type dataType)
        {
            ColumnName = columnName;
            DataType = dataType;
        }
    }
}
