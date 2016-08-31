using System;

namespace DanielCook.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class DefaultMappingAttribute : Attribute
    {
        public string System { get; set; }
    }
}
