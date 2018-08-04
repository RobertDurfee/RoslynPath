using System.Text.RegularExpressions;

namespace RoslynPath
{
    interface IRPTokenType
    {
        Regex Regex { get; }
        int Precedence { get; }
    }
}
