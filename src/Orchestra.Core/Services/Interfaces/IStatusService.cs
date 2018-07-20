// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStatusService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    public interface IStatusRepresenter
    {
        void UpdateStatus(string status);
    }

    public interface IStatusService
    {
        #region Methods
        void Initialize(IStatusRepresenter statusRepresenter);
        void UpdateStatus(string status);
        #endregion
    }
}