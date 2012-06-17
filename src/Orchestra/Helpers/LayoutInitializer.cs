// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LayoutInitializer.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra
{
    using System.Linq;
    using AvalonDock.Layout;

    /// <summary>
    /// Layout initializer for Orchestra.
    /// </summary>
    public class LayoutInitializer : ILayoutUpdateStrategy
    {
        #region ILayoutUpdateStrategy Members
        /// <summary>
        /// Befores the insert anchorable.
        /// </summary>
        /// <param name="layout">The layout.</param>
        /// <param name="anchorableToShow">The anchorable to show.</param>
        /// <param name="destinationContainer">The destination container.</param>
        public bool BeforeInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableToShow, ILayoutContainer destinationContainer)
        {
            //AD wants to add the anchorable into destinationContainer
            //just for test provide a new anchorablepane 
            //if the pane is floating let the manager go ahead
            //var destPane = destinationContainer as LayoutAnchorablePane;
            if (destinationContainer != null && destinationContainer.FindParent<LayoutFloatingWindow>() != null)
            {
                return false;
            }

            var toolsPane = layout.Descendents().OfType<LayoutAnchorablePane>().FirstOrDefault(d => d.Name == "ToolsPane");
            if (toolsPane != null)
            {
                toolsPane.Children.Add(anchorableToShow);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Inserts the anchorable.
        /// </summary>
        /// <param name="layout">The layout.</param>
        /// <param name="anchorableToShow">The anchorable to show.</param>
        /// <param name="destinationContainer">The destination container.</param>
        public bool InsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableToShow, ILayoutContainer destinationContainer)
        {
            return false;
        }
        #endregion
    }
}