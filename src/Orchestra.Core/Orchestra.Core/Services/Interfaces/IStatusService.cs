// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStatusService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
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