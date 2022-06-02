namespace Orchestra.Automation.FluentRibbon
{
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    [AutomatedControl(ClassName = nameof(Fluent.RibbonGroupBox))]
    public class RibbonGroupBox : FrameworkElement<RibbonGroupBoxModel>
    {
        public RibbonGroupBox(AutomationElement element) 
            : base(element)
        {
        }

        public string Name => Element.Current.Name;

        public bool IsExpanded
        {
            get => Element.GetIsExpanded();
            set => Element.SetIsExpanded(value);
        }

        public TView GetContent<TView>()
            where TView : AutomationControl
        {
            return Element?.Find<TView>();
        }
    }
}
