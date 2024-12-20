namespace Orchestra.Tests.Logging
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Catel.IO;
    using Catel.Services;
    using Moq;
    using NUnit.Framework;
    using Orchestra.Logging;

    public class FileLogListenerFacts
    {
        public class The_GetApplicationDataDirectory_Method
        {
            [TestCase("", "")]
            [TestCase("company", "")]
            [TestCase("", "product")]
            [TestCase("company", "product")]
            public async Task Returns_Correct_Version_Async(string company, string product)
            {
                var appDataServiceMock = new Mock<IAppDataService>();
                appDataServiceMock.Setup(x => x.GetApplicationDataDirectory(It.IsAny<ApplicationDataTarget>()))
                    .Returns<ApplicationDataTarget>(x => "%appdata%");

                var fileLogListener = new TestFileLogListener(appDataServiceMock.Object, null);

                Assert.That(fileLogListener.FilePath, Is.Not.Empty);

                Assert.That(fileLogListener.Calls.Count, Is.Not.EqualTo(0));

                appDataServiceMock.Verify(x => x.GetApplicationDataDirectory(It.IsAny<ApplicationDataTarget>()));
            }
        }

        private class TestFileLogListener : FileLogListener
        {
            public TestFileLogListener(IAppDataService appDataService, Assembly? assembly = null)
                : base(appDataService, assembly)
            {
            }

            public bool HasCreatedDirectory { get { return Calls.Any(); } }

            public List<string> Calls { get; private set; } = new List<string>();

            protected override void CreateDirectory(string directory)
            {
                Calls.Add(directory);
            }
        }
    }
}
