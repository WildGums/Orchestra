namespace Orchestra.Services
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using System.Windows;
    using Catel.Reflection;

    public class XamlResourceService : IXamlResourceService
    {
        public virtual IEnumerable<ResourceDictionary> GetApplicationResourceDictionaries()
        {
            var resourceDictionaries = new List<ResourceDictionary>();

            // Orchestra.Core
            resourceDictionaries.Add(GetResourceDictionaryFromAssembly(typeof(IXamlResourceService).Assembly));

            // Shell specific
            resourceDictionaries.Add(GetResourceDictionaryFromAssembly(typeof(ApplicationInitializationServiceBase).Assembly));

            // Current app specific
            resourceDictionaries.Add(GetResourceDictionaryFromAssembly(AssemblyHelper.GetEntryAssembly()));

            return resourceDictionaries;
        }

        protected virtual ResourceDictionary GetResourceDictionaryFromAssembly(Assembly assembly)
        {
            var uri = GetResourceDictionaryUriFromAssembly(assembly);

            var resourceDictionary = new ResourceDictionary
            {
                Source = uri
            };

            return resourceDictionary;
        }

        protected virtual Uri GetResourceDictionaryUriFromAssembly(Assembly assembly)
        {
            var uri = string.Format("/{0};component/themes/generic.xaml", assembly.GetName().Name);
            return new Uri(uri, UriKind.RelativeOrAbsolute);
        }
    }
}
