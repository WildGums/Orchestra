namespace Orchestra.Services
{
    using System;
    using System.Threading.Tasks;

    public class AboutInfoService : IAboutInfoService
    {
        public async Task<AboutInfo> GetAboutInfoAsync()
        {
            var aboutInfo = new AboutInfo(new Uri("pack://application:,,,/Resources/Images/CompanyLogo.png", UriKind.RelativeOrAbsolute));
            return aboutInfo;
        }
    }
}
