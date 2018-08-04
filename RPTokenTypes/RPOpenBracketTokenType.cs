using System.Text.RegularExpressions;

namespace RoslynPath
{
    class RPOpenBracketTokenType : IRPTokenType
    {
        public Regex Regex => new Regex(@"\[");

        public RPTokenPrecedence Precedence => RPTokenPrecedence.VeryLow;
    }
}
