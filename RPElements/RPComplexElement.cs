using System.Collections.Generic;

namespace RoslynPath
{
    internal class RPComplexElement : IRPElement
    {
        public RPScanTypes ScanType { get; }
        public IEnumerable<RPToken> Expression { get; }

        public RPComplexElement(IEnumerable<RPToken> expression, RPScanTypes scanType)
        {
            ScanType = scanType;
            Expression = expression;
        }
    }
}
