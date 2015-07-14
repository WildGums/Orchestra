// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PathToStringConverter.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Converters
{
    using System;
    using Catel.MVVM.Converters;

    /// <summary>
    /// Converts a path to a string.
    /// </summary>
    public class PathToStringConverter : ValueConverterBase<string>
    {
        #region Methods
        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The <see cref="T:System.Type" /> of data expected by the target dependency property.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <returns>The value to be passed to the target dependency property.</returns>
        protected override object Convert(string value, Type targetType, object parameter)
        {
            var stringValue = value as string;
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return stringValue;
            }

            var maxCharacters = 25;

            if (parameter is int)
            {
                maxCharacters = (int) parameter;
            }

            if (parameter is string)
            {
                int newMaxCharacters;
                if (int.TryParse((string)parameter, out newMaxCharacters))
                {
                    maxCharacters = newMaxCharacters;
                }
            }

            if (stringValue.Length > maxCharacters)
            {
                stringValue = string.Format("...{0}", stringValue.Substring(stringValue.Length - maxCharacters));
            }

            return stringValue;
        }
        #endregion
    }
}