using System;
using System.Collections.Generic;
using System.Linq;

namespace RoslynPath
{
    // This is an altered version of https://jack-vanlightly.com/blog/2016/2/24/a-more-efficient-regex-tokenizer 
    // which uses reflection instead of enumerations for added flexibility.
    class RPTokenizer
    {
        private static IEnumerable<RPTokenDefinition> _tokenDefinitions;

        static RPTokenizer()
        {
            _tokenDefinitions = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
                .Where(t => typeof(IRPTokenType).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .Select(t => new RPTokenDefinition(t));
        }

        public IEnumerable<RPToken> Tokenize(string roslynPath)
        {
            var matches = _tokenDefinitions.SelectMany(td => td.Matches(roslynPath));

            var groupedMatches = matches.GroupBy(m => m.StartIndex).OrderBy(mg => mg.Key);

            RPTokenMatch lastMatch = null;
            foreach (var matchGroup in groupedMatches)
            {
                RPTokenMatch bestMatch = matchGroup.OrderBy(m => (int)m.Precedence).First();

                if (lastMatch != null && bestMatch.StartIndex < lastMatch.EndIndex)
                    continue;

                yield return new RPToken(bestMatch.TokenType, bestMatch.Value);

                lastMatch = bestMatch;
            }
        }
    }
}
