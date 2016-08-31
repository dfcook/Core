using System.Collections.Generic;
using System.Data;

namespace DanielCook.Core.DataAccess
{
    public sealed class UntypedDataRecordMapper : ObjectMapperBase<IDictionary<string, object>>
    {
        public override IDictionary<string, object> Map(IDataReader reader, IDictionary<string, int> ordinalMappings) =>
            reader.MapUntyped(ordinalMappings);
    }
}
