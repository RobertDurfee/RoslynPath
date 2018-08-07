using System.Text.RegularExpressions;

namespace RoslynPath
{
    class RPSyntaxKindRegexPairTokenType : IRPTokenType
    {
        public Regex Regex => new Regex(@":");

        public RPTokenPrecedence Precedence => RPTokenPrecedence.VeryLow;
    }
}
