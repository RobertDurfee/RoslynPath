using Microsoft.CodeAnalysis;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using System.Collections.Generic;
using System;

namespace RoslynPath
{
    public static class SyntaxNodeRoslynPathExtensions
    {
        public static IEnumerable<SyntaxNode> SelectNodes(this SyntaxNode syntaxNode, string path)
        {
            throw new NotImplementedException();
        }

        public static SyntaxNode SelectNode(this SyntaxNode syntaxNode, string path)
        {
            throw new NotImplementedException();
        }
    }
}
