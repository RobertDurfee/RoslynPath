using System.Collections.Generic;
using System.Linq;

namespace RoslynPath
{
    [RPResultNodeBuilder(typeof(RPGlobalRootElement))]
    internal class RPGlobalRootResultNodeBuilder : RPResultNodeBuilder
    {
        public override IRPResultNode EvaluateElement(IRPResultNode resultNode, IEnumerable<IRPElement> elements)
        {
            return base.EvaluateElement(resultNode, elements.Skip(1));
        }
    }
}
