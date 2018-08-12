using System.Text.RegularExpressions;

namespace RoslynPath
{
    internal class RPAndTokenType : IRPTokenType
    {
        public Regex Regex => new Regex(@"&&");

        public RPTokenPrecedence Precedence => RPTokenPrecedence.Low;
    }
}
