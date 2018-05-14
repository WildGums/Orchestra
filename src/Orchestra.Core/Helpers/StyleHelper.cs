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
    using Catel;
    using Catel.Logging;

    using Catel.Caching;
    using System.Windows.Markup;
    using StylesExplorer.MarkupReflection;
    using XmlNamespaceManager = System.Xml.XmlNamespaceManager;
    using System.Windows;
    using System.Xml;
    using Catel.Reflection;
    using Catel.Collections;

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

            if (Application.Current == null)
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
        public static void CreateStyleForwardersForDefaultStyles(ResourceDictionary sourceResources, ResourceDictionary targetResources,
            string defaultPrefix = DefaultKeyPrefix)
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
        /// <param name="recreateStylesBasedOnTheme">if set to <c>true</c>, the styles will be recreated with BasedOn on the current theme.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="rootResourceDictionary" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="sourceResources" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="targetResources" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="defaultPrefix" /> is <c>null</c> or whitespace.</exception>
        public static void CreateStyleForwardersForDefaultStyles(ResourceDictionary rootResourceDictionary, ResourceDictionary sourceResources,
            ResourceDictionary targetResources, string defaultPrefix = DefaultKeyPrefix, bool recreateStylesBasedOnTheme = false)
        {
            Argument.IsNotNull("rootResourceDictionary", rootResourceDictionary);
            Argument.IsNotNull("sourceResources", sourceResources);
            Argument.IsNotNull("targetResources", targetResources);
            Argument.IsNotNullOrWhitespace("defaultPrefix", defaultPrefix);

            var defaultStyles = FindDefaultStyles(sourceResources, defaultPrefix).ToList();

            for (int i = 0; i < defaultStyles.Count; i++)
            {
                var defaultStyle = defaultStyles[i];

                try
                {
                    var baseStyle = defaultStyle;

                    var targetType = defaultStyle.TargetType;
                    if (targetType != null)
                    {
                        var targetTypeName = targetType.Name;

                        // Step 1: if already defined in the target resources, we need to update it
                        var resourceDictionaryDefiningStyle = FindResourceDictionaryDeclaringType(targetResources, targetType, 2);
                        if (resourceDictionaryDefiningStyle != null)
                        {
                            var existingStyle = resourceDictionaryDefiningStyle[targetType] as Style;
                            if (existingStyle != null)
                            {
                                // Double check whether the style is not sealed, if so, use it as a base style instead
                                if (!existingStyle.IsSealed)
                                {
                                    //Log.Debug("Completing the style info for '{0}' with the additional info from the default style definition", targetType);

                                    resourceDictionaryDefiningStyle[targetType] = CompleteStyleWithAdditionalInfo(existingStyle, defaultStyle, false);
                                    continue;
                                }

                                // Create a new style with the style forwarders based on the source file
                                baseStyle = existingStyle;
                            }
                        }

                        //Log.Debug("Couldn't find style definition for '{0}', creating style forwarder", targetType);

                        var style = new Style(targetType, baseStyle);

                        if (!ReferenceEquals(baseStyle, defaultStyle))
                        {
                            // Copy the default style stuff
                            CompleteStyleWithAdditionalInfo(style, defaultStyle, true);
                        }

                        //if (!targetResources.Contains(targetType))
                        //{
                        // Always overwrite
                        targetResources[targetType] = style;
                        //}
                    }
                }
                catch (Exception ex)
                {
                    var tag = defaultStyle?.TargetType?.ToString() ?? defaultStyle?.ToString();
                    Log.Warning(ex, "Failed to complete the style for '{0}'", tag);
                }
            }

            if (recreateStylesBasedOnTheme)
            {
                RecreateDefaultStylesBasedOnTheme(rootResourceDictionary, targetResources, defaultPrefix);
            }

            IsStyleForwardingEnabled = true;
        }

        /// <summary>
        /// Finds the <see cref="ResourceDictionary"/> declaring the real style for the target type.
        /// </summary>
        /// <param name="rootResourceDictionary">The root resource dictionary.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="maxDepth">The max depth to search.</param>
        /// <returns><see cref="ResourceDictionary"/> in which the style is defined, or <c>null</c> if not found.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="rootResourceDictionary"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="targetType"/> is <c>null</c>.</exception>
        private static ResourceDictionary FindResourceDictionaryDeclaringType(ResourceDictionary rootResourceDictionary, Type targetType,
            int maxDepth = int.MaxValue)
        {
            Argument.IsNotNull("rootResourceDictionary", rootResourceDictionary);
            Argument.IsNotNull("targetType", targetType);

            var styleKey = (from key in rootResourceDictionary.Keys as ICollection<object>
                            let typeKey = key as Type
                            where typeKey != null && typeKey == targetType
                            select typeKey).FirstOrDefault();
            if (styleKey != null)
            {
                return rootResourceDictionary;
            }

            if (maxDepth > 0)
            {
                maxDepth--;

                foreach (var mergedResourceDictionary in rootResourceDictionary.MergedDictionaries)
                {
                    var foundResourceDictionary = FindResourceDictionaryDeclaringType(mergedResourceDictionary, targetType, maxDepth);
                    if (foundResourceDictionary != null)
                    {
                        return foundResourceDictionary;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Finds all the the default styles definitions
        /// </summary>
        /// <param name="sourceResources">The source resources.</param>
        /// <param name="defaultPrefix">The default prefix.</param>
        /// <returns>An enumerable of default styles.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="sourceResources"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="defaultPrefix"/> is <c>null</c> or whitespace.</exception>
        private static IEnumerable<Style> FindDefaultStyles(ResourceDictionary sourceResources, string defaultPrefix)
        {
            Argument.IsNotNull("sourceResources", sourceResources);
            Argument.IsNotNullOrWhitespace("defaultPrefix", defaultPrefix);

            var styles = new List<Style>();

            var keys = (from key in sourceResources.Keys as ICollection<object>
                        let stringKey = key as string
                        where stringKey != null &&
                              (stringKey).StartsWith(defaultPrefix, StringComparison.Ordinal) &&
                              (stringKey).EndsWith(DefaultKeyPostfix, StringComparison.Ordinal)
                        select stringKey).Distinct().ToList();

            foreach (string key in keys)
            {
                try
                {
                    var style = sourceResources[key] as Style;
                    if (style != null)
                    {
                        styles.Add(style);
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
        /// Completes a style with additional info.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="styleWithAdditionalInfo">The style with additional info.</param>
        /// <param name="updateExistingStyle">If <c>true</c>, the existing style will be updated; otherwise a new style will be created. </param>
        /// <returns>New completed style.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="style"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="styleWithAdditionalInfo"/> is <c>null</c>.</exception>
        private static Style CompleteStyleWithAdditionalInfo(Style style, Style styleWithAdditionalInfo, bool updateExistingStyle)
        {
            Argument.IsNotNull("style", style);
            Argument.IsNotNull("styleWithAdditionalInfo", styleWithAdditionalInfo);

            Style newStyle = style;

            if (!updateExistingStyle)
            {
                newStyle = new Style(style.TargetType);
            }

            #region Copy style with additional info
            foreach (var setter in styleWithAdditionalInfo.Setters)
            {
                newStyle.Setters.Add(setter);
            }

            foreach (var trigger in styleWithAdditionalInfo.Triggers)
            {
                newStyle.Triggers.Add(trigger);
            }
            #endregion

            #region Copy original style
            if (!updateExistingStyle)
            {
                foreach (SetterBase setter in style.Setters)
                {
                    bool exists = (from styleSetter in newStyle.Setters
                                   where setter is Setter && ((Setter)styleSetter).Property == ((Setter)setter).Property
                                   select styleSetter).Any();
                    if (!exists)
                    {
                        newStyle.Setters.Add(setter);
                    }
                }

                foreach (var trigger in style.Triggers)
                {
                    newStyle.Triggers.Add(trigger);
                }
            }
            #endregion

            return newStyle;
        }

        /// <summary>
        /// Recreates the default styles based on theme.
        /// </summary>
        /// <param name="rootResourceDictionary">The root resource dictionary.</param>
        /// <param name="resources">The resources to fix.</param>
        /// <param name="defaultPrefix">The default prefix.</param>
        /// <remarks>
        /// This method is introduced due to the lack of the ability to use DynamicResource for the BasedOn property when
        /// defining styles inside a derived theme.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="rootResourceDictionary"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="resources"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="defaultPrefix"/> is <c>null</c> or whitespace.</exception>
        private static void RecreateDefaultStylesBasedOnTheme(ResourceDictionary rootResourceDictionary, ResourceDictionary resources, string defaultPrefix)
        {
            Argument.IsNotNull("rootResourceDictionary", rootResourceDictionary);
            Argument.IsNotNull("resources", resources);
            Argument.IsNotNull("defaultPrefix", defaultPrefix);

            var keys = (from key in resources.Keys as ICollection<object>
                        let stringKey = key as string
                        where stringKey != null &&
                              (stringKey).StartsWith(defaultPrefix, StringComparison.InvariantCulture) &&
                              (stringKey).EndsWith(DefaultKeyPostfix, StringComparison.InvariantCulture)
                        select stringKey).ToList();

            foreach (string key in keys)
            {
                var style = resources[key] as Style;
                if (style == null)
                {
                    continue;
                }

                var basedOnType = FindFrameworkElementStyleIsBasedOn(resources.Source, key);
                if (basedOnType == null)
                {
                    continue;
                }

                resources[key] = CloneStyleIfBasedOnControl(rootResourceDictionary, style, basedOnType);
            }

            foreach (var resourceDictionary in resources.MergedDictionaries)
            {
                RecreateDefaultStylesBasedOnTheme(rootResourceDictionary, resourceDictionary, defaultPrefix);
            }
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

        /// <summary>
        /// Cached decompiled XAML resource dictionaries.
        /// </summary>
        private static readonly CacheStorage<Uri, Tuple<XmlDocument, XmlNamespaceManager>> _resourceDictionaryCache = new CacheStorage<Uri, Tuple<XmlDocument, XmlNamespaceManager>>();

        /// <summary>
        /// Cached types of <see cref="FrameworkElement"/> belonging to the string representation of the type.
        /// </summary>
        private static readonly CacheStorage<string, Type> _styleToFrameworkElementTypeCache = new CacheStorage<string, Type>();

        /// <summary>
        /// Finds the <see cref="FrameworkElement"/> a specific style is based on.
        /// </summary>
        /// <param name="resourceDictionaryUri">The resource dictionary URI.</param>
        /// <param name="styleKey">The style key.</param>
        /// <returns>
        /// <see cref="Type"/> or <c>null</c> if the style is not based on a <see cref="FrameworkElement"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">The <paramref name="resourceDictionaryUri"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="styleKey"/> is <c>null</c>.</exception>
        /// <remarks>
        /// This method is introduced due to the lack of the ability to use DynamicResource for the BasedOn property when
        /// defining styles inside a derived theme.
        /// Should be used in combination with the <see cref="RecreateDefaultStylesBasedOnTheme"/> method.
        /// </remarks>
        private static Type FindFrameworkElementStyleIsBasedOn(Uri resourceDictionaryUri, string styleKey)
        {
            Argument.IsNotNull("resourceDictionaryUri", resourceDictionaryUri);
            Argument.IsNotNull("styleKey", styleKey);

            return _styleToFrameworkElementTypeCache.GetFromCacheOrFetch(styleKey, () =>
            {
                try
                {
                    var xmlDocInfo = GetResourceXmlDocument(resourceDictionaryUri);
                    var doc = xmlDocInfo.Item1;
                    var xmlNamespaceManager = xmlDocInfo.Item2;

                    string xpath = string.Format("/ctl:ResourceDictionary/ctl:Style[@x:Key='{0}']/@BasedOn", styleKey);
                    var xmlAttribute = doc.SelectSingleNode(xpath, xmlNamespaceManager) as XmlAttribute;
                    if (xmlAttribute == null)
                    {
                        Log.Warning("Style '{0}' does not have the 'BasedOn' attribute defined", styleKey);
                        return null;
                    }

                    string basedOnValue = xmlAttribute.Value;
                    basedOnValue = basedOnValue.Replace("StaticResource", string.Empty);
                    basedOnValue = basedOnValue.Replace("x:Type", string.Empty).Trim(' ', '{', '}');

                    #region Create xml type mapper
                    var xamlTypeMapper = new XamlTypeMapper(new[] { "PresentationFramework" });
                    foreach (XmlAttribute namespaceAttribute in doc.DocumentElement.Attributes)
                    {
                        string xmlNamespace = namespaceAttribute.Name.Replace("xmlns", string.Empty).TrimStart(':');

                        string value = namespaceAttribute.Value;
                        string clrNamespace = value;
                        string assemblyName = string.Empty;

                        if (clrNamespace.StartsWith("clr-namespace:"))
                        {
                            // We have a hit (formatting is normally one of the 2 below):
                            // * clr-namespace:[NAMESPACE]
                            // * clr-namespace:[NAMESPACE];assembly=[ASSEMBLY]
                            if (clrNamespace.Contains(";"))
                            {
                                clrNamespace = clrNamespace.Split(';')[0];
                            }
                            clrNamespace = clrNamespace.Replace("clr-namespace:", string.Empty);

                            if (value.Contains(";"))
                            {
                                assemblyName = value.Split(';')[1].Replace("assembly:", string.Empty);
                            }

                            xamlTypeMapper.AddMappingProcessingInstruction(xmlNamespace, clrNamespace, assemblyName);
                        }
                    }
                    #endregion

                    string[] splittedType = basedOnValue.Split(':');
                    string typeNamespace = (splittedType.Length == 2) ? splittedType[0] : "http://schemas.microsoft.com/winfx/2006/xaml/presentation";
                    string typeName = (splittedType.Length == 2) ? splittedType[1] : splittedType[0];
                    var type = xamlTypeMapper.GetType(typeNamespace, typeName);
                    if (type == null)
                    {
                        return null;
                    }

                    Log.Debug("Style '{0}' is based on type '{1}'", styleKey, type);

                    if ((type == typeof(FrameworkElement)) || type.IsSubclassOf(typeof(FrameworkElement)))
                    {
                        return type;
                    }

                    return null;
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Failed to find the framework element where style '{0}' is based on", styleKey);
                    return null;
                }
            });
        }

        private static Tuple<XmlDocument, XmlNamespaceManager> GetResourceXmlDocument(Uri resourceDictionaryUri)
        {
            return _resourceDictionaryCache.GetFromCacheOrFetch(resourceDictionaryUri, () =>
            {
                var streamResourceInfo = Application.GetResourceStream(resourceDictionaryUri);
                var reader = new XmlBamlReader(streamResourceInfo.Stream);

                var doc = new XmlDocument();
                doc.Load(reader);

                // Create namespace manager (all namespaces are required)
                var xmlNamespaceManager = new XmlNamespaceManager(doc.NameTable);
                foreach (XmlAttribute namespaceAttribute in doc.DocumentElement.Attributes)
                {
                    // Clean up namespace (remove xmlns prefix)
                    string xmlNamespace = namespaceAttribute.Name.Replace("xmlns", string.Empty).TrimStart(':');
                    xmlNamespaceManager.AddNamespace(xmlNamespace, namespaceAttribute.Value);
                }

                // Add a dummy node
                xmlNamespaceManager.AddNamespace("x", "http://schemas.microsoft.com/winfx/2006/xaml");
                xmlNamespaceManager.AddNamespace("ctl", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");

                return new Tuple<XmlDocument, XmlNamespaceManager>(doc, xmlNamespaceManager);
            });
        }
    }
}