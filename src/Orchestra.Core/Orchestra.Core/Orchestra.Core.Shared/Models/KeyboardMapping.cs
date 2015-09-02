// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyboardMapping.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Models
{
    using System.Windows.Input;
    using Catel;
    using Catel.Data;
    using Catel.Windows.Input;
    using InputGesture = Catel.Windows.Input.InputGesture;

    public class KeyboardMapping : ModelBase
    {
        public KeyboardMapping()
        {
        }

        public KeyboardMapping(string commandName, string additionalText, ModifierKeys modifierKeys)
        {
            var text = string.Empty;

            if (modifierKeys != ModifierKeys.None)
            {
                foreach (var modifier in Enum<ModifierKeys>.GetValues())
                {
                    if (Enum<ModifierKeys>.Flags.IsFlagSet(modifierKeys, modifier) && modifier != ModifierKeys.None)
                    {
                        text += string.Format("{0} + ", modifier);
                    }
                }
            }

            CommandName = commandName;
            Text = string.Format("{0}{1}", text, additionalText);
        }

        public string CommandName { get; set; }

        public InputGesture InputGesture { get; set; } 

        public string Text { get; set; }

        public bool IsEditable { get; set; }
    }
}