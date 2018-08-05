using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using RoslynPath.Extensions;

namespace RoslynPath
{
    class RPRegexElement : IRPElement
    {
        private RPScanTypes _scanType;
        private Regex _regex;

        public RPRegexElement(Regex regex, RPScanTypes scanType)
        {
            _regex = regex;
            _scanType = scanType;
        }

        public IEnumerable<SyntaxNode> Matches(SyntaxNode input)
        {
            string parentText = input.ToString();

            IEnumerable<TextSpan> matchingTextSpans = _regex.Matches(parentText)
                .Cast<Match>()
                .Select(m => new TextSpan(m.Index + input.SpanStart, m.Length));

            if (matchingTextSpans.Count() == 0)
                yield return null; // This may need to change...

            foreach (TextSpan matchingTextSpan in matchingTextSpans)
            {
                IEnumerable<SyntaxNode> searchPool;

                switch (_scanType)
                {
                    case RPScanTypes.Children:
                        searchPool = input.ChildNodes();
                        break;
                    case RPScanTypes.Descendants:
                        searchPool = input.DescendantNodes();
                        break;
                    default:
                        throw new ArgumentException("Invalid scan type.");
                }

                yield return searchPool.Where(sn => sn.Span.OverlapsWith(matchingTextSpan))
                    .OrderBy(sn => Math.Abs((int)sn.Span.Overlap(matchingTextSpan)?.Length - (int)sn.Span.Union(matchingTextSpan)?.Length))
                    .First();
            }
        }
    }
}
