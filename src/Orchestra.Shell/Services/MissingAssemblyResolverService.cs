// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingAssemblyResolverService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Catel;
    using Catel.IO;
    using Catel.Logging;

    /// <summary>
    /// Service to resolve missing assemblies.
    /// </summary>
    public class MissingAssemblyResolverService : IMissingAssemblyResolverService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Resolves the assembly and referenced assemblies.
        /// </summary>
        /// <param name="assemblyFileName">Name of the assembly file.</param>
        /// <returns>The resolved assembly or <c>null</c> if the assembly cannot be resolved.</returns>
        /// <exception cref="ArgumentException">The <paramref name="assemblyFileName"/> is <c>null</c> or whitespace.</exception>
        public Assembly ResolveAssembly(string assemblyFileName)
        {
            Argument.IsNotNull("assemblyFileName", assemblyFileName);

            Log.Debug("Resolving assembly '{0}' manually", assemblyFileName);

            var appDomain = AppDomain.CurrentDomain;
            var assemblyDirectory = Catel.IO.Path.GetParentDirectory(assemblyFileName);

            // Load references
            var assemblyForReflectionOnly = Assembly.ReflectionOnlyLoadFrom(assemblyFileName);
            foreach (var referencedAssembly in assemblyForReflectionOnly.GetReferencedAssemblies())
            {
                if (!appDomain.GetAssemblies().Any(a => string.CompareOrdinal(a.GetName().Name, referencedAssembly.Name) == 0))
                {
                    // First, try to load from GAC
                    if (referencedAssembly.GetPublicKeyToken() != null)
                    {
                        try
                        {
                            appDomain.Load(referencedAssembly.FullName);
                            continue;
                        }
                        catch (Exception)
                        {
                            Log.Debug("Failed to load assembly '{0}' from GAC, trying local file", referencedAssembly.FullName);
                        }
                    }

                    // Second, try to load from directory (including referenced assemblies)
                    var referencedAssemblyPath = Path.Combine(assemblyDirectory, referencedAssembly.Name + ".dll");
                    ResolveAssembly(referencedAssemblyPath);
                }
            }

            // Load assembly itself
            Assembly assembly = null;

            try
            {
                if (appDomain.GetAssemblies().Any(a => string.CompareOrdinal(a.GetName().FullName, assemblyForReflectionOnly.FullName) == 0))
                {
                    Log.Info("Loading assembly '{0}' is not required because it is already loaded", assemblyForReflectionOnly.FullName);
                }
                else
                {
                    assembly = Assembly.LoadFrom(assemblyFileName);

                    Log.Info("Resolved assembly '{0}' manually", assemblyFileName);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to load assembly '{0}'", assemblyFileName);
            }

            return assembly;
        }
    }
}