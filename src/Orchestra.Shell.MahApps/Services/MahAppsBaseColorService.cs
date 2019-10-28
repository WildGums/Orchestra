namespace Orchestra.Services
{
    using System.Collections.Generic;
    public class MahAppsBaseColorSchemeService : BaseColorSchemeService
    {
        public override IReadOnlyList<string> GetAvailableBaseColorSchemes()
        {
            return new List<string>() { OrchestraEnvironment.LightBaseColorScheme, OrchestraEnvironment.DarkBaseColorScheme }.AsReadOnly();
        }
    }
}
