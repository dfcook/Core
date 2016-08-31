using System;

namespace DanielCook.Core.DataAccess
{
    internal interface IPropertyMapping
    {
        string ColumnName { get; }
        string PropertyName { get; }

        Action<object, object> Setter { get; }

        Func<object, object> Getter { get; }
    }
}