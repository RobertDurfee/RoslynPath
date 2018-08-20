using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RoslynPath
{
    internal class ComplexMatchTupleEqualityComparer : IEqualityComparer<(SyntaxNode, HashSet<TextSpan>)>
    {
        public bool Equals((SyntaxNode, HashSet<TextSpan>) x, (SyntaxNode, HashSet<TextSpan>) y)
        {
            return x.Item1.Equals(y.Item1);
        }

        public int GetHashCode((SyntaxNode, HashSet<TextSpan>) obj)
        {
            return obj.Item1.GetHashCode();
        }
    }

    public static class ComplexMatchTupleSetExtensions
    {
        public static HashSet<(SyntaxNode, HashSet<TextSpan>)> Union(this HashSet<(SyntaxNode, HashSet<TextSpan>)> first, HashSet<(SyntaxNode, HashSet<TextSpan>)> second)
        {
            HashSet<(SyntaxNode, HashSet<TextSpan>)> union = new HashSet<(SyntaxNode, HashSet<TextSpan>)>(first, new ComplexMatchTupleEqualityComparer());

            foreach ((SyntaxNode, HashSet<TextSpan>) secondElement in second)
            {
                if (union.TryGetValue(secondElement, out (SyntaxNode, HashSet<TextSpan>) firstElement))
                {
                    foreach (TextSpan textSpan in secondElement.Item2)
                        firstElement.Item2.Add(textSpan);
                }
                else
                    union.Add(secondElement);
            }

            return union;
        }

        public static HashSet<(SyntaxNode, HashSet<TextSpan>)> Intersect(this HashSet<(SyntaxNode, HashSet<TextSpan>)> first, HashSet<(SyntaxNode, HashSet<TextSpan>)> second)
        {
            HashSet<(SyntaxNode, HashSet<TextSpan>)> intersect = new HashSet<(SyntaxNode, HashSet<TextSpan>)>(new ComplexMatchTupleEqualityComparer());

            foreach ((SyntaxNode, HashSet<TextSpan>) firstElement in first)
            {
                if (second.TryGetValue(firstElement, out (SyntaxNode, HashSet<TextSpan>) secondElement))
                {
                    foreach (TextSpan textSpan in secondElement.Item2)
                        firstElement.Item2.Add(textSpan);

                    intersect.Add(firstElement);
                }
            }

            return intersect;
        }
    }

    [RPResultNodeBuilder(typeof(RPComplexElement))]
    internal class RPComplexResultNodeBuilder : RPResultNodeBuilder
    {
        public override IRPResultNode EvaluateElement(IRPResultNode resultNode, IEnumerable<IRPElement> elements)
        {
            if (!(elements.First() is RPComplexElement complexElement))
                throw new ArgumentException($"{elements.First()} is not an RPComplexElement.");

            string parentText = resultNode.SyntaxNode.ToString();

            IEnumerable<SyntaxNode> searchPool;

            switch (complexElement.ScanType)
            {
                case RPScanTypes.Children:
                    searchPool = resultNode.SyntaxNode.ChildNodes();
                    break;
                case RPScanTypes.Descendants:
                    searchPool = resultNode.SyntaxNode.DescendantNodes();
                    break;
                default:
                    throw new ArgumentException("Invalid scan type.");
            }
                        
            Stack<HashSet<(SyntaxNode, HashSet<TextSpan>)>> operandStack = new Stack<HashSet<(SyntaxNode, HashSet<TextSpan>)>>();
            List<RPToken> expression = complexElement.Expression.ToList();

            foreach (RPToken token in complexElement.Expression)
            {
                if (RPInfixToPostfix.Operands.Contains(token.TokenType))
                {
                    switch (token.TokenType)
                    {
                        case Type type when (type == typeof(RPSyntaxKindTokenType)):

                            string syntaxKindString = token.Value.Substring(1, token.Value.Length - 2);

                            if (!Enum.TryParse(syntaxKindString, true, out SyntaxKind syntaxKind))
                                throw new FormatException($"{syntaxKindString} is not a valid SyntaxKind.");

                            IEnumerable<(SyntaxNode, HashSet<TextSpan>)> syntaxKindSet = searchPool.Where(sn => sn.IsKind(syntaxKind))
                                .Select(s => (s, new HashSet<TextSpan>()));

                            operandStack.Push(new HashSet<(SyntaxNode, HashSet<TextSpan>)>(syntaxKindSet, new ComplexMatchTupleEqualityComparer()));

                            break;

                        case Type type when (type == typeof(RPRegexTokenType)):

                            Regex regex = RPRegexFactory.Create(token.Value);

                            IEnumerable<TextSpan> matchingTextSpans = regex.Matches(parentText)
                                .Cast<Match>()
                                .Select(m => new TextSpan(m.Index + resultNode.SyntaxNode.SpanStart, m.Length));

                            IEnumerable<(SyntaxNode, HashSet<TextSpan>)> regexSet = matchingTextSpans.SelectMany(mts => 
                                    searchPool.Where(sn => sn.Span.OverlapsWith(mts))
                                    .OrderBy(sn => (sn.Span, mts), new RPTextSpanComparer())
                                    .Select(s => (s, new HashSet<TextSpan>(new TextSpan[] { mts }))));
                            
                            operandStack.Push(new HashSet<(SyntaxNode, HashSet<TextSpan>)>(regexSet, new ComplexMatchTupleEqualityComparer()));

                            break;
                    }
                }

                else if (RPInfixToPostfix.Operators.Contains(token.TokenType))
                {
                    HashSet<(SyntaxNode, HashSet<TextSpan>)> right = operandStack.Pop();
                    HashSet<(SyntaxNode, HashSet<TextSpan>)> left = operandStack.Pop();

                    switch (token.TokenType)
                    {
                        case Type type when (type == typeof(RPAndTokenType)):

                            operandStack.Push(right.Intersect(left));

                            break;

                        case Type type when (type == typeof(RPOrTokenType)):

                            operandStack.Push(right.Union(left));

                            break;
                    }
                }
            }

            HashSet<(SyntaxNode, HashSet<TextSpan>)> results = operandStack.Pop();
            HashSet<TextSpan> textSpans = new HashSet<TextSpan>(results.SelectMany(r => r.Item2));

            foreach (TextSpan textSpan in textSpans)
            {
                IEnumerable<(SyntaxNode, HashSet<TextSpan>)> resultsWithTextSpan = results.Where(r => r.Item2.Contains(textSpan))
                    .OrderBy(r => (r.Item1.Span, textSpan), new RPTextSpanComparer());

                foreach ((SyntaxNode, HashSet<TextSpan>) element in resultsWithTextSpan.Skip(1))
                {
                    element.Item2.Remove(textSpan);

                    if (element.Item2.Count == 0)
                        results.Remove(element);
                }
            }

            foreach (SyntaxNode matchingNode in results.Select(r => r.Item1))
                resultNode.Children.Add(base.EvaluateElement(new RPResultNode(resultNode, matchingNode), elements.Skip(1)));

            return (results.Count != 0) ? resultNode : null;
        }
    }
}
