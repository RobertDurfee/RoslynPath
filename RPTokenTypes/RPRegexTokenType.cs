using System.Text.RegularExpressions;

namespace RoslynPath
{
    class RPRegexTokenType : IRPTokenType
    {
        public Regex Regex => new Regex(@"\[\/.*?\/.*?\]");

        public int Precedence => 2;
    }
}
