namespace Orchestra
{
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Catel;

    public class ResourceBasedThirdPartyNotice : ThirdPartyNotice
    {
        public ResourceBasedThirdPartyNotice(string title, string url, string assemblyName, string relativeResourceName)
        {
            var assembly = Catel.Reflection.AssemblyHelper.GetLoadedAssemblies().First(x => x.GetName().Name.EqualsIgnoreCase(assemblyName));

            Initialize(title, url, assembly, assembly.GetName().Name, relativeResourceName);
        }

        public ResourceBasedThirdPartyNotice(string title, string url, string assemblyName, string rootNamespace, string relativeResourceName)
        {
            var assembly = Catel.Reflection.AssemblyHelper.GetLoadedAssemblies().First(x => x.GetName().Name.EqualsIgnoreCase(assemblyName));

            Initialize(title, url, assembly, rootNamespace, relativeResourceName);
        }

        public ResourceBasedThirdPartyNotice(string title, string url, Assembly assembly, string relativeResourceName)
        {
            Initialize(title, url, assembly, assembly.GetName().Name, relativeResourceName);
        }

        public ResourceBasedThirdPartyNotice(string title, string url, Assembly assembly, string rootNamespace, string relativeResourceName)
        {
            Initialize(title, url, assembly, rootNamespace, relativeResourceName);
        }

        private void Initialize(string title, string url, Assembly assembly, string rootNamespace, string relativeResourceName)
        {
            Argument.IsNotNull(() => title);
            Argument.IsNotNull(() => assembly);

            Title = title;
            Url = url;

            using (var memoryStream = new MemoryStream())
            {
                ResourceHelper.ExtractEmbeddedResource(assembly, rootNamespace, relativeResourceName, memoryStream);

                memoryStream.Position = 0L;
                var textReader = new StreamReader(memoryStream);

                Content = "[failed to load resources]";

                var content = textReader.ReadToEnd();
                if (!string.IsNullOrEmpty(content))
                {
                    Content = content;
                }
            }
        }
    }
}
