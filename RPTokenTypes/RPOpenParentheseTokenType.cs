using System.Text.RegularExpressions;

namespace RoslynPath
{
    class RPOpenParentheseTokenType : IRPTokenType
    {
        public Regex Regex => new Regex(@"\(");

        public RPTokenPrecedence Precedence => RPTokenPrecedence.VeryLow;
    }
}
