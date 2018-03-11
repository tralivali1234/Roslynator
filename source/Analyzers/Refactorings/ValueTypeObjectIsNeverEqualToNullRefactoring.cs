// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Refactorings
{
    internal static class ValueTypeObjectIsNeverEqualToNullRefactoring
    {
        internal static void AnalyzeEqualsExpression(SyntaxNodeAnalysisContext context)
        {
            Analyze(context, (BinaryExpressionSyntax)context.Node);
        }

        internal static void AnalyzeNotEqualsExpression(SyntaxNodeAnalysisContext context)
        {
            Analyze(context, (BinaryExpressionSyntax)context.Node);
        }

        private static void Analyze(SyntaxNodeAnalysisContext context, BinaryExpressionSyntax binaryExpression)
        {
            ExpressionSyntax left = binaryExpression.Left;

            if (left?.IsMissing == false)
            {
                ExpressionSyntax right = binaryExpression.Right;

                if (right?.Kind() == SyntaxKind.NullLiteralExpression
                    && IsStructButNotNullableOfT(context.SemanticModel.GetTypeSymbol(left, context.CancellationToken))
                    && !binaryExpression.SpanContainsDirectives())
                {
                    context.ReportDiagnostic(
                        DiagnosticDescriptors.ValueTypeObjectIsNeverEqualToNull,
                        binaryExpression);
                }
            }
        }

        private static bool IsStructButNotNullableOfT(ITypeSymbol typeSymbol)
        {
            switch (typeSymbol?.TypeKind)
            {
                case TypeKind.Struct:
                    return !typeSymbol.IsConstructedFrom(SpecialType.System_Nullable_T);
                case TypeKind.TypeParameter:
                    return ((ITypeParameterSymbol)typeSymbol).HasValueTypeConstraint;
                default:
                    return false;
            }
        }
    }
}
