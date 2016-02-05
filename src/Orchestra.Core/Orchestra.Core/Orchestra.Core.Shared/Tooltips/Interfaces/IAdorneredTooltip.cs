// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAdorneredTooltip.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
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