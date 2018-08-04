using System.Text.RegularExpressions;

namespace RoslynPath
{
    class RPLocalRootTokenType : IRPTokenType
    {
        public Regex Regex => new Regex(@"\@");

        public RPTokenPrecedence Precedence => RPTokenPrecedence.VeryLow;
    }
}
