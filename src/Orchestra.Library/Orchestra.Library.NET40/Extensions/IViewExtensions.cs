// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IViewExtensions.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using Catel;
    using Catel.MVVM;
    using Catel.MVVM.Views;

    /// <summary>
    /// Extension methods for <see cref="IView"/>.
    /// </summary>
    public static class IViewExtensions
    {
        /// <summary>
        /// Gets the view model. This method first tries to use the <c>ViewModel</c> property. If that property is <c>null</c>,
        /// it will try the <c>DataContext</c> instead.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <returns>IViewModel.</returns>
        public static IViewModel GetViewModel(this IView view)
        {
            Argument.IsNotNull("view", view);

            return (view.ViewModel ?? view.DataContext) as IViewModel;
        }
    }
}