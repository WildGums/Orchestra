// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStatusFilterService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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