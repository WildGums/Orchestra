// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAdorneredTooltipsCollection.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Collections
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Documents;
    using Tooltips;

    public interface IAdorneredTooltipsCollection
    {
        #region Methods
        void Add(FrameworkElement parentControl, Adorner tooltip, bool adornerLayerVisibility);
        void Remove(FrameworkElement parentControl);

        IList<IAdorneredTooltip> GetAdorners(FrameworkElement parentControl);

        void ShowAll();
        void HideAll();
        void AdornerLayerEnabled();
        void AdornerLayerDisabled();
        #endregion
    }
}