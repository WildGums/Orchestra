namespace Orchestra.Automation
{
    using System;
    using Catel;
    using FluentRibbon;
    using Orc.Automation;

    public static class RibbonExtensions
    {
        private static readonly object DummyScopeObject = new();

        public static TView? GetView<TView>(this Ribbon ribbon, string tabName, string viewName)
            where TView : AutomationControl
        {
            ArgumentNullException.ThrowIfNull(ribbon);

            var ribbonGroupBox = ribbon.GetGroupBox(tabName, viewName);
            var view = ribbonGroupBox?.GetContent<TView>();

            return view;
        }

        public static IDisposable OpenBackstageView<TBackstageContentView>(this Ribbon ribbon, out TBackstageContentView? view)
            where TBackstageContentView : AutomationControl
        {
            ArgumentNullException.ThrowIfNull(ribbon);

            var backstage = ribbon.OpenBackstage();

            Wait.UntilInputProcessed(1000);

            view = backstage?.GetContent<TBackstageContentView>();

            return new DisposableToken(DummyScopeObject, _ => { }, _ => ribbon.CloseBackstage());
        }

        public static IDisposable OpenTabItemBackstageView<TBackstageTabItemContentView>(this Ribbon ribbon, string header, out TBackstageTabItemContentView? view)
            where TBackstageTabItemContentView : AutomationControl
        {
            ArgumentNullException.ThrowIfNull(ribbon);

            var backstageScope = ribbon.OpenBackstageView<BackstageTabControl>(out var tabControl);

            Wait.UntilInputProcessed(1000);

            view = tabControl?.GetItemContent<TBackstageTabItemContentView>(header);

            return backstageScope;
        }
    }
}
