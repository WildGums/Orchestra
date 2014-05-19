// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleInitializer.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Catel.IoC;

/// <summary>
/// Used by the ModuleInit. All code inside the Initialize method is ran as soon as the assembly is loaded.
/// </summary>
public static partial class ModuleInitializer
{
    static partial void InitializeSpecific()
    {
        var serviceLocator = ServiceLocator.Default;

        // TODO: Write services
    }
}