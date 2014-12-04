// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IconHelper.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System.Drawing;
    using Catel;

    internal static class IconHelper
    {
        #region Methods
        public static Icon ExtractIconFromFile(string executablePath)
        {
            Argument.IsNotNull(() => executablePath);

            var icon = Icon.ExtractAssociatedIcon(executablePath);
            return icon;
        }
        #endregion
    }
}