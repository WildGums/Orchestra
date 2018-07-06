// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThemeHelper.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
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
    using Catel.Caching;
    using Catel.IoC;
    using Catel.Logging;
    using Orchestra.Services;

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
        /// <summary>
        /// The log.
        /// </summary>
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private static ResourceDictionary _accentColorResourceDictionary;

        private static bool _ensuredOrchestraThemes;
        private static SolidColorBrush _accentColorBrushCache;

        private static readonly CacheStorage<AccentColorStyle, Color> _accentColorsCache = new CacheStorage<AccentColorStyle, Color>();
        private static readonly CacheStorage<AccentColorStyle, SolidColorBrush> _accentColorBrushesCache = new CacheStorage<AccentColorStyle, SolidColorBrush>();

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

        public static Color GetAccentColor(AccentColorStyle colorStyle = AccentColorStyle.AccentColor)
        {
            return _accentColorsCache.GetFromCacheOrFetch(colorStyle, () =>
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

                    case AccentColorStyle.AccentColor5:
                        return Color.FromArgb(20, color.R, color.G, color.B);

                    default:
                        throw new ArgumentOutOfRangeException(nameof(colorStyle));
                }
            });
        }

        public static SolidColorBrush GetAccentColorBrush(AccentColorStyle colorStyle)
        {
            return _accentColorBrushesCache.GetFromCacheOrFetch(colorStyle, () =>
            {
                var color = GetAccentColor(colorStyle);
                return GetSolidColorBrush(color);
            });
        }

        public static SolidColorBrush GetAccentColorBrush()
        {
            if (_accentColorBrushCache != null)
            {
                return _accentColorBrushCache;
            }

            var accentColorService = ServiceLocator.Default.ResolveType<IAccentColorService>();
            _accentColorBrushCache = GetSolidColorBrush(accentColorService.GetAccentColor());

            return _accentColorBrushCache;
        }

        private static SolidColorBrush GetSolidColorBrush(Color color, double opacity = 1d)
        {
            var brush = new SolidColorBrush(color)
            {
                Opacity = opacity
            };

            brush.Freeze();

            return brush;
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
            return _accentColorResourceDictionary;
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
            resourceDictionary.Add("HighlightBrush", GetSolidColorBrush((Color)resourceDictionary["HighlightColor"]));

            resourceDictionary.Add("AccentColor", GetAccentColor(AccentColorStyle.AccentColor1));
            resourceDictionary.Add("AccentColor2", GetAccentColor(AccentColorStyle.AccentColor2));
            resourceDictionary.Add("AccentColor3", GetAccentColor(AccentColorStyle.AccentColor3));
            resourceDictionary.Add("AccentColor4", GetAccentColor(AccentColorStyle.AccentColor4));
            resourceDictionary.Add("AccentColor5", GetAccentColor(AccentColorStyle.AccentColor5));

            resourceDictionary.Add("AccentBaseColor", (Color)resourceDictionary["AccentColor"]);
            resourceDictionary.Add("AccentBaseColorBrush", new SolidColorBrush((Color)resourceDictionary["AccentColor"]));

            resourceDictionary.Add("AccentColorBrush", GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));
            resourceDictionary.Add("AccentColorBrush2", GetSolidColorBrush((Color)resourceDictionary["AccentColor2"]));
            resourceDictionary.Add("AccentColorBrush3", GetSolidColorBrush((Color)resourceDictionary["AccentColor3"]));
            resourceDictionary.Add("AccentColorBrush4", GetSolidColorBrush((Color)resourceDictionary["AccentColor4"]));
            resourceDictionary.Add("AccentColorBrush5", GetSolidColorBrush((Color)resourceDictionary["AccentColor5"]));
            resourceDictionary.Add("WindowTitleColorBrush", GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));

            // Wpf styles
            resourceDictionary.Add(SystemColors.HighlightColorKey, (Color)resourceDictionary["AccentColor"]);
            resourceDictionary.Add(SystemColors.HighlightBrushKey, GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));

            // MahApps styles (we should in an ideal situation move this to the MahApps shell code)
            #region MahApps
            resourceDictionary.Add("ProgressBrush", new LinearGradientBrush(new GradientStopCollection(new[]
                {
                    new GradientStop((Color)resourceDictionary["HighlightColor"], 0),
                    new GradientStop((Color)resourceDictionary["AccentColor3"], 1)
                }), new Point(0.001, 0.5), new Point(1.002, 0.5)));

            resourceDictionary.Add("CheckmarkFill", GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));
            resourceDictionary.Add("RightArrowFill", GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));

            resourceDictionary.Add("IdealForegroundColor", Colors.White);
            resourceDictionary.Add("IdealForegroundColorBrush", GetSolidColorBrush((Color)resourceDictionary["IdealForegroundColor"]));
            resourceDictionary.Add("IdealForegroundDisabledBrush", GetSolidColorBrush((Color)resourceDictionary["IdealForegroundColor"], 0.4d));
            resourceDictionary.Add("AccentSelectedColorBrush", GetSolidColorBrush(Colors.White));

            resourceDictionary.Add("MetroDataGrid.HighlightBrush", GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));
            resourceDictionary.Add("MetroDataGrid.HighlightTextBrush", GetSolidColorBrush((Color)resourceDictionary["IdealForegroundColor"]));
            resourceDictionary.Add("MetroDataGrid.MouseOverHighlightBrush", GetSolidColorBrush((Color)resourceDictionary["AccentColor3"]));
            resourceDictionary.Add("MetroDataGrid.FocusBorderBrush", GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));
            resourceDictionary.Add("MetroDataGrid.InactiveSelectionHighlightBrush", GetSolidColorBrush((Color)resourceDictionary["AccentColor2"]));
            resourceDictionary.Add("MetroDataGrid.InactiveSelectionHighlightTextBrush", GetSolidColorBrush((Color)resourceDictionary["IdealForegroundColor"]));
            #endregion

            // Fluent.Ribbon styles (we should in an ideal situation move this to the Fluent.Ribbon shell code)
            #region Fluent.Ribbon
            resourceDictionary.Add("Fluent.Ribbon.Colors.HighlightColor", color);
            resourceDictionary.Add("Fluent.Ribbon.Colors.AccentBaseColor", color);
            resourceDictionary.Add("Fluent.Ribbon.Colors.AccentColor80", Color.FromArgb(204, color.R, color.G, color.B));
            resourceDictionary.Add("Fluent.Ribbon.Colors.AccentColor60", Color.FromArgb(153, color.R, color.G, color.B));
            resourceDictionary.Add("Fluent.Ribbon.Colors.AccentColor40", Color.FromArgb(102, color.R, color.G, color.B));
            resourceDictionary.Add("Fluent.Ribbon.Colors.AccentColor20", Color.FromArgb(51, color.R, color.G, color.B));

            resourceDictionary.Add("Fluent.Ribbon.Brushes.HighlightBrush", GetSolidColorBrush((Color)resourceDictionary["Fluent.Ribbon.Colors.HighlightColor"]));
            resourceDictionary.Add("Fluent.Ribbon.Brushes.AccentBaseColorBrush", GetSolidColorBrush((Color)resourceDictionary["Fluent.Ribbon.Colors.AccentBaseColor"]));
            resourceDictionary.Add("Fluent.Ribbon.Brushes.AccentColorBrush80", GetSolidColorBrush((Color)resourceDictionary["Fluent.Ribbon.Colors.AccentColor80"]));
            resourceDictionary.Add("Fluent.Ribbon.Brushes.AccentColorBrush60", GetSolidColorBrush((Color)resourceDictionary["Fluent.Ribbon.Colors.AccentColor60"]));
            resourceDictionary.Add("Fluent.Ribbon.Brushes.AccentColorBrush40", GetSolidColorBrush((Color)resourceDictionary["Fluent.Ribbon.Colors.AccentColor40"]));
            resourceDictionary.Add("Fluent.Ribbon.Brushes.AccentColorBrush20", GetSolidColorBrush((Color)resourceDictionary["Fluent.Ribbon.Colors.AccentColor20"]));

            if (DynamicallyDetermineIdealTextColor)
            {
                resourceDictionary.Add("Fluent.Ribbon.Colors.IdealForegroundColor", GetIdealTextColor(color));
            }
            else
            {
                // White since we use the light them
                resourceDictionary.Add("Fluent.Ribbon.Colors.IdealForegroundColor", Colors.White);
            }

            resourceDictionary.Add("Fluent.Ribbon.Brushes.IdealForegroundColorBrush", GetSolidColorBrush((Color)resourceDictionary["Fluent.Ribbon.Colors.IdealForegroundColor"]));
            resourceDictionary.Add("Fluent.Ribbon.Brushes.IdealForegroundDisabledBrush", GetSolidColorBrush((Color)resourceDictionary["Fluent.Ribbon.Colors.IdealForegroundColor"], 0.4));

            // Button
            resourceDictionary.Add("Fluent.Ribbon.Brushes.Button.MouseOver.BorderBrush", GetSolidColorBrush((Color)resourceDictionary["Fluent.Ribbon.Colors.AccentColor40"]));
            resourceDictionary.Add("Fluent.Ribbon.Brushes.Button.MouseOver.Background", GetSolidColorBrush((Color)resourceDictionary["Fluent.Ribbon.Colors.AccentColor20"]));
            resourceDictionary.Add("Fluent.Ribbon.Brushes.Button.Pressed.BorderBrush", GetSolidColorBrush((Color)resourceDictionary["Fluent.Ribbon.Colors.AccentColor60"]));
            resourceDictionary.Add("Fluent.Ribbon.Brushes.Button.Pressed.Background", GetSolidColorBrush((Color)resourceDictionary["Fluent.Ribbon.Colors.AccentColor40"]));

            // ToggleButton
            resourceDictionary.Add("Fluent.Ribbon.Brushes.ToggleButton.Checked.Background", GetSolidColorBrush((Color)resourceDictionary["Fluent.Ribbon.Colors.AccentColor20"]));
            resourceDictionary.Add("Fluent.Ribbon.Brushes.ToggleButton.Checked.BorderBrush", GetSolidColorBrush((Color)resourceDictionary["Fluent.Ribbon.Colors.HighlightColor"]));
            resourceDictionary.Add("Fluent.Ribbon.Brushes.ToggleButton.CheckedMouseOver.Background", GetSolidColorBrush((Color)resourceDictionary["Fluent.Ribbon.Colors.AccentColor20"]));
            resourceDictionary.Add("Fluent.Ribbon.Brushes.ToggleButton.CheckedMouseOver.BorderBrush", GetSolidColorBrush((Color)resourceDictionary["Fluent.Ribbon.Colors.AccentColor60"]));

            // GalleryItem
            resourceDictionary.Add("Fluent.Ribbon.Brushes.GalleryItem.MouseOver", GetSolidColorBrush((Color)resourceDictionary["Fluent.Ribbon.Colors.AccentColor20"]));
            resourceDictionary.Add("Fluent.Ribbon.Brushes.GalleryItem.Selected", GetSolidColorBrush((Color)resourceDictionary["Fluent.Ribbon.Colors.AccentColor40"]));
            resourceDictionary.Add("Fluent.Ribbon.Brushes.GalleryItem.Pressed", GetSolidColorBrush((Color)resourceDictionary["Fluent.Ribbon.Colors.AccentColor60"]));

            // WindowCommands
            resourceDictionary.Add("FFluent.Ribbon.Brushes.WindowCommands.CaptionButton.MouseOver.Background", GetSolidColorBrush((Color)resourceDictionary["Fluent.Ribbon.Colors.AccentColor20"]));
            resourceDictionary.Add("Fluent.Ribbon.Brushes.WindowCommands.CaptionButton.Pressed.Background", GetSolidColorBrush((Color)resourceDictionary["Fluent.Ribbon.Colors.AccentColor40"]));
            #endregion

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
            if (_ensuredOrchestraThemes)
            {
                return;
            }

            _ensuredOrchestraThemes = true;

            EnsureApplicationThemes(typeof(ThemeHelper).Assembly, createStyleForwarders);
        }
    }
}
