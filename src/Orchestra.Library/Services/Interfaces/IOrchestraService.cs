// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOrchestraService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Services
{
    using System;
    using Catel.MVVM;
    using Models;

    /// <summary>
    /// The orchestra service that allows communication with the shell.
    /// </summary>
    public interface IOrchestraService
    {
        /// <summary>
        /// Shows the document in the main shell.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="tag">The tag.</param>
        void ShowDocument<TViewModel>(object tag = null)
            where TViewModel : IViewModel, new();

        /// <summary>
        /// Shows the document in the main shell.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="viewModel">The view model to show which will automatically be resolved to a view.</param>
        /// <param name="tag">The tag.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="viewModel"/> is <c>null</c>.</exception>
        void ShowDocument<TViewModel>(TViewModel viewModel, object tag = null)
            where TViewModel : IViewModel;

        /// <summary>
        /// Adds the specified ribbon item to the main ribbon.
        /// </summary>
        /// <param name="ribbonItem">The ribbon item.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="ribbonItem"/> is <c>null</c>.</exception>
        void AddRibbonItem(IRibbonItem ribbonItem);

        /// <summary>
        /// Removes the specified ribbon item to the main ribbon.
        /// <para />
        /// This method will ignore calls when the item is not available in the ribbon.
        /// </summary>
        /// <param name="ribbonItem">The ribbon item.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="ribbonItem"/> is <c>null</c>.</exception>
        void RemoveRibbonItem(IRibbonItem ribbonItem);
    }
}