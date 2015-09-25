// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System;
    using Catel;

    /// <summary>
    /// Extension methods for strings.
    /// </summary>
    public static class StringExtensions
    {
        #region Methods
        /// <summary>
        /// Gets the command group from the command name.
        /// </summary>
        /// <param name="commandName">Name of the command.</param>
        /// <returns>System.String.</returns>
        public static string GetCommandGroup(this string commandName)
        {
            Argument.IsNotNull(() => commandName);

            if (!commandName.Contains("."))
            {
                return string.Empty;
            }

            return commandName.Split(new[] {'.'})[0];
        }

        /// <summary>
        /// Gets the name of the command from the command name.
        /// </summary>
        /// <param name="commandName">Name of the command.</param>
        /// <returns>System.String.</returns>
        public static string GetCommandName(this string commandName)
        {
            Argument.IsNotNull(() => commandName);

            if (!commandName.Contains("."))
            {
                return commandName;
            }

            return commandName.Split(new[] { '.' })[1];
        }

        /// <summary>
        /// Converts the value from a camel case string to a splitted string. For example, <c>ThisTest</c> will be
        /// converted to <c>this test</c>.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The splitted string.</returns>
        public static string SplitCamelCaseString(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            var finalString = string.Empty;

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

        /// <summary>
        /// Executes a string comparison that is case insensitive.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="valueToCheck">The value to check.</param>
        /// <returns><c>true</c> if the strings are equal, <c>false</c> otherwise.</returns>
        [ObsoleteEx(ReplacementTypeOrMember = "Catel.StringExtensions.EqualsIgnoreCase", TreatAsErrorFromVersion = "2.3.0", RemoveInVersion = "3.0.0")]
        public static bool EqualsIgnoreCase(this string str, string valueToCheck)
        {
            return string.Equals(str, valueToCheck, StringComparison.OrdinalIgnoreCase);
        }
        #endregion
    }
}