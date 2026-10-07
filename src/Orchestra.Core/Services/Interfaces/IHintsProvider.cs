namespace Orchestra;

using System.Windows;

public interface IHintsProvider
{
    IHint[] GetHintsFor(FrameworkElement element);
}
