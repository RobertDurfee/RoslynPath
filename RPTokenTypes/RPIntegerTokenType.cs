using System.Text.RegularExpressions;

namespace RoslynPath
{
    class RPIntegerTokenType : IRPTokenType
    {
        public Regex Regex => new Regex(@"\d+");

        public RPTokenPrecedence Precedence => RPTokenPrecedence.Moderate;
    }
}
