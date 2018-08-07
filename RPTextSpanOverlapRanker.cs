using Microsoft.CodeAnalysis.Text;
using RoslynPath.Extensions;
using System;
using System.Linq;

namespace RoslynPath
{
    static class RPTextSpanOverlapRanker
    {
        // This assumes there is an overlap to begin with (use TextSpan.OverlapsWith() to check).
        public static int Rank(TextSpan syntaxNodeSpan, TextSpan regexMatchSpan)
        {
            return Math.Abs((int)syntaxNodeSpan.Overlap(regexMatchSpan)?.Length - (int)syntaxNodeSpan.Union(regexMatchSpan)?.Length);
        }
    }
}
