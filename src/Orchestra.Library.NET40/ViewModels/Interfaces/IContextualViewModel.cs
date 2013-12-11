// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IContextualViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.ViewModels
{
    /// <summary>
    /// Interface can be used to inform a viewmodel that it's View has become active in Orchestra.
    /// </summary>
    public interface IContextualViewModel
    {
        #region Methods
        /// <summary>
        /// Method is called when the active view changes within the orchestra application
        /// </summary>
        void ViewModelActivated();
        #endregion
    }
}