// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Refactorings
{
    internal static class RemoveBracesRefactoring
    {
        public static void AnalyzeIfStatement(SyntaxNodeAnalysisContext context)
        {
            var ifStatement = (IfStatementSyntax)context.Node;

            if (!ifStatement.IsSimpleIf())
                return;

            BlockSyntax block = GetFixableBlock(ifStatement.Statement);

            if (block == null)
                return;

            if (!EmbeddedStatementHelper.FormattingSupportsEmbeddedStatement(ifStatement))
                return;

            Analyze(context, block);
        }

        public static void AnalyzeCommonForEachStatement(SyntaxNodeAnalysisContext context)
        {
            var forEachStatement = (CommonForEachStatementSyntax)context.Node;

            BlockSyntax block = GetFixableBlock(forEachStatement.Statement);

            if (block == null)
                return;

            if (!EmbeddedStatementHelper.FormattingSupportsEmbeddedStatement(forEachStatement))
                return;

            Analyze(context, block);
        }

        public static void AnalyzeForStatement(SyntaxNodeAnalysisContext context)
        {
            var forStatement = (ForStatementSyntax)context.Node;

            BlockSyntax block = GetFixableBlock(forStatement.Statement);

            if (block == null)
                return;

            if (!EmbeddedStatementHelper.FormattingSupportsEmbeddedStatement(forStatement))
                return;

            Analyze(context, block);
        }

        public static void AnalyzeUsingStatement(SyntaxNodeAnalysisContext context)
        {
            var usingStatement = (UsingStatementSyntax)context.Node;

            BlockSyntax block = GetFixableBlock(usingStatement.Statement);

            if (block == null)
                return;

            if (!EmbeddedStatementHelper.FormattingSupportsEmbeddedStatement(usingStatement))
                return;

            Analyze(context, block);
        }

        public static void AnalyzeWhileStatement(SyntaxNodeAnalysisContext context)
        {
            var whileStatement = (WhileStatementSyntax)context.Node;

            BlockSyntax block = GetFixableBlock(whileStatement.Statement);

            if (block == null)
                return;

            if (!EmbeddedStatementHelper.FormattingSupportsEmbeddedStatement(whileStatement))
                return;

            Analyze(context, block);
        }

        public static void AnalyzeDoStatement(SyntaxNodeAnalysisContext context)
        {
            var doStatement = (DoStatementSyntax)context.Node;

            BlockSyntax block = GetFixableBlock(doStatement.Statement);

            if (block == null)
                return;

            if (!EmbeddedStatementHelper.FormattingSupportsEmbeddedStatement(doStatement))
                return;

            Analyze(context, block);
        }

        public static void AnalyzeLockStatement(SyntaxNodeAnalysisContext context)
        {
            var lockStatement = (LockStatementSyntax)context.Node;

            BlockSyntax block = GetFixableBlock(lockStatement.Statement);

            if (block == null)
                return;

            if (!EmbeddedStatementHelper.FormattingSupportsEmbeddedStatement(lockStatement))
                return;

            Analyze(context, block);
        }

        public static void AnalyzeFixedStatement(SyntaxNodeAnalysisContext context)
        {
            var fixedStatement = (FixedStatementSyntax)context.Node;

            BlockSyntax block = GetFixableBlock(fixedStatement.Statement);

            if (block == null)
                return;

            if (!EmbeddedStatementHelper.FormattingSupportsEmbeddedStatement(fixedStatement))
                return;

            Analyze(context, block);
        }

        private static BlockSyntax GetFixableBlock(StatementSyntax statement)
        {
            if (!(statement is BlockSyntax block))
                return null;

            statement = block.Statements.SingleOrDefault(shouldThrow: false);

            if (statement == null)
                return null;

            if (statement.IsKind(SyntaxKind.LocalDeclarationStatement, SyntaxKind.LabeledStatement))
                return null;

            if (!statement.IsSingleLine())
                return null;

            return block;
        }

        private static void Analyze(SyntaxNodeAnalysisContext context, BlockSyntax block)
        {
            SyntaxToken openBrace = block.OpenBraceToken;
            SyntaxToken closeBrace = block.CloseBraceToken;

            if (openBrace.IsMissing
                || closeBrace.IsMissing
                || !openBrace.LeadingTrivia.IsEmptyOrWhitespace()
                || !openBrace.TrailingTrivia.IsEmptyOrWhitespace()
                || !closeBrace.LeadingTrivia.IsEmptyOrWhitespace()
                || !closeBrace.TrailingTrivia.IsEmptyOrWhitespace())
            {
                return;
            }

            string title = CSharpFacts.GetTitle(block.Parent);

            context.ReportDiagnostic(DiagnosticDescriptors.RemoveBraces, block, title);

            context.ReportBraces(DiagnosticDescriptors.RemoveBracesFadeOut, block, title);
        }

        public static Task<Document> RefactorAsync(
            Document document,
            BlockSyntax block,
            CancellationToken cancellationToken)
        {
            StatementSyntax statement = block
                .Statements[0]
                .TrimLeadingTrivia()
                .WithFormatterAnnotation();

            return document.ReplaceNodeAsync(block, statement, cancellationToken);
        }
    }
}
