namespace Orchestra.Automation.FluentRibbon
{
    using System.Linq;
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    [Control(ClassName = nameof(Fluent.BackstageTabItem))]
    public class BackstageTabItem : FrameworkElement<BackstageTabItemModel>
    {
        public BackstageTabItem(AutomationElement element)
            : base(element)
        {
        }

        public string Header => Element.Current.Name;

        public AutomationElement Content => Element.GetChildElements().FirstOrDefault();

        public bool IsSelected
        {
            get => Element.GetIsSelected();
            set => Element.TrySetSelection(value);
        }

        public bool TrySelect()
        {
            return Element.TrySelect();
        }
    }
}
