﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace RoslynPath
{
    class RPTokenizer
    {
        private IEnumerable<RPTokenDefinition> _tokenDefinitions;

        public RPTokenizer()
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
                RPTokenMatch bestMatch = matchGroup.OrderBy(m => m.Precedence).First();

                if (lastMatch != null && bestMatch.StartIndex < lastMatch.EndIndex)
                    continue;

                yield return new RPToken(bestMatch.TokenType, bestMatch.Value);
            }
        }
    }
}
