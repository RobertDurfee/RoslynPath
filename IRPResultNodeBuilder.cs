using System.Collections.Generic;

namespace RoslynPath
{
    internal interface IRPResultNodeBuilder
    {
        IRPResultNode EvaluateElement(IRPResultNode resultNode, IEnumerable<IRPElement> element);
    }
}
