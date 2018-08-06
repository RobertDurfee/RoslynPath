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

            PopulateLeavesRecursive(resultNode);

            return _leaves.Select(rn => rn.SyntaxNode);
        }

        private List<IRPResultNode> _leaves;

        private void PopulateLeavesRecursive(IRPResultNode resultNode)
        {
            if (resultNode == null || resultNode.SyntaxNode == null)
                return;

            if (!resultNode.Children.Any())
                _leaves.Add(resultNode);
            else
            {
                foreach (IRPResultNode childNode in resultNode.Children)
                    PopulateLeavesRecursive(childNode);
            }
        }
    }
}
