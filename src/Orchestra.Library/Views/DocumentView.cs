// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentViewBase.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
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
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Windows.Controls.UserControl"/> class.
        /// </summary>
        public DocumentView()
        {
            CloseViewModelOnUnloaded = false;
        }

        /// <summary>
        /// Closes the document.
        /// </summary>
        public void CloseDocument()
        {
            ViewModel.CloseViewModel(null);
        }
    }
}