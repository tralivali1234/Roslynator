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
    internal static class RemoveEmptyElseClauseRefactoring
    {
        public static void AnalyzeElseClause(SyntaxNodeAnalysisContext context)
        {
            var elseClause = (ElseClauseSyntax)context.Node;

            StatementSyntax statement = elseClause.Statement;

            if (statement?.Kind() != SyntaxKind.Block)
                return;

            var block = (BlockSyntax)statement;

            if (block.Statements.Any())
                return;

            if (!elseClause.ElseKeyword.TrailingTrivia.IsEmptyOrWhitespace())
                return;

            SyntaxToken openBrace = block.OpenBraceToken;

            if (!openBrace.LeadingTrivia.IsEmptyOrWhitespace())
                return;

            if (!openBrace.TrailingTrivia.IsEmptyOrWhitespace())
                return;

            if (!block.CloseBraceToken.LeadingTrivia.IsEmptyOrWhitespace())
                return;

            context.ReportDiagnostic(DiagnosticDescriptors.RemoveEmptyElseClause, elseClause);
        }

        public static async Task<Document> RefactorAsync(
            Document document,
            ElseClauseSyntax elseClause,
            CancellationToken cancellationToken)
        {
            if (elseClause.IsParentKind(SyntaxKind.IfStatement))
            {
                var ifStatement = (IfStatementSyntax)elseClause.Parent;
                StatementSyntax statement = ifStatement.Statement;

                if (statement?.GetTrailingTrivia().IsEmptyOrWhitespace() == true)
                {
                    IfStatementSyntax newIfStatement = ifStatement
                        .WithStatement(statement.WithTrailingTrivia(elseClause.GetTrailingTrivia()))
                        .WithElse(null);

                    return await document.ReplaceNodeAsync(ifStatement, newIfStatement, cancellationToken).ConfigureAwait(false);
                }
            }

            return await document.RemoveNodeAsync(elseClause, SyntaxRemoveOptions.KeepExteriorTrivia, cancellationToken).ConfigureAwait(false);
        }
    }
}
