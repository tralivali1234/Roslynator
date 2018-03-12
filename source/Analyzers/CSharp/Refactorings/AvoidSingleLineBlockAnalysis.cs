// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace Roslynator.CSharp.Refactorings
{
    internal static class AvoidSingleLineBlockAnalysis
    {
        public static void AnalyzeBlock(SyntaxNodeAnalysisContext context)
        {
            var block = (BlockSyntax)context.Node;

            if (block.Parent is AccessorDeclarationSyntax)
                return;

            if (block.Parent is AnonymousFunctionExpressionSyntax)
                return;

            SyntaxList<StatementSyntax> statements = block.Statements;

            if (!statements.Any())
                return;

            SyntaxToken openBrace = block.OpenBraceToken;

            if (openBrace.IsMissing)
                return;

            SyntaxToken closeBrace = block.CloseBraceToken;

            if (closeBrace.IsMissing)
                return;

            if (!block.SyntaxTree.IsSingleLineSpan(TextSpan.FromBounds(openBrace.SpanStart, closeBrace.Span.End), context.CancellationToken))
                return;

            context.ReportDiagnostic(DiagnosticDescriptors.AvoidSingleLineBlock, block);
        }
    }
}
