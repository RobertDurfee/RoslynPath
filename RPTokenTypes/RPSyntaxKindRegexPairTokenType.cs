using System.Text.RegularExpressions;

namespace RoslynPath
{
    internal class RPSyntaxKindRegexPairTokenType : IRPTokenType
    {
        public Regex Regex => new Regex(@":");

        public RPTokenPrecedence Precedence => RPTokenPrecedence.VeryLow;
    }
}
