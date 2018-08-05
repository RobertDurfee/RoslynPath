using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.IO;

namespace RoslynPath
{
    class Program
    {
        static void Main(string[] args)
        {
            SyntaxNode sourceRoot = CSharpSyntaxTree.ParseText(File.ReadAllText("Test.cs")).GetRoot();

            IEnumerable<SyntaxNode> matches = sourceRoot.SelectNodes(@"$[/namespace Test/][/class Program./][/static void Method./]..[/Console\.WriteLine\(.+?\)/]");
        }
    }
}
