// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FontAwesome.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.Ribbon
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Class FontAwesome, characters can be found at http://fortawesome.github.io/Font-Awesome/cheatsheet/
    /// </summary>
    public static class FontAwesome
    {
        public static readonly string Refresh = GetCharacter("&#xf021;");
        public static readonly string Windows = GetCharacter("&#xf17a;");

        private static string GetCharacter(string unicode)
        {
            unicode = unicode.Replace("&#x", string.Empty);
            unicode = unicode.TrimEnd(';');

            var code = int.Parse(unicode, NumberStyles.AllowHexSpecifier);
            return ((char)code).ToString();
        }
    }
}