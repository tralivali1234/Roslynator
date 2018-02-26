// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings
{
    internal static class AvoidNullLiteralOnLeftOfBinaryExpressionRefactoring
    {
        public static void AnalyzeBinaryExpression(SyntaxNodeAnalysisContext context)
        {
            var binaryExpression = (BinaryExpressionSyntax)context.Node;

            if (binaryExpression.SpanContainsDirectives())
                return;

            BinaryExpressionInfo info = SyntaxInfo.BinaryExpressionInfo(binaryExpression);

            if (!info.Success)
                return;

            if (info.Right.Kind() == SyntaxKind.NullLiteralExpression)
                return;

            if (info.Left.Kind() != SyntaxKind.NullLiteralExpression)
                return;

            context.ReportDiagnostic(
                DiagnosticDescriptors.AvoidNullLiteralExpressionOnLeftSideOfBinaryExpression,
                info.Left);
        }
    }
}
