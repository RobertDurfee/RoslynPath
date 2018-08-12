using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;

namespace RoslynPath
{
    class Program
    {
        static void Main(string[] args)
        {
            SyntaxNode sourceRoot = CSharpSyntaxTree.ParseText(File.ReadAllText("Test.cs")).GetRoot();
        }
    }
}
