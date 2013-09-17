using System;
using System.Collections.Generic;

namespace Orchestra.Models
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Helper DTO class for holding data relevant for a context sensitive 'parent' viewmodel.
    /// </summary>
    internal class ContextSensitviveViewModelData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContextSensitviveViewModelData" /> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="dockLocation">The dock location.</param>
        public ContextSensitviveViewModelData(string title, DockLocation dockLocation)
        {
            ContextDependentViewModels = new Collection<Type>();
            Title = title;
            DockLocation = dockLocation;
        }

        /// <summary>
        /// Gets or sets the viewmodels that are contextsensive, for the ViewModel this ContextSensitviveViewModelData is related to.
        /// </summary>
        /// <value>
        /// The context dependent view models.
        /// </value>
        public ICollection<Type> ContextDependentViewModels { get; set; }

        /// <summary>
        /// Gets or sets the title for the contextsensitive view.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the dock location.
        /// </summary>
        /// <value>
        /// The dock location.
        /// </value>
        public DockLocation DockLocation { get; set; }
    }
}
