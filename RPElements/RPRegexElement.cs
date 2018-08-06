﻿using System.Text.RegularExpressions;

namespace RoslynPath
{
    class RPRegexElement : IRPElement
    {
        public RPScanTypes ScanType { get; }
        public Regex Regex { get; }

        public RPRegexElement(Regex regex, RPScanTypes scanType)
        {
            Regex = regex;
            ScanType = scanType;
        }
    }
}
