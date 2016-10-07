// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyHelper.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System.Reflection;

    /// <summary>
    /// Assembly helper class.
    /// </summary>
    public static class AssemblyHelper
    {
        #region Methods
        /// <summary>
        /// Gets the entry assembly.
        /// </summary>
        /// <returns>Assembly.</returns>
        public static Assembly GetEntryAssembly()
        {
            return Catel.Reflection.AssemblyHelper.GetEntryAssembly();
        }
        #endregion
    }
}