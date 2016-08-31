using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DanielCook.Core.Extensions
{
    public static class Guard
    {
        [DebuggerHidden]
        public static void ThrowIfNullOrEmpty(this string arg, string argName)
        {
            if (string.IsNullOrWhiteSpace(arg))
                throw new ArgumentException($"{argName} cannot be empty.");
        }

        [DebuggerHidden]
        public static void ThrowIfNull<T>(this T arg, string argName)
        {
            if (arg == null)
                throw new ArgumentException($"{argName} cannot be null.");
        }

        [DebuggerHidden]
        public static void ThrowIfEmpty<T>(this IEnumerable<T> arg, string argName)
        {
            if (arg != null && !arg.Any())
                throw new ArgumentException($"{argName} cannot be empty.");
        }

        [DebuggerHidden]
        public static void ThrowIf<T>(this T arg, Predicate<T> clause, string message)
        {
            if (clause == null)
                throw new ArgumentException(nameof(clause));

            if (arg != null && clause(arg))
                throw new ArgumentException(message);
        }
    }
}
