namespace Orchestra.Automation.FluentRibbon
{
    using System.Windows;
    using System.Windows.Media;
    using Orc.Automation;

    [ActiveAutomationModel]
    public class BackstageTabControlModel : SelectorModel
    {
        public BackstageTabControlModel(AutomationElementAccessor accessor)
            : base(accessor)
        {
        }

        public Thickness? SelectedContentMargin { get; set; }
        public object? SelectedContent { get; set; }
        public string? ContentStringFormat { get; set; }
        public string? SelectedContentStringFormat { get; set; }
        public double ItemsPanelMinWidth { get; set; }
        public SolidColorBrush? ItemsPanelBackground { get; set; }
        public bool IsWindowSteeringHelperEnabled { get; set; }
        public bool IsBackButtonVisible { get; set; }
    }
}
