// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterHelper.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Catel;

    public static class FilterHelper
    {
        public static bool MatchesFilters(IEnumerable<string> filters, string fileName)
        {
            Argument.IsNotNull(() => filters);
            Argument.IsNotNull(() => fileName);

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