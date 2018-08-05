using System.Collections.Generic;

namespace RoslynPath
{
    [RPElementBuilder(typeof(RPGlobalRootTokenType))]
    class RPGlobalRootElementBuilder : RPElementBuilder
    {
        public RPGlobalRootElementBuilder(Dictionary<string, string> options, IRPElement element) : base(options, element) { }
        
        public override int ConvertTokens(IEnumerable<RPToken> tokens)
        {
            Element = new RPGlobalRootElement();

            return 1;
        }
    }
}
