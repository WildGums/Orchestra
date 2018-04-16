// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHint.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
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