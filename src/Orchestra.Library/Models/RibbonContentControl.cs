namespace Orchestra.Models
{
    using System.Windows;
    using Catel;

    /// <summary>
    /// Interface for RibbonItem with the ability to show dynamic content
    /// </summary>
    public interface IRibbonContentControl
    {
        /// <summary>
        /// Gets or sets ContentTemplate for the custom content.
        /// </summary>
        /// <value>The item image.</value>
        DataTemplate ContentTemplate { get; set; }
    }

    /// <summary>
    /// RibbonItem with the ability to show dynamic content.
    /// </summary>
    public class RibbonContentControl : RibbonControlBase, IRibbonContentControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonContentControl"/> class.
        /// </summary>
        /// <param name="tabItemHeader">The tab item header.</param>
        /// <param name="groupBoxHeader">The group box header.</param>
        /// <param name="itemHeader">The item header.</param>
        /// <param name="behavior">The behavior.</param>
        public RibbonContentControl(string tabItemHeader, string groupBoxHeader, string itemHeader = null, RibbonBehavior behavior = RibbonBehavior.ActivateTab)
            : base(tabItemHeader, groupBoxHeader, itemHeader, behavior)
        {
        }

        /// <summary>
        /// Gets or sets ContentTemplate for the custom content.
        /// </summary>
        /// <value>The item image.</value>
        public DataTemplate ContentTemplate { get; set; }
    }
}
