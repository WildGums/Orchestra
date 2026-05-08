namespace Orchestra.Tests;

using System;
using NUnit.Framework;

public partial class RecentlyUsedItemFacts
{
    [TestFixture]
    public class The_Constructor
    {
        [Test]
        public void Sets_Name_And_DateTime()
        {
            var dateTime = new DateTime(2024, 1, 15, 10, 30, 0);

            var item = new RecentlyUsedItem("MyProject.xml", dateTime);

            Assert.That(item.Name, Is.EqualTo("MyProject.xml"));
            Assert.That(item.DateTime, Is.EqualTo(dateTime));
        }
    }
}
