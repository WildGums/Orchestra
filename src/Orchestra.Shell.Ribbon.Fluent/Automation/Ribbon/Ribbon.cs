namespace Orchestra.Automation
{
    using System.Windows.Automation;
    using System.Linq;
    using Orc.Automation;
    using Orc.Automation.Controls;

    //https://github.com/fluentribbon/Fluent.Ribbon/tree/05a7c036fe8c8469229e842d0183bf9a7664c82a/Fluent.Ribbon/Automation/Peers
    [AutomatedControl(ClassName = "Ribbon")]
    public class Ribbon : FrameworkElement
    {
        public Ribbon(AutomationElement element) 
            : base(element)
        {
        }

        private RibbonMap Map => Map<RibbonMap>();

        public Backstage OpenBackstage()
        {
            var map = Map;

            var backstage = map.Backstage;
            backstage.IsOpen = true;

            return backstage;
        }
    }
}
