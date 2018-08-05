using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace RoslynPath
{
    class RPIndexElement : IRPElement
    {
        private int _index;
        private RPScanTypes _scanType;

        public RPIndexElement(int index, RPScanTypes scanType)
        {
            _index = index;
            _scanType = scanType;
        }

        public IEnumerable<SyntaxNode> Matches(SyntaxNode input)
        {
            IEnumerable<SyntaxNode> searchPool;

            switch (_scanType)
            {
                case RPScanTypes.Children:
                    searchPool = input.ChildNodes();
                    break;
                case RPScanTypes.Descendants:
                    searchPool = input.DescendantNodes();
                    break;
                default:
                    throw new ArgumentException("Invalid scan type.");
            }

            SyntaxNode matchingNode = searchPool.ElementAtOrDefault(_index);
            
            yield return matchingNode;
        }
    }
}
