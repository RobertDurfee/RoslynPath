using System;
using System.Collections.Generic;
using System.Linq;

namespace RoslynPath
{
    [RPElementBuilder(typeof(RPRegexTokenType))]
    class RPRegexElementBuilder : RPElementBuilder
    {
        public RPRegexElementBuilder(Dictionary<string, string> options, IRPElement element) : base(options, element) { }

        public override int ConvertTokens(IEnumerable<RPToken> tokens)
        {
            string regex = tokens.First().Value;

            if (!Options.ContainsKey(typeof(RPScanTypes).Name))
                throw new Exception("ScanTypes not defined.");

            if (!Enum.TryParse(Options[typeof(RPScanTypes).Name], true, out RPScanTypes scanType))
                throw new Exception("Invalid ScanType");

            Element = new RPRegexElement(RPRegexFactory.Create(regex), scanType);

            return base.ConvertTokens(tokens.Skip(1)) + 1;
        }
    }
}
