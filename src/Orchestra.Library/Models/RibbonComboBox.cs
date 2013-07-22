namespace Orchestra.Models
{
    /// <summary>
    /// Represents a ribbon combo box
    /// </summary>
    public class RibbonComboBox : RibbonControlBase, IRibbonComboBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonComboBox" /> class.
        /// </summary>
        /// <param name="tabItemHeader">The tab item header.</param>
        /// <param name="groupBoxHeader">The group box header.</param>
        /// <param name="itemHeader">The item header.</param>
        /// <param name="behavior">The behavior.</param>
        public RibbonComboBox(string tabItemHeader, string groupBoxHeader, string itemHeader = null, RibbonBehavior behavior = RibbonBehavior.ActivateTab) 
            : base(tabItemHeader, groupBoxHeader, itemHeader, behavior)
        {
        }

        /// <summary>
        /// Gets or sets the items source.
        /// </summary>
        /// <value>
        /// The items source.
        /// </value>
        public string ItemsSource { get; set; }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>
        /// The selected item.
        /// </value>
        public string SelectedItem { get; set; }
    }
}
