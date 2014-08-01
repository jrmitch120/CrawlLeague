using System;

namespace CrawlLeague.Core.Util
{
    public static class StringExtensions
    {
        public static bool Contains(this string source, string find, StringComparison comparisonType)
        {
            return source.IndexOf(find, comparisonType) >= 0;
        }
    }
}
