namespace Orchestra.Collections
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Documents;
    using Tooltips;

    public interface IAdorneredTooltipsCollection
    {
        void Add(FrameworkElement parentControl, Adorner tooltip, bool adornerLayerVisibility);
        void Remove(FrameworkElement parentControl);

        IList<IAdorneredTooltip> GetAdorners(FrameworkElement parentControl);

        void ShowAll();
        void HideAll();
        void AdornerLayerEnabled();
        void AdornerLayerDisabled();
    }
}
