namespace Orchestra.Automation.FluentRibbon
{
    using System.Collections.Generic;
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    public class RibbonMap : AutomationBase
    {
        public RibbonMap(AutomationElement element) 
            : base(element)
        {
        }

        public Backstage Backstage => By.One<Backstage>();
        public List<TabItem> TabItems => By.ClassName("RibbonTabItem").Many<TabItem>();
    }
}
