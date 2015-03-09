using DanielCook.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Data;

namespace DanielCook.Core.DataAccess
{
    public class DataTableHelper
    {
        public DataTable Table { get; private set; }

        public DataTableHelper(string tableName)
        {
            Table = new DataTable(tableName);            
        }

        public DataTableHelper() : this ("Table1") {}

        public static DataTableHelper Create(string tableName)
        {
            return new DataTableHelper(tableName);
        }

        public static DataTableHelper Create()
        {
            return new DataTableHelper();
        }

        public DataTableHelper AddColumn<T>(string columnName, bool nullable)
        {
            var col = Table.Columns.Add(columnName, typeof(T));
            col.AllowDBNull = nullable;

            return this;
        }

        public DataTableHelper AddColumn<T>(string columnName)
        {
            return AddColumn<T>(columnName, true);
        }

        public DataTableHelper AddRows<T>(IEnumerable<T> items, Func<T, object[]> mapper)
        {
            items.Each(x => Table.Rows.Add(mapper(x)));
            return this;
        }
    }
}
