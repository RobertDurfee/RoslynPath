using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RoslynPath
{
    internal class RoslynPath
    {
        public RoslynPath(IEnumerable<Regex> regexPath)
        {
            if (regexPath.Any())
            {
                RoslynPathNode currentMatchPathStep = new RoslynPathNode(regexPath.Last(), null);
                
                foreach (Regex regexStep in regexPath.Reverse().Skip(1))
                    currentMatchPathStep = new RoslynPathNode(regexStep, currentMatchPathStep);

                Root = currentMatchPathStep;
            }
        }

        public RoslynPathNode Root { get; }
    }
}
