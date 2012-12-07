using System.Windows;
using System.Windows.Media;

namespace TableView
{
  public class TableViewUtils
  {
    // searches a template tree for an element
    public static T FindParent<T>(FrameworkElement element) where T : class
    {
      for (FrameworkElement element2 = element.TemplatedParent as FrameworkElement; element2 != null; element2 = element2.TemplatedParent as FrameworkElement)
      {
        T local = element2 as T;
        if (local != null)
          return local;
      }
      return default(T);
    }

    // Searches the visual tree for the element of the specified type
    public static T GetAncestorByType<T>(DependencyObject element) where T : class
    {
      if (element == null)
        return default(T);

      if (element as T != null)
        return element as T;

      return GetAncestorByType<T>(VisualTreeHelper.GetParent(element));
    }
  }
}
