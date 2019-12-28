// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScreenHelper.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System.Reflection;
    using System.Windows;

    [ObsoleteEx(ReplacementTypeOrMember = "Orc.Controls.ScreenHelper", TreatAsErrorFromVersion = "5.0", RemoveInVersion = "6.0")]
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
