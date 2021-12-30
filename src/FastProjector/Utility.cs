using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FastProjector.MapGenerator.Proccessing.Models;

namespace FastProjector.MapGenerator
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

    }
}