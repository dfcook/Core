using System;
using System.Collections.Generic;
using System.Data;

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
            return new AttributeMapper<T>().MapList(reader);
        }

        public static T Map<T>(this IDataReader reader, IObjectMapper<T> mapper) 
        {
            return mapper.Map(reader);
        }

        public static T Map<T>(this IDataReader reader) where T : new()
        {
            return new AttributeMapper<T>().Map(reader);
        }
    }
}
