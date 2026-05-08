namespace Orchestra.Tests;

using NUnit.Framework;

public partial class UriInfoFacts
{
    [TestFixture]
    public class The_Constructor
    {
        [Test]
        public void Sets_Uri_And_Uses_It_As_DisplayText_When_DisplayText_Not_Provided()
        {
            var uriInfo = new UriInfo("https://example.com");

            Assert.That(uriInfo.Uri, Is.EqualTo("https://example.com"));
            Assert.That(uriInfo.DisplayText, Is.EqualTo("https://example.com"));
        }

        [Test]
        public void Sets_Uri_And_DisplayText_When_Both_Provided()
        {
            var uriInfo = new UriInfo("https://example.com", "Example");

            Assert.That(uriInfo.Uri, Is.EqualTo("https://example.com"));
            Assert.That(uriInfo.DisplayText, Is.EqualTo("Example"));
        }
    }
}
