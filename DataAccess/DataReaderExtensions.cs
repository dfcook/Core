using DanielCook.Core.Attributes;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace DanielCook.Core.DataAccess
{
    public static class DataReaderExtensions
    {
        public static ICollection<T> MapList<T>(this IDataReader reader, IObjectMapper<T> mapper)
        {
            return mapper.MapList(reader);
        }

        public static ICollection<T> MapList<T>(this IDataReader reader) where T : new()
        {
            return reader.MapList<T>(GetMapper<T>());
        }

        public static T Map<T>(this IDataReader reader, IObjectMapper<T> mapper)
        {
            return mapper.Map(reader);
        }

        public static T Map<T>(this IDataReader reader) where T : new()
        {
            return reader.Map<T>(GetMapper<T>());
        }

        private static IObjectMapper<T> GetMapper<T>() where T : new()
        {
            var useAttributeMapping = typeof(T).GetProperties().
                Any(x => x.GetCustomAttributes().
                    Any(y => y is ColumnAttribute));

            return useAttributeMapping ? (IObjectMapper<T>)new AttributeMapper<T>() : new ConventionMapper<T>();
        }
    }
}
