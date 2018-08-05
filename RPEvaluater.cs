using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace RoslynPath
{
    class RPEvaluater
    {
        private IRPResultBuilder _builder;

        public RPEvaluater(IRPResultBuilder builder)
        {
            _builder = builder;
        }

        public void EvaluateRoslynPath(IEnumerable<IRPElement> roslynPath, SyntaxNode root)
        {
            _builder.Root = root;
            
            foreach (IRPElement element in roslynPath)
                _builder.EvaluateElement(element);
        }
    }
}
