// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyboardMappings.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Models
{
    using System.Collections.Generic;
    using Catel.Data;

    public class KeyboardMappings : ModelBase
    {
        public KeyboardMappings()
        {
            Mappings = new List<KeyboardMapping>();
        }

        public string GroupName { get; set; }

        public List<KeyboardMapping> Mappings { get; private set; }
    }
}