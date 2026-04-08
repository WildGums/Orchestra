namespace Orchestra.Tests.Changelog;

using NUnit.Framework;
using Orchestra.Changelog;

public class ChangelogExtensionsFacts
{
    [TestFixture]
    public class The_GetDelta_Method
    {
        [TestCase]
        public void Returns_Empty_Snapshot()
        {
            var changelog1 = new Changelog();
            var changelog2 = new Changelog();

            var delta = changelog1.GetDelta(changelog2);

            Assert.That(delta.Items.Count, Is.EqualTo(0));
            Assert.That(delta.IsEmpty, Is.True);
        }

        [TestCase]
        public void Returns_Valid_Snapshot()
        {
            var changelog1 = new Changelog();

            var changelog2 = new Changelog();
            changelog2.Items.Add(new ChangelogItem
            {
                Group = "Test",
                Name = "This is a test item",
                Type = ChangelogType.Improvement
            });

            var delta = changelog1.GetDelta(changelog2);

            Assert.That(delta.Items.Count, Is.EqualTo(1));
        }
    }
}
