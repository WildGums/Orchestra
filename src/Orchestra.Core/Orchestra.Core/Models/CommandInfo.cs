// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandInfo.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Models
{
    using Catel.Windows.Input;

    public class CommandInfo
    {
        #region Constructors
        public CommandInfo(string commandName, InputGesture inputGesture)
        {
            CommandName = commandName;
            InputGesture = inputGesture;
        }
        #endregion

        public string CommandName { get; set; }

        public InputGesture InputGesture { get; set; } 
    }
}