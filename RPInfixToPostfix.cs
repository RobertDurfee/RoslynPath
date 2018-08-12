using System;
using System.Linq;
using System.Collections.Generic;

namespace RoslynPath
{
    internal static class RPInfixToPostfix
    {
        private static readonly HashSet<Type> _operators = new HashSet<Type>()
        {
            typeof(RPAndTokenType),
            typeof(RPOrTokenType)
        };

        private static readonly HashSet<Type> _operands = new HashSet<Type>()
        {
            typeof(RPRegexTokenType),
            typeof(RPSyntaxKindTokenType),
            typeof(RPIntegerTokenType)
        };

        public static IEnumerable<RPToken> Sort(IEnumerable<RPToken> inputSequence)
        {
            Stack<RPToken> operatorStack = new Stack<RPToken>();
            List<RPToken> outputSequence = new List<RPToken>();

            foreach (RPToken token in inputSequence)
            {
                if (_operands.Contains(token.TokenType))
                    outputSequence.Add(token);

                else if (_operators.Contains(token.TokenType) && (operatorStack.Count == 0 || operatorStack.Peek().TokenType == typeof(RPOpenParentheseTokenType)))
                    operatorStack.Push(token);

                else if (token.TokenType == typeof(RPOpenParentheseTokenType))
                    operatorStack.Push(token);

                else if (token.TokenType == typeof(RPCloseParentheseTokenType))
                {
                    if (operatorStack.Count == 0)
                        throw new Exception("Parenthese mismatch detected.");

                    RPToken @operator = operatorStack.Pop();
                    while (@operator.TokenType != typeof(RPOpenParentheseTokenType))
                    {
                        outputSequence.Add(@operator);

                        if (operatorStack.Count == 0)
                            throw new Exception("Parenthese mismatch detected.");

                        @operator = operatorStack.Pop();
                    }
                }

                else if (_operators.Contains(token.TokenType))
                {
                    outputSequence.Add(operatorStack.Pop());
                    operatorStack.Push(token);
                }

                else
                    throw new Exception($"Unknown token '{token.Value}'.");                
            }

            if (operatorStack.Any(t => t.TokenType == typeof(RPOpenParentheseTokenType) || t.TokenType == typeof(RPCloseParentheseTokenType)))
                throw new Exception("Parenthese mismatch detected.");

            while (operatorStack.Count > 0)
                outputSequence.Add(operatorStack.Pop());

            return outputSequence;
        }
    }
}
