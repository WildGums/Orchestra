// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Person.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.MahApps.Models
{
    using System.Collections.Generic;
    using Catel.Data;

    public class Person : ModelBase
    {
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public override string ToString()
        {
            var names = new List<string>();

            if (!string.IsNullOrEmpty(FirstName))
            {
                names.Add(FirstName);
            }

            if (!string.IsNullOrEmpty(MiddleName))
            {
                names.Add(MiddleName);
            }

            if (!string.IsNullOrEmpty(LastName))
            {
                names.Add(LastName);
            }

            return string.Join(" ", names);
        }
    }
}