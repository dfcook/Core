using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DanielCook.Core.Extensions;

namespace DanielCook.Core.DataAccess
{
    internal static class QueryBuilder
    {
        private static IQueryObject BuildQuery(IQueryObjectFactory queryFactory,
            Func<string> sqlBuilder,
            IEnumerable<KeyValuePair<string, object>> parameters)
        {
            if (sqlBuilder == null)
                throw new ArgumentNullException(nameof(sqlBuilder));

            var sql = sqlBuilder();

            var query = queryFactory.
                GetQuery(sql);

            parameters.Each(x => query.AddParameter($"@{x.Key}", x.Value));

            return query;
        }

        private static IQueryObject BuildQuery(IQueryObjectFactory queryFactory,
            Func<string> sqlBuilder)
        {
            if (sqlBuilder == null)
                throw new ArgumentNullException(nameof(sqlBuilder));

            var sql = sqlBuilder();

            var query = queryFactory.
                GetQuery(sql);

            return query;
        }

        public static IQueryObject BuildInsertQuery<T>(IQueryObjectFactory queryFactory,
            string tableName, IEnumerable<IPropertyMapping> columnMappings, T toInsert)
        {
            var sql = SqlBuilder.BuildInsertSql(tableName, columnMappings);

            var query = queryFactory.
                GetQuery(sql);

            columnMappings.Each(x => query.AddParameter(x.ColumnName, x.Getter(toInsert)));

            return query;
        }

        public static IQueryObject BuildUpdateQuery<T>(IQueryObjectFactory queryFactory,
            string tableName, string idColumn, IEnumerable<IPropertyMapping> columnMappings,
            T toInsert)
        {
            var sql = SqlBuilder.BuildUpdateSql(tableName, idColumn, columnMappings);

            var query = queryFactory.
                GetQuery(sql);

            columnMappings.Each(x => query.AddParameter(x.ColumnName, x.Getter(toInsert)));

            return query;
        }

        public static IQueryObject BuildSelectQuery(IQueryObjectFactory queryFactory,
            string tableName,
            IEnumerable<IPropertyMapping> columnMappings) =>
            BuildQuery(queryFactory, () => SqlBuilder.BuildSelectSql(tableName, columnMappings, new KeyValuePair<string, object>[0]));

        public static IQueryObject BuildSelectQuery(IQueryObjectFactory queryFactory,
            string tableName,
            IEnumerable<IPropertyMapping> columnMappings,
            object dynamicParameters) =>
            BuildSelectQuery(queryFactory, tableName,
                columnMappings, GetObjectProperties(dynamicParameters));

        public static IQueryObject BuildSelectQuery(IQueryObjectFactory queryFactory,
                string tableName,
                IEnumerable<IPropertyMapping> columnMappings,
                IEnumerable<KeyValuePair<string, object>> parameters) =>
            BuildQuery(queryFactory,
                () => SqlBuilder.BuildSelectSql(tableName, columnMappings, parameters),
                parameters);


        private static object GetId(string idField, object entity) =>
            TypeDescriptor.
                GetProperties(entity).
                OfType<PropertyDescriptor>().
                Find(x => x.Name.EqualsIgnoreCase(idField)).
                GetValue(entity);

        public static IQueryObject BuildDeleteQuery<T>(IQueryObjectFactory queryFactory,
                                                    string tableName,
                                                    IEnumerable<IPropertyMapping> columnMappings,
                                                    string idField,
                                                    T toDelete)
        {
            var parameters = new[]
            {
                new KeyValuePair<string, object>(idField, GetId(idField, toDelete))
            };

            return BuildQuery(queryFactory,
                () => SqlBuilder.BuildDeleteSql(tableName, columnMappings, parameters),
                parameters);
        }

        private static IEnumerable<KeyValuePair<string, object>> GetObjectProperties(object o) =>
            TypeDescriptor.
                GetProperties(o).
                OfType<PropertyDescriptor>().
                Map(x => new KeyValuePair<string, object>(x.Name, x.GetValue(o)));
    }
}
