using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorForTests
{
    public static class ArrayExtensions
    {
        public static string ToFormattedString<T>(this IEnumerable<T> collection)
        {
            return $"[{string.Join(", ", collection.Select(item => $"\"{item}\""))}]";
        }
    }
}
