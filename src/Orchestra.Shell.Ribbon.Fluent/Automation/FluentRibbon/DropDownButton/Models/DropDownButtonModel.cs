namespace Orchestra.Automation.FluentRibbon
{
    using Fluent;
    using Orc.Automation;

    [ActiveAutomationModel]
    public class DropDownButtonModel : ItemsControlModel
    {
        public DropDownButtonModel(AutomationElementAccessor accessor)
            : base(accessor)
        {

        }

        public RibbonControlSize Size { get; set; }
        public RibbonControlSizeDefinition? SimplifiedSizeDefinition { get; set; }
        public string? KeyTip { get; set; }
        public bool IsContextMenuOpened { get; set; }
        public object? Header { get; set; }
        public object? Icon { get; set; }
        public object? LargeIcon { get; set; }
        public object? MediumIcon { get; set; }
        public bool HasTriangle { get; set; }
        public bool IsDropDownOpen { get; set; }
        public ContextMenuResizeMode ResizeMode { get; set; }
        public double MaxDropDownHeight { get; set; }
        public double DropDownHeight { get; set; }
        public bool ClosePopupOnMouseDown { get; set; }
        public int ClosePopupOnMouseDownDelay { get; set; }
        public bool IsSimplified { get; set; }
    }
}
