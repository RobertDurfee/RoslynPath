using System.Text.RegularExpressions;

namespace RoslynPath
{
    internal class RPLocalRootTokenType : IRPTokenType
    {
        public Regex Regex => new Regex(@"\@");

        public RPTokenPrecedence Precedence => RPTokenPrecedence.VeryLow;
    }
}
