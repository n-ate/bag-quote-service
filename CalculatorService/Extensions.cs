using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculatorService
{
    public static class Extensions
    {
        public static IEnumerable<int> IndexesOf(this string value, string match)
        {
            int minIndex = value.IndexOf(match);
            while (minIndex != -1)
            {
                yield return minIndex;
                minIndex = value.IndexOf(match, minIndex + match.Length);
            }
        }
    }
}
