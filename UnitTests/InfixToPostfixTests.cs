using System;
using RoslynPath;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class InfixToPostfixTests
    {
        [TestMethod]
        public void Should_Sort_When_GivenValidSequence1()
        {
            // ( /Regex1/ || "SyntaxKind1" )
            List<RPToken> inputTokens = new List<RPToken>()
            {
                new RPToken(typeof(RPOpenParentheseTokenType), "("),
                new RPToken(typeof(RPRegexTokenType), "/Regex1/"),
                new RPToken(typeof(RPOrTokenType), "||"),
                new RPToken(typeof(RPSyntaxKindTokenType), "\"SyntaxKind1\""),
                new RPToken(typeof(RPCloseParentheseTokenType), ")")
            };

            IEnumerable<RPToken> actualTokens = RPInfixToPostfix.Apply(inputTokens);

            // /Regex1/ "SyntaxKind1" ||
            List<RPToken> expectedTokens = new List<RPToken>()
            {
                new RPToken(typeof(RPRegexTokenType), "/Regex1/"),
                new RPToken(typeof(RPSyntaxKindTokenType), "\"SyntaxKind1\""),
                new RPToken(typeof(RPOrTokenType), "||")
            };

            foreach ((RPToken, RPToken) tokenPair in actualTokens.Zip(expectedTokens, (a, e) => (a, e)))
            {
                Assert.AreEqual(tokenPair.Item1.TokenType, tokenPair.Item2.TokenType);
                Assert.AreEqual(tokenPair.Item1.Value, tokenPair.Item2.Value);
            }
        }

        [TestMethod]
        public void Should_Sort_When_GivenValidSequence2()
        {
            // ( /Regex1/ || "SyntaxKind1" ) && ( /Regex2/ || "SyntaxKind2" )"
            List<RPToken> inputTokens = new List<RPToken>()
            {
                new RPToken(typeof(RPOpenParentheseTokenType), "("),
                new RPToken(typeof(RPRegexTokenType), "/Regex1/"),
                new RPToken(typeof(RPOrTokenType), "||"),
                new RPToken(typeof(RPSyntaxKindTokenType), "\"SyntaxKind1\""),
                new RPToken(typeof(RPCloseParentheseTokenType), ")"),
                new RPToken(typeof(RPAndTokenType), "&&"),
                new RPToken(typeof(RPOpenParentheseTokenType), "("),
                new RPToken(typeof(RPRegexTokenType), "/Regex2/"),
                new RPToken(typeof(RPOrTokenType), "||"),
                new RPToken(typeof(RPSyntaxKindTokenType), "\"SyntaxKind2\""),
                new RPToken(typeof(RPCloseParentheseTokenType), ")")
            };

            IEnumerable<RPToken> actualTokens = RPInfixToPostfix.Apply(inputTokens);

            // "/Regex1/ "SyntaxKind1" || /Regex2/ "SyntaxKind2" || &&
            List<RPToken> expectedTokens = new List<RPToken>()
            {
                new RPToken(typeof(RPRegexTokenType), "/Regex1/"),
                new RPToken(typeof(RPSyntaxKindTokenType), "\"SyntaxKind1\""),
                new RPToken(typeof(RPOrTokenType), "||"),
                new RPToken(typeof(RPRegexTokenType), "/Regex2/"),
                new RPToken(typeof(RPSyntaxKindTokenType), "\"SyntaxKind2\""),
                new RPToken(typeof(RPOrTokenType), "||"),
                new RPToken(typeof(RPAndTokenType), "&&")
            };

            foreach ((RPToken, RPToken) tokenPair in actualTokens.Zip(expectedTokens, (a, e) => (a, e)))
            {
                Assert.AreEqual(tokenPair.Item1.TokenType, tokenPair.Item2.TokenType);
                Assert.AreEqual(tokenPair.Item1.Value, tokenPair.Item2.Value);
            }
        }

        [TestMethod]
        public void Should_Sort_When_GivenValidSequence3()
        {
            // ( /Regex1/ || ( /Regex2/ && /Regex3/ ) )
            List<RPToken> inputTokens = new List<RPToken>()
            {
                new RPToken(typeof(RPOpenParentheseTokenType), "("),
                new RPToken(typeof(RPRegexTokenType), "/Regex1/"),
                new RPToken(typeof(RPOrTokenType), "||"),
                new RPToken(typeof(RPOpenParentheseTokenType), "("),
                new RPToken(typeof(RPSyntaxKindTokenType), "/Regex2/"),
                new RPToken(typeof(RPAndTokenType), "&&"),
                new RPToken(typeof(RPRegexTokenType), "/Regex3/"),
                new RPToken(typeof(RPCloseParentheseTokenType), ")"),
                new RPToken(typeof(RPCloseParentheseTokenType), ")")
            };

            IEnumerable<RPToken> actualTokens = RPInfixToPostfix.Apply(inputTokens);

            // /Regex1/ /Regex2/ /Regex3/ && ||
            List<RPToken> expectedTokens = new List<RPToken>()
            {
                new RPToken(typeof(RPRegexTokenType), "/Regex1/"),
                new RPToken(typeof(RPSyntaxKindTokenType), "/Regex2/"),
                new RPToken(typeof(RPRegexTokenType), "/Regex3/"),
                new RPToken(typeof(RPAndTokenType), "&&"),
                new RPToken(typeof(RPOrTokenType), "||")
            };

            foreach ((RPToken, RPToken) tokenPair in actualTokens.Zip(expectedTokens, (a, e) => (a, e)))
            {
                Assert.AreEqual(tokenPair.Item1.TokenType, tokenPair.Item2.TokenType);
                Assert.AreEqual(tokenPair.Item1.Value, tokenPair.Item2.Value);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Parenthese mismatch detected.")]
        public void Should_ThrowException_When_LackingCloseParenthese()
        {
            // ( /Regex1/ || ( /Regex2/ && /Regex3/ )
            List<RPToken> inputTokens = new List<RPToken>()
            {
                new RPToken(typeof(RPOpenParentheseTokenType), "("),
                new RPToken(typeof(RPRegexTokenType), "/Regex1/"),
                new RPToken(typeof(RPOrTokenType), "||"),
                new RPToken(typeof(RPOpenParentheseTokenType), "("),
                new RPToken(typeof(RPSyntaxKindTokenType), "/Regex2/"),
                new RPToken(typeof(RPAndTokenType), "&&"),
                new RPToken(typeof(RPRegexTokenType), "/Regex3/"),
                new RPToken(typeof(RPCloseParentheseTokenType), ")")
            };

            IEnumerable<RPToken> actualTokens = RPInfixToPostfix.Apply(inputTokens);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Parenthese mismatch detected.")]
        public void Should_ThrowException_When_LackingOpenParenthese()
        {
            // /Regex1/ || ( /Regex2/ && /Regex3/ ) )
            List<RPToken> inputTokens = new List<RPToken>()
            {
                new RPToken(typeof(RPRegexTokenType), "/Regex1/"),
                new RPToken(typeof(RPOrTokenType), "||"),
                new RPToken(typeof(RPOpenParentheseTokenType), "("),
                new RPToken(typeof(RPSyntaxKindTokenType), "/Regex2/"),
                new RPToken(typeof(RPAndTokenType), "&&"),
                new RPToken(typeof(RPRegexTokenType), "/Regex3/"),
                new RPToken(typeof(RPCloseParentheseTokenType), ")"),
                new RPToken(typeof(RPCloseParentheseTokenType), ")")
            };

            IEnumerable<RPToken> actualTokens = RPInfixToPostfix.Apply(inputTokens);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Parenthese mismatch detected.")]
        public void Should_ThrowException_When_ExtraCloseParenthese()
        {
            // ( /Regex1/ || ( /Regex2/ && /Regex3/ ) ) )
            List<RPToken> inputTokens = new List<RPToken>()
            {
                new RPToken(typeof(RPOpenParentheseTokenType), "("),
                new RPToken(typeof(RPRegexTokenType), "/Regex1/"),
                new RPToken(typeof(RPOrTokenType), "||"),
                new RPToken(typeof(RPOpenParentheseTokenType), "("),
                new RPToken(typeof(RPSyntaxKindTokenType), "/Regex2/"),
                new RPToken(typeof(RPAndTokenType), "&&"),
                new RPToken(typeof(RPRegexTokenType), "/Regex3/"),
                new RPToken(typeof(RPCloseParentheseTokenType), ")"),
                new RPToken(typeof(RPCloseParentheseTokenType), ")"),
                new RPToken(typeof(RPCloseParentheseTokenType), ")")
            };

            IEnumerable<RPToken> actualTokens = RPInfixToPostfix.Apply(inputTokens);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Parenthese mismatch detected.")]
        public void Should_ThrowException_When_ExtraOpenParenthese()
        {
            // ( ( /Regex1/ || ( /Regex2/ && /Regex3/ ) )
            List<RPToken> inputTokens = new List<RPToken>()
            {
                new RPToken(typeof(RPOpenParentheseTokenType), "("),
                new RPToken(typeof(RPOpenParentheseTokenType), "("),
                new RPToken(typeof(RPRegexTokenType), "/Regex1/"),
                new RPToken(typeof(RPOrTokenType), "||"),
                new RPToken(typeof(RPOpenParentheseTokenType), "("),
                new RPToken(typeof(RPSyntaxKindTokenType), "/Regex2/"),
                new RPToken(typeof(RPAndTokenType), "&&"),
                new RPToken(typeof(RPRegexTokenType), "/Regex3/"),
                new RPToken(typeof(RPCloseParentheseTokenType), ")"),
                new RPToken(typeof(RPCloseParentheseTokenType), ")")
            };

            IEnumerable<RPToken> actualTokens = RPInfixToPostfix.Apply(inputTokens);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Unknown token '{'.")]
        public void Should_ThrowException_When_UnkownToken1()
        {
            // { /Regex1/ || ( /Regex2/ && /Regex3/ ) }
            List<RPToken> inputTokens = new List<RPToken>()
            {
                new RPToken(typeof(RPOpenBracketTokenType), "{"),
                new RPToken(typeof(RPRegexTokenType), "/Regex1/"),
                new RPToken(typeof(RPOrTokenType), "||"),
                new RPToken(typeof(RPOpenParentheseTokenType), "("),
                new RPToken(typeof(RPSyntaxKindTokenType), "/Regex2/"),
                new RPToken(typeof(RPAndTokenType), "&&"),
                new RPToken(typeof(RPRegexTokenType), "/Regex3/"),
                new RPToken(typeof(RPCloseParentheseTokenType), ")"),
                new RPToken(typeof(RPCloseBracketTokenType), "}")
            };

            IEnumerable<RPToken> actualTokens = RPInfixToPostfix.Apply(inputTokens);
        }

        [TestMethod]
        public void Should_Sort_When_GivenFilterExpression1()
        {
            // [/Regex1/][/Regex2/][/Regex3/] || [/Regex4/]

        }
    }
}
