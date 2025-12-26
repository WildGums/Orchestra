namespace Orchestra
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IThirdPartyNoticesService
    {
        void Add(ThirdPartyNotice thirdPartyNotice);

        Task<IReadOnlyList<ThirdPartyNotice>> GetThirdPartyNoticesAsync();
    }
}
