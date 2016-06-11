using System;

namespace DanielCook.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ColumnAttribute : Attribute
    {
        public string ColumnName { get; }

        public ColumnAttribute(string columnName)
        {
            ColumnName = columnName;
        }
    }
}
