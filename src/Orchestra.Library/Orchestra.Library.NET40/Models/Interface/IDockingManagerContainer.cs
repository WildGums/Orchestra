using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orchestra.Models.Interface
{
    using Xceed.Wpf.AvalonDock;

    interface IDockingManagerContainer
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
    }
}
