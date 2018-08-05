using System;
using System.Collections.Generic;
using System.Linq;

namespace RoslynPath
{
    [RPElementBuilder(typeof(RPIntegerTokenType))]
    class RPIndexElementBuilder : RPElementBuilder
    {
        public RPIndexElementBuilder(Dictionary<string, string> options, IRPElement element) : base(options, element) { }

        public override int ConvertTokens(IEnumerable<RPToken> tokens)
        {
            int index = Int32.Parse(tokens.First().Value); // This parse should be safe...

            if (!Options.ContainsKey(typeof(RPScanTypes).Name))
                throw new Exception("ScanTypes not defined.");

            if (!Enum.TryParse(Options[typeof(RPScanTypes).Name], true, out RPScanTypes scanType))
                throw new Exception("Invalid ScanType");

            Element = new RPIndexElement(index, scanType);

            return base.ConvertTokens(tokens.Skip(1)) + 1;
        }
    }
}
