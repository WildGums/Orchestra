// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICommandInfo.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Models
{
    using Catel.Windows.Input;

    public interface ICommandInfo
    {
        #region Properties
        string CommandName { get; }
        InputGesture InputGesture { get; set; }
        bool IsHidden { get; set; }
        #endregion
    }
}