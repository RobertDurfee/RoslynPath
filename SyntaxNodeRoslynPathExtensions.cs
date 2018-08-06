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

            RPElementBuilder elementBuilder = new RPElementBuilder();
            RPTokenListReader tokenListReader = new RPTokenListReader(elementBuilder);
            IEnumerable<IRPElement> roslynPath = tokenListReader.ConvertTokens(tokens);

            RPResultNodeBuilder resultNodeBuilder = new RPResultNodeBuilder();
            RPEvaluator evaluator = new RPEvaluator(resultNodeBuilder);
            return evaluator.Evaluate(syntaxNode, roslynPath);
        }
    }
}
