﻿using System.Text.RegularExpressions;

namespace RoslynPath
{
    class RPFilterTokenType : IRPTokenType
    {
        public Regex Regex => new Regex(@"\?");

        public RPTokenPrecedence Precedence => RPTokenPrecedence.VeryLow;
    }
}
