namespace Orchestra
{
    using Catel.Windows.Input;

    public interface ICommandInfo
    {
        string CommandName { get; }
        InputGesture? InputGesture { get; set; }
        bool IsHidden { get; set; }
    }
}
