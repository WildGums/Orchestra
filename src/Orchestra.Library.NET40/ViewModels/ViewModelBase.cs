// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewModelBase.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Orchestra.ViewModels
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Base class for all view models in Orchestra. Additional functionality might be introduced in the future.
    /// </summary>
    public class ViewModelBase : Catel.MVVM.ViewModelBase
    {
        private readonly Collection<ViewModelBase> _contextualViewModels = new Collection<ViewModelBase>();

        // TODO MAVE: Should this functionality really be here??

        /// <summary>
        /// Gets or sets the contextual parent view model.
        /// </summary>
        /// <value>
        /// The contextual parent view model.
        /// </value>
        public Collection<ViewModelBase> ContextualViewModels
        {
            get 
            {
                return _contextualViewModels; 
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is contextual view model.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is contextual view model; otherwise, <c>false</c>.
        /// </value>
        public bool IsContextualViewModel
        {
            get { return _contextualViewModels != null && _contextualViewModels.Count > 0; }
        }

        /// <summary>
        /// Determines whether this view model has a contextual relation ship with the specified view model.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>
        ///   <c>true</c> if a contextual relation ship exists with the specified view model]; otherwise, <c>false</c>.
        /// </returns>
        public bool HasContextualRelationShip(ViewModelBase viewModel)
        {
            return ContextualViewModels.Contains(viewModel);
        }
    }
}