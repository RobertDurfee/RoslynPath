using Microsoft.CodeAnalysis;

namespace RoslynPath
{
    interface IRPResultBuilder
    {
        SyntaxNode Root { set; }

        void EvaluateElement(IRPElement element);
    }
}
