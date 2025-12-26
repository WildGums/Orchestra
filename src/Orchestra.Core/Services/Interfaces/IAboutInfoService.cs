namespace Orchestra
{
    using System.Threading.Tasks;

    public interface IAboutInfoService
    {
        /// <summary>
        /// Returns the about info. If <c>null</c>, the shell will not show the about window.
        /// </summary>
        /// <returns></returns>
        Task<AboutInfo> GetAboutInfoAsync();
    }
}
