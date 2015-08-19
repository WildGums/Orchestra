// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CloseApplicationService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System.Diagnostics;
    using System.Threading;
    using Catel;
    using Catel.Logging;

    public class CloseApplicationService : ICloseApplicationService
    {
        #region Fields
        private readonly IEnsureStartupService _ensureStartupService;
        #endregion

        #region Constructors
        public CloseApplicationService(IEnsureStartupService ensureStartupService)
        {
            Argument.IsNotNull(() => ensureStartupService);

            _ensureStartupService = ensureStartupService;
        }
        #endregion

        #region Methods
        public void Close()
        {
            _ensureStartupService.ConfirmApplicationStartedSuccessfully();

            LogManager.FlushAll();

            // Very dirty, but allow app to write the file
            Thread.Sleep(50);

            // TODO: consider application.Shutdown (if that results in a cleaner shutdown)
            Process.GetCurrentProcess().Kill();
        }
        #endregion
    }
}