using System.Text.RegularExpressions;

namespace RoslynPath
{
    internal class RPRegexTokenType : IRPTokenType
    {
        public Regex Regex => new Regex(@"\/.*?(?<!\\)\/[imsnxr]*");

        public RPTokenPrecedence Precedence => RPTokenPrecedence.High;
    }
}
