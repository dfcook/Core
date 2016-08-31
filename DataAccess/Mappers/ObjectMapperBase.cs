using System.Collections.Generic;
using System.Data;

using DanielCook.Core.Extensions;

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
                list.Add(Map(reader));

            return list;
        }

        public virtual T Map(IDataReader reader) =>
            Map(reader, OrdinalMappings ?? GetOrdinalMappings(reader));

        public abstract T Map(IDataReader reader, IDictionary<string, int> ordinalMappings);
    }
}
