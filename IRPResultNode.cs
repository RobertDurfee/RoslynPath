using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace RoslynPath
{
    interface IRPResultNode
    {
        IRPResultNode Parent { get; }

        SyntaxNode SyntaxNode { get; }

        List<IRPResultNode> Children { get; }
    }
}
