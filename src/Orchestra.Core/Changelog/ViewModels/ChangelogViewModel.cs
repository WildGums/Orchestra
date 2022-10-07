namespace Orchestra.Changelog.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;
    using Catel.MVVM;

    public class ChangelogViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IChangelogService _changelogService;
        private readonly IChangelogSnapshotService _changelogSnapshotService;

        public ChangelogViewModel(Changelog changelog, IChangelogService changelogService, IChangelogSnapshotService changelogSnapshotService)
        {
            ArgumentNullException.ThrowIfNull(changelog);
            ArgumentNullException.ThrowIfNull(changelogService);
            ArgumentNullException.ThrowIfNull(changelogSnapshotService);

            Changelog = changelog;
            _changelogService = changelogService;
            _changelogSnapshotService = changelogSnapshotService;

            Groups = changelog.CreateGroups();
            Title = changelog.Title ?? LanguageHelper.GetString("Orchestra_Changelog");
        }

        public Changelog Changelog { get; }

        public List<ChangelogGroup> Groups { get; }

        protected override async Task<bool> SaveAsync()
        {
            var fullChangelog = await _changelogService.GetChangelogAsync();
            await _changelogSnapshotService.SerializeSnapshotAsync(fullChangelog);

            return await base.SaveAsync();
        }
    }
}
