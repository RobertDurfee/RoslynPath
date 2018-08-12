using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoslynPath
{
    [RPResultNodeBuilder(typeof(RPSyntaxKindElement))]
    internal class RPSyntaxKindResultNodeBuilder : RPResultNodeBuilder
    {
        public override IRPResultNode EvaluateElement(IRPResultNode resultNode, IEnumerable<IRPElement> elements)
        {
            if (!(elements.First() is RPSyntaxKindElement syntaxKindElement))
                throw new ArgumentException($"{elements.First()} is not an RPSyntaxKindElement.");

            IEnumerable<SyntaxNode> searchPool;

            switch (syntaxKindElement.ScanType)
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

            IEnumerable<SyntaxNode> matchingNodes = searchPool.Where(sn => sn.IsKind(syntaxKindElement.SyntaxKind));

            foreach (SyntaxNode matchingNode in matchingNodes)
                resultNode.Children.Add(base.EvaluateElement(new RPResultNode(resultNode, matchingNode), elements.Skip(1)));

            return resultNode;
        }
    }
}
