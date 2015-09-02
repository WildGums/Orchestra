// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHint.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Models
{
    public interface IHint
    {
        #region Properties
        string Text { get; }

        string ControlName { get; }
        #endregion
    }
}