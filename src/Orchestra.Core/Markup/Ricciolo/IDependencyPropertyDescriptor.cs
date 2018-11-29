#if NET || NETCORE

#pragma warning disable 1591 // 1591 = missing xml

namespace Orchestra.StylesExplorer.MarkupReflection
{
    internal interface IDependencyPropertyDescriptor
    {
        bool IsAttached { get; }
    }
}

#endif
