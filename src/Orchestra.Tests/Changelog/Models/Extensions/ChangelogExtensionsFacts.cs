namespace Orchestra.Tests.Changelog
{
    using NUnit.Framework;
    using Orchestra.Changelog;

    public class ChangelogExtensionsFacts
    {
        [TestFixture]
        public class TheGetDeltaMethod
        {
            [TestCase]
            public void ReturnsEmptySnapshot()
            {
                var changelog1 = new Orchestra.Changelog.Changelog();
                var changelog2 = new Orchestra.Changelog.Changelog();

                var delta = changelog1.GetDelta(changelog2);

                Assert.AreEqual(0, delta.Items.Count);
                Assert.IsTrue(delta.IsEmpty);
            }

            [TestCase]
            public void ReturnsValidSnapshot()
            {
                var changelog1 = new Orchestra.Changelog.Changelog();

                var changelog2 = new Orchestra.Changelog.Changelog();
                changelog2.Items.Add(new Orchestra.Changelog.ChangelogItem
                {
                    Group = "Test",
                    Name = "This is a test item",
                    Type = Orchestra.Changelog.ChangelogType.Improvement
                });

                var delta = changelog1.GetDelta(changelog2);

                Assert.AreEqual(1, delta.Items.Count);
            }
        }
    }
}
