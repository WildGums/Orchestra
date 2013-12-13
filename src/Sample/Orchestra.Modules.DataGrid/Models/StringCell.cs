// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringCell.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Modules.DataGrid.Models
{
    /// <summary>
    /// Cell with string value.
    /// </summary>
    public class StringCell : Cell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringCell"/> class.
        /// </summary>
        public StringCell()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringCell"/> class.
        /// </summary>
        /// <param name="value">Initial value of the cell.</param>
        public StringCell(string value)
            : base(value)
        {
        }
    }
}