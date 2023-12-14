namespace Orchestra.Automation
{
    using System;
    using FluentRibbon;
    using Orc.Automation;

    public static class BackstageTabControlExtensions
    {
        public static TView? GetItemContent<TView>(this BackstageTabControl tabControl, string header)
            where TView : AutomationControl
        {
            ArgumentNullException.ThrowIfNull(tabControl);

            tabControl.SelectItem(header);

            var tabItem = tabControl.GetItem<BackstageTabItem>(header);
            var content = tabItem?.GetContent<TView>();

            return content;
        }

        public static void SelectItem(this BackstageTabControl tabControl, string header)
        {
            ArgumentNullException.ThrowIfNull(tabControl);

            var tabItem = tabControl.GetItem<BackstageTabItem>(header);

            //There is no pattern select/Invoke/toggle etc in BackstageTabItem
            tabItem?.MouseClick();
        }
    }
}
