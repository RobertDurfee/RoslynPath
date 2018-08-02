using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace RoslynPath
{
    class Program
    {
        static void Main(string[] args)
        {
            SyntaxNode sourceRoot = CSharpSyntaxTree.ParseText(File.ReadAllText("Test.cs")).GetRoot();

            //(?s:.)+?(?<brack>\{(?>[^{}]|(?&brack))*\})

            RoslynPath matchPath = new RoslynPath(new Regex[] 
            {
                new Regex(@"namespace Test"),
                new Regex(@"class Program."),
                new Regex(@"static void Method."),
                new Regex(@"(?s).+"),
                new Regex(@"Console\.WriteLine\(.+?\)")
            });

            RoslynPathMatcher matcher = new RoslynPathMatcher(sourceRoot);

            RoslynPathMatch match = matcher.Matches(matchPath);
        }
    }
}
