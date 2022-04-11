namespace Orchestra.Automation.FluentRibbon
{
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;
    
    [AutomatedControl(ClassName = "RibbonButton")]
    public class RibbonButton : Button
    {
        public RibbonButton(AutomationElement element) 
            : base(element)
        {
        }
    }
}
