// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyboardMapping.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Models
{
    using Catel.Data;
    using Catel.Windows.Input;

    public class KeyboardMapping : ModelBase
    {
        public KeyboardMapping()
        {
        }

        public string CommandName { get; set; }

        public InputGesture InputGesture { get; set; } 
    }
}