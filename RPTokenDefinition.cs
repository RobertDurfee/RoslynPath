using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RoslynPath
{
    internal class RPTokenDefinition
    {
        private Type _tokenType;
        private Regex _regex;

        public RPTokenDefinition(Type tokenType)
        {
            if (tokenType.IsInterface || tokenType.IsAbstract || !typeof(IRPTokenType).IsAssignableFrom(tokenType))
                throw new ArgumentException($"{tokenType} does not implement IRPTokenType.");

            _tokenType = tokenType;
            _regex = ((IRPTokenType)Activator.CreateInstance(tokenType)).Regex;
        }

        public IEnumerable<RPTokenMatch> Matches(string input)
        {
            MatchCollection matches = _regex.Matches(input);

            return matches.Cast<Match>().Select(m => new RPTokenMatch(_tokenType)
            {
                StartIndex = m.Index,
                EndIndex = m.Index + m.Length,
                Value = m.Value
            });
        }
    }
}
