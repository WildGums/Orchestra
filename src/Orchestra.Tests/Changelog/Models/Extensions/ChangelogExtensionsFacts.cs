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
                var changelog1 = new Changelog();
                var changelog2 = new Changelog();

                var delta = changelog1.GetDelta(changelog2);

                Assert.AreEqual(0, delta.Items.Count);
                Assert.IsTrue(delta.IsEmpty);
            }

            [TestCase]
            public void ReturnsValidSnapshot()
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

                Assert.AreEqual(1, delta.Items.Count);
            }
        }
    }
}
