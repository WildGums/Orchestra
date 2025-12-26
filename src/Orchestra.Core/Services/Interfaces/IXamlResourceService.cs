namespace Orchestra
{
    using System.Collections.Generic;
    using System.Windows;

    public interface IXamlResourceService
    {
        IEnumerable<ResourceDictionary> GetApplicationResourceDictionaries();
    }
}
