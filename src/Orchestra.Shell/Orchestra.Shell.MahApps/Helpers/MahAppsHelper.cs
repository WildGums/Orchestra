// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MahAppsHelper.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;
    using Catel.Logging;
    using MahApps.Metro;

    public static class MahAppsHelper
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Sets the theme color of the application. This method dynamically creates an in-memory resource
        /// dictionary containing the accent colors used by MahApps.
        /// </summary>
        /// <param name="color">The color.</param>
        public static void SetThemeColor(Color color)
        {
            Log.Info("Setting theme to '{0}'", color.ToString());

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

            Log.Debug("Creating runtime accent resource dictionary");

            var resourceDictionary = new ResourceDictionary();

            resourceDictionary.Add("HighlightColor", color);
            resourceDictionary.Add("AccentColor", Color.FromArgb((byte)(204), color.R, color.G, color.B));
            resourceDictionary.Add("AccentColor2", Color.FromArgb((byte)(153), color.R, color.G, color.B));
            resourceDictionary.Add("AccentColor3", Color.FromArgb((byte)(102), color.R, color.G, color.B));
            resourceDictionary.Add("AccentColor4", Color.FromArgb((byte)(51), color.R, color.G, color.B));

            resourceDictionary.Add("HighlightBrush", new SolidColorBrush((Color)resourceDictionary["HighlightColor"]));
            resourceDictionary.Add("AccentColorBrush", new SolidColorBrush((Color)resourceDictionary["AccentColor"]));
            resourceDictionary.Add("AccentColorBrush2", new SolidColorBrush((Color)resourceDictionary["AccentColor2"]));
            resourceDictionary.Add("AccentColorBrush3", new SolidColorBrush((Color)resourceDictionary["AccentColor3"]));
            resourceDictionary.Add("AccentColorBrush4", new SolidColorBrush((Color)resourceDictionary["AccentColor4"]));
            resourceDictionary.Add("WindowTitleColorBrush", new SolidColorBrush((Color)resourceDictionary["AccentColor"]));
            resourceDictionary.Add("AccentSelectedColorBrush", new SolidColorBrush(Colors.White));

            resourceDictionary.Add("ProgressBrush", new LinearGradientBrush(new GradientStopCollection(new[]
                {
                    new GradientStop((Color)resourceDictionary["HighlightColor"], 0),
                    new GradientStop((Color)resourceDictionary["AccentColor3"], 1)
                }), new Point(0.001, 0.5), new Point(1.002, 0.5)));

            resourceDictionary.Add("CheckmarkFill", new SolidColorBrush((Color)resourceDictionary["AccentColor"]));
            resourceDictionary.Add("RightArrowFill", new SolidColorBrush((Color)resourceDictionary["AccentColor"]));

            resourceDictionary.Add("IdealForegroundColor", Colors.Black);
            resourceDictionary.Add("IdealForegroundColorBrush", new SolidColorBrush((Color)resourceDictionary["IdealForegroundColor"]));

            var application = Application.Current;
            var applicationResources = application.Resources;

            var applicationTheme = ThemeManager.AppThemes.First(x => string.Equals(x.Name, "BaseLight"));

            // Insert to get the best MahApps performance (when looking up themes)
            applicationResources.MergedDictionaries.Insert(0, resourceDictionary);
            applicationResources.MergedDictionaries.Insert(1, applicationTheme.Resources);

            Log.Debug("Applying theme to MahApps");

            ThemeManager.ChangeAppStyle(application, new Accent { Name = "ApplicationAccent", Resources = resourceDictionary }, applicationTheme);
        }
    }
}