using System.Text.RegularExpressions;

namespace RoslynPath
{
    class RPIndexTokenType : IRPTokenType
    {
        public Regex Regex => new Regex(@"\[\d+?\]");

        public int Precedence => 1;
    }
}
