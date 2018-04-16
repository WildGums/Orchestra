// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdorneredTooltipFactory.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Tooltips
{
    using System.Windows.Documents;

    internal class AdorneredTooltipFactory : IAdorneredTooltipFactory
    {
        #region Implementation of IAdorneredHintFactory
        public IAdorneredTooltip Create(Adorner adornered, bool adornerLayerVisibility)
        {
            return new AdorneredTooltip(adornered, adornerLayerVisibility);
        }
        #endregion
    }
}