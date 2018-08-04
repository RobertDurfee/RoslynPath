using System;

namespace RoslynPath
{
    class RPTokenMatch
    {
        public RPTokenMatch(Type tokenType)
        {
            if (tokenType.IsInterface || tokenType.IsAbstract || !typeof(IRPTokenType).IsAssignableFrom(tokenType))
                throw new ArgumentException($"{tokenType} does not implement IRPTokenType.");

            TokenType = tokenType;
            Precedence = ((IRPTokenType)Activator.CreateInstance(tokenType)).Precedence;
        }

        public Type TokenType { get; }

        public RPTokenPrecedence Precedence { get; }

        public string Value { get; set; }

        public int StartIndex { get; set; }

        public int EndIndex { get; set; }
    }
}
