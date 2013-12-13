// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrchestraBootstrapperTaskFactory.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.CatelCustomization
{
    using System;
    using Catel.MVVM.Tasks;
    using Catel.Tasks;

    /// <summary>
    /// A customized orchestra bootstrapper task factory.
    /// </summary>
    public class OrchestraBootstrapperTaskFactory : BootstrapperTaskFactory
    {
        /// <summary>
        /// Creates the configure service locator container task.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="dispatch">if set to <c>true</c>, this action is dispatched to the UI thread.</param>
        /// <returns>The task.</returns>
        public override ITask CreateConfigureServiceLocatorContainerTask(Action action, bool dispatch = false)
        {
            return base.CreateConfigureServiceLocatorContainerTask(action, true);
        }

        /// <summary>
        /// Creates the configure service locator task.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="dispatch">if set to <c>true</c>, this action is dispatched to the UI thread.</param>
        /// <returns>The task.</returns>
        public override ITask CreateConfigureServiceLocatorTask(Action action, bool dispatch = false)
        {
            return base.CreateConfigureServiceLocatorTask(action, true);
        }
    }
}