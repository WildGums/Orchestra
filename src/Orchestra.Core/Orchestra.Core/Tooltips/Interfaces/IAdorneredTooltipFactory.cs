// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAdorneredTooltipFactory.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Tooltips
{
    using System.Windows.Documents;

    public interface IAdorneredTooltipFactory
    {
        #region Methods
        IAdorneredTooltip Create(Adorner adornered, bool adornerLayerVisibility);
        #endregion
    }
}