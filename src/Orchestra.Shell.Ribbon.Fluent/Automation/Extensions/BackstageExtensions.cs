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

        //public static TView GetView<TView>(this Backstage backstage, string tabName, string id = null)
        //    where TView : AutomationControl
        //{
        //    Argument.IsNotNull(() => backstage);

        //    var tab = backstage.Find<Tab>(id: "ribbonBackstageTabControl");
        //    var item = tab.Items?.FirstOrDefault(x => Equals(x.Header, tabName));
        //    if (item is null)
        //    {
        //        return default;
        //    }

        //    item.IsSelected = true;

        //    return backstage.Find<TView>(id: id);
        //}

        //public static TView GetView<TView>(this Backstage backstage, int tabIndex = 0, string id = null)
        //    where TView : AutomationControl
        //{
        //    Argument.IsNotNull(() => backstage);

        //    var tab = backstage.Find<Tab>(id: "ribbonBackstageTabControl");
        //    tab.SelectedIndex = tabIndex;

        //    return backstage.Find<TView>(id: id);
        //}
    }
}
