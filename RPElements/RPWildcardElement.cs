namespace RoslynPath
{
    internal class RPWildcardElement : IRPElement
    {
        public RPScanTypes ScanType { get; }

        public RPWildcardElement(RPScanTypes scanType)
        {
            ScanType = scanType;
        }
    }
}
