// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationInitializationService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using System.Threading.Tasks;
    using Catel.Threading;
    using MethodTimer;

    public class ApplicationInitializationServiceBase : IApplicationInitializationService
    {
        [Time] 
        public virtual async Task InitializeBeforeShowingSplashScreen()
        {
        }

        [Time]
        public virtual async Task InitializeBeforeCreatingShell()
        {
        }

        [Time]
        public virtual async Task InitializeAfterCreatingShell()
        {
        }

        [Time]
        public virtual async Task InitializeBeforeShowingShell()
        {
        }

        [Time]
        public virtual async Task InitializeAfterShowingShell()
        {
        }

        protected static async Task RunAndWaitAsync(params Func<Task>[] actions)
        {
            await TaskHelper.RunAndWaitAsync(actions);
        }
    }
}