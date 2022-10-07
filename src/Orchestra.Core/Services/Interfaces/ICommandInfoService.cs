namespace Orchestra.Services
{
    public interface ICommandInfoService
    {
        ICommandInfo GetCommandInfo(string commandName);
        void UpdateCommandInfo(string commandName, ICommandInfo commandInfo);
        void Invalidate();
    }
}
