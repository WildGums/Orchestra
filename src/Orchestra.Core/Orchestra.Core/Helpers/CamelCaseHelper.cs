// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CamelCaseHelper.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    /// <summary>
    /// Helper for camel case text.
    /// </summary>
    public static class CamelCaseHelper
    {
        #region Methods
        /// <summary>
        /// Splits the specified input by camel case.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The string as camel case splitted string.</returns>
        public static string SplitByCamelCase(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            string finalString = string.Empty;

            for (int i = 0; i < input.Length; i++)
            {
                if (i != 0)
                {
                    if (char.IsUpper(input[i]))
                    {
                        finalString += " " + char.ToLower(input[i]);
                        continue;
                    }
                }

                finalString += input[i];
            }

            return finalString;
        }
        #endregion
    }
}