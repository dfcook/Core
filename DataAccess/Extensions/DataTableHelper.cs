using System;
using System.Collections.Generic;
using System.Data;
using DanielCook.Core.Extensions;

namespace DanielCook.Core.DataAccess
{
    public class DataTableHelper
    {
        public DataTable Table { get; set; }

        public DataTableHelper(string tableName)
        {
            Table = new DataTable(tableName);
        }

        public DataTableHelper() : this("Table1") { }

        public static DataTableHelper Create(string tableName) =>
            new DataTableHelper(tableName);

        public static DataTableHelper Create() =>
            new DataTableHelper();

        public DataTableHelper AddColumn<T>(string columnName, bool nullable)
        {
            var col = Table.Columns.Add(columnName, typeof(T));
            col.AllowDBNull = nullable;

            return this;
        }

        public DataTableHelper AddColumn<T>(string columnName) =>
            AddColumn<T>(columnName, true);

        public DataTableHelper AddRows<T>(IEnumerable<T> items, Func<T, object[]> mapper)
        {
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));

            items.Each(x => Table.Rows.Add(mapper(x)));
            return this;
        }
    }
}
