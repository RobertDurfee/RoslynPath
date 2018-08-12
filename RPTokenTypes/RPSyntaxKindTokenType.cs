using System.Text.RegularExpressions;

namespace RoslynPath
{
    internal class RPSyntaxKindTokenType : IRPTokenType
    {
        public Regex Regex => new Regex(@"\""\w+\""");

        public RPTokenPrecedence Precedence => RPTokenPrecedence.ModeratelyHigh;
    }
}
