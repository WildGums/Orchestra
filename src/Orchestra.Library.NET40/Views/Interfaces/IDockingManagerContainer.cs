// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDockingManagerContainer.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Views
{
    using Orchestra.Models;
    using Xceed.Wpf.AvalonDock;
    using Xceed.Wpf.AvalonDock.Layout;

    /// <summary>
    /// Docking manager container.
    /// </summary>
    public interface IDockingManagerContainer
    {
        /// <summary>
        /// Gets the <see cref="DockingManager" />.
        /// </summary>
        /// <value>
        /// The <see cref="DockingManager" />.
        /// </value>
        DockingManager DockingManager { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        bool IsActive { get; }

        /// <summary>
        /// Gets the content document.
        /// </summary>
        /// <value>
        /// The content document.
        /// </value>
        LayoutAnchorable ContentDocument { get; }

        /// <summary>
        /// Adds the main document for the NestedDockingManager.
        /// </summary>
        /// <param name="documentView">The document view.</param>
        /// <exception cref="System.Exception">NestedDockingManager can only have one main document with the main content.</exception>
        void AddContentDocument(LayoutAnchorable documentView);

        /// <summary>
        /// Adds the document as a tab to the NestedDocking view.
        /// </summary>
        /// <param name="documentView">The document view.</param>
        /// <param name="dockLocation">The dock location.</param>
        void AddDockedWindow(LayoutAnchorable documentView, DockLocation dockLocation);
    }
}