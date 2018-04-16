// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyInfo.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using System.Reflection;
using System.Resources;
using System.Windows;
using System.Windows.Markup;

// All other assembly info is defined in SolutionAssemblyInfo.cs

[assembly: AssemblyTitle("Orchestra.Core")]
[assembly: AssemblyProduct("Orchestra.Core")]
[assembly: AssemblyDescription("Orchestra.Core library")]
[assembly: NeutralResourcesLanguage("en-US")]

[assembly: XmlnsPrefix("http://schemas.wildgums.com/orchestra", "orchestra")]
[assembly: XmlnsDefinition("http://schemas.wildgums.com/orchestra", "Orchestra.Behaviors")]
[assembly: XmlnsDefinition("http://schemas.wildgums.com/orchestra", "Orchestra.Converters")]
[assembly: XmlnsDefinition("http://schemas.wildgums.com/orchestra", "Orchestra.Controls")]
[assembly: XmlnsDefinition("http://schemas.wildgums.com/orchestra", "Orchestra.Markup")]
[assembly: XmlnsDefinition("http://schemas.wildgums.com/orchestra", "Orchestra.Views")]
[assembly: XmlnsDefinition("http://schemas.wildgums.com/orchestra", "Orchestra.Windows")]

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
    //(used if a resource is not found in the page, 
    // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
    //(used if a resource is not found in the page, 
    // app, or any theme specific resource dictionaries)
    )]