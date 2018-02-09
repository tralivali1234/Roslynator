// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace Roslynator.CSharp.SyntaxRewriters
{
    internal class WhitespaceRewriter : CSharpSyntaxRewriter
    {
        private readonly SyntaxTrivia _replacementTrivia;
        private readonly TextSpan? _span;

        private static WhitespaceRewriter DefaultInstance { get; } = new WhitespaceRewriter(DefaultReplacementTrivia);

        public static SyntaxTrivia DefaultReplacementTrivia { get; } = CSharpFactory.EmptyWhitespace();

        private WhitespaceRewriter(SyntaxTrivia replacementTrivia, TextSpan? span = null)
        {
            _replacementTrivia = replacementTrivia;
            _span = span;
        }

        public static TNode RemoveWhitespace<TNode>(TNode node, TextSpan? span = null) where TNode : SyntaxNode
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (span == null)
            {
                return (TNode)DefaultInstance.Visit(node);
            }
            else
            {
                var remover = new WhitespaceRewriter(DefaultReplacementTrivia, span);

                return (TNode)remover.Visit(node);
            }
        }

        public static TNode ReplaceWhitespace<TNode>(TNode node, SyntaxTrivia replacement, TextSpan? span = null) where TNode : SyntaxNode
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var rewriter = new WhitespaceRewriter(replacement, span);

            return (TNode)rewriter.Visit(node);
        }

        public override SyntaxTrivia VisitTrivia(SyntaxTrivia trivia)
        {
            if (trivia.IsWhitespaceOrEndOfLineTrivia()
                && (_span == null || _span.Value.Contains(trivia.Span)))
            {
                return _replacementTrivia;
            }
            else
            {
                return base.VisitTrivia(trivia);
            }
        }
    }
}
