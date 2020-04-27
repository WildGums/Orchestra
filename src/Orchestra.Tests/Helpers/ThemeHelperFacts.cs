namespace Orchestra.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class ThemeHelperFacts
    {
        [TestCase("/Orchestra.Core;component/themes/nonexisting.xaml", ExpectedResult = false)]
        [TestCase("/Orchestra.Core;component/themes/generic.xaml", ExpectedResult = true)]
        public bool IsResourceDictionaryAvailable(string uri)
        {
            return ThemeHelper.IsResourceDictionaryAvailable(uri);
        }
    }
}
