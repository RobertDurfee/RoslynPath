using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoslynPath
{
    [RPElementBuilder(typeof(RPSyntaxKindTokenType))]
    internal class RPSyntaxKindElementBuilder : RPElementBuilder
    {
        public RPSyntaxKindElementBuilder(Dictionary<string, string> options, IRPElement element) : base(options, element) { }

        public override int ConvertTokens(IEnumerable<RPToken> tokens)
        {
            string syntaxKindString = tokens.First().Value;
            syntaxKindString = syntaxKindString.Substring(1, syntaxKindString.Length - 2);

            if (!Enum.TryParse(syntaxKindString, true, out SyntaxKind syntaxKind))
                throw new FormatException($"{syntaxKindString} is not a valid SyntaxKind.");

            if (!Options.ContainsKey(typeof(RPScanTypes).Name))
                throw new Exception("ScanTypes not defined.");

            if (!Enum.TryParse(Options[typeof(RPScanTypes).Name], true, out RPScanTypes scanType))
                throw new Exception("Invalid ScanType");

            Element = new RPSyntaxKindElement(syntaxKind, scanType);

            return base.ConvertTokens(tokens.Skip(1)) + 1;
        }
    }
}
