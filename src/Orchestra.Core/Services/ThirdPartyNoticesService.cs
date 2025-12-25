namespace Orchestra.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel.Collections;
    using Microsoft.Extensions.Logging;

    public class ThirdPartyNoticesService : IThirdPartyNoticesService
    {
        private readonly ILogger<ThirdPartyNoticesService> _logger;

        private readonly Dictionary<string, ThirdPartyNotice> _thirdPartyNotices = new Dictionary<string, ThirdPartyNotice>(StringComparer.OrdinalIgnoreCase);

        public ThirdPartyNoticesService(ILogger<ThirdPartyNoticesService> logger, IEnumerable<ThirdPartyNotice> thirdPartyNotices)
        {
            _logger = logger;

            thirdPartyNotices.ForEach(x => Add(x));
        }

        public void Add(ThirdPartyNotice thirdPartyNotice)
        {
            ArgumentNullException.ThrowIfNull(thirdPartyNotice);

            lock (_thirdPartyNotices)
            {
                _logger.LogDebug($"Adding third party notice '{thirdPartyNotice.Title}'");

                _thirdPartyNotices[thirdPartyNotice.Title] = thirdPartyNotice;
            }
        }

        public async Task<IReadOnlyList<ThirdPartyNotice>> GetThirdPartyNoticesAsync()
        {
            lock (_thirdPartyNotices)
            {
                return (from x in _thirdPartyNotices.Values
                        orderby x.Title
                        select x).ToList();
            }
        }
    }
}
