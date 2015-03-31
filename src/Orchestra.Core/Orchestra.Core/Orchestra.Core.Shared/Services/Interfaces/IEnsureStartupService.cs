// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEnsureStartupService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System.Threading.Tasks;

    public interface IEnsureStartupService
    {
        #region Properties
        bool SuccessfullyStarted { get; }
        #endregion

        #region Methods
        void ConfirmApplicationStartedSuccessfully();
        Task EnsureFailSafeStartup();
        #endregion
    }
}