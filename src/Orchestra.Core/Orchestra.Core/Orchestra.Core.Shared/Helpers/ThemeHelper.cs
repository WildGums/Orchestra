// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThemeHelper.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Media;
    using Catel;
    using Catel.Logging;
    using Catel.Windows;

    public enum AccentColorStyle
    {
        AccentColor,
        AccentColor1,
        AccentColor2,
        AccentColor3,
        AccentColor4,
    }

    public static class ThemeHelper
    {
        /// <summary>
        /// The log.
        /// </summary>
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private static ResourceDictionary _accentColorResourceDictionary;

        private static bool _ensuredOrchestraThemes;
        private static SolidColorBrush _cachedAccentColorBrush;

        public static Color GetAccentColor(AccentColorStyle colorStyle = AccentColorStyle.AccentColor)
        {
            var color = GetAccentColorBrush().Color;

            switch (colorStyle)
            {
                case AccentColorStyle.AccentColor:
                    return Color.FromArgb(255, color.R, color.G, color.B);

                case AccentColorStyle.AccentColor1:
                    return Color.FromArgb(204, color.R, color.G, color.B);

                case AccentColorStyle.AccentColor2:
                    return Color.FromArgb(153, color.R, color.G, color.B);

                case AccentColorStyle.AccentColor3:
                    return Color.FromArgb(102, color.R, color.G, color.B);

                case AccentColorStyle.AccentColor4:
                    return Color.FromArgb(51, color.R, color.G, color.B);

                default:
                    throw new ArgumentOutOfRangeException("colorStyle");
            }
        }

        public static SolidColorBrush GetAccentColorBrush()
        {
            if (_cachedAccentColorBrush != null)
            {
                return _cachedAccentColorBrush;
            }

            _cachedAccentColorBrush = Application.Current.TryFindResource("AccentColorBrush") as SolidColorBrush;
            return _cachedAccentColorBrush ?? OrchestraEnvironment.DefaultAccentColorBrush;
        }

        /// <summary>
        /// Creates the accent color resource dictionary and automatically adds it to the application resources.
        /// </summary>
        /// <returns>ResourceDictionary.</returns>
        public static ResourceDictionary CreateAccentColorResourceDictionary(Color color)
        {
            if (_accentColorResourceDictionary != null)
            {
                return _accentColorResourceDictionary;
            }

            Log.Info("Setting theme to '{0}'", color.ToString());

            Log.Debug("Creating runtime accent resource dictionary");

            var resourceDictionary = new ResourceDictionary();

            resourceDictionary.Add("HighlightColor", color);
            resourceDictionary.Add("AccentColor", GetAccentColor(AccentColorStyle.AccentColor1));
            resourceDictionary.Add("AccentColor2", GetAccentColor(AccentColorStyle.AccentColor2));
            resourceDictionary.Add("AccentColor3", GetAccentColor(AccentColorStyle.AccentColor3));
            resourceDictionary.Add("AccentColor4", GetAccentColor(AccentColorStyle.AccentColor4));

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

            applicationResources.MergedDictionaries.Insert(0, resourceDictionary);

            _accentColorResourceDictionary = resourceDictionary;

            return applicationResources;
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

            if (_accentColorResourceDictionary == null)
            {
                var accentColor = GetAccentColor();
                CreateAccentColorResourceDictionary(accentColor);
            }

            EnsureOrchestraTheme(createStyleForwarders);

            try
            {
                var uri = new Uri(resourceDictionaryUri, UriKind.RelativeOrAbsolute);

                var application = Application.Current;
                if (application == null)
                {
                    throw Log.ErrorAndCreateException<OrchestraException>("Application.Current is null, cannot ensure application themes");
                }

                var existingDictionary = (from dic in application.Resources.MergedDictionaries
                                          where dic.Source != null && dic.Source == uri
                                          select dic).FirstOrDefault();
                if (existingDictionary == null)
                {
                    application.Resources.MergedDictionaries.Add(new ResourceDictionary
                    {
                        Source = uri
                    });
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
            if (_ensuredOrchestraThemes)
            {
                return;
            }

            _ensuredOrchestraThemes = true;

            EnsureApplicationThemes(typeof(ThemeHelper).Assembly, createStyleForwarders);
        }
    }
}