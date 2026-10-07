namespace Orchestra;

public static class LogFilePrefixes
{
    /// <summary>
    /// The crashreport prefix.
    /// </summary>
    public static readonly string CrashReport = "Crashreport";

    /// <summary>
    /// The entry assembly name prefix.
    /// </summary>
    public static readonly string EntryAssemblyName = Catel.Reflection.AssemblyHelper.GetRequiredEntryAssembly().GetName().Name ?? string.Empty;

    /// <summary>
    /// The 'Log' file log prefix.
    /// </summary>
    public static readonly string Log = "Log";

    /// <summary>
    /// All file log prefixes
    /// </summary>
    public static readonly string[] All =
    {
        EntryAssemblyName,
        CrashReport,
        Log
    };
}
