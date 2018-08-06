﻿using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace RoslynPath
{
    class RPResult
    {
        private readonly IRPResultNode _root;

        public RPResult(IRPResultNode root)
        {
            _root = root;
        }

        private List<IRPResultNode> _leaves;

        public List<IRPResultNode> Leaves()
        {
            _leaves = new List<IRPResultNode>();

            PopulateLeavesRecursive(_root);

            return _leaves;
        }

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
