using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RoslynPath
{
    [RPResultNodeBuilder(typeof(RPRegexElement))]
    class RPRegexResultNodeBuilder : RPResultNodeBuilder
    {
        public override IRPResultNode EvaluateElement(IRPResultNode resultNode, IEnumerable<IRPElement> elements)
        {
            if (!(elements.First() is RPRegexElement regexElement))
                throw new ArgumentException($"{elements.First()} is not an RPRegexElement.");

            string parentText = resultNode.SyntaxNode.ToString();

            IEnumerable<TextSpan> matchingTextSpans = regexElement.Regex.Matches(parentText)
                .Cast<Match>()
                .Select(m => new TextSpan(m.Index + resultNode.SyntaxNode.SpanStart, m.Length));

            if (matchingTextSpans.Count() == 0)
                return null;

            foreach (TextSpan matchingTextSpan in matchingTextSpans)
            {
                IEnumerable<SyntaxNode> searchPool;

                switch (regexElement.ScanType)
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

                SyntaxNode matchingNode = searchPool.Where(sn => sn.Span.OverlapsWith(matchingTextSpan))
                    .OrderBy(sn => RPTextSpanOverlapRanker.Rank(sn.Span, matchingTextSpan))
                    // There has to be at least one match given that there is a TextSpan
                    .First();

                resultNode.Children.Add(base.EvaluateElement(new RPResultNode(resultNode, matchingNode), elements.Skip(1)));
            }

            return resultNode;
        }
    }
}
