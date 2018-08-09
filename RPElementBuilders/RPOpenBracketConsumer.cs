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

            return base.ConvertTokens(tokens.Skip(1)) + 1;
        }
    }
}
