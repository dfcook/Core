using System;

namespace DanielCook.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public sealed class MappingAttribute : Attribute
    {
        public string System { get; set; }
        public string Value { get; set; }
    }
}
