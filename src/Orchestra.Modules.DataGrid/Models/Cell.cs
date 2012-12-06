// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Cell.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Modules.DataGrid.Models
{
    using System.Diagnostics;

    using Catel.Data;

    /// <summary>
    /// Cell with value.
    /// </summary>
    [DebuggerDisplay("C:'{Value}'")]
    public class Cell : ObservableObject, ICell
    {
        private object _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// </summary>
        /// <param name="value">Initial value of the cell.</param>
        public Cell(object value = null)
        {
            _value = value;
        }

        #region ICell Members
        /// <summary>
        /// The value of a cell.
        /// </summary>
        public object Value
        {
            get { return _value; }
            set
            {
                if (Equals(value, _value))
                {
                    return;
                }
                _value = value;
                RaisePropertyChanged("Value");
            }
        }
        #endregion
    }
}