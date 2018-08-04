using System.Text.RegularExpressions;

namespace RoslynPath
{
    class RPGlobalRootTokenType : IRPTokenType
    {
        public Regex Regex => new Regex(@"\$");

        public RPTokenPrecedence Precedence => RPTokenPrecedence.VeryLow;
    }
}
