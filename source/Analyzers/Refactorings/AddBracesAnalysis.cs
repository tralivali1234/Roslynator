// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using static Roslynator.CSharp.EmbeddedStatementAnalysis;

namespace Roslynator.CSharp.Refactorings
{
    internal static class AddBracesAnalysis
    {
        public static void AnalyzeIfStatement(SyntaxNodeAnalysisContext context)
        {
            var ifStatement = (IfStatementSyntax)context.Node;

            if (!ifStatement.IsSimpleIf())
                return;

            StatementSyntax statement = ifStatement.EmbeddedStatement();

            if (statement == null)
                return;

            if (statement.IsSingleLine() && FormattingSupportsEmbeddedStatement(ifStatement))
                return;

            ReportDiagnostic(context, ifStatement, statement);
        }

        public static void AnalyzeCommonForEachStatement(SyntaxNodeAnalysisContext context)
        {
            var forEachStatement = (CommonForEachStatementSyntax)context.Node;

            StatementSyntax statement = forEachStatement.EmbeddedStatement();

            if (statement == null)
                return;

            if (statement.IsSingleLine() && FormattingSupportsEmbeddedStatement(forEachStatement))
                return;

            ReportDiagnostic(context, forEachStatement, statement);
        }

        public static void AnalyzeForStatement(SyntaxNodeAnalysisContext context)
        {
            var forStatement = (ForStatementSyntax)context.Node;

            StatementSyntax statement = forStatement.EmbeddedStatement();

            if (statement == null)
                return;

            if (statement.IsSingleLine() && FormattingSupportsEmbeddedStatement(forStatement))
                return;

            ReportDiagnostic(context, forStatement, statement);
        }

        public static void AnalyzeUsingStatement(SyntaxNodeAnalysisContext context)
        {
            var usingStatement = (UsingStatementSyntax)context.Node;

            StatementSyntax statement = usingStatement.EmbeddedStatement(allowUsingStatement: false);

            if (statement == null)
                return;

            if (statement.IsSingleLine() && FormattingSupportsEmbeddedStatement(usingStatement))
                return;

            ReportDiagnostic(context, usingStatement, statement);
        }

        public static void AnalyzeWhileStatement(SyntaxNodeAnalysisContext context)
        {
            var whileStatement = (WhileStatementSyntax)context.Node;

            StatementSyntax statement = whileStatement.EmbeddedStatement();

            if (statement == null)
                return;

            if (statement.IsSingleLine() && FormattingSupportsEmbeddedStatement(whileStatement))
                return;

            ReportDiagnostic(context, whileStatement, statement);
        }

        public static void AnalyzeDoStatement(SyntaxNodeAnalysisContext context)
        {
            var doStatement = (DoStatementSyntax)context.Node;

            StatementSyntax statement = doStatement.EmbeddedStatement();

            if (statement == null)
                return;

            if (statement.IsSingleLine() && FormattingSupportsEmbeddedStatement(doStatement))
                return;

            ReportDiagnostic(context, doStatement, statement);
        }

        public static void AnalyzeLockStatement(SyntaxNodeAnalysisContext context)
        {
            var lockStatement = (LockStatementSyntax)context.Node;

            StatementSyntax statement = lockStatement.EmbeddedStatement();

            if (statement == null)
                return;

            if (statement.IsSingleLine() && FormattingSupportsEmbeddedStatement(lockStatement))
                return;

            ReportDiagnostic(context, lockStatement, statement);
        }

        public static void AnalyzeFixedStatement(SyntaxNodeAnalysisContext context)
        {
            var fixedStatement = (FixedStatementSyntax)context.Node;

            StatementSyntax statement = fixedStatement.EmbeddedStatement();

            if (statement == null)
                return;

            if (statement.IsSingleLine() && FormattingSupportsEmbeddedStatement(fixedStatement))
                return;

            ReportDiagnostic(context, fixedStatement, statement);
        }

        private static void ReportDiagnostic(SyntaxNodeAnalysisContext context, StatementSyntax statement, StatementSyntax embeddedStatement)
        {
            context.ReportDiagnostic(
                DiagnosticDescriptors.AddBracesWhenExpressionSpansOverMultipleLines,
                embeddedStatement,
                CSharpFacts.GetTitle(statement));
        }
    }
}
