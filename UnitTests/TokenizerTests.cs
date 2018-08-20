using System;
using RoslynPath;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class TokenizerTests
    {
        [TestMethod]
        public void Should_Tokenize_When_GivenValidPath1()
        {
            string inputPath = @"$[/Regex1/][""SyntaxKind1""][/Regex2/ || /Regex3/][(/Regex4/ || /Regex5/) && ""SyntaxKind2""]";

            IEnumerable<RPToken> actualTokens = new RPTokenizer().Tokenize(inputPath);

            List<RPToken> expectedTokens = new List<RPToken>()
            {
                new RPToken(typeof(RPGlobalRootTokenType), "$"),
                new RPToken(typeof(RPOpenBracketTokenType), "["),
                new RPToken(typeof(RPRegexTokenType), "/Regex1/"),
                new RPToken(typeof(RPCloseBracketTokenType), "]"),
                new RPToken(typeof(RPOpenBracketTokenType), "["),
                new RPToken(typeof(RPSyntaxKindTokenType), "\"SyntaxKind1\""),
                new RPToken(typeof(RPCloseBracketTokenType), "]"),
                new RPToken(typeof(RPOpenBracketTokenType), "["),
                new RPToken(typeof(RPRegexTokenType), "/Regex2/"),
                new RPToken(typeof(RPOrTokenType), "||"),
                new RPToken(typeof(RPRegexTokenType), "/Regex3/"),
                new RPToken(typeof(RPCloseBracketTokenType), "]"),
                new RPToken(typeof(RPOpenBracketTokenType), "["),
                new RPToken(typeof(RPOpenParentheseTokenType), "("),
                new RPToken(typeof(RPRegexTokenType), "/Regex4/"),
                new RPToken(typeof(RPOrTokenType), "||"),
                new RPToken(typeof(RPRegexTokenType), "/Regex5/"),
                new RPToken(typeof(RPCloseParentheseTokenType), ")"),
                new RPToken(typeof(RPAndTokenType), "&&"),
                new RPToken(typeof(RPSyntaxKindTokenType), "\"SyntaxKind2\""),
                new RPToken(typeof(RPCloseBracketTokenType), "]"),
            };

            foreach ((RPToken, RPToken) tokenPair in actualTokens.Zip(expectedTokens, (a, e) => (a, e)))
            {
                Assert.AreEqual(tokenPair.Item1.TokenType, tokenPair.Item2.TokenType);
                Assert.AreEqual(tokenPair.Item1.Value, tokenPair.Item2.Value);
            }
        }
    }
}
