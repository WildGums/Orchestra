namespace Orchestra.Models
{
    /// <summary>
    /// Settings for docked views, that will be used when showing docked windows.
    /// </summary>
    public class DockingSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DockingSettings"/> class.
        /// </summary>
        public DockingSettings()
        {
            DockLocation = DockLocation.Right;
            Width = 250;
        }

        /// <summary>
        /// Gets or sets the dock location.
        /// </summary>
        /// <value>
        /// The dock location.
        /// </value>
        public DockLocation DockLocation { get; set; }

        /// <summary>
        /// Gets or sets the top of the floating window.
        /// This setting is only used for floating windpos.
        /// </summary>
        /// <value>
        /// The top.
        /// </value>
        public int Top { get; set; }

        /// <summary>
        /// Gets or sets the left of the floating window.
        /// This setting is only used for floating windpos.
        /// </summary>
        /// <value>
        /// The left.
        /// </value>
        public int Left { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int Height { get; set; }
    }
}
