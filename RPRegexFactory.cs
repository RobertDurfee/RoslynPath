using System;
using System.Text.RegularExpressions;

namespace RoslynPath
{
    internal static class RPRegexFactory
    {
        public static Regex Create(string input)
        {
            int firstSlash = input.IndexOf('/');
            int lastSlash = input.LastIndexOf('/');

            string regexOptionsString = string.Empty;
            if (lastSlash < input.Length - 1)
                regexOptionsString = input.Substring(lastSlash + 1);

            string pattern = input.Substring(firstSlash + 1, lastSlash - firstSlash - 1);
            RegexOptions regexOptions = ParseRegexOptions(regexOptionsString);

            return new Regex(pattern, regexOptions);
        }

        private static RegexOptions ParseRegexOptions(string regexOptionsString)
        {
            RegexOptions regexOptions = RegexOptions.None;

            foreach (char regexOption in regexOptionsString)
            {
                switch (regexOption)
                {
                    case 'i':
                        regexOptions |= RegexOptions.IgnoreCase;
                        break;
                    case 'm':
                        regexOptions |= RegexOptions.Multiline;
                        break;
                    case 's':
                        regexOptions |= RegexOptions.Singleline;
                        break;
                    case 'n':
                        regexOptions |= RegexOptions.ExplicitCapture;
                        break;
                    case 'x':
                        regexOptions |= RegexOptions.IgnorePatternWhitespace;
                        break;
                    case 'r':
                        regexOptions |= RegexOptions.RightToLeft;
                        break;
                    default:
                        throw new FormatException("Unrecognized RegexOption.");
                }
            }

            return regexOptions;
        }
    }
}
