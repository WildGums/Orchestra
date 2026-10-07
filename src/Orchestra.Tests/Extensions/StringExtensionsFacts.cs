namespace Orchestra.Tests;

using NUnit.Framework;

public partial class StringExtensionsFacts
{
    [TestFixture]
    public class The_GetCommandGroup_Method
    {
        [TestCase("File.Open", "File")]
        [TestCase("Edit.Copy", "Edit")]
        public void Returns_Group_For_CommandName_With_Dot(string commandName, string expectedGroup)
        {
            var result = commandName.GetCommandGroup();

            Assert.That(result, Is.EqualTo(expectedGroup));
        }

        [TestCase("Open")]
        [TestCase("Copy")]
        public void Returns_Empty_String_For_CommandName_Without_Dot(string commandName)
        {
            var result = commandName.GetCommandGroup();

            Assert.That(result, Is.EqualTo(string.Empty));
        }
    }

    [TestFixture]
    public class The_GetCommandName_Method
    {
        [TestCase("File.Open", "Open")]
        [TestCase("Edit.Copy", "Copy")]
        public void Returns_Name_Part_For_CommandName_With_Dot(string commandName, string expectedName)
        {
            var result = commandName.GetCommandName();

            Assert.That(result, Is.EqualTo(expectedName));
        }

        [TestCase("Open")]
        [TestCase("Copy")]
        public void Returns_Full_CommandName_When_No_Dot(string commandName)
        {
            var result = commandName.GetCommandName();

            Assert.That(result, Is.EqualTo(commandName));
        }
    }
}
