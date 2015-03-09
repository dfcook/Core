using System;
using System.Collections.Generic;

namespace DanielCook.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool EqualsIgnoreCase(this string source, string comparison)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(comparison))
                return false;

            return source.Equals(comparison, StringComparison.OrdinalIgnoreCase);
        }

        public static string Join(this IEnumerable<string> source, string delimiter)
        {
            return source == null ? string.Empty : string.Join(delimiter, source);
        }
    }
}
