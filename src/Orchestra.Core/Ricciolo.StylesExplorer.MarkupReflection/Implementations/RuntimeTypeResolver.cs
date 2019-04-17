namespace Ricciolo.StylesExplorer.MarkupReflection.Implementations
{
    using System;
    using System.Reflection;
    using Catel;
    using Catel.Reflection;

    internal class RuntimeTypeResolver : IDotNetTypeResolver
    {
        private readonly Assembly _entryAssembly;

        public RuntimeTypeResolver()
        {
            _entryAssembly = AssemblyHelper.GetEntryAssembly();
        }

        public IDependencyPropertyDescriptor GetDependencyPropertyDescriptor(string name, IDotNetType ownerType, IDotNetType targetType)
        {
            Argument.IsNotNull(() => ownerType);

            if (ownerType is RuntimeType runtimeType)
            {
                return new RuntimeTypeDependencyPropertyDescriptor(runtimeType.Type, name);
            }

            if (ownerType is UnresolvableType)
            {
                return new UnresolvableDependencyPropertyDescriptor();
            }

            throw new ArgumentException("Invalid type: " + ownerType.GetType());
        }

        public IDotNetType GetTypeByAssemblyQualifiedName(string name)
        {
            var type = TypeCache.GetType(name);
            if (type is null)
            {
                return null;
            }

            return new RuntimeType(type);
        }

        public bool IsLocalAssembly(string name)
        {
            return TypeHelper.GetAssemblyName(name) == TypeHelper.GetAssemblyName(_entryAssembly.FullName);
        }
    }
}
