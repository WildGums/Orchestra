namespace Orchestra.Services
{
    using System;
    using Catel;
    using Catel.Logging;

    public static class IThirdPartyNoticesServiceExtensions
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public static void AddWithTryCatch(this IThirdPartyNoticesService thirdPartyNoticesService,
            Func<ThirdPartyNotice> func)
        {
            ArgumentNullException.ThrowIfNull(thirdPartyNoticesService);
            ArgumentNullException.ThrowIfNull(func);

            try
            {
                var thirdPartyNotice = func();
                thirdPartyNoticesService.Add(thirdPartyNotice);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to add third party notice");
            }
        }
    }
}
