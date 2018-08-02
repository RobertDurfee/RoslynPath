using System.Collections.Generic;
using System.Linq;

namespace RoslynPath
{
    internal class RoslynPathMatch
    {
        public RoslynPathMatch(RoslynPathMatchNode root)
        {
            Root = root;
        }

        public RoslynPathMatchNode Root { get; }

        public RoslynPathMatch RemoveNullBranches()
        {
            return new RoslynPathMatch(RemoveNullBranchesRecursive(Root));
        }

        private RoslynPathMatchNode RemoveNullBranchesRecursive(RoslynPathMatchNode node)
        {
            if (node == null)
                return null;

            IEnumerable<RoslynPathMatchNode> evaluatedChildren = node.Children.Select(cn => RemoveNullBranchesRecursive(cn));

            // All the children (with null branches removed) are null !!AND!! there are children
            if (evaluatedChildren.All(ec => ec == null) && node.Children.Any())
                return null;
            // There some children (with null branches removed) which aren't null !!OR!! there aren't any children
            else
                return new RoslynPathMatchNode(node.Parent, node.SyntaxNode, evaluatedChildren.Where(ec => ec != null));
        }
    }
}
