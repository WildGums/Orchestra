namespace Orchestra;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catel.Collections;
using Catel.ThirdPartyNotices;
using Microsoft.Extensions.Logging;

public class ThirdPartyNoticesService : IThirdPartyNoticesService
{
    private readonly ILogger<ThirdPartyNoticesService> _logger;

    private readonly Dictionary<string, IThirdPartyNotice> _thirdPartyNotices = new Dictionary<string, IThirdPartyNotice>(StringComparer.OrdinalIgnoreCase);

    public ThirdPartyNoticesService(ILogger<ThirdPartyNoticesService> logger, 
        IEnumerable<IThirdPartyNotice> thirdPartyNotices)
    {
        _logger = logger;

        thirdPartyNotices.ForEach(x => Add(x));
    }

    public void Add(IThirdPartyNotice thirdPartyNotice)
    {
        ArgumentNullException.ThrowIfNull(thirdPartyNotice);

        lock (_thirdPartyNotices)
        {
            _logger.LogDebug($"Adding third party notice '{thirdPartyNotice.Title}'");

            _thirdPartyNotices[thirdPartyNotice.Title] = thirdPartyNotice;
        }
    }

    public async Task<IReadOnlyList<IThirdPartyNotice>> GetThirdPartyNoticesAsync()
    {
        lock (_thirdPartyNotices)
        {
            return (from x in _thirdPartyNotices.Values
                    orderby x.Title
                    select x).ToArray();
        }
    }
}
