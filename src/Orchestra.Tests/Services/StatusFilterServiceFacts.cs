namespace Orchestra.Tests;

using NUnit.Framework;

public partial class StatusFilterServiceFacts
{
    [TestFixture]
    public class The_GetStatus_Method
    {
        [Test]
        public void Returns_Status_When_Not_Suspended()
        {
            var service = new StatusFilterService { IsSuspended = false };

            var result = service.GetStatus("Processing...");

            Assert.That(result, Is.EqualTo("Processing..."));
        }

        [Test]
        public void Returns_Null_When_Suspended()
        {
            var service = new StatusFilterService { IsSuspended = true };

            var result = service.GetStatus("Processing...");

            Assert.That(result, Is.Null);
        }
    }
}
