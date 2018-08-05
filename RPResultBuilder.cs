using System.Linq;
using Microsoft.CodeAnalysis;

namespace RoslynPath
{
    class RPResultBuilder : IRPResultBuilder
    {
        public RPResult Result { get; private set; }

        public SyntaxNode Root { private get; set; }

        public void EvaluateElement(IRPElement element)
        {
            if (Result == null)
                Result = new RPResult(new RPResultNode(null, element.Matches(Root).First()));
            else
            {
                foreach (IRPResultNode resultNode in Result.Leaves())
                {
                    resultNode.Children.AddRange(element.Matches(resultNode.SyntaxNode).Select(m => new RPResultNode(resultNode, m)));
                }
            }
        }
    }
}
