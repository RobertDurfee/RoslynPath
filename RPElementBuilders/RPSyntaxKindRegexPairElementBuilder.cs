using System;
using System.Collections.Generic;
using System.Linq;

namespace RoslynPath
{
    [RPElementBuilder(typeof(RPSyntaxKindRegexPairTokenType))]
    class RPSyntaxKindRegexPairElementBuilder : RPElementBuilder
    {
        public RPSyntaxKindRegexPairElementBuilder(Dictionary<string, string> options, IRPElement element) : base(options, element) { }

        public override int ConvertTokens(IEnumerable<RPToken> tokens)
        {
            if (Element == null || !(Element is RPSyntaxKindElement syntaxKindElement))
                throw new Exception("An RPSyntaxKindRegexPairTokenType must be preceeded by an RPSyntaxKindTokenType.");

            RPToken token = tokens.ElementAtOrDefault(1);

            if (token == null || !(token.TokenType == typeof(RPRegexTokenType)))
                throw new Exception("The RPSyntaxKindRegexPairTokenType must be followed by an RPRegexTokenType.");

            string regex = token.Value;

            Element = new RPSyntaxKindRegexPairElement(syntaxKindElement.SyntaxKind, RPRegexFactory.Create(regex), syntaxKindElement.ScanType);

            return base.ConvertTokens(tokens.Skip(2)) + 2;
        }
    }
}
