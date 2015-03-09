// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatusFilterService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    public class StatusFilterService : IStatusFilterService
    {
        #region Properties
        public bool IsSuspended { get; set; }
        #endregion

        #region Methods
        public string GetStatus(string status)
        {
            if (IsSuspended)
            {
                return null;
            }

            // Default implementation just passes through
            return status;
        }
        #endregion
    }
}