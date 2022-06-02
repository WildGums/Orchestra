namespace Orchestra.Automation.FluentRibbon
{
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    [AutomatedControl(ClassName = "SplitButton")]
    public class SplitButton : FrameworkElement<SplitButtonModel>
    {
        public SplitButton(AutomationElement element) 
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
