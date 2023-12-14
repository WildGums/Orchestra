namespace Orchestra.Configuration
{
    using Catel;
    using Catel.Configuration;

    public static class ConfigurationExtensions
    {
        public static bool IsConfigurationKey(this ConfigurationChangedEventArgs e, string expectedKey)
        {
            return IsConfigurationKey(e.Key, expectedKey);
        }

        public static bool IsConfigurationKey(this string key, string expectedKey)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return true;
            }

            return key.EqualsIgnoreCase(expectedKey);
        }
    }
}
