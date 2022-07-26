namespace Orchestra.Automation.FluentRibbon
{
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    [Control(ControlTypeName = nameof(ControlType.Tab))]
    public class BackstageTabControl : FrameworkElement<BackstageTabControlModel>
    {
        public BackstageTabControl(AutomationElement element) 
            : base(element)
        {
            
        }

        public TControl GetItem<TControl>(string name)
            where TControl : AutomationControl
        {
            var childElement = Element.Find<TControl>(name: name, scope: TreeScope.Children);

            return childElement;
        }
    }
}
