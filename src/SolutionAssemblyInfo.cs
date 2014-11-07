// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SharedAssemblyInfo.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using System.Resources;
using System.Windows.Markup;

// Shared assembly info that is common for all assemblies of this project

////[assembly: AssemblyTitle("DEFINED IN ACTUAL ASSEMBLYINFO")]
////[assembly: AssemblyProduct("DEFINED IN ACTUAL ASSEMBLYINFO")]
////[assembly: AssemblyDescription("DEFINED IN ACTUAL ASSEMBLYINFO")]

[assembly: AssemblyCompany("CatenaLogic")]
[assembly: AssemblyCopyright("Copyright © Orchestra team 2010 - 2014")]
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

[assembly: AssemblyVersion("2.0")]
[assembly: AssemblyInformationalVersion("2.0, Nightly, Released on 2013-09-01 13:32")]


[assembly: XmlnsPrefix("https://github.com/orcomp/orchestra", "orchestra")]
[assembly: XmlnsDefinition("https://github.com/orcomp/orchestra", "Orchestra.Behaviors")]
[assembly: XmlnsDefinition("https://github.com/orcomp/orchestra", "Orchestra.Converters")]
[assembly: XmlnsDefinition("https://github.com/orcomp/orchestra", "Orchestra.Controls")]
[assembly: XmlnsDefinition("https://github.com/orcomp/orchestra", "Orchestra.Markup")]
[assembly: XmlnsDefinition("https://github.com/orcomp/orchestra", "Orchestra.Views")]
[assembly: XmlnsDefinition("https://github.com/orcomp/orchestra", "Orchestra.Windows")]