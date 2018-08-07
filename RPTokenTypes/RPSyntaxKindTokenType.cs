using System.Text.RegularExpressions;

namespace RoslynPath
{
    class RPSyntaxKindTokenType : IRPTokenType
    {
        public Regex Regex => new Regex(@"\""\w+\""");

        public RPTokenPrecedence Precedence => RPTokenPrecedence.ModeratelyHigh;
    }
}
