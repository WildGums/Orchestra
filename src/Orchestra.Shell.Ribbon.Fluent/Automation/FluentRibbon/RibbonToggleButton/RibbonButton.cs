namespace Orchestra.Automation.FluentRibbon;

using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

[Control(ClassName = "RibbonToggleButton")]
public class RibbonToggleButton(AutomationElement element)
    : FrameworkElement<ButtonModel>(element)
{
    public string? Content => Element.Current.Name;
    public bool? IsChecked
    {
        get => Element.GetToggleState();
        set => Element.TrySetToggleState(value);
    }

    public bool Click()
    {
        return Element.TryInvoke();
    }
}
