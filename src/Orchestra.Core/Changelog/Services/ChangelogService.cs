namespace Orchestra.Changelog;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catel;
using Catel.Collections;
using Catel.IoC;
using Catel.Logging;
using Catel.Reflection;
using Microsoft.Extensions.Logging;

public class ChangelogService : IChangelogService
{
    private readonly List<IChangelogProvider> _providers = new List<IChangelogProvider>();

    private readonly ILogger<ChangelogService> _logger;
    private readonly IChangelogSnapshotService _changelogSnapshotService;

    public ChangelogService(ILogger<ChangelogService> logger, IChangelogSnapshotService changelogSnapshotService,
        IEnumerable<IChangelogProvider> changelogProviders)
    {
        _logger = logger;
        _changelogSnapshotService = changelogSnapshotService;

        _providers.AddRange(changelogProviders);
    }

    public virtual async Task<Changelog> GetChangelogSinceSnapshotAsync()
    {
        var snapshot = await _changelogSnapshotService.DeserializeSnapshotAsync();
        var changelog = await GetChangelogAsync();
        if (snapshot is null)
        {
            await _changelogSnapshotService.SerializeSnapshotAsync(changelog);
            return new Changelog();
        }

        var delta = snapshot.GetDelta(changelog);
        delta.Title = LanguageHelper.GetRequiredString("Orchestra_ChangelogWhatsNew");
        return delta;
    }

    public virtual async Task<Changelog> GetChangelogAsync()
    {
        var changelog = new Changelog();

        foreach (var provider in _providers)
        {
            try
            {
                _logger.LogDebug("Retrieving changelog from '{ProviderType}'", provider.GetType().FullName);

                var providerItems = await GetChangelogAsync(provider);
                changelog.Items.AddRange(providerItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get changelog from provider '{ProviderType}'", provider.GetType().FullName);
            }
        }

        return changelog;
    }

    protected virtual Task<IReadOnlyList<ChangelogItem>> GetChangelogAsync(IChangelogProvider provider)
    {
        return provider.GetChangelogAsync();
    }
}
