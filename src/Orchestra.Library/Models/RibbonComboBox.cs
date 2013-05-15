namespace Orchestra.Models
{
    using Catel;

    /// <summary>
    /// Represents a ribbon combo box
    /// </summary>
    public class RibbonComboBox : RibbonItemBase, IRibbonComboBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonComboBox" /> class.
        /// </summary>
        /// <param name="tabItemHeader">The tab item header.</param>
        /// <param name="groupBoxHeader">The group box header.</param>
        /// <param name="itemHeader">The item header.</param>
        /// <param name="itemsSource">The items source property name.</param>
        /// <param name="selectedItem">The selected item property name.</param>
        /// <param name="behavior">The behavior.</param>
        public RibbonComboBox(string tabItemHeader, string groupBoxHeader, string itemHeader, string itemsSource, string selectedItem, RibbonBehavior behavior = RibbonBehavior.ActivateTab) 
            : base(tabItemHeader, groupBoxHeader, itemHeader, behavior)
        {
            // TODO: consider to make itemHeader parameter optional and set default null value

            Argument.IsNotNullOrEmpty(() => itemsSource);
            Argument.IsNotNullOrEmpty(() => selectedItem);

            ItemsSource = itemsSource;
            SelectedItem = selectedItem;
        }

        /// <summary>
        /// Gets or sets the items source.
        /// </summary>
        /// <value>
        /// The items source.
        /// </value>
        public string ItemsSource { get; private set; }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>
        /// The selected item.
        /// </value>
        public string SelectedItem { get; private set; }
    }
}
