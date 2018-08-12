using System.Collections.Generic;

namespace RoslynPath
{
    [RPElementBuilder(typeof(RPCloseBracketTokenType))]
    internal class RPCloseBracketConsumer : RPElementBuilder
    {
        public RPCloseBracketConsumer(Dictionary<string, string> options, IRPElement element) : base(options, element) { }

        public override int ConvertTokens(IEnumerable<RPToken> tokens)
        {
            return 1;
        }
    }
}
