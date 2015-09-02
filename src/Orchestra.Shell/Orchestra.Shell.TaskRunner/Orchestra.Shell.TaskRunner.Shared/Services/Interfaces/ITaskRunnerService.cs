// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITaskRunnerService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Windows;

    public interface ITaskRunnerService : IAboutInfoService
    {
        /// <summary>
        /// Gets the title of the runner which will be shown in the window header.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; }

        /// <summary>
        /// Notifies when the title of the runner changes
        /// </summary>
        event EventHandler TitleChanged;

        /// <summary>
        /// Gets a value indicating whether to show the customize shortcuts button.
        /// </summary>
        /// <value><c>true</c> if the customize shortcuts button should be shown; otherwise, <c>false</c>.</value>
        bool ShowCustomizeShortcutsButton { get; }

        /// <summary>
        /// Gets the data context that will be set to the view.
        /// </summary>
        /// <returns>The data context or <c>null</c>.</returns>
        object GetViewDataContext();

        /// <summary>
        /// Gets the view to show in the configuration.
        /// </summary>
        /// <returns>FrameworkElement.</returns>
        FrameworkElement GetView();

        /// <summary>
        /// Runs the logic when the run button is clicked.
        /// </summary>
        /// <param name="dataContext">The data context retrieved earlier using the <see cref="GetViewDataContext"/> method.</param>
        Task RunAsync(object dataContext);

        /// <summary>
        /// Gets the desired startup size.
        /// <para />
        /// If <see cref="Size.Empty"/>, this value will be ignored. If only the width or height is set, the other value will be ignored automatically.
        /// </summary>
        /// <returns>The desired startup size.</returns>
        Size GetInitialWindowSize();
    }
}