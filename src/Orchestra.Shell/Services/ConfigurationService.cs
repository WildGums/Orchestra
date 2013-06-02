namespace Orchestra.Services
{
    using Models;

    /// <summary>
    /// The configuration service is responisble for managing all configurable items in the Orchestra shell.
    /// The application using the Orchestra Shell, can configure it's appearence by using this service.
    /// </summary>
    public interface IConfigurationService
    {
        /// <summary>
        /// Gets the configuration object, holding all configarable items.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        Configuration Configuration { get; }
    }

    /// <summary>
    /// The configuration service is responisble for managing all configurable items in the Orchestra shell.
    /// The application using the Orchestra Shell, can configure it's appearence by using this service.
    /// </summary>
    class ConfigurationService : IConfigurationService
    {
        private Configuration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationService"/> class.
        /// </summary>
        public ConfigurationService()
        {
            CreateDefaultConfiguration();
        }

        private void CreateDefaultConfiguration()
        {
            _configuration = new Configuration();
            _configuration.HelpTabText = Resources.Orchestra_Shell_Resources.HelpRibbonTabText;
            _configuration.HelpGroupText = Resources.Orchestra_Shell_Resources.HelpGroupText;
            _configuration.HelpButtonText = Resources.Orchestra_Shell_Resources.HelpButtonText;
        }

        /// <summary>
        /// Gets the configuration object, holding all configarable items.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public Configuration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    CreateDefaultConfiguration();
                }
                return _configuration;
            }
            
        }
    }
}
