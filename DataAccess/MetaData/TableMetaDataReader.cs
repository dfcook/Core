using System.Collections.Generic;
using System.Linq;
using DanielCook.Core.Extensions;

namespace DanielCook.Core.DataAccess.MetaData
{
    internal static class TableMetaDataReader
    {
        public static IEnumerable<TableColumn> GetTableColumns(IQueryObjectFactory queryFactory,
            string tableName) =>
            Disposable.Using(() => queryFactory.
                GetQuery($"select * from {tableName} where 1 = 0").
                ExecuteReader(),
                rdr => 0.
                    To(rdr.FieldCount - 1).
                    Map(x => new TableColumn(rdr.GetName(x), rdr.GetFieldType(x))).
                    ToList());
    }
}
