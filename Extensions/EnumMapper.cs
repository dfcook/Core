using System;
using System.Collections.Generic;
using System.Linq;
using DanielCook.Core.Attributes;

namespace DanielCook.Core.Extensions
{
    public class EnumMapper<T>
    {
        private readonly IDictionary<string, T> _mappings = new Dictionary<string, T>();
        private readonly T _defaultValue;

        public EnumMapper(string sourceSystem)
        {
            typeof(T).
                GetFields().
                Each(x => Attribute.
                    GetCustomAttributes(x, typeof(MappingAttribute)).
                    OfType<MappingAttribute>().
                    Filter(attr => attr.System.EqualsIgnoreCase(sourceSystem)).
                    Each(attr => _mappings.Add(attr.Value, (T)x.GetValue(null))));

            _defaultValue = typeof(T).
                GetFields().
                Find(x => Attribute.
                    GetCustomAttributes(x, typeof(DefaultMappingAttribute)).
                    Any()).
                With(x => (T)x.GetValue(null));
        }

        public T MapFrom(string value) =>
            (string.IsNullOrWhiteSpace(value) || !_mappings.ContainsKey(value)) ?
                _defaultValue : _mappings[value];
    }
}
