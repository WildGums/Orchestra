// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAdorneredTooltip.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Tooltips
{
    public interface IAdorneredTooltip
    {
        #region Properties
        bool Visible { get; set; }

        bool AdornerLayerVisible { get; set; }
        #endregion
    }
}