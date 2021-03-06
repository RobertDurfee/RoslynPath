﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace RoslynPath
{
    internal class RPTokenListReader
    {
        private IRPElementBuilder _elementBuilder;

        public RPTokenListReader(IRPElementBuilder elementBuilder)
        {
            _elementBuilder = elementBuilder;
        }

        public IEnumerable<IRPElement> ConvertTokens(IEnumerable<RPToken> tokens)
        {
            List<IRPElement> roslynPath = new List<IRPElement>();
            List<RPToken> tokenList = tokens.ToList();

            for (int index = 0; index < tokenList.Count; index++)
            {
                _elementBuilder.Clean();

                int tokensConsumed = _elementBuilder.ConvertTokens(tokenList.Skip(index));

                if (_elementBuilder.Element == null || tokensConsumed == 0)
                    throw new Exception($"Unable to convert token {tokenList[index]}");

                roslynPath.Add(_elementBuilder.Element);

                index += tokensConsumed - 1;
            }

            return roslynPath;
        }
    }
}
