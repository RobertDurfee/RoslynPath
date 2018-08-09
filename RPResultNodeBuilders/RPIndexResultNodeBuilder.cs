using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoslynPath
{
    [RPResultNodeBuilder(typeof(RPIndexElement))]
    internal class RPIndexResultNodeBuilder : RPResultNodeBuilder
    {
        public override IRPResultNode EvaluateElement(IRPResultNode resultNode, IEnumerable<IRPElement> elements)
        {
            if (!(elements.First() is RPIndexElement indexElement))
                throw new ArgumentException($"{elements.First()} is not an RPIndexElement.");

            IEnumerable<SyntaxNode> searchPool;

            switch (indexElement.ScanType)
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

            SyntaxNode matchingNode = searchPool.ElementAtOrDefault(indexElement.Index);

            if (matchingNode == null)
                return null;

            resultNode.Children.Add(base.EvaluateElement(new RPResultNode(resultNode, matchingNode), elements.Skip(1)));

            return resultNode;
        }
    }
}
