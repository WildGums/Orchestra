namespace Ricciolo.StylesExplorer.MarkupReflection.Implementations
{
    using System;
    using Catel.Reflection;

    internal class RuntimeTypeDependencyPropertyDescriptor : IDependencyPropertyDescriptor
    {
        private readonly Type _type;
        private readonly string _member;

        public RuntimeTypeDependencyPropertyDescriptor(Type type, string name)
        {
            _type = type;
            _member = name;
        }

        public bool IsAttached
        {
            get
            {
                var getMethod = _type.GetMethodEx($"Get{_member}", allowStaticMembers: true);
                return getMethod != null;
            }
        }

        public override string ToString()
        {
            return string.Format("[RuntimeTypeDependencyPropertyDescriptor Member={0}, Type={1}]", _member, _type);
        }
    }
}
