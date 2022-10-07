namespace Orchestra.Tooltips
{
    using System;
    using System.Windows.Documents;

    internal class AdorneredTooltipFactory : IAdorneredTooltipFactory
    {
        public IAdorneredTooltip Create(Adorner adornered, bool adornerLayerVisibility)
        {
            ArgumentNullException.ThrowIfNull(adornered);

            return new AdorneredTooltip(adornered, adornerLayerVisibility);
        }
    }
}
