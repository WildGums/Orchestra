namespace Orchestra.Automation
{
    using FluentRibbon;
    using Orc.Automation;

    public static class BackstageTabItemExtensions
    {
        public static TView GetContent<TView>(this BackstageTabItem backstage)
            where TView : AutomationControl
        {
            return backstage.Content?.As<TView>();
        }
    }
}
