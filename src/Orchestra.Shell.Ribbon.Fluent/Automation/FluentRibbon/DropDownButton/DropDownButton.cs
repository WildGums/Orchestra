namespace Orchestra.Automation.FluentRibbon
{
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    [Control(ClassName = "DropDownButton")]
    public class DropDownButton : FrameworkElement<DropDownButtonModel>
    {
        public DropDownButton(AutomationElement element)
            : base(element)
        {

        }

        public bool IsExpanded
        {
            get => Element.GetIsExpanded();
            set => Element.SetIsExpanded(value);
        }

        public void Invoke()
        {
            Element.Invoke();
        }
    }
}
