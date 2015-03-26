// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandNameToStringConverter.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Converters
{
    using System;
    using System.Linq;
    using Catel.MVVM.Converters;

    /// <summary>
    /// Converts a command name to a string.
    /// </summary>
    public class CommandNameToStringConverter : ValueConverterBase
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
            var stringValue = value as string;
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return stringValue;
            }

            var splittedStrings = (from x in stringValue.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries)
                                   select x.SplitCamelCaseString());

            return string.Join(" ➝ ", splittedStrings);
        }
    }
}