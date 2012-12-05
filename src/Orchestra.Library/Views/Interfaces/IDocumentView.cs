// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDocumentView.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Views
{
    using System.Collections.Generic;
    using Catel.Windows.Controls;
    using Models;

    /// <summary>
    /// Interface defining a document view.
    /// </summary>
    public interface IDocumentView : IView
    {
        /// <summary>
        /// Closes the document.
        /// </summary>
        void CloseDocument();

        /// <summary>
        /// Returns a list of <see cref="IRibbonItem"/> elements that should be visible in the ribbon. The
        /// framework will automatically take care of the adding and removing of the ribbon items.
        /// </summary>
        /// <returns>The ribbon items.</returns>
        IEnumerable<IRibbonItem> GetRibbonItems();
    }
}