using System.Text.RegularExpressions;

namespace RoslynPath
{
    internal class RPCloseBracketTokenType : IRPTokenType
    {
        public Regex Regex => new Regex(@"\]");

        public RPTokenPrecedence Precedence => RPTokenPrecedence.VeryLow;
    }
}
