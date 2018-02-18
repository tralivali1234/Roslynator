// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using static Roslynator.CSharp.CSharpFactory;

namespace Roslynator.CSharp.SyntaxRewriters
{
    internal class CommentRemover : CSharpSyntaxRewriter
    {
        internal CommentRemover(SyntaxNode node, CommentKind kind, TextSpan span)
            : base(visitIntoStructuredTrivia: true)
        {
            Node = node;
            Span = span;

            ShouldRemoveSingleLineComment = (kind & CommentKind.SingleLine) != 0;
            ShouldRemoveMultiLineComment = (kind & CommentKind.MultiLine) != 0;
            ShouldRemoveSingleLineDocumentationComment = (kind & CommentKind.SingleLineDocumentation) != 0;
            ShouldRemoveMultiLineDocumentationComment = (kind & CommentKind.MultiLineDocumentation) != 0;
        }

        public SyntaxNode Node { get; }

        public TextSpan Span { get; }

        private bool ShouldRemoveSingleLineComment { get; }

        private bool ShouldRemoveMultiLineComment { get; }

        private bool ShouldRemoveSingleLineDocumentationComment { get; }

        private bool ShouldRemoveMultiLineDocumentationComment { get; }

        public override SyntaxTrivia VisitTrivia(SyntaxTrivia trivia)
        {
            TextSpan span = trivia.Span;

            if (Span.Contains(span))
            {
                switch (trivia.Kind())
                {
                    case SyntaxKind.SingleLineCommentTrivia:
                        {
                            if (ShouldRemoveSingleLineComment)
                                return EmptyWhitespace();

                            break;
                        }
                    case SyntaxKind.MultiLineCommentTrivia:
                        {
                            if (ShouldRemoveMultiLineComment)
                                return EmptyWhitespace();

                            break;
                        }
                    case SyntaxKind.SingleLineDocumentationCommentTrivia:
                        {
                            if (ShouldRemoveSingleLineDocumentationComment)
                                return EmptyWhitespace();

                            break;
                        }
                    case SyntaxKind.MultiLineDocumentationCommentTrivia:
                        {
                            if (ShouldRemoveMultiLineDocumentationComment)
                                return EmptyWhitespace();

                            break;
                        }
                    case SyntaxKind.EndOfLineTrivia:
                        {
                            if (ShouldRemoveSingleLineComment
                                && ShouldRemoveEndOfLine(SyntaxKind.SingleLineCommentTrivia)
                                && ShouldRemoveEndOfLine(SyntaxKind.WhitespaceTrivia)
                                && ShouldRemoveEndOfLine(SyntaxKind.EndOfLineTrivia))
                            {
                                return EmptyWhitespace();
                            }

                            break;
                        }
                }
            }

            return base.VisitTrivia(trivia);

            bool ShouldRemoveEndOfLine(SyntaxKind kind)
            {
                if (span.Start > 0)
                {
                    SyntaxTrivia t = Node.FindTrivia(span.Start - 1);

                    if (t.Kind() != kind)
                        return false;

                    span = t.Span;
                }

                return true;
            }
        }
    }
}
