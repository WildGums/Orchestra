using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Orchestra.ViewModels;

namespace Orchestra.Controls
{
    /// <summary>
    /// Interaction logic for LocalizedTextBlock.xaml
    /// </summary>
    public partial class LocalizedTextBlock : TextBlock, ILocalizedControl
    {
        /// <summary>
        /// Resource string key prefix.
        /// </summary>
        public string LocalizationContainer { get; set; }

        /// <summary>
        /// Property to localize
        /// </summary>
        public DependencyProperty LocalizedProperty
        {
            get
            {
                return TextBlock.TextProperty;
            }
        }

        /// <summary>
        /// Adds binding of content to localized string.
        /// </summary>
        public override void EndInit()
        {
            base.EndInit();
            var binding = new Binding(LocalizationContainer + "_" + this.Name);
            binding.Source = new DynamicTextsSource();
            this.SetBinding(LocalizedProperty, binding);
        }
    }
}