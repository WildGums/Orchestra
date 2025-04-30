namespace Orchestra
{
    using System;
    using Orchestra.Views;

    public partial class ShellRecoveryContext
    {
        public ShellRecoveryContext()
        {
        }

        public Exception? Exception { get; init; }

        public IShell? Shell { get; init; }
    }
}
