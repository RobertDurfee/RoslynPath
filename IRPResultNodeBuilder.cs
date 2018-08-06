using System.Collections.Generic;

namespace RoslynPath
{
    interface IRPResultNodeBuilder
    {
        IRPResultNode EvaluateElement(IRPResultNode resultNode, IEnumerable<IRPElement> element);
    }
}
