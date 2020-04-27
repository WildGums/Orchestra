namespace Orchestra.Tests
{
    using NUnit.Framework;
    using Orchestra.Theming;

    [TestFixture]
    public class ThemeHelperFacts
    {
        [TestCase("/Orchestra.Core;component/themes/nonexisting.xaml", ExpectedResult = false)]
        [TestCase("/Orchestra.Core;component/themes/generic.xaml", ExpectedResult = true)]
        public bool IsResourceDictionaryAvailable(string uri)
        {
            var themeManager = new ThemeManager(new AccentColorService(), new BaseColorSchemeService());
            return themeManager.IsResourceDictionaryAvailable(uri);
        }
    }
}
