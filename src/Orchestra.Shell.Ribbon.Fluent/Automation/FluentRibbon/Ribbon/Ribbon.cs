namespace Orchestra.Automation.FluentRibbon
{
    using System.Linq;
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    //https://github.com/fluentribbon/Fluent.Ribbon/tree/05a7c036fe8c8469229e842d0183bf9a7664c82a/Fluent.Ribbon/Automation/Peers
    [Control(ClassName = "Ribbon")]
    public class Ribbon : FrameworkElement
    {
        public Ribbon(AutomationElement element) 
            : base(element)
        {
        }

        private RibbonMap Map => Map<RibbonMap>();

        public Backstage? OpenBackstage()
        {
            var map = Map;

            var backstage = map.Backstage;
            if (backstage is null)
            {
                return null;
            }

            backstage.IsOpen = true;

            return backstage;
        }

        public void CloseBackstage()
        {
            var map = Map;

            var backstage = map.Backstage;
            if (backstage is null)
            {
                return;
            }

            backstage.IsOpen = false;
        }
        
        public RibbonGroupBox? GetGroupBox(string tabName, string viewName)
        {
            var map = Map;

            var tabItems = map.TabItems;

            var searchingTabItem = tabItems.FirstOrDefault(x => Equals(x.Header, tabName));
            var ribbonGroupBox = searchingTabItem?.Find<RibbonGroupBox>(name: viewName);

            return ribbonGroupBox;
        }
    }
}
