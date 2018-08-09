using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RoslynPath
{
    [RPResultNodeBuilder(typeof(RPSyntaxKindRegexPairElement))]
    internal class RPSyntaxKindRegexPairResultNodeBuilder : RPResultNodeBuilder
    {
        private static readonly RPTextSpanComparer _rpTextSpanComparer = new RPTextSpanComparer();

        public override IRPResultNode EvaluateElement(IRPResultNode resultNode, IEnumerable<IRPElement> elements)
        {
            if (!(elements.First() is RPSyntaxKindRegexPairElement syntaxKindRegexPairElement))
                throw new ArgumentException($"{elements.First()} is not an RPSyntaxKindRegexPairElement.");

            string parentText = resultNode.SyntaxNode.ToString();

            IEnumerable<TextSpan> matchingTextSpans = syntaxKindRegexPairElement.Regex.Matches(parentText)
                .Cast<Match>()
                .Select(m => new TextSpan(m.Index + resultNode.SyntaxNode.SpanStart, m.Length));

            if (matchingTextSpans.Count() == 0)
                return null;

            bool anyMatches = false;

            foreach (TextSpan matchingTextSpan in matchingTextSpans)
            {
                IEnumerable<SyntaxNode> searchPool;

                switch (syntaxKindRegexPairElement.ScanType)
                {
                    case RPScanTypes.Children:
                        searchPool = resultNode.SyntaxNode.ChildNodes();
                        break;
                    case RPScanTypes.Descendants:
                        searchPool = resultNode.SyntaxNode.DescendantNodes();
                        break;
                    default:
                        throw new ArgumentException("Invalid scan type.");
                }

                searchPool = searchPool.Where(sn => sn.IsKind(syntaxKindRegexPairElement.SyntaxKind));

                SyntaxNode matchingNode = searchPool.Where(sn => sn.Span.OverlapsWith(matchingTextSpan))
                    .OrderBy(sn => (sn.Span, matchingTextSpan), _rpTextSpanComparer)
                    .FirstOrDefault();

                if (matchingNode == null)
                    continue;

                anyMatches = true;
                resultNode.Children.Add(base.EvaluateElement(new RPResultNode(resultNode, matchingNode), elements.Skip(1)));
            }

            return anyMatches ? resultNode : null;
        }
    }
}
