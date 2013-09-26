// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleInitialized.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Orchestra.Messages
{
    /// <summary>
    /// The message, which sent when the module has been initialized.
    /// </summary>
    public class ModuleInitializedMessage
    {
        /// <summary>
        /// The name of module
        /// </summary>
        public string ModuleName { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleName"></param>
        public ModuleInitializedMessage(string moduleName)
        {
            ModuleName = moduleName;
        }
    }
}