// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEnsureStartupService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
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
        Task EnsureFailSafeStartupAsync();
        #endregion
    }
}