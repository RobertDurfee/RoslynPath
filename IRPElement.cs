using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace RoslynPath
{
    interface IRPElement
    {
        IEnumerable<SyntaxNode> Matches(SyntaxNode input);
    }
}
