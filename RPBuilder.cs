using System;
using System.Collections.Generic;

namespace RoslynPath
{
    class RPBuilder : IRPBuilder
    {
        public List<IRPElement> RoslynPath { get; } = new List<IRPElement>();

        public int ConvertTokens(IEnumerable<RPToken> tokens)
        {
            int tokensConsumed = 0;
            RPScanTypes scanType = RPScanTypes.Invalid;
            bool inBrackets = false;
            IRPElement newElement = null;

            foreach (RPToken token in tokens)
            {
                tokensConsumed++;

                if (token.TokenType == typeof(RPGlobalRootTokenType))
                {
                    newElement = new RPGlobalRootElement();
                    break;
                }
                else if (token.TokenType == typeof(RPDeepTokenType))
                {
                    scanType = RPScanTypes.Descendants;
                }
                else if (token.TokenType == typeof(RPOpenBracketTokenType))
                {
                    inBrackets = true;

                    if (scanType == RPScanTypes.Invalid)
                        scanType = RPScanTypes.Children;
                }
                else if (token.TokenType == typeof(RPRegexTokenType))
                {
                    if (!inBrackets)
                        throw new FormatException("Regex is not wrapped in brackets.");

                    newElement = new RPRegexElement(RPRegexFactory.Create(token.Value), scanType);
                }
                else if (token.TokenType == typeof(RPIntegerTokenType))
                {
                    if (!inBrackets)
                        throw new FormatException("Index is not wrapped in brackets.");

                    if (!int.TryParse(token.Value, out int index))
                        throw new FormatException("Unable to parse index.");

                    newElement = new RPIndexElement(index, scanType);
                }
                else if (token.TokenType == typeof(RPCloseBracketTokenType))
                {
                    inBrackets = false;
                    break;
                }
            }

            if (newElement == null)
                throw new FormatException("Unable to create IRPElement from tokens.");

            if (inBrackets)
                throw new FormatException("Mismatched or missing brackets detected.");

            RoslynPath.Add(newElement);

            return tokensConsumed;
        }
    }
}
