using System.Collections.Generic;
using System.Linq;

namespace RoslynPath
{
    class RPTokenListReader
    {
        private IRPBuilder _builder;

        public RPTokenListReader(IRPBuilder builder)
        {
            _builder = builder;
        }

        public void ReadTokenList(IEnumerable<RPToken> tokens)
        {
            List<RPToken> tokenList = tokens.ToList();

            for (int index = 0; index < tokenList.Count; index++)
            {
                int tokensConsumed = _builder.ConvertTokens(tokenList.Skip(index));

                if (tokensConsumed > 0)
                    index += tokensConsumed - 1;
            }
        }
    }
}
