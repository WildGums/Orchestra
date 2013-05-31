using System.Windows;

namespace Orchestra.Controls
{
    /// <summary>
    /// Localized control interface
    /// </summary>
    public interface ILocalizedControl
    {
        /// <summary>
        /// Resource string key prefix.
        /// </summary>
        string LocalizationContainer { get; set; }

        /// <summary>
        /// Property to localize
        /// </summary>
        DependencyProperty LocalizedProperty { get; }
    }
}