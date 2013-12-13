// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICell.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Modules.DataGrid.Models
{
    /// <summary>
    /// Interface of a cell.
    /// </summary>
    public interface ICell
    {
        /// <summary>
        /// The value of a cell.
        /// </summary>
        object Value { get; set; }
    }
}