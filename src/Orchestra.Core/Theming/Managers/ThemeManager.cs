namespace Orchestra.Theming
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using Catel;
    using Catel.Logging;
    using System.Collections;
    using ControlzEx.Theming;
    using MethodTimer;
    using Orc.Theming;

    public class ThemeManager : IThemeManager
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IAccentColorService _accentColorService;
        private readonly IBaseColorSchemeService _baseColorSchemeService;
        private readonly ControlzEx.Theming.ThemeManager _themeManager;

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
        public virtual void EnsureApplicationThemes(string resourceDictionaryUri, bool createStyleForwarders = false)
        {
            Argument.IsNotNullOrWhitespace(() => resourceDictionaryUri);

            EnsureOrchestraTheme(createStyleForwarders);

            try
            {
                // Check whether this uri exists in order to prevent first chance exceptions
                var resourceExists = IsResourceDictionaryAvailable(resourceDictionaryUri);
                if (resourceExists)
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
        /// Checks whether the specified resource dictionary is available as resource.
        /// </summary>
        /// <param name="resourceDictionaryUri">The resource dictionary uri.</param>
        /// <returns></returns>
        public virtual bool IsResourceDictionaryAvailable(string resourceDictionaryUri)
        {
            var expectedResourceNames = resourceDictionaryUri.Split(new[] { ";component/" }, StringSplitOptions.RemoveEmptyEntries);
            if (expectedResourceNames.Length == 2)
            {
                // Part 1 is assembly
                var assemblyName = expectedResourceNames[0].Replace("/", string.Empty);
                var assembly = (from x in AppDomain.CurrentDomain.GetAssemblies()
                                where x.GetName().Name.EqualsIgnoreCase(assemblyName)
                                select x).FirstOrDefault();
                if (assembly != null)
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

                        var relativeResourceName = expectedResourceNames[1].Replace(".xaml", ".baml");

                        using (var reader = new System.Resources.ResourceReader(resourceStream))
                        {
                            var exists = (from x in reader.Cast<DictionaryEntry>()
                                          where ((string)x.Key).EqualsIgnoreCase(relativeResourceName)
                                          select x).Any();
                            if (exists)
                            {
                                Log.Debug($"Resource '{resourceDictionaryUri}' exists");
                                return true;
                            }
                        }
                    }
                }
            }

            Log.Debug($"Failed to confirm that resource '{resourceDictionaryUri}' exists");

            return false;
        }

        /// <summary>
        /// Ensures the orchestra theme.
        /// </summary>
        /// <param name="createStyleForwarders">if set to <c>true</c>, create style forwarders.</param>
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
