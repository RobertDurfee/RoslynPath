using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace RoslynPath
{
    class Program
    {
        static void Main(string[] args)
        {
            //SyntaxNode sourceRoot = CSharpSyntaxTree.ParseText(File.ReadAllText("Test.cs")).GetRoot();

            ////(?s:.)+?(?<brack>\{(?>[^{}]|(?&brack))*\})

            ////"$[/namespace Test/][/class Program./][/static void Method./]..[/Console\.WriteLine\(.+?\)/]"

            //RoslynPath roslynPath = new RoslynPath(new RoslynPathStep[]
            //{
            //    new RoslynPathStep()
            //    {
            //        StepType = RoslynPathStep.StepTypes.Root
            //    },
            //    new RoslynPathStep()
            //    {
            //        StepType = RoslynPathStep.StepTypes.Regex,
            //        ScanType = RoslynPathStep.ScanTypes.Children,
            //        Pattern = new Regex(@"namespace Test")
            //    },
            //    new RoslynPathStep()
            //    {
            //        StepType = RoslynPathStep.StepTypes.Regex,
            //        ScanType = RoslynPathStep.ScanTypes.Children,
            //        Pattern = new Regex(@"class Program.")
            //    },
            //    new RoslynPathStep()
            //    {
            //        StepType = RoslynPathStep.StepTypes.Regex,
            //        ScanType = RoslynPathStep.ScanTypes.Children,
            //        Pattern = new Regex(@"static void Method.")
            //    },
            //    new RoslynPathStep()
            //    {
            //        StepType = RoslynPathStep.StepTypes.Regex,
            //        ScanType = RoslynPathStep.ScanTypes.Descendants,
            //        Pattern = new Regex(@"Console\.WriteLine\(.+?\)")
            //    }
            //});

            //RoslynPathMatcher matcher = new RoslynPathMatcher(sourceRoot);

            //RoslynPathMatch match = matcher.Matches(roslynPath);

            IEnumerable<RPToken> tokens = new RPTokenizer().Tokenize(@"$[/namespace Test/][/class Program./][/static void Method./][/Console\.WriteLine\(.+?\)/]");
        }
    }
}
