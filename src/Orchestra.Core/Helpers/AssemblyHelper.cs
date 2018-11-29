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
    public static class AssemblyHelper
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        #region Methods
        /// <summary>
        /// Gets the entry assembly.
        /// </summary>
        /// <returns>Assembly.</returns>
        public static Assembly GetEntryAssembly()
        {
            // For now we use a different implementation than in Catel
            // As a workaround for https://github.com/Catel/Catel/issues/1208, use Orchestra for now. In the future,
            // this whole AssemblyHelper must be removed
            //return Catel.Reflection.AssemblyHelper.GetEntryAssembly();

            Assembly assembly = null;

            try
            {
                if (assembly == null)
                {
                    assembly = Assembly.GetEntryAssembly();
                }

                if (assembly == null)
                {
                    throw new NotSupportedException("AppDomains without an entry assembly are not supported");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to get assembly");
            }

            if (assembly == null)
            {
                Log.Warning("Entry assembly could not be determined, returning Orchestra.Core as fallback");

                assembly = typeof(AssemblyHelper).GetAssemblyEx();
            }

            return assembly;
        }
        #endregion
    }
}
