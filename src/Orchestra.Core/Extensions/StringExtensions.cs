// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
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
            ArgumentNullException.ThrowIfNull(commandName);

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
            ArgumentNullException.ThrowIfNull(commandName);

            if (!commandName.Contains("."))
            {
                return commandName;
            }

            return commandName.Split(new[] { '.' })[1];
        }
        #endregion
    }
}