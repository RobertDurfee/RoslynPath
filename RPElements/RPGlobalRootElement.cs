using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace RoslynPath
{
    class RPGlobalRootElement : IRPElement
    {
        public IEnumerable<SyntaxNode> Matches(SyntaxNode input)
        {
            yield return input;
        }
    }
}
