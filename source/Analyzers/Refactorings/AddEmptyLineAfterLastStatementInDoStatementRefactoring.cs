// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Refactorings
{
    internal static class AddEmptyLineAfterLastStatementInDoStatementRefactoring
    {
        public static void AnalyzeDoStatement(SyntaxNodeAnalysisContext context)
        {
            var doStatement = (DoStatementSyntax)context.Node;

            StatementSyntax statement = doStatement.Statement;

            if (statement?.Kind() != SyntaxKind.Block)
                return;

            var block = (BlockSyntax)statement;

            StatementSyntax lastStatement = block.Statements.LastOrDefault();

            if (lastStatement == null)
                return;

            SyntaxToken closeBrace = block.CloseBraceToken;

            if (closeBrace.IsMissing)
                return;

            SyntaxToken whileKeyword = doStatement.WhileKeyword;

            if (whileKeyword.IsMissing)
                return;

            int closeBraceLine = closeBrace.GetSpanEndLine();

            if (closeBraceLine != whileKeyword.GetSpanStartLine())
                return;

            int line = lastStatement.GetSpanEndLine(context.CancellationToken);

            if (closeBraceLine - line != 1)
                return;

            SyntaxTrivia trivia = lastStatement
                .GetTrailingTrivia()
                .FirstOrDefault(f => f.IsEndOfLineTrivia());

            if (!trivia.IsEndOfLineTrivia())
                return;

            context.ReportDiagnostic(
                DiagnosticDescriptors.AddEmptyLineAfterLastStatementInDoStatement,
                Location.Create(doStatement.SyntaxTree, trivia.Span));
        }
    }
}
