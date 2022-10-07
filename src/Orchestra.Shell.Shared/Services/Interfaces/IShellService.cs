namespace Orchestra.Services
{
    using System.Threading.Tasks;
    using Views;

    public partial interface IShellService
    {
        /// <summary>
        /// Gets the shell.
        /// </summary>
        /// <value>The shell.</value>
        IShell? Shell { get; }

        /// <summary>
        /// Creates a new shell.
        /// </summary>
        /// <typeparam name="TShell">The type of the shell.</typeparam>
        /// <returns>The created shell.</returns>
        /// <exception cref="OrchestraException">The shell is already created and cannot be created again.</exception>
        Task<TShell> CreateAsync<TShell>()
            where TShell : class, IShell;
    }
}
