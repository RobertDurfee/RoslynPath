using System;

namespace RoslynPath
{
    internal class RPToken
    {
        public RPToken(Type tokenType, string value)
        {
            if (tokenType.IsInterface || tokenType.IsAbstract || !typeof(IRPTokenType).IsAssignableFrom(tokenType))
                throw new ArgumentException($"{tokenType} does not implement IRPTokenType.");

            TokenType = tokenType;
            Value = value;
        }

        public Type TokenType { get; }
        public string Value { get; set; } = string.Empty;
    }
}
