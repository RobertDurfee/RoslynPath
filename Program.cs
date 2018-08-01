using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace RoslynPath
{
    class Program
    {
        static void Main(string[] args)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(@"
            using System;

            namespace Test
            {
                class Program
                {
                    static void Main(string[] args)
                    {
                        Console.WriteLine(""This is a test"");
                    }
                }
            }");

            SyntaxNode root = tree.GetRoot();

            IEnumerable<MemberDeclarationSyntax> members = root.DescendantNodes().OfType<MemberDeclarationSyntax>();
        }
    }
}
