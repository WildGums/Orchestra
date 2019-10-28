// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrchestraEnvironment.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System.Windows.Media;

    public static class OrchestraEnvironment
    {
        #region Constants
        public const string LightBaseColorScheme = "Light";
        public const string DarkBaseColorScheme = "Dark";
        public const string DefaultBaseColorSchema = LightBaseColorScheme;
        public static readonly SolidColorBrush DefaultAccentColorBrush = new SolidColorBrush(Colors.Orange);
        #endregion
    }
}
