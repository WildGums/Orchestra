namespace Orchestra.Examples.Ribbon.Services
{
    using System;
    using System.Threading.Tasks;

    internal class AboutInfoService : IAboutInfoService
    {
        public async Task<AboutInfo> GetAboutInfoAsync()
        {
            var aboutInfo = new AboutInfo(new Uri($"pack://application:,,,/{Catel.Reflection.AssemblyHelper.GetEntryAssembly().GetName().Name};component/Resources/Images/CompanyLogo.png", UriKind.RelativeOrAbsolute),
                uriInfo: new UriInfo("https://www.catelproject.com", "Product website"));

            return aboutInfo;
        }
    }
}
