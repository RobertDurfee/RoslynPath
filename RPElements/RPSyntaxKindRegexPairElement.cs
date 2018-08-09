using Microsoft.CodeAnalysis.CSharp;
using System.Text.RegularExpressions;

namespace RoslynPath
{
    internal class RPSyntaxKindRegexPairElement : IRPElement
    {
        public SyntaxKind SyntaxKind { get; }
        public Regex Regex { get; }
        public RPScanTypes ScanType { get; }

        public RPSyntaxKindRegexPairElement(SyntaxKind syntaxKind, Regex regex, RPScanTypes scanType)
        {
            SyntaxKind = syntaxKind;
            Regex = regex;
            ScanType = scanType;
        }
    }
}
