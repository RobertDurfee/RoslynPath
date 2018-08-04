using System.Text.RegularExpressions;

namespace RoslynPath
{
    class RPRootTokenType : IRPTokenType
    {
        public Regex Regex => new Regex(@"(\$|\@)");

        public int Precedence => int.MaxValue;
    }
}
