namespace Orchestra
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public static class FilterHelper
    {
        public static bool MatchesFilters(IEnumerable<string> filters, string fileName)
        {
            ArgumentNullException.ThrowIfNull(filters);
            ArgumentNullException.ThrowIfNull(fileName);

            foreach (var filter in filters)
            {
                var mask = new Regex(filter.Replace(".", "[.]").Replace("*", ".*").Replace("?", "."), RegexOptions.IgnoreCase);
                if (mask.IsMatch(fileName))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
