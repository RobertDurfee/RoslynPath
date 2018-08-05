using System;
using System.Collections.Generic;
using System.Linq;

namespace RoslynPath
{
    [RPElementBuilder(typeof(RPDeepTokenType))]
    class RPDeepConsumer : RPElementBuilder
    {
        public RPDeepConsumer(Dictionary<string, string> options, IRPElement element) : base(options, element) { }

        public override int ConvertTokens(IEnumerable<RPToken> tokens)
        {
            Options[typeof(RPScanTypes).Name] = Enum.GetName(typeof(RPScanTypes), (int)RPScanTypes.Descendants);

            return base.ConvertTokens(tokens.Skip(1)) + 1;
        }
    }
}
