namespace Orchestra.Changelog.ViewModels
{
    using System.Collections.Generic;
    using Catel;
    using Catel.MVVM;

    public class ChangelogViewModel : ViewModelBase
    {
        public ChangelogViewModel(Changelog changelog)
        {
            Argument.IsNotNull(() => changelog);

            Changelog = changelog;
            Items = changelog.Items;

            Title = changelog.Title ?? LanguageHelper.GetString("Orchestra_Changelog");
        }

        public Changelog Changelog { get; }

        public List<ChangelogItem> Items { get; private set; }
    }
}
