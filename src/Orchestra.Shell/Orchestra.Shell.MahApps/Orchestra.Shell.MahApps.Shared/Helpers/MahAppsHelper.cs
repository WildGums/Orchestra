// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MahAppsHelper.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System.Linq;
    using System.Windows;
    using Catel.Logging;
    using MahApps.Metro;

    public static class MahAppsHelper
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Sets the theme color of the application. This method dynamically creates an in-memory resource
        /// dictionary containing the accent colors used by MahApps.
        /// </summary>
        public static void ApplyTheme()
        {
            //<Color x:Key="HighlightColor">
            //    #800080
            //</Color>
            //<Color x:Key="AccentColor">
            //    #CC800080
            //</Color>
            //<Color x:Key="AccentColor2">
            //    #99800080
            //</Color>
            //<Color x:Key="AccentColor3">
            //    #66800080
            //</Color>
            //<Color x:Key="AccentColor4">
            //    #33800080
            //</Color>

            //<SolidColorBrush x:Key="HighlightBrush" Color="{StaticResource HighlightColor}" />
            //<SolidColorBrush x:Key="AccentColorBrush" Color="{StaticResource AccentColor}" />
            //<SolidColorBrush x:Key="AccentColorBrush2" Color="{StaticResource AccentColor2}" />
            //<SolidColorBrush x:Key="AccentColorBrush3" Color="{StaticResource AccentColor3}" />
            //<SolidColorBrush x:Key="AccentColorBrush4" Color="{StaticResource AccentColor4}" />
            //<SolidColorBrush x:Key="WindowTitleColorBrush" Color="{StaticResource AccentColor}" />
            //<SolidColorBrush x:Key="AccentSelectedColorBrush" Color="White" />
            //<LinearGradientBrush x:Key="ProgressBrush" EndPoint="0.001,0.5" StartPoint="1.002,0.5">
            //    <GradientStop Color="{StaticResource HighlightColor}" Offset="0" />
            //    <GradientStop Color="{StaticResource AccentColor3}" Offset="1" />
            //</LinearGradientBrush>
            //<SolidColorBrush x:Key="CheckmarkFill" Color="{StaticResource AccentColor}" />
            //<SolidColorBrush x:Key="RightArrowFill" Color="{StaticResource AccentColor}" />

            //<Color x:Key="IdealForegroundColor">
            //    Black
            //</Color>
            //<SolidColorBrush x:Key="IdealForegroundColorBrush" Color="{StaticResource IdealForegroundColor}" />

            // Theme is always the 0-index of the resources
            var application = Application.Current;
            var applicationResources = application.Resources;
            var resourceDictionary = ThemeHelper.GetAccentColorResourceDictionary();

            var applicationTheme = ThemeManager.AppThemes.First(x => string.Equals(x.Name, "BaseLight"));

            // Insert to get the best MahApps performance (when looking up themes)
            applicationResources.MergedDictionaries.Remove(resourceDictionary);
            applicationResources.MergedDictionaries.Insert(2, applicationTheme.Resources);

            Log.Debug("Applying theme to MahApps");

            var newAccent = new Accent
            {
                Name = "Runtime accent (Orchestra)",
                Resources = resourceDictionary
            };

            ThemeManager.ChangeAppStyle(application, newAccent, applicationTheme);

            // Note: important to add the resources dictionary *after* changing the app style, but then insert at the top 
            // so MahApps theme detection performance is best
            applicationResources.MergedDictionaries.Insert(1, resourceDictionary);

            //var theme = ThemeManager.GetAppTheme(applicationResources);
            //if (theme == null)
            //{
            //    // Note: check ThemeManager.IsAccentDictionary() in https://github.com/MahApps/MahApps.Metro/blob/develop/MahApps.Metro/ThemeManager.cs#L234 
            //    // for a list of expected resources
            //    Log.Warning("No app theme found after applying it, make sure to include all resources that MahApps expects inside ThemeHelper.GetAccentColorResourceDictionary()");
            //}
        }
    }
}