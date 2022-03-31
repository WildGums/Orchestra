namespace Orchestra.Automation
{
    using System.Windows.Automation;
    using Orc.Automation;

    public class RibbonMap : AutomationBase
    {
        public RibbonMap(AutomationElement element) 
            : base(element)
        {
        }

        public Backstage Backstage => By.One<Backstage>();
    }
}
