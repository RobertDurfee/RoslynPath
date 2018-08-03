namespace RoslynPath
{
    internal class RoslynPathNode
    {
        public RoslynPathNode(RoslynPathStep step, RoslynPathNode next)
        {
            Step = step;
            Next = next;
        }
        
        public RoslynPathStep Step { get; }

        public RoslynPathNode Next { get; }
    }
}
