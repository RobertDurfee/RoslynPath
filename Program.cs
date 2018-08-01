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
    class MatchTreeNode
    {
        public MatchTreeNode Parent { get; }
        
        public SyntaxNode SyntaxNode { get; }
        
        public List<MatchTreeNode> Children { get; } = new List<MatchTreeNode>();

        public MatchTreeNode(MatchTreeNode parent, SyntaxNode syntaxNode)
        {
            Parent = parent;
            SyntaxNode = syntaxNode;
        }
    }

    class MatchPathNode
    {
        public MatchPathNode Parent { get; set; }

        public Regex Pattern { get; set; }

        public MatchPathNode Child { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            SyntaxNode sourceRoot = CSharpSyntaxTree.ParseText(File.ReadAllText("Test.cs")).GetRoot();

            //(?s:.)+?(?<brack>\{(?>[^{}]|(?&brack))*\})

            MatchPathNode linkA = new MatchPathNode()
            {
                Pattern = new Regex(@"namespace Test")
            };
            MatchPathNode linkB = new MatchPathNode()
            {
                Pattern = new Regex(@"class Program.")
            };
            MatchPathNode linkC = new MatchPathNode()
            {
                Pattern = new Regex(@"static void Method.")
            };
            MatchPathNode linkD = new MatchPathNode()
            {
                Pattern = new Regex(@"(?s).+")
            };
            MatchPathNode linkE = new MatchPathNode()
            {
                Pattern = new Regex(@"Console\.WriteLine\(.+?\)")
            };

            linkA.Child = linkB;
            linkB.Parent = linkA;

            linkB.Child = linkC;
            linkC.Parent = linkB;

            linkC.Child = linkD;
            linkD.Parent = linkC;

            linkD.Child = linkE;
            linkE.Parent = linkD;

            MatchTreeNode matches = FindMatchingNodesRecursive(new MatchTreeNode(null, sourceRoot), linkA, File.ReadAllText("Test.cs"));
        }

        static MatchTreeNode FindMatchingNodesRecursive(MatchTreeNode parentMatchTreeNode, MatchPathNode matchPathNode, string sourceText)
        {
            if (matchPathNode == null)
                return parentMatchTreeNode;

            IEnumerable<TextSpan> matchingTextSpans = matchPathNode.Pattern.Matches(sourceText.Substring(parentMatchTreeNode.SyntaxNode.SpanStart, parentMatchTreeNode.SyntaxNode.Span.Length)).Cast<Match>().Select(m => new TextSpan(m.Index + parentMatchTreeNode.SyntaxNode.SpanStart, m.Length));

            if (matchingTextSpans.Count() == 0)
                return null;

            foreach (TextSpan matchingTextSpan in matchingTextSpans)
            {
                SyntaxNode matchingNode = parentMatchTreeNode.SyntaxNode.ChildNodes().Where(dn => dn.Span.OverlapsWith(matchingTextSpan))
                                                                                    .OrderBy(dn => Math.Abs(dn.Span.Length - (int)dn.Span.Overlap(matchingTextSpan)?.Length))
                                                                                    .Last();

                parentMatchTreeNode.Children.Add(FindMatchingNodesRecursive(new MatchTreeNode(parentMatchTreeNode, matchingNode), matchPathNode.Child, sourceText));
            }

            return parentMatchTreeNode;
        }
    }
}
