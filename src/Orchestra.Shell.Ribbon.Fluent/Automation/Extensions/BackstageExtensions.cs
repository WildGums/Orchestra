namespace Orchestra.Automation
{
    using FluentRibbon;
    using Orc.Automation;

    public static class BackstageExtensions
    {
        public static TView GetContent<TView>(this Backstage backstage)
            where TView : AutomationControl
        {
            return backstage.Content?.As<TView>();
        }
    }
}
