// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Roslynator.CSharp.SyntaxRewriters;

namespace Roslynator.CSharp
{
    internal static class SyntaxRemover
    {
        public static SyntaxRemoveOptions DefaultOptions
        {
            get { return SyntaxRemoveOptions.KeepExteriorTrivia | SyntaxRemoveOptions.KeepUnbalancedDirectives; }
        }

        internal static TRoot RemoveNode<TRoot>(this TRoot root, SyntaxNode node) where TRoot : SyntaxNode
        {
            return root.RemoveNode(node, GetOptions(node));
        }

        public static SyntaxRemoveOptions GetOptions(SyntaxNode node)
        {
            SyntaxRemoveOptions removeOptions = DefaultOptions;

            if (node.GetLeadingTrivia().IsEmptyOrWhitespace())
                removeOptions &= ~SyntaxRemoveOptions.KeepLeadingTrivia;

            if (node.GetTrailingTrivia().IsEmptyOrWhitespace())
                removeOptions &= ~SyntaxRemoveOptions.KeepTrailingTrivia;

            return removeOptions;
        }

        public static SyntaxRemoveOptions GetOptions(CSharpSyntaxNode node)
        {
            SyntaxRemoveOptions removeOptions = DefaultOptions;

            if (node.GetLeadingTrivia().IsEmptyOrWhitespace())
                removeOptions &= ~SyntaxRemoveOptions.KeepLeadingTrivia;

            if (node.GetTrailingTrivia().IsEmptyOrWhitespace())
                removeOptions &= ~SyntaxRemoveOptions.KeepTrailingTrivia;

            return removeOptions;
        }

        internal static MemberDeclarationSyntax RemoveSingleLineDocumentationComment(MemberDeclarationSyntax declaration)
        {
            if (declaration == null)
                throw new ArgumentNullException(nameof(declaration));

            SyntaxTriviaList leadingTrivia = declaration.GetLeadingTrivia();

            SyntaxTriviaList.Reversed.Enumerator en = leadingTrivia.Reverse().GetEnumerator();

            int i = 0;
            while (en.MoveNext())
            {
                SyntaxKind kind = en.Current.Kind();

                if (kind == SyntaxKind.WhitespaceTrivia
                    || kind == SyntaxKind.EndOfLineTrivia)
                {
                    i++;
                }
                else if (kind == SyntaxKind.SingleLineDocumentationCommentTrivia)
                {
                    return declaration.WithLeadingTrivia(leadingTrivia.Take(leadingTrivia.Count - (i + 1)));
                }
                else
                {
                    return declaration;
                }
            }

            return declaration;
        }

        public static TNode RemoveComments<TNode>(TNode node, CommentRemoveOptions removeOptions) where TNode : SyntaxNode
        {
            return RemoveComments(node, node.FullSpan, removeOptions);
        }

        public static TNode RemoveComments<TNode>(TNode node, TextSpan span, CommentRemoveOptions removeOptions) where TNode : SyntaxNode
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var remover = new CommentRemover(node, removeOptions, span);

            return (TNode)remover.Visit(node);
        }

        public static TNode RemoveTrivia<TNode>(TNode node, TextSpan? span = null) where TNode : SyntaxNode
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (span == null)
            {
                return (TNode)TriviaRemover.DefaultInstance.Visit(node);
            }
            else
            {
                var remover = new TriviaRemover(span);

                return (TNode)remover.Visit(node);
            }
        }
    }
}
