// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOrchestraService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Services
{
    using System;
    using Catel.MVVM;
    using Orchestra.Models;
    using Views;

    /// <summary>
    /// The orchestra service that allows communication with the shell.
    /// </summary>
    public interface IOrchestraService
    {
        #region Properties
        /// <summary>
        /// Gets or sets a value indicating whether to show the debug window.
        /// </summary>
        /// <value><c>true</c> if the debug window should be shown; otherwise, <c>false</c>.</value>
        bool ShowDebuggingWindow { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Shows the document in the main shell.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="tag">The tag.</param>
        void ShowDocument<TViewModel>(object tag = null)
            where TViewModel : IViewModel;

        /// <summary>
        /// Shows the document in the main shell.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="viewModel">The view model to show which will automatically be resolved to a view.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="dockLocation">The dock location.</param>        
        /// <exception cref="ArgumentNullException">The <paramref name="viewModel" /> is <c>null</c>.</exception>
        void ShowDocument<TViewModel>(TViewModel viewModel, object tag = null, DockLocation? dockLocation = null)
            where TViewModel : IViewModel;

        /// <summary>
        /// Shows the document if it is hidden.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="tag">The tag.</param>
        void ShowDocumentIfHidden<TViewModel>(object tag = null)
            where TViewModel : IViewModel;

        /// <summary>
        /// Shows the document in nested dock view.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="dockingManager">The docking manager.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="dockLocation">The dock location.</param>
        void ShowDocumentInNestedDockView(IViewModel viewModel, NestedDockingManager dockingManager, object tag = null, DockLocation? dockLocation = null);

        /// <summary>
        /// Closes the document in the main shell with the specified view model.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="tag">The tag.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="viewModel"/> is <c>null</c>.</exception>
        void CloseDocument(IViewModel viewModel, object tag = null);

        /// <summary>
        /// Adds the specified ribbon item to the main ribbon.
        /// </summary>
        /// <param name="ribbonItem">The ribbon item.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="ribbonItem"/> is <c>null</c>.</exception>
        [ObsoleteEx(Replacement = "IRibbonService", TreatAsErrorFromVersion = "0.1", RemoveInVersion = "1.0")]
        void AddRibbonItem(IRibbonItem ribbonItem);

        /// <summary>
        /// Removes the specified ribbon item to the main ribbon.
        /// <para />
        /// This method will ignore calls when the item is not available in the ribbon.
        /// </summary>
        /// <param name="ribbonItem">The ribbon item.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="ribbonItem"/> is <c>null</c>.</exception>
        [ObsoleteEx(Replacement = "IRibbonService", TreatAsErrorFromVersion = "0.1", RemoveInVersion = "1.0")]
        void RemoveRibbonItem(IRibbonItem ribbonItem);
        #endregion
    }
}