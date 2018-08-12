using System.Text.RegularExpressions;

namespace RoslynPath
{
    internal class RPWildcardTokenType : IRPTokenType
    {
        public Regex Regex => new Regex(@"\*");

        public RPTokenPrecedence Precedence => RPTokenPrecedence.VeryLow;
    }
}
