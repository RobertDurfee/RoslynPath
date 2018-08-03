using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using RoslynPath.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
            RoslynPathMatchNode rawMatch = MatchesRecursive(rootMatchNode, _text, roslynPath.Root.Next);

            return new RoslynPathMatch(rawMatch).RemoveNullBranches();
        }

        private RoslynPathMatchNode MatchesRecursive(RoslynPathMatchNode parentRoslynPathMatchNode, string parentText, RoslynPathNode roslynPathNode)
        {
            // The end of the path sequence has been reached
            if (roslynPathNode == null)
                return parentRoslynPathMatchNode;

            switch (roslynPathNode.Step.StepType)
            {
                case RoslynPathStep.StepTypes.Regex:
                    return MatchesRecursive_Regex(parentRoslynPathMatchNode, parentText, roslynPathNode);
                case RoslynPathStep.StepTypes.Index:
                    return MatchesRecursive_Index(parentRoslynPathMatchNode, parentText, roslynPathNode);
                default:
                    throw new Exception("Invalid step type.");
            }
        }
        
        private RoslynPathMatchNode MatchesRecursive_Regex(RoslynPathMatchNode parentRoslynPathMatchNode, string parentText, RoslynPathNode roslynPathNode)
        {
            IEnumerable<TextSpan> matchingTextSpans = roslynPathNode.Step.Pattern.Matches(parentText)
                                                                                 .Cast<Match>()
                                                                                 // Offset the matching index to correspond to the parent SyntaxNode's TextSpan
                                                                                 .Select(m => new TextSpan(m.Index + parentRoslynPathMatchNode.SyntaxNode.SpanStart, m.Length));

            // No matches under this parent match tree node (THUS, NOT A FULL MATCH! This branch of the tree will be removed outside of recursion, see above.)
            if (matchingTextSpans.Count() == 0)
                return null;

            foreach (TextSpan matchingTextSpan in matchingTextSpans)
            {
                IEnumerable<SyntaxNode> searchPool;
                
                switch (roslynPathNode.Step.ScanType)
                {
                    case RoslynPathStep.ScanTypes.Children:
                        searchPool = parentRoslynPathMatchNode.SyntaxNode.ChildNodes();
                        break;
                    case RoslynPathStep.ScanTypes.Descendants:
                        searchPool = parentRoslynPathMatchNode.SyntaxNode.DescendantNodes();
                        break;
                    default:
                        throw new Exception("Invalid scan type.");
                }

                // Get the closest matching node to the current TextSpan
                SyntaxNode matchingNode = searchPool.Where(dn => dn.Span.OverlapsWith(matchingTextSpan))
                                                    // This function may be tuned in the future...
                                                    .OrderBy(dn => Math.Abs((int)dn.Span.Overlap(matchingTextSpan)?.Length - (int)dn.Span.Union(matchingTextSpan)?.Length))
                                                    .First();

                // Restrict the child search text
                string childText = parentText.Substring(matchingNode.Span.Start - parentRoslynPathMatchNode.SyntaxNode.SpanStart, matchingNode.Span.Length);

                RoslynPathMatchNode childRoslynPathMatchNode = new RoslynPathMatchNode(parentRoslynPathMatchNode, matchingNode);
                childRoslynPathMatchNode = MatchesRecursive(childRoslynPathMatchNode, childText, roslynPathNode.Next);

                // Add the matching child (and subsequent children)
                parentRoslynPathMatchNode.Children.Add(childRoslynPathMatchNode);
            }

            return parentRoslynPathMatchNode;
        }

        private RoslynPathMatchNode MatchesRecursive_Index(RoslynPathMatchNode parentRoslynPathMatchNode, string parentText, RoslynPathNode roslynPathNode)
        {
            IEnumerable<SyntaxNode> searchPool;

            switch (roslynPathNode.Step.ScanType)
            {
                case RoslynPathStep.ScanTypes.Children:
                    searchPool = parentRoslynPathMatchNode.SyntaxNode.ChildNodes();
                    break;
                case RoslynPathStep.ScanTypes.Descendants:
                    searchPool = parentRoslynPathMatchNode.SyntaxNode.DescendantNodes();
                    break;
                default:
                    throw new Exception("Invalid scan type.");
            }

            SyntaxNode matchingNode = searchPool.ElementAtOrDefault(roslynPathNode.Step.Index ?? -1);

            // Index out of range (THUS, NOT A FULL MATCH! This branch of the tree will be removed outside of recursion, see above.)
            if (matchingNode == null)
                return null;

            // Restrict the child search text
            string childText = parentText.Substring(matchingNode.Span.Start - parentRoslynPathMatchNode.SyntaxNode.SpanStart, matchingNode.Span.Length);

            RoslynPathMatchNode childRoslynPathMatchNode = new RoslynPathMatchNode(parentRoslynPathMatchNode, matchingNode);
            childRoslynPathMatchNode = MatchesRecursive(childRoslynPathMatchNode, childText, roslynPathNode.Next);

            // Add the matching child (and subsequent children)
            parentRoslynPathMatchNode.Children.Add(childRoslynPathMatchNode);

            return parentRoslynPathMatchNode;
        }
    }
}
