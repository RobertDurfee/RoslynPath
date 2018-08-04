using System.Text.RegularExpressions;

namespace RoslynPath
{
    class RPCloseBracketTokenType : IRPTokenType
    {
        public Regex Regex => new Regex(@"\]");

        public RPTokenPrecedence Precedence => RPTokenPrecedence.VeryLow;
    }
}
