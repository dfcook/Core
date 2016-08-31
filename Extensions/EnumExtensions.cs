using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DanielCook.Core.Extensions
{
    public class EnumPivot<T>
    {
        public EnumPivot(T value, string description)
        {
            Description = description;
            Value = value;
        }

        public string Description { get; }
        public T Value { get; }
    }

    public static class EnumExtensions
    {
        public static IEnumerable<T> GetValues<T>() where T : IComparable
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            foreach (T val in Enum.GetValues(typeof(T)))
                yield return val;
        }

        public static IEnumerable<string> GetNames<T>() where T : IComparable
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            foreach (var name in Enum.GetNames(typeof(T)))
                yield return name;
        }

        public static T Parse<T>(string str) where T : IComparable
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            return (T)Enum.Parse(typeof(T), str);
        }

        public static string GetDescription<T>(this T value) =>
            Enum.GetName(typeof(T), value).
                With(x => GetEnumDescription<T>(x)).
                Else(() => string.Empty);

        private static string GetEnumDescription<T>(string name)
        {
            var field = typeof(T).GetField(name);
            if (field != null)
            {
                var attr =
                       Attribute.GetCustomAttribute(field,
                         typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attr != null)
                {
                    return attr.Description;
                }
            }

            return string.Empty;
        }

        public static IEnumerable<string> GetDescriptions<T>() where T : IComparable
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            return Enum.
                GetNames(typeof(T)).
                Map(x => GetEnumDescription<T>(x));
        }

        public static IEnumerable<EnumPivot<T>> Pivot<T>() where T : IComparable
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            return GetValues<T>().
                Map(x => new EnumPivot<T>(x, GetDescription(x))).
                Filter(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}
