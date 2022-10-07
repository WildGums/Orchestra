namespace Orchestra.Services
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Windows;
    using Catel.Reflection;

    public class XamlResourceService : IXamlResourceService
    {
        public virtual IEnumerable<ResourceDictionary> GetApplicationResourceDictionaries()
        {
            var resourceDictionaries = new List<ResourceDictionary>
            {
                // Orchestra.Core
                GetResourceDictionaryFromAssembly(typeof(IXamlResourceService).Assembly),

                // Shell specific
                GetResourceDictionaryFromAssembly(typeof(ApplicationInitializationServiceBase).Assembly),

                // Current app specific
                GetResourceDictionaryFromAssembly(AssemblyHelper.GetRequiredEntryAssembly())
            };

            return resourceDictionaries;
        }

        protected virtual ResourceDictionary GetResourceDictionaryFromAssembly(Assembly assembly)
        {
            ArgumentNullException.ThrowIfNull(assembly);

            var uri = GetResourceDictionaryUriFromAssembly(assembly);

            var resourceDictionary = new ResourceDictionary
            {
                Source = uri
            };

            return resourceDictionary;
        }

        protected virtual Uri GetResourceDictionaryUriFromAssembly(Assembly assembly)
        {
            ArgumentNullException.ThrowIfNull(assembly);

            var uri = string.Format("/{0};component/themes/generic.xaml", assembly.GetName().Name);
            return new Uri(uri, UriKind.RelativeOrAbsolute);
        }
    }
}
