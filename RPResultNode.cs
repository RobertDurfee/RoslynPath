using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace RoslynPath
{
    internal class RPResultNode : IRPResultNode
    {
        public RPResultNode(IRPResultNode parent, SyntaxNode syntaxNode)
        {
            Parent = parent;
            SyntaxNode = syntaxNode;
        }

        public IRPResultNode Parent { get; }

        public SyntaxNode SyntaxNode { get; }

        public List<IRPResultNode> Children { get; } = new List<IRPResultNode>();
    }
}
