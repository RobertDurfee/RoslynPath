using System;

namespace RoslynPath
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class RPElementBuilderAttribute : Attribute
    {
        public Type TokenType { get; }

        public RPElementBuilderAttribute(Type tokenType)
        {
            TokenType = tokenType;
        }
    }
}
