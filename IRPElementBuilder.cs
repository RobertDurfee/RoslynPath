using System.Collections.Generic;

namespace RoslynPath
{
    interface IRPElementBuilder
    {
        void Clean();

        IRPElement Element { get; }

        int ConvertTokens(IEnumerable<RPToken> tokens);
    }
}
