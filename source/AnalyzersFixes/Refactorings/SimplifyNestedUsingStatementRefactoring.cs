// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp;

namespace Roslynator.CSharp.Refactorings
{
    internal static class SimplifyNestedUsingStatementRefactoring
    {
        public static void Analyze(SyntaxNodeAnalysisContext context, UsingStatementSyntax usingStatement)
        {
            if (!ContainsEmbeddableUsingStatement(usingStatement))
                return;

            if (usingStatement
                .Ancestors()
                .Any(f => f.IsKind(SyntaxKind.UsingStatement) && ContainsEmbeddableUsingStatement((UsingStatementSyntax)f)))
            {
                return;
            }

            var block = (BlockSyntax)usingStatement.Statement;

            context.ReportDiagnostic(DiagnosticDescriptors.SimplifyNestedUsingStatement, block);

            context.ReportBraces(DiagnosticDescriptors.SimplifyNestedUsingStatementFadeOut, block);
        }

        public static bool ContainsEmbeddableUsingStatement(UsingStatementSyntax usingStatement)
        {
            return usingStatement.Statement is BlockSyntax block
                && block.Statements.SingleOrDefault(shouldThrow: false) is UsingStatementSyntax usingStatement2
                && block.OpenBraceToken.TrailingTrivia.IsEmptyOrWhitespace()
                && block.CloseBraceToken.LeadingTrivia.IsEmptyOrWhitespace()
                && usingStatement2.GetLeadingTrivia().IsEmptyOrWhitespace()
                && usingStatement2.GetTrailingTrivia().IsEmptyOrWhitespace();
        }

        public static Task<Document> RefactorAsync(
            Document document,
            UsingStatementSyntax usingStatement,
            CancellationToken cancellationToken)
        {
            var rewriter = new SyntaxRewriter();

            var newNode = (UsingStatementSyntax)rewriter.Visit(usingStatement)
                .WithFormatterAnnotation();

            return document.ReplaceNodeAsync(usingStatement, newNode, cancellationToken);
        }

        private class SyntaxRewriter : CSharpSyntaxRewriter
        {
            public override SyntaxNode VisitUsingStatement(UsingStatementSyntax node)
            {
                if (ContainsEmbeddableUsingStatement(node))
                {
                    var block = (BlockSyntax)node.Statement;

                    node = node.WithStatement(block.Statements[0]);
                }

                return base.VisitUsingStatement(node);
            }
        }
    }
}
