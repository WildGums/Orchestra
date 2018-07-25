namespace Orchestra.Reflection
{
    using Catel.Reflection;
    using System.Reflection;

    public class EntryAssemblyResolver : IEntryAssemblyResolver
    {
        public Assembly Resolve()
        {
            // As a workaround for https://github.com/Catel/Catel/issues/1208, use Orchestra for now
            return Orchestra.AssemblyHelper.GetEntryAssembly();
        }
    }
}
