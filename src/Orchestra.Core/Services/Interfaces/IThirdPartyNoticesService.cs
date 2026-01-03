namespace Orchestra
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Catel.ThirdPartyNotices;

    public interface IThirdPartyNoticesService
    {
        void Add(IThirdPartyNotice thirdPartyNotice);

        Task<IReadOnlyList<IThirdPartyNotice>> GetThirdPartyNoticesAsync();
    }
}
