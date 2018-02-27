// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Refactorings
{
    internal static class RemoveEmptyFinallyClauseRefactoring
    {
        public static void AnalyzeFinallyClause(SyntaxNodeAnalysisContext context)
        {
            var finallyClause = (FinallyClauseSyntax)context.Node;

            if (!(finallyClause.Parent is TryStatementSyntax tryStatement))
                return;

            if (!tryStatement.Catches.Any())
                return;

            BlockSyntax block = finallyClause.Block;

            if (block?.Statements.Any() != false)
                return;

            if (!finallyClause.FinallyKeyword.TrailingTrivia.IsEmptyOrWhitespace())
                return;

            if (!block.OpenBraceToken.LeadingTrivia.IsEmptyOrWhitespace())
                return;

            if (!block.OpenBraceToken.TrailingTrivia.IsEmptyOrWhitespace())
                return;

            if (!block.CloseBraceToken.LeadingTrivia.IsEmptyOrWhitespace())
                return;

            context.ReportDiagnostic(DiagnosticDescriptors.RemoveEmptyFinallyClause, finallyClause);
        }

        public static async Task<Document> RefactorAsync(
            Document document,
            FinallyClauseSyntax finallyClause,
            CancellationToken cancellationToken)
        {
            if (finallyClause.GetLeadingTrivia().IsEmptyOrWhitespace())
            {
                var tryStatement = (TryStatementSyntax)finallyClause.Parent;

                SyntaxList<CatchClauseSyntax> catches = tryStatement.Catches;
                CatchClauseSyntax lastCatch = catches[catches.Count - 1];

                if (lastCatch.GetTrailingTrivia().IsEmptyOrWhitespace())
                {
                    TryStatementSyntax newTryStatement = tryStatement
                        .WithCatches(catches.Replace(lastCatch, lastCatch.WithTrailingTrivia(finallyClause.GetTrailingTrivia())))
                        .WithFinally(null);

                    return await document.ReplaceNodeAsync(tryStatement, newTryStatement, cancellationToken).ConfigureAwait(false);
                }
            }

            return await document.RemoveNodeAsync(finallyClause, SyntaxRemoveOptions.KeepExteriorTrivia, cancellationToken).ConfigureAwait(false);
        }
    }
}
