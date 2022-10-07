namespace Orchestra.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IThirdPartyNoticesService
    {
        void Add(ThirdPartyNotice thirdPartyNotice);

        Task<List<ThirdPartyNotice>> GetThirdPartyNoticesAsync();
    }
}
