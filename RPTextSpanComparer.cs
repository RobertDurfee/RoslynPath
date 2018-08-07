using Microsoft.CodeAnalysis.Text;
using RoslynPath.Extensions;
using System;
using System.Collections.Generic;

namespace RoslynPath
{
    class RPTextSpanComparer : IComparer<(TextSpan, TextSpan)>
    {
        public int Compare((TextSpan, TextSpan) x, (TextSpan, TextSpan) y)
        {
            int xOverlap = (int)x.Item1.Overlap(x.Item2)?.Length;
            int xUnion = (int)x.Item1.Union(x.Item2)?.Length;

            int xOverlapUnionDifference = Math.Abs(xOverlap - xUnion);

            int yOverlap = (int)y.Item1.Overlap(y.Item2)?.Length;
            int yUnion = (int)y.Item1.Union(y.Item2)?.Length;

            int yOverlapUnionDifference = Math.Abs(yOverlap - yUnion);

            if (xOverlapUnionDifference > yOverlapUnionDifference)
                return -1;
            else if (xOverlapUnionDifference < yOverlapUnionDifference)
                return 1;
            else
                return 0;
        }
    }
}
