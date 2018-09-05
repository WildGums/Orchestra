namespace Orchestra.Services
{
    using System.Collections.Generic;

    public interface IThirdPartyNoticesService
    {
        void Add(ThirdPartyNotice thirdPartyNotice);
        List<ThirdPartyNotice> GetThirdPartyNotices();
    }
}
