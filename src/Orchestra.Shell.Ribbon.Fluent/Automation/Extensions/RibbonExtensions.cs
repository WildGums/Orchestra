namespace Orchestra.Automation
{
    using System;
    using Catel;
    using FluentRibbon;
    using Orc.Automation;

    public static class RibbonExtensions
    {
        public static IDisposable OpenBackstageView<TBackstageContentView>(this Ribbon ribbon, out TBackstageContentView view)
            where TBackstageContentView : AutomationControl
        {
            Argument.IsNotNull(() => ribbon);

            var backstage = ribbon.OpenBackstage();

            view = backstage.GetContent<TBackstageContentView>();

            return new DisposableToken(null, _ => { }, _ => ribbon.CloseBackstage());
        }

        public static IDisposable OpenTabItemBackstageView<TBackstageTabItemContentView>(this Ribbon ribbon, string header, out TBackstageTabItemContentView view)
            where TBackstageTabItemContentView : AutomationControl
        {
            Argument.IsNotNull(() => ribbon);

            var backstageScope = ribbon.OpenBackstageView<BackstageTabControl>(out var tabControl);

            view = tabControl.GetItemContent<TBackstageTabItemContentView>(header);

            return backstageScope;
        }

        //public static TBackstage OpenBackstage<TBackstage>(this Ribbon ribbon)
        //{
        //    var backstage = ribbon.OpenBackstage();
        //    return backstage.As<TBackstage>();
        //}

        //public static IDisposable OpenBackstageViewScope<TView>(this Ribbon ribbon, string tabName, out TView view, string id = null)
        //    where TView : AutomationControl
        //{
        //    Argument.IsNotNull(() => ribbon);

        //    var backstage = ribbon.OpenBackstage();
        //    view = backstage.GetView<TView>(tabName, id);

        //    return new DisposableToken(null, _ => { }, _ => ribbon.CloseBackstage());
        //}

        //public static IDisposable OpenBackstageViewScope<TView>(this Ribbon ribbon, out TView view, int tabIndex = 0, string id = null)
        //    where TView : AutomationControl
        //{
        //    Argument.IsNotNull(() => ribbon);
        //    var backstage = ribbon.OpenBackstage();
        //    view = backstage.GetView<TView>(tabIndex, id);

        //    return new DisposableToken(null, _ => { }, _ => ribbon.CloseBackstage());
        //}
    }
}
