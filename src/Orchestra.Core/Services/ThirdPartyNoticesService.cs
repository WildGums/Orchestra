namespace Orchestra.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;

    public class ThirdPartyNoticesService : IThirdPartyNoticesService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly Dictionary<string, ThirdPartyNotice> _thirdPartyNotices = new Dictionary<string, ThirdPartyNotice>(StringComparer.OrdinalIgnoreCase);

        public void Add(ThirdPartyNotice thirdPartyNotice)
        {
            Argument.IsNotNull(() => thirdPartyNotice);

            lock (_thirdPartyNotices)
            {
                Log.Debug($"Adding third party notice '{thirdPartyNotice.Title}'");

                _thirdPartyNotices[thirdPartyNotice.Title] = thirdPartyNotice;
            }
        }

        public List<ThirdPartyNotice> GetThirdPartyNotices()
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
