// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThemeHelper.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System;
    using System.Reflection;
    using System.Windows;
    using Catel;
    using Catel.Logging;

    public static class ThemeHelper
    {
        /// <summary>
        /// The log.
        /// </summary>
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Ensures the application themes by using the assembly and the <c>/Themes/Generic.xaml</c>.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="assembly"/> is <c>null</c>.</exception>
        public static void EnsureApplicationThemes(Assembly assembly)
        {
            Argument.IsNotNull("assembly", assembly);

            var uri = string.Format("/{0};component/themes/generic.xaml", assembly.GetName().Name);
            
            EnsureApplicationThemes(uri);
        }

        /// <summary>
        /// Ensures the application themes.
        /// </summary>
        /// <param name="resourceDictionaryUri">The resource dictionary.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="resourceDictionaryUri" /> is <c>null</c> or whitespace.</exception>
        public static void EnsureApplicationThemes(string resourceDictionaryUri)
        {
            Argument.IsNotNullOrWhitespace("resourceDictionaryUri", resourceDictionaryUri);

            var uri = new Uri(resourceDictionaryUri, UriKind.RelativeOrAbsolute);

            var application = Application.Current;
            if (application == null)
            {
                Log.ErrorAndThrowException<OrchestraException>("Application.Current is null, cannot ensure application themes");
            }

            application.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = uri });
        }
    }
}