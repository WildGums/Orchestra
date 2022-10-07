namespace Orchestra.Automation
{
    using System;
    using Catel;
    using FluentRibbon;
    using Orc.Automation;

    public static class RibbonExtensions
    {
        public static TView GetView<TView>(this Ribbon ribbon, string tabName, string viewName)
            where TView : AutomationControl
        {
            ArgumentNullException.ThrowIfNull(ribbon);

            var ribbonGroupBox = ribbon.GetGroupBox(tabName, viewName);
            var view = ribbonGroupBox?.GetContent<TView>();

            return view;
        }

        public static IDisposable OpenBackstageView<TBackstageContentView>(this Ribbon ribbon, out TBackstageContentView view)
            where TBackstageContentView : AutomationControl
        {
            ArgumentNullException.ThrowIfNull(ribbon);

            var backstage = ribbon.OpenBackstage();

            view = backstage.GetContent<TBackstageContentView>();

            return new DisposableToken(null, _ => { }, _ => ribbon.CloseBackstage());
        }

        public static IDisposable OpenTabItemBackstageView<TBackstageTabItemContentView>(this Ribbon ribbon, string header, out TBackstageTabItemContentView view)
            where TBackstageTabItemContentView : AutomationControl
        {
            ArgumentNullException.ThrowIfNull(ribbon);

            var backstageScope = ribbon.OpenBackstageView<BackstageTabControl>(out var tabControl);

            view = tabControl.GetItemContent<TBackstageTabItemContentView>(header);

            return backstageScope;
        }
    }
}
