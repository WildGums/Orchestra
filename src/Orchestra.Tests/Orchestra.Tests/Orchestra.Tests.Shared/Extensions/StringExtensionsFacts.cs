// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensionsFacts.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Tests
{
    using NUnit.Framework;

    public class StringExtensionsFacts
    {
        [TestFixture]
        public class TheEqualsIgnoreCaseMethod
        {
            #region Methods
            [TestCase("bla", "BLA", true)]
            [TestCase("bla", "bla", true)]
            [TestCase("bla", "BLAT", false)]
            [TestCase("bla", "blat", false)]
            public void WorksCorrectly(string value1, string value2, bool expectedValue)
            {
                Assert.AreEqual(expectedValue, value1.EqualsIgnoreCase(value2));
            }
            #endregion
        }
    }
}