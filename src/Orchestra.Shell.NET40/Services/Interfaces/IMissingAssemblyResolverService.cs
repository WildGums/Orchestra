// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMissingAssemblyResolverService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Interface to resolve missing assemblies by Orchestra.
    /// </summary>
    public interface IMissingAssemblyResolverService
    {
        /// <summary>
        /// Resolves the assembly and referenced assemblies.
        /// </summary>
        /// <param name="assemblyFileName">Name of the assembly file.</param>
        /// <returns>The resolved assembly or <c>null</c> if the assembly cannot be resolved.</returns>
        /// <exception cref="ArgumentException">The <paramref name="assemblyFileName"/> is <c>null</c> or whitespace.</exception>
        Assembly ResolveAssembly(string assemblyFileName);
    }
}