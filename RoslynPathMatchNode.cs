using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace RoslynPath
{
    internal class RoslynPathMatchNode
    {
        public RoslynPathMatchNode(RoslynPathMatchNode parent, SyntaxNode syntaxNode)
        {
            Parent = parent;
            SyntaxNode = syntaxNode;
            Children = new List<RoslynPathMatchNode>();
        }

        public RoslynPathMatchNode(RoslynPathMatchNode parent, SyntaxNode syntaxNode, IEnumerable<RoslynPathMatchNode> children)
        {
            Parent = parent;
            SyntaxNode = syntaxNode;
            Children = children.ToList();
        }

        public RoslynPathMatchNode Parent { get; }

        public SyntaxNode SyntaxNode { get; }

        public List<RoslynPathMatchNode> Children { get; }
    }
}
