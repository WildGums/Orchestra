namespace Orchestra.Services
{
    using System;

    public static class IStatusServiceExtensions
    {
        public static void UpdateStatus(this IStatusService statusService, string statusFormat, params object[] parameters)
        {
            ArgumentNullException.ThrowIfNull(statusService);

            if (string.IsNullOrWhiteSpace(statusFormat))
            {
                statusFormat = string.Empty;
            }

            if (parameters.Length > 0)
            {
                statusFormat = string.Format(statusFormat, parameters);
            }

            statusService.UpdateStatus(statusFormat);
        }
    }
}
