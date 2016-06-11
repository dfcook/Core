using DanielCook.Core.Attributes;
using DanielCook.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace DanielCook.Core.DataAccess
{
    public class AttributeMapper<T> : ObjectMapperBase<T> where T : new()
    {
        private static IDictionary<Type, ICollection<AttributeMapping>> CachedMappings { get; set; }
        private ICollection<AttributeMapping> Mappings { get; set; }

        static AttributeMapper()
        {
            CachedMappings = new Dictionary<Type, ICollection<AttributeMapping>>();
        }

        public AttributeMapper()
        {
            var type = typeof(T);

            if (CachedMappings.ContainsKey(type))
            {
                Mappings = CachedMappings[type];
            }
            else
            {
                Mappings = new List<AttributeMapping>();

                type.
                    GetProperties().
                    Each(x =>
                {
                    x.
                        GetCustomAttributes().
                        Filter(y => y is ColumnAttribute).
                        Each(z =>
                        {
                            var col = z as ColumnAttribute;
                            Mappings.Add(new AttributeMapping(col.ColumnName, x));
                        });
                });

                CachedMappings.Add(type, Mappings);
            }
        }

        public override T Map(IDataRecord record, IDictionary<string, int> ordinals)
        {
            var item = new T();

            ordinals.Each(x =>
            {
                if (!record.IsDBNull(x.Value))
                {
                    var columnName = x.Key;
                    Mappings.
                        Filter(y => y.ColumnName.EqualsIgnoreCase(columnName)).
                        Each(z =>
                        {
                            try
                            {
                                z.Setter(item, record[columnName]);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"Error mapping {columnName} : {ex.Message}");
                            }
                        });
                }
            });

            return item;
        }

        private class AttributeMapping
        {
            public string ColumnName { get; private set; }
            public Action<object, object> Setter { get; private set; }

            public AttributeMapping(string columnName, PropertyInfo property)
            {
                ColumnName = columnName;
                Setter = CreateSetter(property);
            }

            private Action<object, object> CreateSetter(PropertyInfo property)
            {
                var setter = property.GetSetMethod();
                if (setter == null)
                    throw new ArgumentException($"Property {property.Name} does not have a setter method");

                var method = typeof(AttributeMapping).GetMethod(nameof(CreateGenericSetter), BindingFlags.Instance | BindingFlags.NonPublic);
                var helper = method.MakeGenericMethod(property.PropertyType);
                return (Action<object, object>)helper.Invoke(this, new object[] { setter });
            }

            private static Action<object, object> CreateGenericSetter<V>(MethodInfo setter)
            {
                var typedSetter = (Action<T, V>)Delegate.CreateDelegate(typeof(Action<T, V>), setter);
                return (Action<object, object>)((instance, value) => typedSetter((T)instance, (V)value));
            }
        }
    }
}
