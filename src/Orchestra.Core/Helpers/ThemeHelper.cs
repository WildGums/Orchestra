// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThemeHelper.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

// This is becoming obsolete anyway
#pragma warning disable CS0619, 619

namespace Orchestra
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Media;
    using Catel;
    using Catel.Caching;
    using Catel.IoC;
    using Catel.Logging;
    using Orchestra.Services;
    using Orc.Controls;
    using System.Collections.Generic;
    using Catel.Reflection;
    using Orchestra.Themes;

    [ObsoleteEx(TreatAsErrorFromVersion = "5.2", RemoveInVersion = "6.0", ReplacementTypeOrMember = "Orc.Controls.AccentColorStyle")]
    public enum AccentColorStyle
    {
        AccentColor,
        AccentColor1,
        AccentColor2,
        AccentColor3,
        AccentColor4,
        AccentColor5,
    }

    public static class ThemeHelper
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        private static readonly Dictionary<Color, ResourceDictionary> AccentColorResourceDictionary = new Dictionary<Color, ResourceDictionary>();

        private static bool EnsuredOrchestraThemes;

        static ThemeHelper()
        {
            DynamicallyDetermineIdealTextColor = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the ideal text color for the ribbon and other controls
        /// should be determined automatically based on the color.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the text color should be determined automatically; otherwise, <c>false</c>.
        /// </value>
        public static bool DynamicallyDetermineIdealTextColor { get; set; }

        [ObsoleteEx(TreatAsErrorFromVersion = "5.0", RemoveInVersion = "6.0", ReplacementTypeOrMember = "Orc.Controls.ThemeHelper")]
        public static Color GetAccentColor(AccentColorStyle colorStyle = AccentColorStyle.AccentColor)
        {
            return Orc.Controls.ThemeHelper.GetThemeColor(colorStyle.GetOrcControlsThemeColor());
        }

        [ObsoleteEx(TreatAsErrorFromVersion = "5.0", RemoveInVersion = "6.0", ReplacementTypeOrMember = "Orc.Controls.ThemeHelper")]
        public static SolidColorBrush GetAccentColorBrush(AccentColorStyle colorStyle)
        {
            return Orc.Controls.ThemeHelper.GetThemeColorBrush(colorStyle.GetOrcControlsThemeColor());
        }

        [ObsoleteEx(TreatAsErrorFromVersion = "5.0", RemoveInVersion = "6.0", ReplacementTypeOrMember = "Orc.Controls.ThemeHelper")]
        public static SolidColorBrush GetAccentColorBrush()
        {
            return Orc.Controls.ThemeHelper.GetThemeColorBrush(Orc.Controls.ThemeColorStyle.AccentColor);
        }

        private static Orc.Controls.ThemeColorStyle GetOrcControlsThemeColor(this AccentColorStyle accentColor)
        {
            switch (accentColor)
            {
                case AccentColorStyle.AccentColor:
                    return Orc.Controls.ThemeColorStyle.AccentColor;

                case AccentColorStyle.AccentColor1:
                    return Orc.Controls.ThemeColorStyle.AccentColor1;

                case AccentColorStyle.AccentColor2:
                    return Orc.Controls.ThemeColorStyle.AccentColor2;

                case AccentColorStyle.AccentColor3:
                    return Orc.Controls.ThemeColorStyle.AccentColor3;

                case AccentColorStyle.AccentColor4:
                    return Orc.Controls.ThemeColorStyle.AccentColor4;

                case AccentColorStyle.AccentColor5:
                    return Orc.Controls.ThemeColorStyle.AccentColor5;
            }

            return Orc.Controls.ThemeColorStyle.AccentColor;
        }

        /// <summary>
        ///     Determining Ideal Text Color Based on Specified Background Color
        ///     http://www.codeproject.com/KB/GDI-plus/IdealTextColor.aspx
        /// </summary>
        /// <param name="color">The bg.</param>
        /// <returns></returns>
        private static Color GetIdealTextColor(Color color)
        {
            const int nThreshold = 105;
            var bgDelta = Convert.ToInt32((color.R * 0.299) + (color.G * 0.587) + (color.B * 0.114));
            var foreColor = (255 - bgDelta < nThreshold) ? Colors.Black : Colors.White;
            return foreColor;
        }

        /// <summary>
        /// Gets the accent color resource dictionary if it has been created.
        /// </summary>
        /// <returns>ResourceDictionary.</returns>
        public static ResourceDictionary GetAccentColorResourceDictionary()
        {
            // Note: for v6, move the CreateAccentColorResourceDictionary(Color color) content here

            var accentColor = GetAccentColor();
            return CreateAccentColorResourceDictionary(accentColor);
        }

        /// <summary>
        /// Creates the accent color resource dictionary and automatically adds it to the application resources.
        /// </summary>
        /// <returns>ResourceDictionary.</returns>
        [ObsoleteEx(TreatAsErrorFromVersion = "5.0", RemoveInVersion = "6.0", Message = "Only use AccentColor and AccentColorBrush markup extensions")]
        public static ResourceDictionary CreateAccentColorResourceDictionary(Color color)
        {
            if (!AccentColorResourceDictionary.TryGetValue(color, out var resourceDictionary))
            {
                Log.Info($"Setting theme to '{color}'");

                Log.Debug($"Creating runtime accent resource dictionary for accent color '{color}'");

                // NOTE: Do not use the markup extensions here, MahApps can't deal with that

                resourceDictionary = new ResourceDictionary();

                resourceDictionary.Add("AccentColor", Orc.Controls.ThemeHelper.GetThemeColor(Orc.Controls.ThemeColorStyle.AccentColor));
                resourceDictionary.Add("AccentColor1", Orc.Controls.ThemeHelper.GetThemeColor(Orc.Controls.ThemeColorStyle.AccentColor1));
                resourceDictionary.Add("AccentColor2", Orc.Controls.ThemeHelper.GetThemeColor(Orc.Controls.ThemeColorStyle.AccentColor2));
                resourceDictionary.Add("AccentColor3", Orc.Controls.ThemeHelper.GetThemeColor(Orc.Controls.ThemeColorStyle.AccentColor3));
                resourceDictionary.Add("AccentColor4", Orc.Controls.ThemeHelper.GetThemeColor(Orc.Controls.ThemeColorStyle.AccentColor4));
                resourceDictionary.Add("AccentColor5", Orc.Controls.ThemeHelper.GetThemeColor(Orc.Controls.ThemeColorStyle.AccentColor5));

                resourceDictionary.Add("AccentBaseColor", Orc.Controls.ThemeHelper.GetThemeColor(Orc.Controls.ThemeColorStyle.AccentColor));
                resourceDictionary.Add("AccentBaseColorBrush", Orc.Controls.ThemeHelper.GetThemeColorBrush(Orc.Controls.ThemeColorStyle.AccentColor));

                resourceDictionary.Add("AccentColorBrush", Orc.Controls.ThemeHelper.GetThemeColorBrush(Orc.Controls.ThemeColorStyle.AccentColor));
                resourceDictionary.Add("AccentColorBrush1", Orc.Controls.ThemeHelper.GetThemeColorBrush(Orc.Controls.ThemeColorStyle.AccentColor1));
                resourceDictionary.Add("AccentColorBrush2", Orc.Controls.ThemeHelper.GetThemeColorBrush(Orc.Controls.ThemeColorStyle.AccentColor2));
                resourceDictionary.Add("AccentColorBrush3", Orc.Controls.ThemeHelper.GetThemeColorBrush(Orc.Controls.ThemeColorStyle.AccentColor3));
                resourceDictionary.Add("AccentColorBrush4", Orc.Controls.ThemeHelper.GetThemeColorBrush(Orc.Controls.ThemeColorStyle.AccentColor4));
                resourceDictionary.Add("AccentColorBrush5", Orc.Controls.ThemeHelper.GetThemeColorBrush(Orc.Controls.ThemeColorStyle.AccentColor5));

                resourceDictionary.Add("WindowTitleColorBrush", Orc.Controls.ThemeHelper.GetThemeColorBrush());

                // Wpf styles
                resourceDictionary.Add(SystemColors.HighlightColorKey, Orc.Controls.ThemeHelper.GetThemeColor());
                resourceDictionary.Add(SystemColors.HighlightBrushKey, Orc.Controls.ThemeHelper.GetThemeColorBrush());
                resourceDictionary.Add("HighlightColor", Orc.Controls.ThemeHelper.GetThemeColor());
                // Note: this causes invalid cast exception, disable for now
                //resourceDictionary.Add("HighlightBrush", Orc.Controls.ThemeHelper.GetThemeColorBrush());

                var application = Application.Current;
                var applicationResources = application.Resources;

                applicationResources.MergedDictionaries.Insert(0, resourceDictionary);

                AccentColorResourceDictionary[color] = resourceDictionary;
            }

            return resourceDictionary;
        }

        /// <summary>
        /// Ensures the application themes by using the assembly and the <c>/Themes/Generic.xaml</c>.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="createStyleForwarders">if set to <c>true</c>, style forwarders will be created.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="assembly" /> is <c>null</c>.</exception>
        public static void EnsureApplicationThemes(Assembly assembly, bool createStyleForwarders = false)
        {
            Argument.IsNotNull(() => assembly);

            var uri = string.Format("/{0};component/themes/generic.xaml", assembly.GetName().Name);

            EnsureApplicationThemes(uri, createStyleForwarders);
        }

        /// <summary>
        /// Ensures the application themes.
        /// </summary>
        /// <param name="resourceDictionaryUri">The resource dictionary.</param>
        /// <param name="createStyleForwarders">if set to <c>true</c>, style forwarders will be created.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="resourceDictionaryUri" /> is <c>null</c> or whitespace.</exception>
        public static void EnsureApplicationThemes(string resourceDictionaryUri, bool createStyleForwarders = false)
        {
            Argument.IsNotNullOrWhitespace(() => resourceDictionaryUri);

            var accentColor = GetAccentColor();
            if (!AccentColorResourceDictionary.ContainsKey(accentColor))
            {
                CreateAccentColorResourceDictionary(accentColor);
            }

            EnsureOrchestraTheme(createStyleForwarders);

            try
            {
                var uri = new Uri(resourceDictionaryUri, UriKind.RelativeOrAbsolute);

                var application = Application.Current;
                if (application is null)
                {
                    throw Log.ErrorAndCreateException<OrchestraException>("Application.Current is null, cannot ensure application themes");
                }

                var existingDictionary = (from dic in application.Resources.MergedDictionaries
                                          where dic.Source != null && dic.Source == uri
                                          select dic).FirstOrDefault();
                if (existingDictionary is null)
                {
                    existingDictionary = new ResourceDictionary
                    {
                        Source = uri
                    };

                    application.Resources.MergedDictionaries.Add(existingDictionary);
                }

                if (createStyleForwarders)
                {
                    StyleHelper.CreateStyleForwardersForDefaultStyles();
                }
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Failed to add application theme '{0}'", resourceDictionaryUri);
            }
        }

        /// <summary>
        /// Ensures the orchestra theme.
        /// </summary>
        /// <param name="createStyleForwarders">if set to <c>true</c>, create style forwarders.</param>
        private static void EnsureOrchestraTheme(bool createStyleForwarders)
        {
            if (EnsuredOrchestraThemes)
            {
                return;
            }

            EnsuredOrchestraThemes = true;

            EnsureApplicationThemes(typeof(ThemeHelper).Assembly, createStyleForwarders);
        }
    }
}
