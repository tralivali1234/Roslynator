// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace Roslynator.CSharp.SyntaxRewriters
{
    internal class TriviaRemover : CSharpSyntaxRewriter
    {
        private readonly TextSpan? _span;

        internal TriviaRemover(TextSpan? span = null)
        {
            _span = span;
        }

        internal static TriviaRemover DefaultInstance { get; } = new TriviaRemover();

        public override SyntaxTrivia VisitTrivia(SyntaxTrivia trivia)
        {
            if (_span == null || _span.Value.Contains(trivia.Span))
            {
                return CSharpFactory.EmptyWhitespace();
            }
            else
            {
                return base.VisitTrivia(trivia);
            }
        }
    }
}
