using System.Text.RegularExpressions;

namespace RoslynPath
{
    internal class RPOrTokenType : IRPTokenType
    {
        public Regex Regex => new Regex(@"\|\|");

        public RPTokenPrecedence Precedence => RPTokenPrecedence.Low;
    }
}
