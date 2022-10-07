namespace Orchestra.Automation.FluentRibbon
{
    using System.Windows.Input;
    using Orc.Automation;

    [ActiveAutomationModel]
    public class RibbonGroupBoxModel : ControlModel
    {
        public RibbonGroupBoxModel(AutomationElementAccessor accessor) 
            : base(accessor)
        {
        }

        public bool CanAddToQuickAccessToolBar { get; set; }
        public bool IsSnapped { get; set; }
        public bool IsSimplified { get; private set; }
        public object? Icon { get; set; }
        public object? MediumIcon { get; set; }
        public bool IsSeparatorVisible { get; set; }
        public bool IsDropDownOpen { get; set; }
        public object? LauncherToolTip { get; set; }
        public bool IsLauncherEnabled { get; set; }
        public ICommand? LauncherCommand { get; set; }
        public object? LauncherCommandParameter { get; set; }
    }
}
