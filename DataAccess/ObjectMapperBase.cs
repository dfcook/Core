using DanielCook.Core.Extensions;
using System.Collections.Generic;
using System.Data;

namespace DanielCook.Core.DataAccess
{
    public abstract class ObjectMapperBase<T> : IObjectMapper<T>
    {
        private IDictionary<string, int> OrdinalMappings { get; set; }

        private static IDictionary<string, int> GetOrdinalMappings(IDataRecord reader)
        {
            var mappings = new Dictionary<string, int>();
            0.To(reader.FieldCount - 1).Each(x => mappings.Add(reader.GetName(x), x));
            return mappings;
        }

        public virtual ICollection<T> MapList(IDataReader reader)
        {
            var list = new List<T>();

            while (reader.Read())
            {
                list.Add(Map(reader));
            }

            return list;
        }

        public virtual T Map(IDataRecord record)
        {
            return Map(record, OrdinalMappings ?? GetOrdinalMappings(record));
        }

        public abstract T Map(IDataRecord record, IDictionary<string, int> ordinalMappings);
    }
}
