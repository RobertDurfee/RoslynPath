using Microsoft.CodeAnalysis.CSharp;

namespace RoslynPath
{
    class RPSyntaxKindElement : IRPElement
    {
        public SyntaxKind SyntaxKind { get; }
        public RPScanTypes ScanType { get; }

        public RPSyntaxKindElement(SyntaxKind syntaxKind, RPScanTypes scanType)
        {
            SyntaxKind = syntaxKind;
            ScanType = scanType;
        }
    }
}
