// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentRibbonHelper.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Markup;
    using System.Xml;
    using Catel.Logging;
    using Fluent;

    public static class FluentRibbonHelper
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Methods
        /// <summary>
        /// Sets the theme color of the application. This method dynamically creates an in-memory resource
        /// dictionary containing the accent colors used by Fluent.Ribbon.
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

            // Create theme, note we need to write this to a file since
            // ThemeManager.AddAccent requires a resource uri
            var resDictName = $"ApplicationAccentColors.xaml";
            var fileName = Path.Combine(Catel.IO.Path.GetApplicationDataDirectory(), resDictName);

            Log.Debug($"Writing dynamic theme file to '{fileName}'");

            using (var writer = XmlWriter.Create(fileName, new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "    "
            }))
            {
                XamlWriter.Save(resourceDictionary, writer);
                writer.Flush();
            }

#if NETCORE
            // Note: because .NET Core can't read IsReadonly="False", we need to remove it
            var fileContents = File.ReadAllText(fileName);
            if (!string.IsNullOrWhiteSpace(fileContents))
            {
                fileContents = fileContents.Replace("IsReadOnly=\"False\"", string.Empty);

                File.WriteAllText(fileName, fileContents);
            }
#endif

            resourceDictionary = new ResourceDictionary
            {
                Source = new Uri(fileName, UriKind.Absolute)
            };

            Log.Debug("Applying theme to Fluent.Ribbon");

            var newAccent = new Accent
            {
                Name = "Runtime accent (Orchestra)",
                Resources = resourceDictionary
            };

            ThemeManager.AddAccent(newAccent.Name, newAccent.Resources.Source);
            ThemeManager.ChangeAppStyle(application, newAccent, applicationTheme);

            // Note: important to add the resources dictionary *after* changing the app style, but then insert at the top 
            // so theme detection performance is best
            applicationResources.MergedDictionaries.Insert(0, resourceDictionary);
        }
        #endregion
    }
}
