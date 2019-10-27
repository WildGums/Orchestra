namespace Orchestra.Services
{
    using System.Collections.Generic;
    public class MahAppsBaseColorService : BaseColorService
    {
        public override IReadOnlyList<string> GetAvailableBaseColors()
        {
            return new List<string>() { OrchestraEnvironment.DefaultBaseColor, "Dark" }.AsReadOnly();
        }
    }
}
