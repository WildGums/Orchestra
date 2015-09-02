// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStatusFilterService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    public interface IStatusFilterService
    {
        #region Properties
        bool IsSuspended { get; set; }
        #endregion

        #region Methods
        string GetStatus(string status);
        #endregion
    }
}