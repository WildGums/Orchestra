// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationInitializationService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Catel.Threading;

    public class ApplicationInitializationServiceBase : IApplicationInitializationService
    {
        public virtual async Task InitializeBeforeCreatingShell()
        {
        }

        public virtual async Task InitializeAfterCreatingShell()
        {
        }

        public virtual async Task InitializeBeforeShowingShell()
        {
        }

        public virtual async Task InitializeAfterShowingShell()
        {
        }

        public virtual async Task InitializeCommands(ICommandManager commandManager)
        {
        }

        [ObsoleteEx(Replacement = "Catel.Thread.TaskHelper.RunAndWaitAsync()", TreatAsErrorFromVersion = "3.0", RemoveInVersion = "3.0")]
        protected static async Task RunAndWaitAsync(params Action[] actions)
        {
            await Task.Factory.StartNew(() => TaskHelper.RunAndWait(actions));
        }
    }
}