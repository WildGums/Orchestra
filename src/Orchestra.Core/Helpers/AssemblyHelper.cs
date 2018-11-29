// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyHelper.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Reflection;

    /// <summary>
    /// Assembly helper class.
    /// </summary>
    [ObsoleteEx(ReplacementTypeOrMember = "Catel.Reflection.AssemblyHelper", TreatAsErrorFromVersion = "5.0", RemoveInVersion = "6.0")]
    public static class AssemblyHelper
    {
        #region Methods
        /// <summary>
        /// Gets the entry assembly.
        /// </summary>
        /// <returns>Assembly.</returns>
        public static Assembly GetEntryAssembly()
        {
            var assembly = Catel.Reflection.AssemblyHelper.GetEntryAssembly();
            return assembly;
        }
        #endregion
    }
}
