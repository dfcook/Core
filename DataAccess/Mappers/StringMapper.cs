using System.Collections.Generic;
using System.Data;

namespace DanielCook.Core.DataAccess.Mappers
{
    public sealed class StringMapper : ObjectMapperBase<string>
    {
        public override string Map(IDataReader reader,
            IDictionary<string, int> ordinalMappings) =>
            reader.Read<string>(0);
    }
}
