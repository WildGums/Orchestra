namespace Orchestra.Examples.MahApps.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    public class MaterialDesignXamlResourceService : Orchestra.Services.XamlResourceService
    {
        public override IEnumerable<ResourceDictionary> GetApplicationResourceDictionaries()
        {
            // This example shows a fully customized resource dictionary being loaded by
            // the XamlResourceService. See https://github.com/WildGums/Orchestra/issues/382S

            // In order to use a fully customized xaml resource dictionary, we will remove the 
            // default orchestra one for the shell 

            var items = new List<ResourceDictionary>(base.GetApplicationResourceDictionaries());

            var shellResourceDictionaries = (from item in items
                                             where item.Source?.ToString().Contains("Orchestra.Shell.MahApps") ?? false
                                             select item).ToList();

            foreach (var shellResourceDictionary in shellResourceDictionaries)
            {
                items.Remove(shellResourceDictionary);
            }

            items.Add(new ResourceDictionary
            {
                Source = new System.Uri("/Orchestra.Examples.MahApps;component/themes/MaterialDesign.xaml", UriKind.RelativeOrAbsolute)
            });

            return items;
        }
    }
}
