using System;
using System.Collections.Generic;
using System.Linq;

namespace RoslynPath
{
    [RPElementBuilder(typeof(RPOpenBracketTokenType))]
    internal class RPOpenBracketConsumer : RPElementBuilder
    {
        public RPOpenBracketConsumer(Dictionary<string, string> options, IRPElement element) : base(options, element) { }

        public override int ConvertTokens(IEnumerable<RPToken> tokens)
        {
            if (!Options.ContainsKey(typeof(RPScanTypes).Name))
                Options[typeof(RPScanTypes).Name] = Enum.GetName(typeof(RPScanTypes), (int)RPScanTypes.Children);

            IEnumerable<RPToken> expressionTokens = tokens.Skip(1).TakeWhile(t => t.TokenType != typeof(RPCloseBracketTokenType));

            if (expressionTokens.Count() > 1)
            {
                Options["IsComplex"] = "True";

                IEnumerable<RPToken> expressionTokensPostfix = RPInfixToPostfix.Apply(expressionTokens);
                IEnumerable<RPToken> remainingTokens = tokens.Skip(1 + expressionTokens.Count());

                int consumedTokens = 1 + (expressionTokens.Count() - expressionTokensPostfix.Count());

                return base.ConvertTokens(expressionTokensPostfix.Concat(remainingTokens)) + consumedTokens;
            }
            else
            {
                Options["IsComplex"] = "False";
                return base.ConvertTokens(tokens.Skip(1)) + 1;
            }
        }
    }
}
