// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScreenHelper.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System;
    using System.Reflection;
    using System.Windows;

    public static class ScreenHelper
    {
        #region Constants
        private static Size DpiCache;
        #endregion

        #region Methods
        public static Size GetDpi()
        {
            if (DpiCache.Width != 0d && DpiCache.Height != 0d)
            {
                return DpiCache;
            }

            var dpiXProperty = typeof(SystemParameters).GetProperty("DpiX", BindingFlags.NonPublic | BindingFlags.Static);
            var dpiYProperty = typeof(SystemParameters).GetProperty("Dpi", BindingFlags.NonPublic | BindingFlags.Static);

            DpiCache.Width = (int)dpiXProperty.GetValue(null, null);
            DpiCache.Height = (int)dpiYProperty.GetValue(null, null);

            return DpiCache;
        }
        #endregion
    }
}