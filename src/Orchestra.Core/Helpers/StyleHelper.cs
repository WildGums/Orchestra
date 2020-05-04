// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StyleHelper.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using Catel;
    using Catel.Logging;
    using MethodTimer;

    /// <summary>
    /// Helper class for WPF styles and themes.
    /// </summary>
    public static class StyleHelper
    {
        #region Fields
        /// <summary>
        /// The log.
        /// </summary>
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets a value indicating whether style forwarding is enabled. Style forwarding can be
        /// enabled by calling one of the <see cref="CreateStyleForwardersForDefaultStyles(string)"/> overloads.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is style forwarding enabled; otherwise, <c>false</c>.
        /// </value>
        public static bool IsStyleForwardingEnabled { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Ensures that an application instance exists and the styles are applied to the application. This method is extremely useful
        /// to apply when WPF is hosted (for example, when loaded as plugin of a non-WPF application).
        /// </summary>
        /// <param name="applicationResourceDictionary">The application resource dictionary.</param>
        /// <param name="defaultPrefix">The default prefix, uses to determine the styles as base for other styles.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="applicationResourceDictionary"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="defaultPrefix"/> is <c>null</c> or whitespace.</exception>
        public static void EnsureApplicationResourcesAndCreateStyleForwarders(Uri applicationResourceDictionary, string defaultPrefix = DefaultKeyPrefix)
        {
            Argument.IsNotNull("applicationResourceDictionary", applicationResourceDictionary);
            Argument.IsNotNullOrWhitespace("defaultPrefix", defaultPrefix);

            if (Application.Current is null)
            {
                try
                {
                    // Ensure we have an application
                    new Application();

                    var resourceDictionary = Application.LoadComponent(applicationResourceDictionary) as ResourceDictionary;
                    if (resourceDictionary != null)
                    {
                        Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
                    }

                    CreateStyleForwardersForDefaultStyles(Application.Current.Resources, defaultPrefix);

                    // Create an invisible dummy window to make sure that this is the main window
                    var dummyMainWindow = new Window();
                    dummyMainWindow.Visibility = Visibility.Hidden;
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Failed to ensure application resources");
                }
            }
        }

        /// <summary>
        /// Creates style forwarders for default styles. This means that all styles found in the theme that are
        /// name like Default[CONTROLNAME]Style (i.e. "DefaultButtonStyle") will be used as default style for the
        /// control.
        /// This method will use the current application to retrieve the resources. The forwarders will be written to the same dictionary.
        /// </summary>
        /// <param name="defaultPrefix">The default prefix, uses to determine the styles as base for other styles.</param>
        /// <exception cref="ArgumentException">The <paramref name="defaultPrefix"/> is <c>null</c> or whitespace.</exception>
        public static void CreateStyleForwardersForDefaultStyles(string defaultPrefix = DefaultKeyPrefix)
        {
            CreateStyleForwardersForDefaultStyles(Application.Current.Resources, defaultPrefix);
        }

        /// <summary>
        /// Creates style forwarders for default styles. This means that all styles found in the theme that are
        /// name like Default[CONTROLNAME]Style (i.e. "DefaultButtonStyle") will be used as default style for the
        /// control.
        /// This method will use the passed resources, but the forwarders will be written to the same dictionary as
        /// the source dictionary.
        /// </summary>
        /// <param name="sourceResources">Resource dictionary to read the keys from (thus that contains the default styles).</param>
        /// <param name="defaultPrefix">The default prefix, uses to determine the styles as base for other styles.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="sourceResources"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="defaultPrefix"/> is <c>null</c> or whitespace.</exception>
        public static void CreateStyleForwardersForDefaultStyles(ResourceDictionary sourceResources, string defaultPrefix = DefaultKeyPrefix)
        {
            CreateStyleForwardersForDefaultStyles(sourceResources, sourceResources, defaultPrefix);
        }

        /// <summary>
        /// Creates style forwarders for default styles. This means that all styles found in the theme that are
        /// name like Default[CONTROLNAME]Style (i.e. "DefaultButtonStyle") will be used as default style for the
        /// control.
        /// <para/>
        /// This method will use the passed resources.
        /// </summary>
        /// <param name="sourceResources">Resource dictionary to read the keys from (thus that contains the default styles).</param>
        /// <param name="targetResources">Resource dictionary where the forwarders will be written to.</param>
        /// <param name="defaultPrefix">The default prefix, uses to determine the styles as base for other styles.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="sourceResources"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="targetResources"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="defaultPrefix"/> is <c>null</c> or whitespace.</exception>
        public static void CreateStyleForwardersForDefaultStyles(ResourceDictionary sourceResources, ResourceDictionary targetResources, string defaultPrefix = DefaultKeyPrefix)
        {
            CreateStyleForwardersForDefaultStyles(sourceResources, sourceResources, targetResources, defaultPrefix);
        }

        /// <summary>
        /// Creates style forwarders for default styles. This means that all styles found in the theme that are
        /// name like Default[CONTROLNAME]Style (i.e. "DefaultButtonStyle") will be used as default style for the
        /// control.
        /// This method will use the passed resources.
        /// </summary>
        /// <param name="rootResourceDictionary">The root resource dictionary.</param>
        /// <param name="sourceResources">Resource dictionary to read the keys from (thus that contains the default styles).</param>
        /// <param name="targetResources">Resource dictionary where the forwarders will be written to.</param>
        /// <param name="defaultPrefix">The default prefix, uses to determine the styles as base for other styles.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="rootResourceDictionary" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="sourceResources" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="targetResources" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="defaultPrefix" /> is <c>null</c> or whitespace.</exception>
        [Time]
        public static void CreateStyleForwardersForDefaultStyles(ResourceDictionary rootResourceDictionary, ResourceDictionary sourceResources,
            ResourceDictionary targetResources, string defaultPrefix = DefaultKeyPrefix)
        {
            Argument.IsNotNull("rootResourceDictionary", rootResourceDictionary);
            Argument.IsNotNull("sourceResources", sourceResources);
            Argument.IsNotNull("targetResources", targetResources);
            Argument.IsNotNullOrWhitespace("defaultPrefix", defaultPrefix);

            var foundDefaultStyles = FindDefaultStyles(sourceResources, defaultPrefix).ToList();

            // Important: invert order and skip duplicates
            var defaultStylesDictionary = new Dictionary<string, StyleInfo>();

            for (var i = foundDefaultStyles.Count - 1; i >= 0; i--)
            {
                var styleInfo = foundDefaultStyles[i];

                if (defaultStylesDictionary.TryGetValue(styleInfo.SourceKey, out var existingStyleInfo))
                {
                    Log.Debug($"Default style for '{styleInfo.TargetType.Name}' already coming from '{existingStyleInfo.SourceDictionary}', ignoring registration from '{styleInfo.SourceDictionary}'");
                    continue;
                }

                defaultStylesDictionary[styleInfo.SourceKey] = styleInfo;
            }

            // Important note: Styles are coming from Orchestra.Core (implicit) by default. In some cases (such as MahApps, or any other UI lib) 
            // we need to override these styles and ignore them (remove them).
            // 
            // Option A: [Orchestra.Core | Style with {x:Type Button}]                ===\
            //                                                                            ===> [Orchestra.Core | DefaultStyles.xaml with margins] ===> [Final style with key {x:Type Button}]
            // Option B: [Orchestra.Shell.MahApps | Style with "DefaultButtonStyle"]  ===/

            foreach (var styleInfoKeyValuePair in defaultStylesDictionary)
            {
                var defaultStyle = styleInfoKeyValuePair.Value.Style;

                try
                {
                    var targetType = defaultStyle.TargetType;
                    if (targetType != null)
                    {
                        var style = new Style(targetType, defaultStyle);

                        // Always overwrite
                        targetResources[targetType] = style;
                    }
                }
                catch (Exception ex)
                {
                    var tag = defaultStyle?.TargetType?.ToString() ?? defaultStyle?.ToString();
                    Log.Warning(ex, "Failed to complete the style for '{0}'", tag);
                }
            }

            IsStyleForwardingEnabled = true;
        }

        /// <summary>
        /// Finds all the the default styles definitions
        /// </summary>
        /// <param name="sourceResources">The source resources.</param>
        /// <param name="defaultPrefix">The default prefix.</param>
        /// <returns>An enumerable of default styles.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="sourceResources"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="defaultPrefix"/> is <c>null</c> or whitespace.</exception>
        private static IEnumerable<StyleInfo> FindDefaultStyles(ResourceDictionary sourceResources, string defaultPrefix)
        {
            Argument.IsNotNull("sourceResources", sourceResources);
            Argument.IsNotNullOrWhitespace("defaultPrefix", defaultPrefix);

            var styles = new List<StyleInfo>();

            var keys = (from key in sourceResources.Keys as ICollection<object>
                        let stringKey = key as string
                        where stringKey != null &&
                              (stringKey).StartsWith(defaultPrefix, StringComparison.Ordinal) &&
                              (stringKey).EndsWith(DefaultKeyPostfix, StringComparison.Ordinal)
                        select stringKey).Distinct().ToList();

            foreach (var key in keys)
            {
                try
                {
                    var style = sourceResources[key] as Style;
                    if (style != null)
                    {
                        styles.Add(new StyleInfo
                        {
                            Style = style,
                            SourceDictionary = sourceResources,
                            SourceKey = key,
                            TargetType = style.TargetType,
                        });
                    }
                }
                catch (Exception ex)
                {
                    Log.Warning(ex, "Failed to add a default style ('{0}') definition to the list of styles", key);
                }
            }

            foreach (var resourceDictionary in sourceResources.MergedDictionaries)
            {
                styles.AddRange(FindDefaultStyles(resourceDictionary, defaultPrefix));
            }

            return styles;
        }

        /// <summary>
        /// Clones a style when the style is based on a control.
        /// </summary>
        /// <param name="rootResourceDictionary">The root resource dictionary.</param>
        /// <param name="style">The style.</param>
        /// <param name="basedOnType">Type which the style is based on.</param>
        /// <returns><see cref="Style"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="rootResourceDictionary"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="style"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="basedOnType"/> is <c>null</c>.</exception>
        /// <remarks>
        /// This method is introduced due to the lack of the ability to use DynamicResource for the BasedOn property when
        /// defining styles inside a derived theme.
        /// <para />
        /// Should be used in combination with the <c>RecreateDefaultStylesBasedOnTheme</c> method.
        /// </remarks>
        private static Style CloneStyleIfBasedOnControl(ResourceDictionary rootResourceDictionary, Style style, Type basedOnType)
        {
            Argument.IsNotNull("rootResourceDictionary", rootResourceDictionary);
            Argument.IsNotNull("style", style);
            Argument.IsNotNull("basedOnType", basedOnType);

            var newStyle = new Style(style.TargetType, rootResourceDictionary[basedOnType] as Style);

            foreach (var setter in style.Setters)
            {
                newStyle.Setters.Add(setter);
            }

            foreach (var trigger in style.Triggers)
            {
                newStyle.Triggers.Add(trigger);
            }

            return newStyle;
        }
        #endregion

        #region Constants
        /// <summary>
        /// Prefix of a default style key.
        /// </summary>
        private const string DefaultKeyPrefix = "Default";

        /// <summary>
        /// Postfix of a default style key.
        /// </summary>
        private const string DefaultKeyPostfix = "Style";
        #endregion

        private class StyleInfo : IEquatable<StyleInfo>
        {
            public StyleInfo()
            {
                //Order = UniqueIdentifierHelper.GetUniqueIdentifier(typeof(StyleInfo));
            }

            //public int Order { get; private set; }

            public Type TargetType { get; set; }

            public ResourceDictionary SourceDictionary { get; set; }

            public string SourceKey { get; set; }

            public Style Style { get; set; }

            public override bool Equals(object obj)
            {
                return Equals(obj as StyleInfo);
            }

            public bool Equals(StyleInfo other)
            {
                return other != null &&
                       SourceKey == other.SourceKey;
            }

            public override int GetHashCode()
            {
                return -1913166433 + EqualityComparer<string>.Default.GetHashCode(SourceKey);
            }

            public override string ToString()
            {
                return $"{TargetType} ({SourceKey} @ {SourceDictionary.Source})";
            }

            public static bool operator ==(StyleInfo left, StyleInfo right)
            {
                return EqualityComparer<StyleInfo>.Default.Equals(left, right);
            }

            public static bool operator !=(StyleInfo left, StyleInfo right)
            {
                return !(left == right);
            }
        }
    }
}
