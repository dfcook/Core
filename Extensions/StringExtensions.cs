using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace DanielCook.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool EqualsIgnoreWhiteSpace(this string source, string comparison)
        {
            if (string.IsNullOrEmpty(source))
                return string.IsNullOrEmpty(comparison);

            if (string.IsNullOrEmpty(comparison))
                return false;

            var s1 = Regex.Replace(source, @"\s", "");
            var s2 = Regex.Replace(comparison, @"\s", "");

            return s1.Equals(s2, StringComparison.OrdinalIgnoreCase);
        }

        public static bool EqualsIgnoreCase(this string source, string comparison)
        {
            if (string.IsNullOrEmpty(source))
                return string.IsNullOrEmpty(comparison);

            if (string.IsNullOrEmpty(comparison))
                return false;

            return source.
                Equals(comparison, StringComparison.OrdinalIgnoreCase);
        }

        public static bool ContainsIgnoreCase(this string source, string comparison)
        {
            if (string.IsNullOrEmpty(source) ||
                string.IsNullOrEmpty(comparison))
                return false;

            return CultureInfo.
                    CurrentUICulture.
                    CompareInfo.
                    IndexOf(source, comparison, CompareOptions.IgnoreCase) >= 0;
        }

        public static string Join(this IEnumerable<string> source, string delimiter) =>
            source == null ? string.Empty : string.Join(delimiter, source);

        public static IList<string> Split(this string source, IEnumerable<string> delimiters) =>
            source == null ? new string[0] : source.Split(delimiters.ToArray(), StringSplitOptions.RemoveEmptyEntries);

        public static IList<string> Split(this string source, string delimiter) =>
            source.Split(new string[] { delimiter });
    }
}
