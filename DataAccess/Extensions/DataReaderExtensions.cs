using System.Collections.Generic;
using System.Data;
using DanielCook.Core.DataAccess.ObjectState;

namespace DanielCook.Core.DataAccess
{
    public static class DataReaderExtensions
    {
        public static ICollection<T> MapList<T>(this IDataReader reader,
            IObjectMapper<T> mapper) =>
            mapper.MapList(reader);

        public static ICollection<T> MapList<T>(this IDataReader reader) where T : new() =>
            reader.MapList(ReflectionMapperCache.GetMapper<T>());

        public static T Map<T>(this IDataReader reader, IObjectMapper<T> mapper)
        {
            var o = mapper.Map(reader);
            (o as IStatefulObject)?.MarkClean();
            return o;
        }

        public static T Map<T>(this IDataReader reader) where T : new() =>
            reader.Map(ReflectionMapperCache.GetMapper<T>());

        public static T MapNext<T>(this IDataReader reader, IObjectMapper<T> mapper)
        {
            reader.NextResult();
            return reader.Map(mapper);
        }

        public static T MapNext<T>(this IDataReader reader) where T : new()
        {
            reader.NextResult();
            return reader.Map<T>();
        }

        public static IEnumerable<T> MapNextList<T>(this IDataReader reader, IObjectMapper<T> mapper)
        {
            reader.NextResult();
            return reader.MapList(mapper);
        }

        public static IEnumerable<T> MapNextList<T>(this IDataReader reader) where T : new()
        {
            reader.NextResult();
            return reader.MapList<T>();
        }
    }
}
