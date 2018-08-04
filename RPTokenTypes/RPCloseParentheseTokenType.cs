using System.Text.RegularExpressions;

namespace RoslynPath
{
    class RPCloseParentheseTokenType : IRPTokenType
    {
        public Regex Regex => new Regex(@"\)");

        public RPTokenPrecedence Precedence => RPTokenPrecedence.VeryLow;
    }
}
