using System;
using System.Data;

namespace DanielCook.Core.DataAccess
{
    public static class DataRecordExtensions
    {
        public static T Read<T>(this IDataRecord source, string columnName)
        {
            try
            {
                var ordinal = source.GetOrdinal(columnName);
                return source.Read<T>(ordinal);
            }
            catch (IndexOutOfRangeException)
            {
                throw new ColumnMissingException(columnName);
            }
        }

        public static T Read<T>(this IDataRecord source, int ordinal)
        {
            if (source.IsDBNull(ordinal))
                return default(T);

            if (typeof(T).IsEnum)
                return (T)Enum.ToObject(typeof(T), source.GetValue(ordinal));

            return (T)source.GetValue(ordinal);
        }
    }
}
