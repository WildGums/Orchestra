// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterHelperFacts.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Tests
{
    using NUnit.Framework;

    public class FilterHelperFacts
    {
        [TestFixture]
        public class TheMatchesFilterMethod
        {
            [TestCase("mytest.log", true)]
            [TestCase("subdirectory\\test.log", true)]
            [TestCase("licenseinfo.xml", true)]
            [TestCase("LicenseInfo.xml", true)]
            [TestCase("license.xml", false)]
            [TestCase("license\\info.xml", false)]
            public void TheMatchesFilter(string file, bool expectedValue)
            {
                var filters = new[]
                {
                    "licenseinfo.xml",
                    "*.log"
                };

                Assert.AreEqual(expectedValue, FilterHelper.MatchesFilters(filters, file));
            }
        }
    }
}