// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Row.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Modules.DataGrid.Models
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Row contains cells.
    /// </summary>
    public class Row
    {
        /// <summary>
        /// Initizlizes a new instance of the <see cref="Row"/> class.
        /// </summary>
        public Row()
        {
            Cells = new ObservableCollection<ICell>();
        }

        /// <summary>
        /// Cells of the row.
        /// </summary>
        public ObservableCollection<ICell> Cells { get; private set; }
    }
}