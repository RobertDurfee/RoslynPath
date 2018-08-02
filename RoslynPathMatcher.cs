using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RoslynPath
{
    internal class RoslynPathMatcher
    {
        private SyntaxNode _root;
        private string _text;

        public RoslynPathMatcher(SyntaxNode root)
        {
            _root = root;
            _text = root.ToFullString();
        }

        public RoslynPathMatch Matches(RoslynPath roslynPath)
        {
            RoslynPathMatchNode rootMatchNode = new RoslynPathMatchNode(null, _root);

            // This match may have incomplete matches (null branches)
            RoslynPathMatchNode rawMatch = MatchesRecursive(rootMatchNode, roslynPath.Root, _text);

            return new RoslynPathMatch(rawMatch).RemoveNullBranches();
        }

        private RoslynPathMatchNode MatchesRecursive(RoslynPathMatchNode parentRoslynPathMatchNode, RoslynPathNode roslynPathNode, string searchText)
        {
            // The end of the path sequence has been reached
            if (roslynPathNode == null)
                return parentRoslynPathMatchNode;

            IEnumerable<TextSpan> matchingTextSpans = roslynPathNode.Pattern.Matches(searchText)
                                                                            .Cast<Match>()
                                                                            // Offset the matching index to correspond to the parent SyntaxNode's TextSpan
                                                                            .Select(m => new TextSpan(m.Index + parentRoslynPathMatchNode.SyntaxNode.SpanStart, m.Length));

            // No matches under this parent match tree node (THUS, NOT A FULL MATCH! This branch of the tree will be removed outside of recursion, see above.)
            if (matchingTextSpans.Count() == 0)
                return null;

            foreach (TextSpan matchingTextSpan in matchingTextSpans)
            {
                // Get the closest matching node to the current TextSpan
                SyntaxNode matchingNode = parentRoslynPathMatchNode.SyntaxNode.ChildNodes().Where(dn => dn.Span.OverlapsWith(matchingTextSpan))
                                                                                           // This function may be tuned in the future...
                                                                                           .OrderBy(dn => Math.Abs(dn.Span.Length - (int)dn.Span.Overlap(matchingTextSpan)?.Length))
                                                                                           .Last();

                // Restrict the child search text
                string childSearchText = searchText.Substring(matchingNode.Span.Start - parentRoslynPathMatchNode.SyntaxNode.SpanStart, matchingNode.Span.Length);

                // Add the matching child (and subsequent children)
                parentRoslynPathMatchNode.Children.Add(MatchesRecursive(new RoslynPathMatchNode(parentRoslynPathMatchNode, matchingNode), roslynPathNode.Next, childSearchText));
            }

            return parentRoslynPathMatchNode;
        }
    }
}
