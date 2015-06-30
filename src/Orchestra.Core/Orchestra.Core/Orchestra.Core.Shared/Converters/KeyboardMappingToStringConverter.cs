// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputGestureToStringConverter.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Converters
{
    using System;
    using Catel.MVVM.Converters;
    using Models;

    /// <summary>
    /// Converts an keyboard mapping to a string.
    /// </summary>
    public class KeyboardMappingToStringConverter : ValueConverterBase
    {
        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The <see cref="T:System.Type" /> of data expected by the target dependency property.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <returns>The value to be passed to the target dependency property.</returns>
        protected override object Convert(object value, Type targetType, object parameter)
        {
            var keyboardMapping = value as KeyboardMapping;
            if (keyboardMapping == null)
            {
                return string.Empty;
            }

            var inputGesture = keyboardMapping.InputGesture;
            if (inputGesture != null)
            {
                return inputGesture.ToString();
            }

            var inputGestureAsText = keyboardMapping.Text;
            if (!string.IsNullOrWhiteSpace(inputGestureAsText))
            {
                return inputGestureAsText;
            }

            return string.Empty;
        }
    }
}