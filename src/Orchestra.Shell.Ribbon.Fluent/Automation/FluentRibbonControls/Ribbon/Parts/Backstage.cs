namespace Orchestra.Automation.FluentRibbon
{
    using System.Windows.Automation;
    using global::Orc.Automation.Controls;
    using Orc.Automation;

    [AutomatedControl(ClassName = "Backstage", ControlTypeName = nameof(ControlType.Menu))]
    public class Backstage : FrameworkElement
    {
        public Backstage(AutomationElement element) 
            : base(element)
        {
        }

        public bool IsOpen
        {
            get => Element.GetIsExpanded();
            set => Element.SetIsExpanded(value);
        }
    }
}
