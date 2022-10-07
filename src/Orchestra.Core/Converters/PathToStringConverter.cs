namespace Orchestra.Converters
{
    using System;
    using Catel.MVVM.Converters;

    /// <summary>
    /// Converts a path to a string.
    /// </summary>
    public class PathToStringConverter : ValueConverterBase<string>
    {
        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The <see cref="T:System.Type" /> of data expected by the target dependency property.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <returns>The value to be passed to the target dependency property.</returns>
        protected override object? Convert(string value, Type targetType, object? parameter)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            var maxCharacters = 25;

            if (parameter is int)
            {
                maxCharacters = (int)parameter;
            }

            if (parameter is string parameterAsString && int.TryParse(parameterAsString, out var newMaxCharacters))
            {
                maxCharacters = newMaxCharacters;
            }

            if (value.Length > maxCharacters)
            {
                value = string.Format("...{0}", value.Substring(value.Length - maxCharacters));
            }

            return value;
        }
    }
}
