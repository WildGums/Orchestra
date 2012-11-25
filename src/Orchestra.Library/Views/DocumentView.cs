// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentView.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Views
{
    using System;
    using System.Collections.Generic;

    using Catel;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Windows.Controls;

    using Orchestra.Models;
    using Orchestra.Services;

    /// <summary>
    /// Base class for all views that should be used as documents in Orchestra.
    /// <para />
    /// A developer is not forced to use this base class, but it is strongly recommended.
    /// </summary>
    public class DocumentView : UserControl, IDocumentView
    {
        private readonly List<IRibbonItem> _ribbonItems = new List<IRibbonItem>();

        private bool _initializedRibbon;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentView"/> class. 
        /// </summary>
        public DocumentView()
        {
            if (Catel.Environment.IsInDesignMode)
                return;

            CloseViewModelOnUnloaded = false;
        }

        #region IDocumentView Members
        /// <summary>
        /// Closes the document.
        /// </summary>
        public void CloseDocument()
        {
            ViewModel.CloseViewModel(null);
        }
        #endregion

        /// <summary>
        /// Adds an item to the ribbon. As soon as the view is closed, the item is removed from the ribbon again.
        /// </summary>
        /// <param name="ribbonItem">
        /// The ribbon item.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="ribbonItem"/> is <c>null</c>.
        /// </exception>
        protected void AddRibbonItem(IRibbonItem ribbonItem)
        {
            Argument.IsNotNull("ribbonItem", ribbonItem);

            var orchestraService = ServiceLocator.Default.ResolveType<IOrchestraService>();
            orchestraService.AddRibbonItem(ribbonItem);

            _ribbonItems.Add(ribbonItem);
        }

        /// <summary>
        /// Called when the <see cref="P:Catel.Windows.Controls.UserControl.ViewModel"/> has been closed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="T:System.EventArgs"/> instance containing the event data.
        /// </param>
        protected override void OnViewModelClosed(object sender, ViewModelClosedEventArgs e)
        {
            var orchestraService = ServiceLocator.Default.ResolveType<IOrchestraService>();

            foreach (IRibbonItem ribbonItem in _ribbonItems)
                orchestraService.RemoveRibbonItem(ribbonItem);

            _ribbonItems.Clear();
        }

        /// <summary>
        /// Initializes the ribbon.
        /// <para />
        /// This is an ease-of-use method to register ribbons without having to care about any view model initialization. This
        /// method is only invoked once and when a view model is available.
        /// </summary>
        protected virtual void InitializeRibbon()
        {
        }

        /// <summary>
        /// Called when the <see cref="P:Catel.Windows.Controls.UserControl.ViewModel"/> has changed.
        /// </summary>
        protected override void OnViewModelChanged()
        {
            if (ViewModel == null || _initializedRibbon)
                return;

            InitializeRibbon();
            _initializedRibbon = true;
        }
    }
}