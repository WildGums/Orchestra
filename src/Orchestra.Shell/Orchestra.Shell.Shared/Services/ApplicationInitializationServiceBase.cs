// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationInitializationService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows.Documents;
    using Catel;
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

        protected static async Task RunAndWaitAsync(params Func<Task>[] actions)
        {
            await TaskHelper.RunAndWaitAsync(actions);
        }
    }
}