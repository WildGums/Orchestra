// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputGestureExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System.Windows.Input;
    using Catel;
    using InputGesture = Catel.Windows.Input.InputGesture;

    public static class InputGestureExtensions
    {
        public static bool IsEmpty(this InputGesture inputGesture)
        {
            if (inputGesture == null)
            {
                return true;
            }

            if (inputGesture.Key != Key.None)
            {
                return false;
            }

            if (inputGesture.Modifiers != ModifierKeys.None)
            {
                return false;
            }

            return true;
        }
    }
}