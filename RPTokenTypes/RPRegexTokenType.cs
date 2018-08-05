using System.Text.RegularExpressions;

namespace RoslynPath
{
    class RPRegexTokenType : IRPTokenType
    {
        public Regex Regex => new Regex(@"\/.*?(?<!\\)\/[imsnxr]*");

        public RPTokenPrecedence Precedence => RPTokenPrecedence.High;
    }
}
