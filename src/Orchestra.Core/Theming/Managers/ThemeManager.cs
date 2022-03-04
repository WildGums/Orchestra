namespace Orchestra.Theming
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using Catel;
    using Catel.Logging;
    using ControlzEx.Theming;
    using MethodTimer;
    using Orc.Theming;

    public class ThemeManager : IThemeManager
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IAccentColorService _accentColorService;
        private readonly IBaseColorSchemeService _baseColorSchemeService;
        private readonly ControlzEx.Theming.ThemeManager _themeManager;

        private readonly Dictionary<string, bool> _resourceDictionaryExists = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _addedResourceDictionaries = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        protected bool _ensuredOrchestraThemes;

        public ThemeManager(IAccentColorService accentColorService, IBaseColorSchemeService baseColorSchemeService)
        {
            Argument.IsNotNull(() => accentColorService);
            Argument.IsNotNull(() => baseColorSchemeService);

            _accentColorService = accentColorService;
            _baseColorSchemeService = baseColorSchemeService;
            _themeManager = ControlzEx.Theming.ThemeManager.Current;

            _accentColorService.AccentColorChanged += OnAccentColorChanged;
            _baseColorSchemeService.BaseColorSchemeChanged += OnBaseColorSchemeChanged;
        }

        private void OnAccentColorChanged(object sender, EventArgs e)
        {
            SynchronizeTheme();
        }

        private void OnBaseColorSchemeChanged(object sender, EventArgs e)
        {
            SynchronizeTheme();
        }

        public virtual void SynchronizeTheme()
        {
            Log.Debug("Synchronizing theme");

            EnsureOrchestraTheme(false);

            var themeGenerator = ControlzEx.Theming.RuntimeThemeGenerator.Current;

            var generatedTheme = themeGenerator.GenerateRuntimeTheme(_baseColorSchemeService.GetBaseColorScheme(), _accentColorService.GetAccentColor());
            if (generatedTheme is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Failed to generate runtime theme");
            }

            ChangeTheme(generatedTheme);
        }

        [Time]
        private void ChangeTheme(Theme generatedTheme)
        {
            _themeManager.ChangeTheme(Application.Current, generatedTheme);
        }

        /// <summary>
        /// Ensures the application themes by using the assembly and the <c>/Themes/Generic.xaml</c>.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="createStyleForwarders">if set to <c>true</c>, style forwarders will be created.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="assembly" /> is <c>null</c>.</exception>
        public virtual void EnsureApplicationThemes(Assembly assembly, bool createStyleForwarders = false)
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
        [Time("{resourceDictionaryUri}")]
        public virtual void EnsureApplicationThemes(string resourceDictionaryUri, bool createStyleForwarders = false)
        {
            Argument.IsNotNullOrWhitespace(() => resourceDictionaryUri);

            // Check whether this uri exists in order to prevent first chance exceptions
            var resourceExists = IsResourceDictionaryAvailable(resourceDictionaryUri);
            if (resourceExists)
            {
                var uri = new Uri(resourceDictionaryUri, UriKind.RelativeOrAbsolute);
                var dict = new ResourceDictionary
                {
                    Source = uri
                };

                EnsureApplicationThemes(dict, createStyleForwarders);
            }
        }

        [Time]
        public virtual void EnsureApplicationThemes(ResourceDictionary resourceDictionary, bool createStyleForwarders = false)
        {
            Argument.IsNotNull(() => resourceDictionary);

            EnsureOrchestraTheme(createStyleForwarders);

            try
            {
                var applicationResourcesDictionary = GetTargetApplicationResourceDictionary();
                if (applicationResourcesDictionary is not null)
                {
                    var alreadyAdded = IsResourceDictionaryAlreadyAdded(applicationResourcesDictionary, resourceDictionary);
                    if (!alreadyAdded)
                    {
                        AddResourceDictionary(applicationResourcesDictionary, resourceDictionary);
                    }

                    // Style forwarders only make sense once something actually changed. If it's a cascaded
                    // call, callers should explicitly call CreateStyleForwarders manually
                    if (createStyleForwarders)
                    {
                        StyleHelper.CreateStyleForwardersForDefaultStyles(applicationResourcesDictionary);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning(ex, $"Failed to add application theme '{resourceDictionary?.Source}'");
            }
        }

        [Time]
        protected virtual bool IsResourceDictionaryAlreadyAdded(ResourceDictionary applicationResourcesDictionary,
            ResourceDictionary resourceDictionary)
        {
            var source = resourceDictionary.Source;
            var uri = source?.ToString();
            if (!string.IsNullOrWhiteSpace(uri))
            {
                if (_addedResourceDictionaries.Contains(uri))
                {
                    Log.Debug("Returning IsResourceDictionaryAlreadyAdded from cache");
                    return true;
                }

                Log.Debug("Returning IsResourceDictionaryAlreadyAdded by checking source");

                // Defined, check by uri
                return (from dic in applicationResourcesDictionary.MergedDictionaries
                        where dic.Source is not null && dic.Source == source
                        select dic).Any();
            }

            Log.Debug("Returning IsResourceDictionaryAlreadyAdded by checking reference");

            // Runtime, check by instance since source is null
            return (from dic in applicationResourcesDictionary.MergedDictionaries
                    where ReferenceEquals(dic, resourceDictionary)
                    select dic).Any();
        }

        [Time]
        protected virtual void AddResourceDictionary(ResourceDictionary applicationResourcesDictionary,
            ResourceDictionary resourceDictionary)
        {
            applicationResourcesDictionary.MergedDictionaries.Add(resourceDictionary);

            var uri = resourceDictionary.Source?.ToString();
            if (!string.IsNullOrWhiteSpace(uri))
            {
                _addedResourceDictionaries.Add(uri);
            }
        }

        [Time]
        protected virtual ResourceDictionary GetTargetApplicationResourceDictionary()
        {
            var application = Application.Current;
            if (application is null)
            {
                throw Log.ErrorAndCreateException<OrchestraException>("Application.Current is null, cannot ensure application themes");
            }

            // Convenience fix. *If* the only merged dictionary is /themes/generic.xaml, we
            // will use that instead
            var applicationResourcesDictionary = application.Resources;
            //if (applicationResourcesDictionary.MergedDictionaries.Count == 1)
            //{
            //    var singleMergedDictionary = applicationResourcesDictionary.MergedDictionaries[0];
            //    if (singleMergedDictionary.Source?.ToString().EqualsIgnoreCase("/themes/generic.xaml") ?? false)
            //    {
            //        applicationResourcesDictionary = singleMergedDictionary;

            //        //// Are we currently merging the apps own /themes/generic.xaml?
            //        //var appGenericThemesDictionaryName = $"/{application.GetType().Assembly.GetName().Name};component/themes/generic.xaml";
            //        //if (appGenericThemesDictionaryName.EqualsIgnoreCase(resourceDictionaryUri))
            //        //{
            //        //    // Already included
            //        //    Log.Debug($"No need to merge '{appGenericThemesDictionaryName}', already merged");
            //        //    return null;
            //        //}

            //        Log.Debug($"Falling back to '/themes/generic.xaml' instead of app resource dictionary");
            //    }
            //}

            return applicationResourcesDictionary;
        }

        /// <summary>
        /// Checks whether the specified resource dictionary is available as resource.
        /// </summary>
        /// <param name="resourceDictionaryUri">The resource dictionary uri.</param>
        /// <returns></returns>
        [Time("{resourceDictionaryUri}")]
        public virtual bool IsResourceDictionaryAvailable(string resourceDictionaryUri)
        {
            if (_resourceDictionaryExists.TryGetValue(resourceDictionaryUri, out var existingValue))
            {
                return existingValue;
            }

            var exists = IsResourceDictionaryAvailableUncached(resourceDictionaryUri);

            _resourceDictionaryExists[resourceDictionaryUri] = exists;

            return false;
        }

        //[Time("{resourceDictionaryUri}")] // 9 ms
        protected virtual bool IsResourceDictionaryAvailableUncached(string resourceDictionaryUri)
        {
            var expectedResourceNames = resourceDictionaryUri.Split(";component/", StringSplitOptions.None);
            if (expectedResourceNames.Length == 2)
            {
                // Part 1 is assembly
                //var assemblyName = expectedResourceNames[0].Replace("/", string.Empty, StringComparison.Ordinal);
                var assemblyName = expectedResourceNames[0].Trim('/');
                
                var assembly = GetAssembly(assemblyName);
                if (assembly is not null)
                {
                    // Part 2 is resource name
                    var resourceName = expectedResourceNames[1];

                    var exists = IsResourceDictionaryAvailableUncached(resourceDictionaryUri, assembly, resourceName);
                    return exists;
                    //return true;
                }
            }

            Log.Debug($"Failed to confirm that resource '{resourceDictionaryUri}' exists");

            return false;
        }

        //[Time("{resourceDictionaryUri}")] // 1 ms
        protected virtual bool IsResourceDictionaryAvailableUncached(string resourceDictionaryUri, Assembly assembly, string expectedResourceName)
        {
            // Orchestra.Core.g.resources
            var generatedResourceName = $"{assembly.GetName().Name}.g.resources";

            using (var resourceStream = assembly.GetManifestResourceStream(generatedResourceName))
            {
                if (resourceStream is null)
                {
                    Log.Debug($"Could not find generated resources @ '{generatedResourceName}', assuming the resource dictionary '{resourceDictionaryUri}' does not exist");

                    return false;
                }

                var relativeResourceName = expectedResourceName.Replace(".xaml", ".baml");

                using (var resourceReader = new System.Resources.ResourceReader(resourceStream))
                {
                    foreach (var resource in resourceReader.Cast<DictionaryEntry>())
                    {
                        if (((string)resource.Key).EqualsIgnoreCase(relativeResourceName))
                        {
                            //Log.Debug($"Resource '{resourceDictionaryUri}' exists");
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        [Time("{assemblyName}")]
        protected virtual Assembly GetAssembly(string assemblyName)
        {
            var assembly = (from x in AppDomain.CurrentDomain.GetAssemblies()
                            where x.GetName().Name.EqualsIgnoreCase(assemblyName)
                            select x).FirstOrDefault();

            return assembly;
        }

        /// <summary>
        /// Ensures the orchestra theme.
        /// </summary>
        /// <param name="createStyleForwarders">if set to <c>true</c>, create style forwarders.</param>
        [Time]
        protected virtual void EnsureOrchestraTheme(bool createStyleForwarders)
        {
            if (_ensuredOrchestraThemes)
            {
                return;
            }

            _ensuredOrchestraThemes = true;

            EnsureApplicationThemes(GetType().Assembly, createStyleForwarders);
        }
    }
}
