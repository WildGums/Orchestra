#if NET

#pragma warning disable 1591 // 1591 = missing xml

namespace Orchestra.StylesExplorer.MarkupReflection
{
    /// <summary>
    /// Interface rappresenting a DotNet type
    /// </summary>
    internal interface IType
    {
        string AssemblyQualifiedName { get; }
        bool IsSubclassOf(IType type);
        bool Equals(IType type);
    }
}

#endif