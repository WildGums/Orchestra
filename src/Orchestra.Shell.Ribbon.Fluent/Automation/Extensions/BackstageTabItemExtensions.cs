namespace Orchestra.Automation
{
    using System;
    using FluentRibbon;
    using Orc.Automation;

    public static class BackstageTabItemExtensions
    {
        public static TView? GetContent<TView>(this BackstageTabItem backstage)
            where TView : AutomationControl
        {
            ArgumentNullException.ThrowIfNull(backstage);

            return backstage.Content?.As<TView>();
        }
    }
}
