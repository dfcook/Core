using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DanielCook.Core.Extensions;
using DanielCook.Core.SqlGenerator;

namespace DanielCook.Core.DataAccess
{
    internal static class SqlBuilder
    {
        private static string Log(StringBuilder sb) =>
            Log(sb.ToString());

        private static string Log(string sql)
        {
            Debug.WriteLine($"[SQL] - {sql}");
            return sql;
        }

        public static string BuildInsertSql(string tableName,
            IEnumerable<IPropertyMapping> columnMappings)
        {
            var sql = SeaQuill.Insert().Target(tableName);
            columnMappings.Each(x =>
            {
                sql.Set(x.ColumnName, $"@{x.ColumnName}", true);
            });
            return Log(sql.ToString());
        }

        public static string BuildUpdateSql(string tableName,
                string idColumn,
                IEnumerable<IPropertyMapping> columnMappings)
        {
            var sql = SeaQuill.Update().Target(tableName).Where($"{idColumn} = @{idColumn}");
            columnMappings.Where(x => !x.ColumnName.EqualsIgnoreCase(idColumn)).Each(x =>
            {
                sql.Set(x.ColumnName, $"@{x.ColumnName}", true);
            });
            return Log(sql.ToString());
        }

        public static string BuildSelectSql(string tableName,
            IEnumerable<IPropertyMapping> columnMappings,
            IEnumerable<KeyValuePair<string, object>> dynamicParameters)
        {
            var sql = SeaQuill.Select().From(tableName);

            columnMappings.
                Map(x => x.ColumnName).
                Distinct().
                Each(x => sql.Field(x));
            AppendClauses(sql, columnMappings, dynamicParameters);

            return Log(sql.ToString());
        }

        public static string BuildDeleteSql(string tableName) =>
            Log(SeaQuill.Delete().Target(tableName).ToString());

        public static string BuildDeleteSql(string tableName,
            IEnumerable<IPropertyMapping> columnMappings,
            IEnumerable<KeyValuePair<string, object>> dynamicParameters)
        {
            var sql = SeaQuill.Delete().Target(tableName);
            AppendClauses(sql, columnMappings, dynamicParameters);

            return Log(sql.ToString());
        }

        private static void AppendClauses(SelectStatement sql,
            IEnumerable<IPropertyMapping> columnMappings,
            IEnumerable<KeyValuePair<string, object>> dynamicParameters)
        {
            if (dynamicParameters.Any())
            {
                dynamicParameters.Each(x =>
                {
                    var col = columnMappings.Find(m => m.PropertyName.EqualsIgnoreCase(x.Key));
                    sql.Where($"{col.ColumnName} = @{x.Key}");
                });
            }
        }

        private static void AppendClauses(DeleteStatement sql,
            IEnumerable<IPropertyMapping> columnMappings,
            IEnumerable<KeyValuePair<string, object>> dynamicParameters)
        {
            if (dynamicParameters.Any())
            {
                dynamicParameters.Each(x =>
                {
                    var col = columnMappings.Find(m => m.PropertyName.EqualsIgnoreCase(x.Key));
                    sql.Where($"{col.ColumnName} = @{x.Key}");
                });
            }
        }

        private static void AppendClauses(StringBuilder sql,
            IEnumerable<IPropertyMapping> columnMappings,
            IEnumerable<KeyValuePair<string, object>> dynamicParameters)
        {
            if (dynamicParameters.Any())
            {
                sql.Append(" where ");

                dynamicParameters.Each(x =>
                {
                    var col = columnMappings.Find(m => m.PropertyName.EqualsIgnoreCase(x.Key));
                    sql.AppendFormat("{0} = @{1} and", col.ColumnName, x.Key);
                });

                sql.Remove(sql.Length - 4, 4);
            }
        }
    }
}
