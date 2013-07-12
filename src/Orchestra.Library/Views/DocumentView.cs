// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentView.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Views
{
    using Catel.Windows.Controls;

    /// <summary>
    /// Base class for all views that should be used as documents in Orchestra.
    /// <para />
    /// A developer is not forced to use this base class, but it is strongly recommended.
    /// </summary>
    public class DocumentView : UserControl, IDocumentView
    {
        // private List<IRibbonItem> _ribbonItems;
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentView"/> class. 
        /// </summary>
        public DocumentView()
        {
            if (Catel.Environment.IsInDesignMode)
            {
                return;
            }

            CloseViewModelOnUnloaded = false;
        }
        #endregion

        #region IDocumentView Members
        /// <summary>
        /// Closes the document.
        /// </summary>
        public void CloseDocument()
        {
            ViewModel.CloseViewModel(null);
        }
        #endregion
    }
}