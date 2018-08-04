using System.Text.RegularExpressions;

namespace RoslynPath
{
    class RPDeepTokenType : IRPTokenType
    {
        public Regex Regex => new Regex(@"\.\.");

        public RPTokenPrecedence Precedence => RPTokenPrecedence.Low;
    }
}
