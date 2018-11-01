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
    using Orc.Controls;

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

        private static ResourceDictionary _accentColorResourceDictionary;

        private static bool _ensuredOrchestraThemes;

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

        [ObsoleteEx(TreatAsErrorFromVersion = "5.2", RemoveInVersion = "6.0", ReplacementTypeOrMember = "Orc.Controls.ThemeHelper")]
        public static Color GetAccentColor(AccentColorStyle colorStyle = AccentColorStyle.AccentColor)
        {
            return Orc.Controls.ThemeHelper.GetAccentColor(colorStyle.GetOrcControlsAccentColor());
        }

        [ObsoleteEx(TreatAsErrorFromVersion = "5.2", RemoveInVersion = "6.0", ReplacementTypeOrMember = "Orc.Controls.ThemeHelper")]
        public static SolidColorBrush GetAccentColorBrush(AccentColorStyle colorStyle)
        {
            return Orc.Controls.ThemeHelper.GetAccentColorBrush(colorStyle.GetOrcControlsAccentColor());
        }

        [ObsoleteEx(TreatAsErrorFromVersion = "5.2", RemoveInVersion = "6.0", ReplacementTypeOrMember = "Orc.Controls.ThemeHelper")]
        public static SolidColorBrush GetAccentColorBrush()
        {
            return Orc.Controls.ThemeHelper.GetAccentColorBrush(Orc.Controls.AccentColorStyle.AccentColor);
        }

        private static Orc.Controls.AccentColorStyle GetOrcControlsAccentColor(this AccentColorStyle accentColor)
        {
            switch (accentColor)
            {
                case AccentColorStyle.AccentColor:
                    return Orc.Controls.AccentColorStyle.AccentColor;

                case AccentColorStyle.AccentColor1:
                    return Orc.Controls.AccentColorStyle.AccentColor1;

                case AccentColorStyle.AccentColor2:
                    return Orc.Controls.AccentColorStyle.AccentColor2;

                case AccentColorStyle.AccentColor3:
                    return Orc.Controls.AccentColorStyle.AccentColor3;

                case AccentColorStyle.AccentColor4:
                    return Orc.Controls.AccentColorStyle.AccentColor4;

                case AccentColorStyle.AccentColor5:
                    return Orc.Controls.AccentColorStyle.AccentColor5;
            }

            return Orc.Controls.AccentColorStyle.AccentColor;
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

            // TODO: Replace by orccontrols:AccentColor and orccontrols:AccentColorBrush markup extensions so
            // the styles can be updated at runtime

            resourceDictionary.Add("HighlightColor", color);
            resourceDictionary.Add("HighlightBrush", ((Color)resourceDictionary["HighlightColor"]).GetSolidColorBrush());

            resourceDictionary.Add("AccentColor", GetAccentColor(AccentColorStyle.AccentColor1));
            resourceDictionary.Add("AccentColor2", GetAccentColor(AccentColorStyle.AccentColor2));
            resourceDictionary.Add("AccentColor3", GetAccentColor(AccentColorStyle.AccentColor3));
            resourceDictionary.Add("AccentColor4", GetAccentColor(AccentColorStyle.AccentColor4));
            resourceDictionary.Add("AccentColor5", GetAccentColor(AccentColorStyle.AccentColor5));

            resourceDictionary.Add("AccentBaseColor", (Color)resourceDictionary["AccentColor"]);
            resourceDictionary.Add("AccentBaseColorBrush", new SolidColorBrush((Color)resourceDictionary["AccentColor"]));

            resourceDictionary.Add("AccentColorBrush", ((Color)resourceDictionary["AccentColor"]).GetSolidColorBrush());
            resourceDictionary.Add("AccentColorBrush2", ((Color)resourceDictionary["AccentColor2"]).GetSolidColorBrush());
            resourceDictionary.Add("AccentColorBrush3", ((Color)resourceDictionary["AccentColor3"]).GetSolidColorBrush());
            resourceDictionary.Add("AccentColorBrush4", ((Color)resourceDictionary["AccentColor4"]).GetSolidColorBrush());
            resourceDictionary.Add("AccentColorBrush5", ((Color)resourceDictionary["AccentColor5"]).GetSolidColorBrush());
            resourceDictionary.Add("WindowTitleColorBrush", ((Color)resourceDictionary["AccentColor"]).GetSolidColorBrush());

            // Wpf styles
            resourceDictionary.Add(SystemColors.HighlightColorKey, (Color)resourceDictionary["AccentColor"]);
            resourceDictionary.Add(SystemColors.HighlightBrushKey, ((Color)resourceDictionary["AccentColor"]).GetSolidColorBrush());

            // MahApps styles (we should in an ideal situation move this to the MahApps shell code)
            #region MahApps
            resourceDictionary.Add("ProgressBrush", new LinearGradientBrush(new GradientStopCollection(new[]
                {
                    new GradientStop((Color)resourceDictionary["HighlightColor"], 0),
                    new GradientStop((Color)resourceDictionary["AccentColor3"], 1)
                }), new Point(0.001, 0.5), new Point(1.002, 0.5)));

            resourceDictionary.Add("CheckmarkFill", ((Color)resourceDictionary["AccentColor"]).GetSolidColorBrush());
            resourceDictionary.Add("RightArrowFill", ((Color)resourceDictionary["AccentColor"]).GetSolidColorBrush());

            resourceDictionary.Add("IdealForegroundColor", Colors.White);
            resourceDictionary.Add("IdealForegroundColorBrush", ((Color)resourceDictionary["IdealForegroundColor"]).GetSolidColorBrush());
            resourceDictionary.Add("IdealForegroundDisabledBrush", ((Color)resourceDictionary["IdealForegroundColor"]).GetSolidColorBrush(0.4d));
            resourceDictionary.Add("AccentSelectedColorBrush", (Colors.White).GetSolidColorBrush());

            resourceDictionary.Add("MetroDataGrid.HighlightBrush", ((Color)resourceDictionary["AccentColor"]).GetSolidColorBrush());
            resourceDictionary.Add("MetroDataGrid.HighlightTextBrush", ((Color)resourceDictionary["IdealForegroundColor"]).GetSolidColorBrush());
            resourceDictionary.Add("MetroDataGrid.MouseOverHighlightBrush", ((Color)resourceDictionary["AccentColor3"]).GetSolidColorBrush());
            resourceDictionary.Add("MetroDataGrid.FocusBorderBrush", ((Color)resourceDictionary["AccentColor"]).GetSolidColorBrush());
            resourceDictionary.Add("MetroDataGrid.InactiveSelectionHighlightBrush", ((Color)resourceDictionary["AccentColor2"]).GetSolidColorBrush());
            resourceDictionary.Add("MetroDataGrid.InactiveSelectionHighlightTextBrush", ((Color)resourceDictionary["IdealForegroundColor"]).GetSolidColorBrush());
            #endregion

            // Fluent.Ribbon styles (we should in an ideal situation move this to the Fluent.Ribbon shell code)
            #region Fluent.Ribbon
            resourceDictionary.Add("Fluent.Ribbon.Colors.HighlightColor", color);
            resourceDictionary.Add("Fluent.Ribbon.Colors.AccentBaseColor", color);
            resourceDictionary.Add("Fluent.Ribbon.Colors.AccentColor80", Color.FromArgb(204, color.R, color.G, color.B));
            resourceDictionary.Add("Fluent.Ribbon.Colors.AccentColor60", Color.FromArgb(153, color.R, color.G, color.B));
            resourceDictionary.Add("Fluent.Ribbon.Colors.AccentColor40", Color.FromArgb(102, color.R, color.G, color.B));
            resourceDictionary.Add("Fluent.Ribbon.Colors.AccentColor20", Color.FromArgb(51, color.R, color.G, color.B));

            resourceDictionary.Add("Fluent.Ribbon.Brushes.HighlightBrush", ((Color)resourceDictionary["Fluent.Ribbon.Colors.HighlightColor"]).GetSolidColorBrush());
            resourceDictionary.Add("Fluent.Ribbon.Brushes.AccentBaseColorBrush", ((Color)resourceDictionary["Fluent.Ribbon.Colors.AccentBaseColor"]).GetSolidColorBrush());
            resourceDictionary.Add("Fluent.Ribbon.Brushes.AccentColorBrush80", ((Color)resourceDictionary["Fluent.Ribbon.Colors.AccentColor80"]).GetSolidColorBrush());
            resourceDictionary.Add("Fluent.Ribbon.Brushes.AccentColorBrush60", ((Color)resourceDictionary["Fluent.Ribbon.Colors.AccentColor60"]).GetSolidColorBrush());
            resourceDictionary.Add("Fluent.Ribbon.Brushes.AccentColorBrush40", ((Color)resourceDictionary["Fluent.Ribbon.Colors.AccentColor40"]).GetSolidColorBrush());
            resourceDictionary.Add("Fluent.Ribbon.Brushes.AccentColorBrush20", ((Color)resourceDictionary["Fluent.Ribbon.Colors.AccentColor20"]).GetSolidColorBrush());

            if (DynamicallyDetermineIdealTextColor)
            {
                resourceDictionary.Add("Fluent.Ribbon.Colors.IdealForegroundColor", GetIdealTextColor(color));
            }
            else
            {
                // White since we use the light them
                resourceDictionary.Add("Fluent.Ribbon.Colors.IdealForegroundColor", Colors.White);
            }

            resourceDictionary.Add("Fluent.Ribbon.Brushes.IdealForegroundColorBrush", ((Color)resourceDictionary["Fluent.Ribbon.Colors.IdealForegroundColor"]).GetSolidColorBrush());
            resourceDictionary.Add("Fluent.Ribbon.Brushes.IdealForegroundDisabledBrush", ((Color)resourceDictionary["Fluent.Ribbon.Colors.IdealForegroundColor"]).GetSolidColorBrush(0.4d));

            // Button
            resourceDictionary.Add("Fluent.Ribbon.Brushes.Button.MouseOver.BorderBrush", ((Color)resourceDictionary["Fluent.Ribbon.Colors.AccentColor40"]).GetSolidColorBrush());
            resourceDictionary.Add("Fluent.Ribbon.Brushes.Button.MouseOver.Background", ((Color)resourceDictionary["Fluent.Ribbon.Colors.AccentColor20"]).GetSolidColorBrush());
            resourceDictionary.Add("Fluent.Ribbon.Brushes.Button.Pressed.BorderBrush", ((Color)resourceDictionary["Fluent.Ribbon.Colors.AccentColor60"]).GetSolidColorBrush());
            resourceDictionary.Add("Fluent.Ribbon.Brushes.Button.Pressed.Background", ((Color)resourceDictionary["Fluent.Ribbon.Colors.AccentColor40"]).GetSolidColorBrush());

            // ToggleButton
            resourceDictionary.Add("Fluent.Ribbon.Brushes.ToggleButton.Checked.Background", ((Color)resourceDictionary["Fluent.Ribbon.Colors.AccentColor20"]).GetSolidColorBrush());
            resourceDictionary.Add("Fluent.Ribbon.Brushes.ToggleButton.Checked.BorderBrush", ((Color)resourceDictionary["Fluent.Ribbon.Colors.HighlightColor"]).GetSolidColorBrush());
            resourceDictionary.Add("Fluent.Ribbon.Brushes.ToggleButton.CheckedMouseOver.Background", ((Color)resourceDictionary["Fluent.Ribbon.Colors.AccentColor20"]).GetSolidColorBrush());
            resourceDictionary.Add("Fluent.Ribbon.Brushes.ToggleButton.CheckedMouseOver.BorderBrush", ((Color)resourceDictionary["Fluent.Ribbon.Colors.AccentColor60"]).GetSolidColorBrush());

            // GalleryItem
            resourceDictionary.Add("Fluent.Ribbon.Brushes.GalleryItem.MouseOver", ((Color)resourceDictionary["Fluent.Ribbon.Colors.AccentColor20"]).GetSolidColorBrush());
            resourceDictionary.Add("Fluent.Ribbon.Brushes.GalleryItem.Selected", ((Color)resourceDictionary["Fluent.Ribbon.Colors.AccentColor40"]).GetSolidColorBrush());
            resourceDictionary.Add("Fluent.Ribbon.Brushes.GalleryItem.Pressed", ((Color)resourceDictionary["Fluent.Ribbon.Colors.AccentColor60"]).GetSolidColorBrush());

            // WindowCommands
            resourceDictionary.Add("FFluent.Ribbon.Brushes.WindowCommands.CaptionButton.MouseOver.Background", ((Color)resourceDictionary["Fluent.Ribbon.Colors.AccentColor20"]).GetSolidColorBrush());
            resourceDictionary.Add("Fluent.Ribbon.Brushes.WindowCommands.CaptionButton.Pressed.Background", ((Color)resourceDictionary["Fluent.Ribbon.Colors.AccentColor40"]).GetSolidColorBrush());
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
