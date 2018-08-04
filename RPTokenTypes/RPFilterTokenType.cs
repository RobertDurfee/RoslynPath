using System.Text.RegularExpressions;

namespace RoslynPath
{
    class RPFilterTokenType : IRPTokenType
    {
        public Regex Regex => new Regex(@"\[\?\(.*?\)\]");

        public int Precedence => 1;
    }
}
