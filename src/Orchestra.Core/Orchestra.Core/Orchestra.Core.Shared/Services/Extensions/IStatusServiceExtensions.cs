// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStatusServiceExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    public static class IStatusServiceExtensions
    {
        public static void UpdateStatus(this IStatusService statusService, string statusFormat, params object[] parameters)
        {
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