namespace Orchestra.Changelog.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Catel.Services;
    using Microsoft.Extensions.Logging;

    public class ChangelogViewModel : ViewModelBase
    {
        private readonly ILogger<ChangelogViewModel> _logger;
        private readonly IChangelogService _changelogService;
        private readonly IChangelogSnapshotService _changelogSnapshotService;

        public ChangelogViewModel(Changelog changelog, IServiceProvider serviceProvider,
            ILogger<ChangelogViewModel> logger, IChangelogService changelogService, 
            IChangelogSnapshotService changelogSnapshotService, ILanguageService languageService)
            : base(serviceProvider)
        {
            ValidateUsingDataAnnotations = false;

            Changelog = changelog;
            _logger = logger;
            _changelogService = changelogService;
            _changelogSnapshotService = changelogSnapshotService;

            Groups = changelog.CreateGroups();
            Title = changelog.Title ?? languageService.GetRequiredString("Orchestra_Changelog");
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
