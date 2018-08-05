using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace RoslynPath
{
    public static class SyntaxNodeRoslynPathExtensions
    {
        public static SyntaxNode SelectNode(this SyntaxNode syntaxNode, string path) => SelectNodes(syntaxNode, path).First();

        public static IEnumerable<SyntaxNode> SelectNodes(this SyntaxNode syntaxNode, string path)
        {
            IEnumerable<RPToken> tokens = new RPTokenizer().Tokenize(path);

            RPBuilder builder = new RPBuilder();
            RPTokenListReader tokenListReader = new RPTokenListReader(builder);
            tokenListReader.ReadTokenList(tokens);
            List<IRPElement> roslynPath = builder.RoslynPath;
            
            RPResultBuilder resultBuilder = new RPResultBuilder();
            RPEvaluater evaluater = new RPEvaluater(resultBuilder);
            evaluater.EvaluateRoslynPath(roslynPath, syntaxNode);
            RPResult result = resultBuilder.Result;

            return result.Leaves().Select(rpr => rpr.SyntaxNode);
        }
    }
}
