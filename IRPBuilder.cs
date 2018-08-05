using System.Collections.Generic;

namespace RoslynPath
{
    interface IRPBuilder
    {
        int ConvertTokens(IEnumerable<RPToken> tokens);
    }
}
