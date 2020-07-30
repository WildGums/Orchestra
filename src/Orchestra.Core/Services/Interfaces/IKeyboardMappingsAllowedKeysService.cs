namespace Orchestra.Services
{
    using System.Windows.Input;

    public interface IKeyboardMappingsAllowedKeysService
    {
        bool IsAllowed(Key key);
    }
}
