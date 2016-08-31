using System;
using System.Collections.Generic;
using System.Linq;
using DanielCook.Core.Attributes;
using DanielCook.Core.DataAccess.MetaData;
using DanielCook.Core.Extensions;
using DanielCook.Core.Functional;

namespace DanielCook.Core.DataAccess
{
    public class PersistenceManager<T> : IPersistenceManager<T> where T : class, new()
    {
        private readonly ReflectionMapper<T> _mapper;
        private readonly IQueryObjectFactory _queryFactory;

        private readonly string _tableName;
        private readonly string _idField;

        private readonly IEnumerable<TableColumn> _tableColumns;

        public PersistenceManager(IQueryObjectFactory queryFactory)
        {
            _queryFactory = queryFactory;
            _tableName = GetTableName(typeof(T));
            _idField = GetIdField(typeof(T));

            _tableColumns = TableMetaDataReader.GetTableColumns(queryFactory, _tableName);
            _mapper = ReflectionMapperCache.GetMapper<T>(_tableColumns);
        }

        private static string GetTableName(Type type) =>
            type.
                GetCustomAttributes(true).
                Find(x => x is TableAttribute).
                With(x => ((TableAttribute)x).TableName).
                Else(() => type.Name);

        private static string GetIdField(Type type) =>
            type.
                GetProperties().
                Find(x => x.GetCustomAttributes(true).Any(y => y is IdAttribute)).
                With(x => x.Name).
                Else(() => string.Empty);

        public IEnumerable<T> All() =>
            QueryBuilder.
                BuildSelectQuery(_queryFactory, _tableName, _mapper.Mappings).
                ExecuteList(_mapper);

        public void Insert(T toInsert) =>
            QueryBuilder.
                BuildInsertQuery(_queryFactory, _tableName, _mapper.Mappings, toInsert).
                Execute();

        public void Update(T toUpdate) =>
            QueryBuilder.
                BuildUpdateQuery(_queryFactory, _tableName, _idField,
                        _mapper.Mappings, toUpdate).
                Execute();

        public IEnumerable<T> Filter(object dynamicParameters) =>
            QueryBuilder.
                BuildSelectQuery(_queryFactory, _tableName,
                    _mapper.Mappings, dynamicParameters).
                ExecuteList(_mapper);

        public Maybe<T> Find(object dynamicParameters) =>
            QueryBuilder.
                BuildSelectQuery(_queryFactory, _tableName, _mapper.Mappings, dynamicParameters).
                ExecuteObject(_mapper);

        public Maybe<T> Get<V>(V key)
        {
            var parameters = new[]
            {
                new KeyValuePair<string, object>(_idField, key)
            };

            return QueryBuilder.
                BuildSelectQuery(_queryFactory, _tableName, _mapper.Mappings, parameters).
                ExecuteObject(_mapper);
        }

        public void Delete(T toDelete) =>
            QueryBuilder.
                BuildDeleteQuery(_queryFactory, _tableName, _mapper.Mappings, _idField, toDelete).
                Execute();

        public bool Any(object dynamicParameters) =>
            Filter(dynamicParameters).Any();
    }
}
