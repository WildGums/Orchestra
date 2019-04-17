namespace Ricciolo.StylesExplorer.MarkupReflection.Implementations
{
    using System;
    using Catel;

    internal class RuntimeType : IDotNetType
    {
        private readonly Type _type;

        public RuntimeType(Type type)
        {
            _type = type;
        }

        public Type Type => _type;

        public string AssemblyQualifiedName
        {
            get
            {
                return $"{_type.FullName}, {_type.Assembly.FullName}";
            }
        }

        public bool IsSubclassOf(IDotNetType type)
        {
            Argument.IsNotNull(() => type);

            if (!(type is RuntimeType runtimeType))
            {
                return false;
            }

            return _type.IsSubclassOf(runtimeType._type);
        }

        public bool Equals(IDotNetType type)
        {
            Argument.IsNotNull(() => type);

            if (!(type is RuntimeType runtimeType))
            {
                return false;
            }

            return _type.Equals(runtimeType._type);
        }

        public override string ToString()
        {
            return string.Format("[Runtime Type={0}]", _type);
        }

        public IDotNetType BaseType
        {
            get
            {
                var baseType = _type.BaseType;
                if (baseType is null)
                {
                    return null;
                }

                return new RuntimeType(baseType);
            }
        }
    }
}
