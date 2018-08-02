using System.Text.RegularExpressions;

namespace RoslynPath
{
    internal class RoslynPathNode
    {
        public RoslynPathNode(Regex pattern, RoslynPathNode next)
        {
            Pattern = pattern;
            Next = next;
        }

        public Regex Pattern { get; }

        public RoslynPathNode Next { get; }
    }
}
