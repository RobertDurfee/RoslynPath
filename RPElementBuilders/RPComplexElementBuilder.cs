using System;
using System.Collections.Generic;
using System.Linq;

namespace RoslynPath
{
    internal class RPComplexElementBuilder : RPElementBuilder
    {
        public RPComplexElementBuilder(Dictionary<string, string> options, IRPElement element) : base(options, element) { }

        public override int ConvertTokens(IEnumerable<RPToken> tokens)
        {
            Options["IsComplex"] = "False";

            IEnumerable<RPToken> expressionPostfix = tokens.TakeWhile(t => t.TokenType != typeof(RPCloseBracketTokenType));

            if (!Options.ContainsKey(typeof(RPScanTypes).Name))
                throw new Exception("ScanTypes not defined.");

            if (!Enum.TryParse(Options[typeof(RPScanTypes).Name], true, out RPScanTypes scanType))
                throw new Exception("Invalid ScanType");

            Element = new RPComplexElement(expressionPostfix, scanType);

            int consumedTokens = expressionPostfix.Count();

            return base.ConvertTokens(tokens.Skip(consumedTokens)) + consumedTokens;
        }
    }
}
