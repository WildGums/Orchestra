namespace Orchestra.Automation.FluentRibbon
{
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;
    
    [AutomatedControl(ClassName = "RibbonButton")]
    public class RibbonButton : FrameworkElement<ButtonModel>
    {
        public RibbonButton(AutomationElement element)
            : base(element)
        {
        }

        public string Content => Element.Current.Name;

        public bool Click()
        {
            return Element.TryInvoke();
        }
    }
}
