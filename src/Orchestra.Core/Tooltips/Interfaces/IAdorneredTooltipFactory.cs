namespace Orchestra.Tooltips
{
    using System.Windows.Documents;

    public interface IAdorneredTooltipFactory
    {
        IAdorneredTooltip Create(Adorner adornered, bool adornerLayerVisibility);
    }
}
