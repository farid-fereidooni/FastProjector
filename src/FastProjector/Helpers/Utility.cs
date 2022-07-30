using System;
using System.Collections.Generic;
using System.Linq;

namespace FastProjector.Helpers
{
    internal static class Utility
    {
        public static bool NotNullAny<T>(this IEnumerable<T> enumerable)
        {
            return enumerable != null && enumerable.Any();
        }
        public static bool NotNullAny<T>(this IEnumerable<T> enumerable, Func<T, bool> predict)
        {
            return enumerable != null && enumerable.Any(predict);
        }

        public static string ToLowerFirstChar(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;
            
            var trimmed = value.TrimStart();

            return char.ToLowerInvariant(trimmed[0]) + trimmed.Substring(1);
        }

    }
}