using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoslynPath
{
    [RPResultNodeBuilder(typeof(RPWildcardElement))]
    internal class RPWildcardResultNodeBuilder : RPResultNodeBuilder
    {
        public override IRPResultNode EvaluateElement(IRPResultNode resultNode, IEnumerable<IRPElement> elements)
        {
            if (!(elements.First() is RPWildcardElement wildcardElement))
                throw new ArgumentException($"{elements.First()} is not an RPWildcardElement.");

            IEnumerable<SyntaxNode> searchPool;

            switch (wildcardElement.ScanType)
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

            if (searchPool.Count() == 0)
                return null;

            foreach (SyntaxNode matchingNode in searchPool)
                resultNode.Children.Add(base.EvaluateElement(new RPResultNode(resultNode, matchingNode), elements.Skip(1)));

            return resultNode;
        }
    }
}
