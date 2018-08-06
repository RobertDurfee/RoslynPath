namespace RoslynPath
{
    class RPIndexElement : IRPElement
    {
        public int Index { get; }
        public RPScanTypes ScanType { get; }

        public RPIndexElement(int index, RPScanTypes scanType)
        {
            Index = index;
            ScanType = scanType;
        }
    }
}
