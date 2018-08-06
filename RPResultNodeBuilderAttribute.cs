using System;

namespace RoslynPath
{
    [AttributeUsage(AttributeTargets.Class)]
    class RPResultNodeBuilderAttribute : Attribute
    {
        public Type ElementType { get; }

        public RPResultNodeBuilderAttribute(Type elementType)
        {
            ElementType = elementType;
        }
    }
}
