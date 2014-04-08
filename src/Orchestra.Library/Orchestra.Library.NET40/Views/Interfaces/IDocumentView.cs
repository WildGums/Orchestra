// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDocumentView.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Views
{
    using Catel.MVVM.Views;

    /// <summary>
    /// Interface defining a document view.
    /// </summary>
    public interface IDocumentView : IView
    {
        #region Methods
        /// <summary>
        /// Closes the document.
        /// </summary>
        /// <returns><c>true</c> if the document was closed, <c>false</c> otherwise.</returns>
        bool CloseDocument();
        #endregion
    }
}