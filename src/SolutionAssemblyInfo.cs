// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SharedAssemblyInfo.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

//#define SIGN_ASSEMBLIES

#pragma warning disable 1699    // 1699 = Use command line option '/keyfile' or appropriate project settings instead of 'AssemblyKeyFile'

using System;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;

// Shared assembly info that is common for all assemblies of this project

////[assembly: AssemblyTitle("DEFINED IN ACTUAL ASSEMBLYINFO")]
////[assembly: AssemblyProduct("DEFINED IN ACTUAL ASSEMBLYINFO")]
////[assembly: AssemblyDescription("DEFINED IN ACTUAL ASSEMBLYINFO")]

[assembly: AssemblyCompany("CatenaLogic")]
[assembly: AssemblyCopyright("Copyright © Orchestra team 2010 - 2013")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: NeutralResourcesLanguage("en-US")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:

[assembly: AssemblyVersion("0.1")]
[assembly: AssemblyInformationalVersion("0.50, Nightly, Released on 2013-05-02 19:41")]

#if DEBUG

#if NET35
[assembly: AssemblyConfiguration("NET35, Debug")]
#elif NET40
[assembly: AssemblyConfiguration("NET40, Debug")]
#elif NET45
[assembly: AssemblyConfiguration("NET45, Debug")]
#elif NET50
[assembly: AssemblyConfiguration("NET45, Debug")]
#elif SL4
[assembly: AssemblyConfiguration("Silverlight 4, Debug")]
#elif SL5
[assembly: AssemblyConfiguration("Silverlight 5, Debug")]
#elif WINDOWS_PHONE
[assembly: AssemblyConfiguration("Windows Phone 7, Debug")]
#elif NETFX_CORE
[assembly: AssemblyConfiguration("WinRT, Debug")]
#else
[assembly: AssemblyConfiguration("Unknown, Debug")]
#endif

#else

#if NET35
[assembly: AssemblyConfiguration("NET35, Release")]
#elif NET40
[assembly: AssemblyConfiguration("NET40, Release")]
#elif NET45
[assembly: AssemblyConfiguration("NET45, Release")]
#elif NET50
[assembly: AssemblyConfiguration("NET45, Release")]
#elif SL4
[assembly: AssemblyConfiguration("Silverlight 4, Release")]
#elif SL5
[assembly: AssemblyConfiguration("Silverlight 5, Release")]
#elif WINDOWS_PHONE
[assembly: AssemblyConfiguration("Windows Phone 7, Release")]
#elif NETFX_CORE
[assembly: AssemblyConfiguration("WinRT, Release")]
#else
[assembly: AssemblyConfiguration("Unknown, Release")]
#endif

#endif