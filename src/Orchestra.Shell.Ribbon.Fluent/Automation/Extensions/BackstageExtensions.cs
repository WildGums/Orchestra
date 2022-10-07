namespace Orchestra.Automation
{
    using System;
    using FluentRibbon;
    using Orc.Automation;

    public static class BackstageExtensions
    {
        public static TView? GetContent<TView>(this Backstage backstage)
            where TView : AutomationControl
        {
            ArgumentNullException.ThrowIfNull(backstage);

            return backstage.Content?.As<TView>();
        }
    }
}
