using System.Text.RegularExpressions;

namespace RoslynPath
{
    internal interface IRPTokenType
    {
        Regex Regex { get; }
        RPTokenPrecedence Precedence { get; }
    }
}
