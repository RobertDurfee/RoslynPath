using System.Collections.Generic;
using System.Linq;

namespace RoslynPath
{
    internal class RoslynPath
    {
        public RoslynPath(IEnumerable<RoslynPathStep> path)
        {
            if (path.Any())
            {
                RoslynPathNode currentMatchPathStep = new RoslynPathNode(path.Last(), null);
                
                foreach (RoslynPathStep step in path.Reverse().Skip(1))
                    currentMatchPathStep = new RoslynPathNode(step, currentMatchPathStep);

                Root = currentMatchPathStep;
            }
        }

        public RoslynPathNode Root { get; }
    }
}
