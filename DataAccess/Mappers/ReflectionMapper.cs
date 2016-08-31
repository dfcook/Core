using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using DanielCook.Core.Attributes;
using DanielCook.Core.DataAccess.MetaData;
using DanielCook.Core.DataAccess.ObjectState;
using DanielCook.Core.Extensions;

namespace DanielCook.Core.DataAccess
{
    internal sealed class ReflectionMapper<T> : ObjectMapperBase<T> where T : new()
    {
        public ICollection<IPropertyMapping> Mappings { get; }

        public ReflectionMapper()
        {
            Mappings = GetMappings();
        }

        public ReflectionMapper(IEnumerable<TableColumn> tableColumns)
        {
            // In this constructor we have a list of columns from the
            // underlying table, so can filter the list returned by
            // reflection to ensure all mappings are valid
            Mappings = GetMappings().
                Filter(x => tableColumns.Any(col => col.ColumnName.EqualsIgnoreCase(x.ColumnName))).
                ToList();
        }

        private static ICollection<IPropertyMapping> GetMappings() =>
            typeof(T).
                GetProperties(BindingFlags.Instance |
                        BindingFlags.Public |
                        BindingFlags.DeclaredOnly).
                Filter(x => x.SetMethod != null).
                Map(GetPropertyMapping).
                ToList();

        private static IPropertyMapping GetPropertyMapping(PropertyInfo property) =>
            property.
                GetCustomAttributes().
                Find(attr => attr is ColumnAttribute).
                With(attr => new PropertyMapping<T>(((ColumnAttribute)attr).ColumnName, property)).
                Else(() => new PropertyMapping<T>(property));

        public override T Map(IDataReader reader, IDictionary<string, int> ordinalMappings)
        {
            var item = new T();

            ordinalMappings.Each(x =>
            {
                if (!reader.IsDBNull(x.Value))
                {
                    var columnName = x.Key;
                    Mappings.
                        Filter(y => y.ColumnName.EqualsIgnoreCase(columnName)).
                        Each(z =>
                        {
                            try
                            {
                                z.Setter(item, reader[columnName]);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"Error mapping {columnName} : {ex.Message}");
                            }
                        });
                }
            });

            (item as IStatefulObject)?.MarkClean();
            return item;
        }
    }
}
