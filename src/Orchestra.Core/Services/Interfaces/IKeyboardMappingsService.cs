namespace Orchestra.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IKeyboardMappingsService
    {
        List<KeyboardMapping> AdditionalKeyboardMappings { get; }

        Task LoadAsync();
        Task SaveAsync();
        Task ResetAsync();
    }
}
