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
        /// <param name="dockingSettings">The docking settings.</param>
        public ContextSensitviveViewModelData(string title, DockingSettings dockingSettings)
        {
            ContextDependentViewModels = new Collection<Type>();
            Title = title;
            DockingSettings = dockingSettings;
        }

        /// <summary>
        /// Gets or sets the viewmodels that are contextsensive, for the ViewModel this ContextSensitviveViewModelData is related to.
        /// </summary>
        /// <value>
        /// The context dependent view models.
        /// </value>
        public ICollection<Type> ContextDependentViewModels { get; private set; }

        public bool IsNestedDockView { get; set; }

        /// <summary>
        /// Gets or sets the title for the contextsensitive view.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }


        public DockingSettings DockingSettings { get; set; }
    }
}
