using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoslynPath;

namespace UnitTests
{
    [TestClass]
    public class SyntaxNodeExtensionTests
    {
        [TestMethod]
        public void Should_Select_When_GivenValidPath()
        {
            SyntaxNode source = CSharpSyntaxTree.ParseText(@"
            using System;

            namespace TestNamespace
            {
                class TestClass
                {
                    public void TestMethod1()
                    {
                        Console.WriteLine(""Test1"");
                    }

                    public void TestMethod2()
                    {
                        Console.WriteLine(""Test2"");
                    }

                    public void TestMethod3()
                    {
                        int Test3 = 4;
                        Console.WriteLine(Test3);
                    }
                }
            }").GetRoot();

            IEnumerable<LiteralExpressionSyntax> actualSyntaxNodes = source.SelectNodes<LiteralExpressionSyntax>(@"$..[/Test\d/ && ""StringLiteralExpression""]");

            Assert.AreEqual(actualSyntaxNodes.Count(), 2);

            List<LiteralExpressionSyntax> expectedSyntaxNodes = new List<LiteralExpressionSyntax>()
            {
                SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal("Test1")),
                SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal("Test2"))
            };
            
            foreach ((LiteralExpressionSyntax, LiteralExpressionSyntax) actualExpectedPair in actualSyntaxNodes.Zip(expectedSyntaxNodes, (a, e) => (a, e)))
            {
                Assert.AreEqual(actualExpectedPair.Item1.Kind(), actualExpectedPair.Item2.Kind());
                Assert.AreEqual(actualExpectedPair.Item1.Token.Value, actualExpectedPair.Item2.Token.Value);
            }
        }
    }
}
