using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace RoslynPath
{
    class RPEvaluator
    {
        private IRPResultNodeBuilder _builder;

        public RPEvaluator(IRPResultNodeBuilder builder)
        {
            _builder = builder;
        }

        public IEnumerable<SyntaxNode> Evaluate(SyntaxNode syntaxNode, IEnumerable<IRPElement> roslynPath)
        {
            IRPResultNode resultNode = _builder.EvaluateElement(new RPResultNode(null, syntaxNode), roslynPath);

            RPResult result = new RPResult(resultNode);

            return result.Leaves().Select(rn => rn.SyntaxNode);
        }
    }
}
