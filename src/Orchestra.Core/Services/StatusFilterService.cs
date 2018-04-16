// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatusFilterService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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