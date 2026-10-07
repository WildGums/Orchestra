namespace Orchestra.Tests;

using NUnit.Framework;

public partial class HintFacts
{
    [TestFixture]
    public class The_Constructor
    {
        [Test]
        public void Sets_Text_And_ControlName()
        {
            var hint = new Hint("Click here to proceed", "btnSubmit");

            Assert.That(hint.Text, Is.EqualTo("Click here to proceed"));
            Assert.That(hint.ControlName, Is.EqualTo("btnSubmit"));
        }
    }
}
