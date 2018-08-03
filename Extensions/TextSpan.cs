using Microsoft.CodeAnalysis.Text;
using System;

namespace RoslynPath.Extensions
{
    public static class TextSpanExtensions
    {
        public static TextSpan? Union(this TextSpan source, TextSpan span)
        {
            if (source.OverlapsWith(span))
            {
                int start = Math.Min(source.Start, span.Start);
                int end = Math.Max(source.End, span.End);

                int length = end - start + 1;

                return new TextSpan(start, length);
            }
            else
                return null;
        }
    }
}
