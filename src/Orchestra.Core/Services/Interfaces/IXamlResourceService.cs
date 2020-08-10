namespace Orchestra.Services
{
    using System.Collections.Generic;
    using System.Windows;

    public interface IXamlResourceService
    {
        IEnumerable<ResourceDictionary> GetApplicationResourceDictionaries();
    }
}
